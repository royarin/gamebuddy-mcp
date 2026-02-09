using GameBuddy.Api.Data;
using GameBuddy.Api.Models;
using GameBuddy.Api.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

// Add data layer (repository)
builder.Services.AddSingleton<IChildRepository, ChildRepository>();

// Add service layer
builder.Services.AddScoped<IKidService, KidService>();
builder.Services.AddScoped<IPreferencesService, PreferencesService>();
builder.Services.AddScoped<IProgressService, ProgressService>();

// Add OpenAPI/Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "GameBuddy API v1");
    options.RoutePrefix = string.Empty; // Serve Swagger UI at the app's root
});

app.UseCors();

// Health check endpoint
app.MapGet("/health", () => Results.Ok(new { status = "ok" }))
    .WithName("HealthCheck");

// Get all kids
app.MapGet("/kids", (IKidService kidService) =>
{
    var kids = kidService.GetAllKids();
    return Results.Ok(new { kids });
})
.WithName("GetKids")
.WithDescription("Get all kids");

// Get kid profile
app.MapGet("/kids/{nickname}/profile", (string nickname, IKidService kidService) =>
{
    var profile = kidService.GetKidProfile(nickname);
    
    if (profile == null)
    {
        return Results.NotFound(new { error = $"Kid with nickname '{nickname}' not found" });
    }
    
    return Results.Ok(profile);
})
.WithName("GetKidProfile")
.WithDescription("Get kid profile");

// Get kid preferences
app.MapGet("/kids/{nickname}/preferences", (string nickname, IPreferencesService preferencesService) =>
{
    var preferences = preferencesService.GetKidPreferences(nickname);
    
    if (preferences == null)
    {
        return Results.NotFound(new { error = $"Kid with nickname '{nickname}' not found" });
    }
    
    return Results.Ok(preferences);
})
.WithName("GetKidPreferences")
.WithDescription("Get kid preferences");

// Get kid progress
app.MapGet("/kids/{nickname}/progress", (string nickname, IProgressService progressService) =>
{
    var progress = progressService.GetKidProgress(nickname);
    
    if (progress == null)
    {
        return Results.Ok(new 
        { 
            nickname,
            message = "No progress data found yet",
            countries = new List<object>(),
            totalQuizzesCompleted = 0
        });
    }
    
    return Results.Ok(progress);
})
.WithName("GetKidProgress")
.WithDescription("Get kid's progress tracking data");

// Get all kids' progress
app.MapGet("/progress/all", (IProgressService progressService) =>
{
    var allProgress = progressService.GetAllProgress();
    return Results.Ok(new { totalKids = allProgress.Count, progress = allProgress });
})
.WithName("GetAllProgress")
.WithDescription("Get progress data for all kids");

// Record quiz completion and update progress
app.MapPost("/kids/{nickname}/quiz-completion", (string nickname, IProgressService progressService, IKidService kidService, QuizCompletionRequest request) =>
{
    // Verify kid exists
    var kidProfile = kidService.GetKidProfile(nickname);
    if (kidProfile == null)
    {
        return Results.NotFound(new { error = $"Kid with nickname '{nickname}' not found" });
    }

    // Validate request
    if (request.CountriesCovered == null || request.CountriesCovered.Count == 0)
    {
        return Results.BadRequest(new { error = "CountriesCovered array is required and must contain at least one country" });
    }

    if (request.CorrectAnswers < 0)
    {
        return Results.BadRequest(new { error = "CorrectAnswers must be a non-negative number" });
    }

    if (request.TotalQuestions <= 0)
    {
        return Results.BadRequest(new { error = "TotalQuestions must be greater than 0" });
    }

    if (request.CorrectAnswers > request.TotalQuestions)
    {
        return Results.BadRequest(new { error = "CorrectAnswers cannot exceed TotalQuestions" });
    }

    try
    {
        // Record the quiz completion - this will update progress automatically
        // Session can cover multiple countries and track question count and countries covered.
        var updatedProgress = progressService.RecordQuizCompletion(
            nickname, 
            request.CountriesCovered, 
            request.CorrectAnswers, 
            request.TotalQuestions
        );
        return Results.Ok(new 
        { 
            message = "Quiz completion recorded successfully",
            progress = updatedProgress 
        });
    }
    catch (Exception ex)
    {
        return Results.Json(new { error = "Failed to record quiz completion", details = ex.Message }, statusCode: 500);
    }
})
.WithName("RecordQuizCompletion")
.WithDescription("Record quiz completion for a kid covering one or more countries and update their progress with session tracking for trend analysis");

// Reset kid's progress
app.MapDelete("/kids/{nickname}/progress", (string nickname, IProgressService progressService, IKidService kidService) =>
{
    // Verify kid exists
    var kidProfile = kidService.GetKidProfile(nickname);
    if (kidProfile == null)
    {
        return Results.NotFound(new { error = $"Kid with nickname '{nickname}' not found" });
    }

    try
    {
        progressService.ResetKidProgress(nickname);
        return Results.Ok(new { message = $"Progress reset for kid '{nickname}'" });
    }
    catch (Exception ex)
    {
        return Results.Json(new { error = "Failed to reset progress", details = ex.Message }, statusCode: 500);
    }
})
.WithName("ResetKidProgress")
.WithDescription("Reset all progress data for a specific kid");

await app.RunAsync();

