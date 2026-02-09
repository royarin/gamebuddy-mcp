using GameBuddy.MCP.Prompts;
using GameBuddy.MCP.Resources;
using GameBuddy.MCP.Tools;

var builder = WebApplication.CreateBuilder(args);

// Add HttpClient for GameBuddy API
builder.Services.AddHttpClient("GameBuddyApi", client =>
{
    var apiBaseUrl = builder.Configuration["GameBuddyApi:BaseUrl"] ?? "https://localhost:5001";
    client.BaseAddress = new Uri(apiBaseUrl);
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddCors();
builder.Services.AddMcpServer()
.WithHttpTransport()
.WithTools<GameBuddyTool>()
.WithResources<SpellingRulesResource>()
.WithResources<CapitalGameAdaptiveDifficultyResource>()
.WithPrompts<ProgressSummaryPrompt>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Add CORS for VSCode MCP client
app.UseCors(policy =>
{
    policy.AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader();
});

// app.UseHttpsRedirection(); // Disabled for MCP development
app.MapMcp("/mcp");

await app.RunAsync();

