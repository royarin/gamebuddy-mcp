# Game Buddy API — Agent Backend

A REST API backend for the GameBuddy AI agent that provides insights about a child's learning progress and preferences.

## Purpose

This API serves as the backend for a GameBuddy AI agent, providing four core functionalities:

1. **Known Capitals** — Track which world capitals the child has mastered
2. **Spelling Level** — Assess the child's spelling proficiency
3. **Favorite Topics** — Identify which topics the child engages with most
4. **Difficulty Preference** — Understand the child's preferred challenge level

## Tech Stack

- **.NET 8** — ASP.NET Core Minimal API
- **In-Memory Data** — JSON-based data storage
- **Serilog** — Structured logging

## Core API Endpoints

### 1. Get Known Capitals
```http
GET /api/v1/children/{childId}/known-capitals
```

Returns the capitals the child knows based on quiz performance.

**Response:**
```json
{
  "knownCapitals": [
    {
      "country": "France",
      "capital": "Paris",
      "accuracyRate": 100.0,
      "timesAnswered": 5,
      "isMastered": true
    }
  ],
  "totalKnown": 15,
  "masteryPercentage": 30.0
}
```

### 2. Get Spelling Level
```http
GET /api/v1/children/{childId}/spelling-level
```

Returns the child's spelling proficiency level.

**Response:**
```json
{
  "level": 2,
  "levelName": "Intermediate",
  "accuracyRate": 75.5,
  "wordsMastered": 12,
  "totalWordsAttempted": 25,
  "recentMasteredWords": ["beautiful", "necessary", "separate"],
  "strugglingWords": ["accommodate", "rhythm"]
}
```

### 3. Get Favorite Topics
```http
GET /api/v1/children/{childId}/favorite-topics
```

Returns the topics the child engages with most frequently.

**Response:**
```json
{
  "topics": [
    {
      "topicKey": "capitals",
      "topicName": "Capitals",
      "sessionCount": 12,
      "averageAccuracy": 78.5,
      "totalQuestions": 85,
      "lastPlayedAt": "2026-02-08T10:30:00Z"
    },
    {
      "topicKey": "spelling",
      "topicName": "Spelling",
      "sessionCount": 8,
      "averageAccuracy": 82.3,
      "totalQuestions": 60,
      "lastPlayedAt": "2026-02-07T14:20:00Z"
    }
  ],
  "mostEngagedTopic": "Capitals"
}
```

### 4. Get Difficulty Preference
```http
GET /api/v1/children/{childId}/difficulty-preference
```

Returns the child's preferred difficulty level and performance-based recommendations.

**Response:**
```json
{
  "preferredDifficulty": 2,
  "difficultyName": "Medium",
  "recommendedDifficulty": 3,
  "recommendation": "You're doing great at medium difficulty. Try hard questions!",
  "performance": {
    "easy": {
      "accuracyRate": 92.5,
      "questionsAnswered": 40
    },
    "medium": {
      "accuracyRate": 78.3,
      "questionsAnswered": 35
    },
    "hard": {
      "accuracyRate": 65.0,
      "questionsAnswered": 20
    }
  }
}
```

## Setup

### Prerequisites

- .NET 8 SDK

### Installation

1. **Restore & build:**
   ```powershell
   cd GameBuddy.Api
   dotnet restore
   dotnet build
   ```

2. **Run the API:**
   ```powershell
   dotnet run
   ```
   - API at: `http://localhost:5000`
   - Data loaded from:  `Data/gamebuddy-data.json` (copied to output directory on build)
   - All data is synthetic, fictional, and safe for demos

## Data Management

The API uses **in-memory data** loaded from a JSON file at startup.

### Data Source
- **File:** `Data/gamebuddy-data.json`
- **Format:** JSON with predefined test data
- **Content:**
  - 3 test children (Alex, Sam, Jordan)
  - 10 world capitals with varying difficulty levels
  - 10 age-appropriate spelling words
  - Sample quiz sessions and answers for demonstration

### Data Characteristics
- **Synthetic:** All data is fictional
- **Age-appropriate:** Suitable for children aged 6-11
- **Safe for demos:** No sensitive or real user information
- **Read-only:** Data is loaded on startup and kept in memory

