using System.ComponentModel;
using System.Text.Json;
using ModelContextProtocol.Server;

namespace GameBuddy.MCP.Resources;

[McpServerResourceType]
public class SpellingRulesResource
{
    [McpServerResource(UriTemplate = "spelling-rules://age/{age}")]
    [Description("Provides age-appropriate spelling rules, complexity guidelines, and teaching strategies for kids")]
    public Task<string> GetSpellingRulesByAge(int age)
    {
        var rules = GetRulesForAge(age);
        var json = JsonSerializer.Serialize(rules, new JsonSerializerOptions { WriteIndented = true });
        return Task.FromResult(json);
    }

    private SpellingRulesDto GetRulesForAge(int age)
    {
        return age switch
        {
            < 0 => new SpellingRulesDto
            {
                AgeGroup = "Invalid Age",
                Age = age,
                ComplexityLevel = "Basic",
                WordLength = "2-4 letters",
                FocusAreas = new List<string> { "Basic phonics", "Letter recognition" },
                RecommendedWordTypes = new List<string>(),
                AvoidPatterns = new List<string>(),
                TeachingStrategies = new List<string> { "Age-appropriate instruction" },
                MaxWordCount = 5,
                RecommendedDifficulty = "easy",
                AllowedLetterPatterns = new List<string>()
            },
            <= 5 => new SpellingRulesDto
            {
                AgeGroup = "PreK-Kindergarten (3-5 years)",
                Age = age,
                ComplexityLevel = "Basic",
                WordLength = "2-4 letters",
                FocusAreas = new List<string>
                {
                    "Letter recognition and phonics",
                    "Simple CVC (consonant-vowel-consonant) words",
                    "Beginning and ending sounds",
                    "Simple sight words"
                },
                RecommendedWordTypes = new List<string>
                {
                    "cat", "dog", "hat", "sun", "run", "sit", "mat", "bat", "can", "fan"
                },
                AvoidPatterns = new List<string>
                {
                    "Silent letters",
                    "Complex vowel combinations",
                    "Multi-syllable words",
                    "Irregular spelling patterns"
                },
                TeachingStrategies = new List<string>
                {
                    "Use visual aids and pictures",
                    "Focus on sound-letter correspondence",
                    "Practice one letter/sound at a time",
                    "Use repetition and games",
                    "Keep words simple and concrete"
                },
                MaxWordCount = 5,
                RecommendedDifficulty = "easy",
                AllowedLetterPatterns = new List<string>
                {
                    "CVC (cat, dog, sun)",
                    "Simple consonant blends at start (st-, pl-, gr-)"
                }
            },
            <= 7 => new SpellingRulesDto
            {
                AgeGroup = "Early Elementary (6-7 years)",
                Age = age,
                ComplexityLevel = "Elementary",
                WordLength = "3-6 letters",
                FocusAreas = new List<string>
                {
                    "Short and long vowel patterns",
                    "Common consonant blends and diagraphs",
                    "Word families (-at, -all, -ing)",
                    "Basic high-frequency words"
                },
                RecommendedWordTypes = new List<string>
                {
                    "make", "like", "stop", "jump", "play", "from", "that", "with", "them", "this"
                },
                AvoidPatterns = new List<string>
                {
                    "Complex irregular spellings",
                    "Advanced silent letters",
                    "Words with more than 2 syllables",
                    "Uncommon letter combinations"
                },
                TeachingStrategies = new List<string>
                {
                    "Introduce word families",
                    "Teach common spelling patterns",
                    "Use rhyming words to reinforce patterns",
                    "Practice blending sounds",
                    "Introduce simple prefixes and suffixes"
                },
                MaxWordCount = 8,
                RecommendedDifficulty = "easy",
                AllowedLetterPatterns = new List<string>
                {
                    "CVC and CVCe patterns (make, like)",
                    "Consonant blends (bl-, st-, tr-)",
                    "Digraphs (ch, sh, th, wh)",
                    "Simple word endings (-ing, -ed, -s)"
                }
            },
            <= 9 => new SpellingRulesDto
            {
                AgeGroup = "Middle Elementary (8-9 years)",
                Age = age,
                ComplexityLevel = "Intermediate",
                WordLength = "4-8 letters",
                FocusAreas = new List<string>
                {
                    "Multi-syllable words",
                    "Common prefixes and suffixes",
                    "Vowel team patterns (ai, ea, oa)",
                    "R-controlled vowels (ar, er, ir, or, ur)",
                    "Homophones and commonly confused words"
                },
                RecommendedWordTypes = new List<string>
                {
                    "because", "different", "important", "beautiful", "together", "another", "through", "thought", "friend", "school"
                },
                AvoidPatterns = new List<string>
                {
                    "Very irregular or archaic spellings",
                    "Advanced Latin/Greek roots",
                    "Words with more than 3 syllables",
                    "Rare letter combinations"
                },
                TeachingStrategies = new List<string>
                {
                    "Break words into syllables",
                    "Teach common prefixes (re-, un-, pre-) and suffixes (-ful, -less, -tion)",
                    "Practice spelling rules (i before e, drop silent e)",
                    "Use mnemonic devices for tricky words",
                    "Focus on word origins and patterns"
                },
                MaxWordCount = 10,
                RecommendedDifficulty = "medium",
                AllowedLetterPatterns = new List<string>
                {
                    "Vowel teams (rain, boat, feed)",
                    "R-controlled vowels (car, her, bird)",
                    "Silent e patterns (hope, made)",
                    "Common prefixes and suffixes",
                    "Double consonants (happy, running)"
                }
            },
            <= 11 => new SpellingRulesDto
            {
                AgeGroup = "Upper Elementary (10-11 years)",
                Age = age,
                ComplexityLevel = "Advanced",
                WordLength = "5-10 letters",
                FocusAreas = new List<string>
                {
                    "Complex multi-syllable words",
                    "Advanced prefixes, suffixes, and roots",
                    "Irregular spelling patterns",
                    "Academic vocabulary",
                    "Words from other languages"
                },
                RecommendedWordTypes = new List<string>
                {
                    "necessary", "occasion", "knowledge", "conscience", "enthusiasm", "privilege", "guarantee", "rhythm", "recommend", "achieve"
                },
                AvoidPatterns = new List<string>
                {
                    "Extremely rare or technical terms",
                    "Obsolete spellings",
                    "Highly specialized jargon"
                },
                TeachingStrategies = new List<string>
                {
                    "Teach Greek and Latin roots",
                    "Explore word etymology and history",
                    "Practice advanced spelling rules and exceptions",
                    "Use context clues and word relationships",
                    "Challenge with commonly misspelled words",
                    "Introduce morphology (word structure)"
                },
                MaxWordCount = 12,
                RecommendedDifficulty = "hard",
                AllowedLetterPatterns = new List<string>
                {
                    "All common patterns",
                    "Silent letters (know, write, psychology)",
                    "Complex suffixes (-tion, -sion, -ous, -ious)",
                    "Greek/Latin roots (photo-, bio-, -graph)",
                    "Unstressed syllables (separate, chocolate)"
                }
            },
            <= 14 => new SpellingRulesDto
            {
                AgeGroup = "Middle School (12-14 years, Grades 7-8)",
                Age = age,
                ComplexityLevel = "Advanced+",
                WordLength = "6-12 letters",
                FocusAreas = new List<string>
                {
                    "Subject-specific vocabulary",
                    "Advanced Greek and Latin roots",
                    "Complex derivational patterns",
                    "Contextual spelling strategies",
                    "Frequently misspelled academic words"
                },
                RecommendedWordTypes = new List<string>
                {
                    "accommodate", "persuade", "embarrass", "adolescence", "maintenance", "committee", "extraordinary", "bureaucracy", "persistence", "sophisticated"
                },
                AvoidPatterns = new List<string>
                {
                    "Highly specialized medical/legal terms",
                    "Archaic or obsolete words"
                },
                TeachingStrategies = new List<string>
                {
                    "Connect spelling to meaning and word history",
                    "Analyze morphemic structure",
                    "Study spelling patterns across word families",
                    "Use spelling to support reading comprehension",
                    "Practice SAT/ACT vocabulary building"
                },
                MaxWordCount = 15,
                RecommendedDifficulty = "hard",
                AllowedLetterPatterns = new List<string>
                {
                    "All standard patterns",
                    "Advanced Greek/Latin roots (psych-, -ology, -ism)",
                    "Complex derivational suffixes (-ance/-ence, -able/-ible)",
                    "Absorbed prefixes (ac-, at-, of-)"
                }
            },
            <= 18 => new SpellingRulesDto
            {
                AgeGroup = "High School (15-18 years, Grades 9-12)",
                Age = age,
                ComplexityLevel = "Expert",
                WordLength = "8-15 letters",
                FocusAreas = new List<string>
                {
                    "College-preparatory vocabulary",
                    "Discipline-specific terminology",
                    "Advanced morphological analysis",
                    "Etymology and historical linguistics",
                    "Standardized test vocabulary"
                },
                RecommendedWordTypes = new List<string>
                {
                    "conscientious", "acquiesce", "corroborate", "incongruous", "ostentatious", "presumptuous", "surreptitious", "ambiguous", "belligerent", "nomenclature"
                },
                AvoidPatterns = new List<string>
                {
                    "Extremely rare technical jargon"
                },
                TeachingStrategies = new List<string>
                {
                    "Integrate spelling with vocabulary development",
                    "Explore word origins and cognates across languages",
                    "Practice systematic word analysis",
                    "Prepare for college-level reading and writing",
                    "Study commonly confused and misspelled words in academic contexts"
                },
                MaxWordCount = 18,
                RecommendedDifficulty = "hard",
                AllowedLetterPatterns = new List<string>
                {
                    "All patterns including rare combinations",
                    "Foreign language borrowings",
                    "Advanced morphological structures",
                    "Multiple derivational layers"
                }
            },
            _ => new SpellingRulesDto
            {
                AgeGroup = $"Adult/College (Age {age})",
                Age = age,
                ComplexityLevel = "Expert+",
                WordLength = "Any length",
                FocusAreas = new List<string> { "Professional vocabulary", "Technical and specialized terms", "All spelling patterns", "Domain-specific terminology" },
                RecommendedWordTypes = new List<string>(),
                AvoidPatterns = new List<string>(),
                TeachingStrategies = new List<string> { "Context-specific instruction", "Professional development focus" },
                MaxWordCount = 20,
                RecommendedDifficulty = "hard",
                AllowedLetterPatterns = new List<string>()
            }
        };
    }

    private class SpellingRulesDto
    {
        public string AgeGroup { get; set; } = string.Empty;
        public int Age { get; set; }
        public string ComplexityLevel { get; set; } = string.Empty;
        public string WordLength { get; set; } = string.Empty;
        public List<string> FocusAreas { get; set; } = new();
        public List<string> RecommendedWordTypes { get; set; } = new();
        public List<string> AvoidPatterns { get; set; } = new();
        public List<string> TeachingStrategies { get; set; } = new();
        public int MaxWordCount { get; set; }
        public string RecommendedDifficulty { get; set; } = string.Empty;
        public List<string> AllowedLetterPatterns { get; set; } = new();
    }
}

