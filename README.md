# GameBuddy MCP Server

A Model Context Protocol (MCP) server demonstrating how to provide AI agents with contextual capabilities through tools, resources, and prompts.

## The Problem: Agents Without Context

AI agents are powerful, but without proper tools and context, they're limited in what they can accomplish. Managing multiple individual tools becomes cumbersome as applications grow in complexity. This project demonstrates how MCP solves this problem by:

1. **Providing Context Through Tools** - Exposing API endpoints as standardized MCP tools that agents can discover and use
2. **Centralizing Tool Management** - Consolidating all tools, resources, and prompts in a single MCP server
3. **Introducing MCP Primitives** - Leveraging resources for static/dynamic content and prompts for guided workflows

## Project Structure

This repository contains a complete GameBuddy learning system with multiple components demonstrating agent capabilities with and without tools:

### 1. GameBuddy.Api
REST API backend that manages children's profiles, preferences, and quiz progress tracking.
- **Port**: `https://localhost:5001`
- **Endpoints**: Profile management, preferences, progress tracking, quiz completion
- **Storage**: File-based JSON storage
- **OpenAPI**: Swagger UI available at root (`/`)

### 2. GameBuddy (Console Agent - No Tools)
A console-based AI agent that attempts to interact with GameBuddy without any tools or context.

**Demonstrates:**
- How agents struggle without proper tools
- Limited capabilities when relying only on general knowledge
- Inability to access real-time data or perform actions

**Run:**
```powershell
cd GameBuddy
dotnet run
```

This agent will show how difficult it is to provide meaningful assistance without access to actual data or capabilities.

### 3. GameBuddyTools (Console Agent - With Tools via Function Calling)
A console-based AI agent that uses function calling to interact with the GameBuddy API.

**Demonstrates:**
- How tools enable agents to access real data
- Function calling pattern for AI agents
- Direct API integration with tool definitions

**Run:**
```powershell
cd GameBuddyTools
dotnet run
```

This agent can successfully retrieve and manipulate data by calling API endpoints through function calling. However, managing and maintaining tool definitions can become cumbersome as the application grows.

### 4. GameBuddy.MCP (MCP Server - Centralized Tool Management)
The MCP server that exposes GameBuddy API functionality through MCP primitives:

**Tools (6):**
- `list_all_children_by_nicknames` - List all children
- `get_child_profile_by_nickname` - Get profile by nickname
- `get_child_preferences_by_nickname` - Get learning preferences
- `get_child_progress_by_nickname` - Get progress for one child
- `get_all_children_progress` - Get progress for all children
- `record_child_quiz_completion_by_nickname` - Record quiz results

**Resources (2):**
- `spelling-rules://age/{age}` - Age-appropriate spelling rules and teaching strategies
- `capital-game://difficulty-strategy` - Adaptive difficulty strategy with 195 countries across 3 levels

**Prompts (2):**
- `summarize_all_children_progress_by_period` - Comprehensive progress analysis for all children
- `summarize_child_progress_by_period` - Detailed progress analysis for a specific child

## Running the MCP Server

### Prerequisites
- .NET 8.0 SDK
- Visual Studio Code with GitHub Copilot

### 1. Start the GameBuddy API
```powershell
cd GameBuddy.Api
dotnet run
```
The API will start at `https://localhost:5001` with Swagger UI at the root.

### 2. Start the GameBuddy MCP Server
```powershell
cd GameBuddy.MCP
dotnet run
```
The MCP server will start and expose tools via HTTP transport.

### 3. Configure VSCode to Use the MCP Server

Add the MCP server configuration to your VSCode settings. You can use either HTTP transport or command-based execution:

#### Option A: HTTP Transport (Recommended)

**Workspace Settings** (`.vscode/settings.json`):
```json
{
  "github.copilot.chat.mcp.servers": {
    "gamebuddy-mcp": {
      "type": "http",
      "url": "http://localhost:5126/mcp",
      "headers": {}
    }
  }
}
```

**Benefits:**
- Server runs independently
- Easier to debug and monitor
- Can be shared across multiple clients
- Faster connection and reconnection

#### Option B: Command-Based (Auto-Start)

**Workspace Settings** (`.vscode/settings.json`):
```json
{
  "github.copilot.chat.mcp.servers": {
    "gamebuddy": {
      "command": "dotnet",
      "args": ["run", "--project", "GameBuddy.MCP/GameBuddy.MCP.csproj"],
      "cwd": "${workspaceFolder}",
      "env": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
```