### Modifying Data
To add or change test data:
1. Edit `Data/gamebuddy-data.json`
2. Rebuild the project (`dotnet build`)
3. Restart the API

The JSON file is automatically copied to the output directory during build.

## Testing the API

### Sample Child IDs

Use these pre-loaded child IDs for testing:
- **Alex:** `550e8400-e29b-41d4-a716-446655440001` (Age band 3, PreferredDifficulty 2)
- **Sam:** `550e8400-e29b-41d4-a716-446655440002` (Age band 2, PreferredDifficulty 1)
- **Jordan:** `550e8400-e29b-41d4-a716-446655440003` (Age band 3, PreferredDifficulty 3)

### Using .http File (Recommended)

The `api-tests.http` file contains ready-to-use HTTP requests for all endpoints. 

**VS Code users:** Install the [REST Client extension](https://marketplace.visualstudio.com/items?itemName=humao.rest-client) to run tests directly in the editor.

**Usage:**
1. Open `api-tests.http`
2. Click "Send Request" above any endpoint
3. View formatted responses inline

The file includes tests for all three children and error scenarios.

### Example Requests (curl)

```bash
# Get Alex's known capitals
curl http://localhost:5000/api/v1/children/550e8400-e29b-41d4-a716-446655440001/known-capitals

# Get Alex's spelling level
curl http://localhost:5000/api/v1/children/550e8400-e29b-41d4-a716-446655440001/spelling-level

# Get Alex's favorite topics
curl http://localhost:5000/api/v1/children/550e8400-e29b-41d4-a716-446655440001/favorite-topics

# Get Alex's difficulty preference
curl http://localhost:5000/api/v1/children/550e8400-e29b-41d4-a716-446655440001/difficulty-preference
```

## Data Schema

**Core Entities:**
- `Child` — learner profile (nickname, age band, preferred difficulty)
- `Topic` — "capitals" or "spelling"
- `CapitalItem` — country-capital pairs (3 difficulty levels)
- `SpellingItem` — words with hints (3 difficulty levels)

**Quiz Tables:**
- `QuizSession` — a play session
- `QuizQuestion` — questions in a session
- `QuizAnswer` — child's responses

## Example Response Data

### Known Capitals Response
```json
{
  "knownCapitals": [
    {
      "country": "France",
      "capital": "Paris",
      "accuracyRate": 100.0,
      "timesAnswered": 4,
      "isMastered": true
    }
  ],
  "totalKnown": 5,
  "masteryPercentage": 50.0
}
```

### Spelling Level Response
```json
{
  "level": 2,
  "levelName": "Intermediate",
  "accuracyRate": 75.5,
  "wordsMastered": 8,
  "totalWordsAttempted": 15,
  "recentMasteredWords": ["beautiful", "adventure"],
  "strugglingWords": ["necessary"]
}
```

## Project Structure

```
GameBuddy.Api/
├── Models/                      Individual domain model files
│   ├── Child.cs
│   ├── Topic.cs
│   ├── CapitalItem.cs
│   ├── SpellingItem.cs
│   ├── QuizSession.cs
│   ├── QuizQuestion.cs
│   └── QuizAnswer.cs
├── Data/
│   ├── DataService.cs           In-memory data service
│   └── gamebuddy-data.json      Test data (auto-copied to output)
├── Services/
│   └── GameBuddyAgentService.cs Agent-focused operations
├── Program.cs                   DI, middleware, endpoints
├── api-tests.http               HTTP test requests for all endpoints
├── appsettings.json
├── README.md
├── AGENT_API_REFERENCE.md       Detailed endpoint documentation
└── MIGRATION_SUMMARY.md         Change history
```

## Architecture

- **No authentication** — Focused on agent integration
- **Stateless** — Each request is independent
- **In-memory data** — Fast, portable, demo-ready
- **Minimal dependencies** — Just .NET 8 and Serilog

---

**Simple, focused, agent-ready** — Four endpoints that power intelligent conversations about a child's learning journey.

