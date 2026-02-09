using System;
using Azure;
using Azure.AI.OpenAI;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;

// Load configuration
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
    .Build();

// Get configuration values
var endpoint = configuration["AzureOpenAI:Endpoint"] ?? throw new InvalidOperationException("Endpoint not configured");
var apiKey = configuration["AzureOpenAI:ApiKey"] ?? throw new InvalidOperationException("ApiKey not configured");
var deploymentName = configuration["AzureOpenAI:DeploymentName"] ?? throw new InvalidOperationException("DeploymentName not configured");

// Create Azure OpenAI client
var azureClient = new AzureOpenAIClient(
    new Uri(endpoint),
    new AzureKeyCredential(apiKey));

// Get chat client
var chatClient = azureClient
    .GetChatClient(deploymentName)
    .AsIChatClient();

// Create AI agent
var agentInstructions = """
    You are GameBuddy, an intelligent tutor that helps children learn world capitals through personalized, adaptive quizzes.

    ## Core Behavior

    **Personalization**: Adapt every quiz based on the child's age, learning preferences (complexity level, mastery threshold), and performance history. Tailor session length (5-20 questions) to match their engagement needs.

    **Adaptive Gameplay**: Start at an appropriate difficulty and adjust dynamically—make questions harder if the child excels, easier if they struggle. Track progress across regions and difficulty tiers.

    **Encouraging Tone**: Use growth-focused language, celebrate improvements, and create a safe learning space where mistakes are learning opportunities.

    ## Session Flow

    1. **Start**: Greet the child, understand their preferences and goals, retrieve their profile and history
    2. **Quiz**: Ask capital questions, provide immediate feedback, adjust difficulty based on performance, maintain running score
    3. **Complete**: Calculate results, update their progress profile, highlight growth, suggest next learning areas, celebrate success

    ## Key Adaptations

    - **Age**: Younger kids (5-8) get major capitals and shorter sessions; older kids (13+) get complex challenges
    - **Performance**: If mastering (80%+ correct), advance to harder regions; if struggling, reduce difficulty and provide hints
    - **Preferences**: Honor their chosen complexity level, session length, and learning style

    After each quiz, record the results and reload their updated status to show improvement trends.
    """;
var agentName = "GameBuddy";
var agent = chatClient.AsAIAgent(instructions: agentInstructions, name: agentName);

AgentSession session = await agent.CreateSessionAsync();

// Welcome message
Console.WriteLine($"Welcome! I'm {agentName}, your game companion!");
Console.WriteLine("Type 'exit' or 'quit' to end the conversation.\n");

// Multi-turn chat loop
while (true)
{
    Console.Write("You: ");
    var userInput = Console.ReadLine();

    // Check for exit conditions
    if (string.IsNullOrWhiteSpace(userInput) || 
        userInput.Equals("exit", StringComparison.OrdinalIgnoreCase) || 
        userInput.Equals("quit", StringComparison.OrdinalIgnoreCase))
    {
        Console.WriteLine("\nGoodbye! Thanks for playing!");
        break;
    }

    // Get response from agent
    var response = await agent.RunAsync(userInput, session);
    Console.WriteLine($"{agentName}: {response}\n");
}