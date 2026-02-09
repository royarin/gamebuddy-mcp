using System;
using System.ComponentModel;
using ModelContextProtocol.Server;

namespace GameBuddy.MCP.Prompts;

[McpServerPromptType]
public class ProgressSummaryPrompt
{
    [McpServerPrompt(Name = "summarize_all_children_progress_by_period")]
    [Description("Generates a comprehensive progress summary and trend analysis for all children over a specified time period in months. Analyzes quiz completion patterns, success rates, and learning trends.")]
    public string SummarizeAllChildrenProgressByPeriod(
        [Description("Number of months to analyze (e.g., 3 for last 3 months, 6 for last 6 months)")] int months)
    {
        return @"Please analyze and summarize the progress trends for ALL children in the GameBuddy system over the last " + months + @" months.

**Analysis Requirements:**

1. **Data Collection:**
   - Use get_all_children_progress to retrieve progress data for all children
   - Filter quiz sessions to only include those from the last " + months + @" months (since " + DateTime.UtcNow.AddMonths(-months).ToString("yyyy-MM-dd") + @")

2. **Individual Child Analysis:**
   For each child, analyze:
   - Total quizzes completed in the last " + months + @" months
   - Overall success rate trend (improving, declining, stable)
   - Country coverage (how many unique countries studied)
   - Session frequency (quizzes per week/month)
   - Best performing areas (countries with highest success rates)
   - Areas needing improvement (countries with lower success rates)

3. **Comparative Analysis:**
   - Rank children by overall progress
   - Identify the most active learner (most quizzes completed)
   - Identify the highest performer (best success rate)
   - Compare learning pace across children

4. **Trend Identification:**
   - Month-over-month progress trends
   - Learning velocity (are children accelerating or slowing down?)
   - Engagement patterns (consistent vs sporadic learning)

5. **Summary Format:**
   Present findings as:
   - Executive summary (2-3 sentences)
   - Individual child highlights with key metrics
   - Comparative rankings table
   - Trend observations
   - Recommendations for each child

**Current Date Reference:** " + DateTime.UtcNow.ToString("yyyy-MM-dd") + @"
**Analysis Period:** Last " + months + @" months (from " + DateTime.UtcNow.AddMonths(-months).ToString("yyyy-MM-dd") + @" to " + DateTime.UtcNow.ToString("yyyy-MM-dd") + @")

Provide a comprehensive, data-driven summary that helps understand each child's learning journey and overall system engagement.";
    }

    [McpServerPrompt(Name = "summarize_child_progress_by_period")]
    [Description("Generates a detailed progress summary and trend analysis for a specific child over a specified time period in months by nickname. Analyzes quiz patterns, success rates, and learning trajectory.")]
    public string SummarizeChildProgressByPeriod(
        [Description("Child's nickname (e.g., 'mila', 'alex', 'sam') - just pass the nickname string directly")] string nickname,
        [Description("Number of months to analyze (e.g., 3 for last 3 months, 6 for last 6 months)")] int months)
    {
        return @"Please analyze and summarize the progress trends for the child with nickname '" + nickname + @"' over the last " + months + @" months.

**Analysis Requirements:**

1. **Data Collection:**
   - Use get_child_progress_by_nickname with nickname: '" + nickname + @"'
   - Filter quiz sessions to only include those from the last " + months + @" months (since " + DateTime.UtcNow.AddMonths(-months).ToString("yyyy-MM-dd") + @")

2. **Performance Analysis:**
   - Total quizzes completed in the last " + months + @" months
   - Overall success rate and trend (improving, declining, stable)
   - Average success rate per month
   - Best performing countries (highest success rates)
   - Countries needing more practice (lower success rates)

3. **Learning Patterns:**
   - Session frequency (quizzes per week/month)
   - Consistency of practice (regular vs sporadic)
   - Learning velocity (accelerating or slowing down)
   - Country coverage diversity

4. **Trend Analysis:**
   - Week-by-week or month-by-month progress visualization
   - Success rate trajectory
   - Engagement trend (increasing or decreasing activity)

5. **Summary Format:**
   Present findings as:
   - Executive summary (2-3 sentences about overall progress)
   - Key metrics table (total quizzes, success rate, countries covered)
   - Strengths (what they're doing well)
   - Areas for improvement
   - Trend observations
   - Personalized recommendations

**Current Date Reference:** " + DateTime.UtcNow.ToString("yyyy-MM-dd") + @"
**Analysis Period:** Last " + months + @" months (from " + DateTime.UtcNow.AddMonths(-months).ToString("yyyy-MM-dd") + @" to " + DateTime.UtcNow.ToString("yyyy-MM-dd") + @")

Provide a comprehensive, encouraging summary that celebrates progress and identifies growth opportunities for " + nickname + @".";
    }
}
