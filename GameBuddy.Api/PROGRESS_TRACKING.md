# GameBuddy API - Progress Tracking Implementation

## Overview

A complete progress tracking system has been implemented in the GameBuddy API to monitor kids' quiz performance. Progress is tracked per kid in JSON files, with detailed session history and country-level statistics.

## Features

✅ **Per-Kid Progress Files**: Each kid's progress is stored in a separate JSON file (`{nickname}_progress.json`)  
✅ **Country-Based Statistics**: Tracks total correct answers, sessions completed, and average success rate per country  
✅ **Quiz Session History**: Complete history of all quiz sessions with countries covered, questions, and results  
✅ **Multi-Country Sessions**: Sessions can cover multiple countries in one quiz  
✅ **Progress Retrieval**: Get individual kid's progress or all kids' progress  
✅ **Progress Reset**: Ability to clear a kid's progress data  
✅ **Automatic Calculations**: Success rates and averages calculated automatically  
✅ **Persistent Storage**: All progress saved to JSON files in `Data/Progress` directory  

## New Files Created

### Models
1. **[Models/KidProgress.cs](Models/KidProgress.cs)** - Main progress model
   - `KidProgress`: Represents overall progress with list of countries
   - `CountryProgress`: Represents progress for a specific country

2. **[Models/QuizCompletionRequest.cs](Models/QuizCompletionRequest.cs)** - Request DTO
   - Defines the contract for quiz completion requests

### Services
1. **[Services/IProgressService.cs](Services/IProgressService.cs)** - Service interface
   - `GetKidProgress()`: Retrieve progress for a specific kid
   - `RecordQuizCompletion()`: Record quiz completion and update progress
   - `GetAllProgress()`: Retrieve all kids' progress
   - `ResetKidProgress()`: Clear kid's progress data

2. **[Services/ProgressService.cs](Services/ProgressService.cs)** - Service implementation
   - JSON file-based storage in `Data/Progress` directory
   - Automatic directory creation
   - Logging for all operations
   - Thread-safe file operations

## API Endpoints

### 1. Get Kid's Progress
```http
GET /kids/{nickname}/progress
```
**Response Example:**
```json
{
  "nickname": "alex",
  "countries": [
    {
      "country": "France",
      "totalCorrectAnswers": 9,
      "sessionsCompleted": 3,
      "averageSuccessRate": 75.5
    }
  ],
  "quizSessions": [
    {
      "sessionId": "abc-123",
      "countriesCovered": ["France", "Germany"],
      "date": "2026-02-09T10:40:19Z",
      "totalQuestions": 5,
      "correctAnswers": 4,
      "successRate": 80.0
    }
  ],
  "totalQuizzesCompleted": 3,
  "overallAverageSuccessRate": 78.3,
  "lastUpdated": "2026-02-09T10:40:19Z"
}
```

### 2. Get All Kids' Progress
```http
GET /progress/all
```
**Response Example:**
```json
{
  "totalKids": 2,
  "progress": [
    {
      "nickname": "alex",
      "countries": [...],
      "quizSessions": [...],
      "totalQuizzesCompleted": 3,
      "overallAverageSuccessRate": 78.3,
      "lastUpdated": "2026-02-09T10:40:19Z"
    }
  ]
}
```

### 3. Record Quiz Completion
```http
POST /kids/{nickname}/quiz-completion
Content-Type: application/json

{
  "countriesCovered": ["France", "Germany"],
  "correctAnswers": 4,
  "totalQuestions": 5
}
```
**Response Example:**
```json
{
  "message": "Quiz completion recorded successfully",
  "progress": {
    "nickname": "alex",
    "countries": [...],
    "quizSessions": [...],
    "totalQuizzesCompleted": 4,
    "overallAverageSuccessRate": 79.1,
    "lastUpdated": "2026-02-09T10:45:19Z"
  }
}
```

### 4. Reset Kid's Progress
```http
DELETE /kids/{nickname}/progress
```
**Response Example:**
```json
{
  "message": "Progress reset for kid 'alex'"
}
```

## Data Storage

Progress files are stored as JSON in: `{ApplicationBaseDirectory}/Data/Progress/`

Example file: `alex_progress.json`
```json
{
  "nickname": "alex",
  "countries": [
    {
      "country": "France",
      "totalCorrectAnswers": 9,
      "sessionsCompleted": 3,
      "averageSuccessRate": 75.5
    }
  ],
  "quizSessions": [
    {
      "sessionId": "abc-123",
      "countriesCovered": ["France", "Germany"],
      "date": "2026-02-09T10:40:19Z",
      "totalQuestions": 5,
      "correctAnswers": 4,
      "successRate": 80.0
    }
  ],
  "totalQuizzesCompleted": 3,
  "overallAverageSuccessRate": 78.3,
  "lastUpdated": "2026-02-09T10:40:19Z"
}
```

## Service Registration

The `ProgressService` is registered in [Program.cs](Program.cs):
```csharp
builder.Services.AddScoped<IProgressService, ProgressService>();
```

## Usage Examples

### Record a Quiz Completion
```powershell
$body = @{ 
  countriesCovered = @("France", "Germany")
  correctAnswers = 4
  totalQuestions = 5
} | ConvertTo-Json

Invoke-WebRequest -Uri "http://localhost:5000/kids/alex/quiz-completion" `
  -Method Post `
  -Headers @{"Content-Type"="application/json"} `
  -Body $body
```

### Get Progress for a Kid
```powershell
Invoke-WebRequest -Uri "http://localhost:5000/kids/alex/progress" -Method Get
```

### Reset Progress
```powershell
Invoke-WebRequest -Uri "http://localhost:5000/kids/alex/progress" -Method Delete
```

## Integration with Quiz Flow

The progress tracking can be integrated into the quiz flow:

1. **After Quiz Completion**: Call `POST /kids/{nickname}/quiz-completion` with the quiz results
2. **Quiz Results Should Include**:
   - `countriesCovered`: Array of countries covered in the quiz session
   - `correctAnswers`: Number of correct answers in this quiz
   - `totalQuestions`: Total number of questions asked

The service automatically:
- Creates a kid's progress record if it doesn't exist
- Creates a new quiz session entry with session ID, timestamp, and results
- Updates country statistics (total correct answers, sessions completed, average success rate)
- Calculates overall average success rate across all sessions
- Records the last update timestamp

## Technical Details

- **Framework**: ASP.NET Core Minimal APIs
- **Storage**: JSON files
- **Concurrency**: File-based (suitable for single-server deployments)
- **Logging**: Using Microsoft.Extensions.Logging
- **Error Handling**: Try-catch blocks with detailed error logging

## Future Enhancements

- Dashboard view for progress analytics
- Export progress data (CSV, PDF)
- Time-based progress tracking (daily stats, weekly trends)
- Performance benchmarking per kid
- Goals and achievement system
- Database storage option for multi-server deployments