**User Settings** (for global access):
```json
{
  "github.copilot.chat.mcp.servers": {
    "gamebuddy": {
      "command": "dotnet",
      "args": ["run", "--project", "D:/Demo/AgenCon/Oslo/MCP/GameBuddy.MCP/GameBuddy.MCP.csproj"],
      "env": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
```

**Benefits:**
- VSCode automatically starts the MCP server
- No need to manually run the server
- Server lifecycle managed by VSCode

### 4. Testing in VSCode

1. Open GitHub Copilot Chat in VSCode
2. Start a new chat session
3. The MCP server tools will be automatically available to Copilot

**Example Prompts to Try:**

```
Show me all available children in the system

Get Mila's profile and preferences

Analyze Mila's progress over the last 3 months

What are the spelling rules appropriate for a 7-year-old?

Record a quiz completion for Alex: France, Germany, Italy - 4 out of 5 correct

Compare the progress of all children
```

## MCP Primitives Demonstrated

### Tools
Functions that agents can invoke to perform actions or retrieve data. All 6 tools use nickname-based identification (e.g., "mila", "alex", "sam") without separate IDs.

**Example:**
```json
{
  "name": "get_child_progress_by_nickname",
  "parameters": {
    "nickname": "mila"
  }
}
```

### Resources
Static or dynamic content that provides context to agents. Resources have URI templates and can be parameterized.

**Example:**
```
spelling-rules://age/7
capital-game://difficulty-strategy
```

### Prompts
Pre-structured instructions that guide agents through complex workflows. Prompts can accept parameters to customize behavior.

**Example:**
```json
{
  "name": "summarize_child_progress_by_period",
  "parameters": {
    "nickname": "mila",
    "months": 3
  }
}
```

## Architecture Benefits

### Before MCP
- Each API endpoint requires separate tool definition
- Tool management scattered across codebase
- No standardized way to provide context
- Difficult to version and maintain

### After MCP
- Centralized tool, resource, and prompt management
- Standardized protocol for agent communication
- Easy discoverability through MCP introspection
- Version control and documentation in one place

## Sample Data

The system comes with 3 pre-configured children:
- **Alex** (age 8) - Prefers easy difficulty, interested in geography and science
- **Mila** (age 6) - Prefers hard difficulty, interested in spelling and math
- **Sam** (age 10) - Prefers medium difficulty, interested in history and geography

Progress data is stored in `GameBuddy.Api/Data/Progress/{nickname}_progress.json`.

## Technologies Used

- **.NET 8.0** - Application framework
- **ASP.NET Core Minimal APIs** - API endpoints
- **ModelContextProtocol.Server** - MCP SDK
- **Swashbuckle.AspNetCore** - OpenAPI/Swagger
- **Serilog** - Structured logging
- **System.Text.Json** - JSON serialization

## Development Notes

- The API uses HTTPS on port 5001 with CORS enabled for development
- MCP server connects to the API via HttpClient with base URL from configuration
- All tools use simple string nicknames (no GUIDs or numeric IDs) to reduce LLM confusion
- Progress tracking includes session history for trend analysis
- Adaptive difficulty resource provides complete 195-country dataset to eliminate LLM guessing

## Learn More

### Official Resources

- **[Model Context Protocol](https://modelcontextprotocol.io/)** - Official MCP website with complete documentation, specifications, and guides
- **[MCP C# SDK](https://github.com/modelcontextprotocol/csharp-sdk)** - Official C# implementation of the Model Context Protocol
- **[MCP for Beginners (Microsoft)](https://github.com/microsoft/mcp-for-beginners)** - Step-by-step tutorials and examples from Microsoft

### Key Concepts to Explore

- **MCP Specification** - Understanding the protocol fundamentals
- **Transport Mechanisms** - Stdio vs HTTP transport patterns
- **Server Implementation** - Building custom MCP servers
- **Client Integration** - Connecting LLMs and agents to MCP servers
- **Tool Design Patterns** - Best practices for tool naming and descriptions

## Contributing

This repository demonstrates MCP server implementation patterns. Feel free to explore, learn, and adapt for your own MCP servers.

## License

MIT License - See LICENSE file for details
