using System.ComponentModel;
using ModelContextProtocol.Server;

namespace GameBuddy.MCP.Resources;

[McpServerResourceType]
public class CapitalGameAdaptiveDifficultyResource
{
    [McpServerResource(UriTemplate = "capital-game://difficulty-strategy")]
    [Description("Provides simple adaptive difficulty progression strategy for the capital game - every 2 consecutive correct answers unlocks the next level and next set of countries")]
    public Task<string> GetAdaptiveDifficultyStrategy()
    {
        return Task.FromResult(@"CAPITAL GAME ADAPTIVE DIFFICULTY STRATEGY
═══════════════════════════════════════════════════════

SIMPLE PROGRESSION RULE:
━━━━━━━━━━━━━━━━━━━━━━━
Every 2 consecutive correct questions = Next Level + Next Set of Countries

DIFFICULTY LEVELS WITH COMPLETE COUNTRY LISTS:
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

Level 1: Well Known Countries (40 countries)
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
United States → Washington, D.C.
United Kingdom → London
France → Paris
Germany → Berlin
Italy → Rome
Spain → Madrid
Canada → Ottawa
Australia → Canberra
Japan → Tokyo
China → Beijing
India → New Delhi
Brazil → Brasília
Russia → Moscow
Mexico → Mexico City
South Korea → Seoul
Argentina → Buenos Aires
Egypt → Cairo
South Africa → Pretoria
Turkey → Ankara
Greece → Athens
Netherlands → Amsterdam
Switzerland → Bern
Sweden → Stockholm
Norway → Oslo
Denmark → Copenhagen
Belgium → Brussels
Austria → Vienna
Poland → Warsaw
Portugal → Lisbon
Ireland → Dublin
Finland → Helsinki
New Zealand → Wellington
Thailand → Bangkok
Indonesia → Jakarta
Saudi Arabia → Riyadh
Israel → Jerusalem
Singapore → Singapore
United Arab Emirates → Abu Dhabi
Chile → Santiago
Colombia → Bogotá

Level 2: Moderately Known Countries (70 countries)
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
Czech Republic → Prague
Hungary → Budapest
Romania → Bucharest
Ukraine → Kyiv
Philippines → Manila
Malaysia → Kuala Lumpur
Vietnam → Hanoi
Pakistan → Islamabad
Bangladesh → Dhaka
Iran → Tehran
Iraq → Baghdad
Peru → Lima
Venezuela → Caracas
Cuba → Havana
Morocco → Rabat
Algeria → Algiers
Tunisia → Tunis
Libya → Tripoli
Kenya → Nairobi
Nigeria → Abuja
Ethiopia → Addis Ababa
Tanzania → Dodoma
Ghana → Accra
Uganda → Kampala
Zimbabwe → Harare
Angola → Luanda
Sudan → Khartoum
Senegal → Dakar
Cameroon → Yaoundé
Ivory Coast → Yamoussoukro
Afghanistan → Kabul
Kazakhstan → Astana
Mongolia → Ulaanbaatar
Nepal → Kathmandu
Sri Lanka → Sri Jayawardenepura Kotte
Myanmar → Naypyidaw
Cambodia → Phnom Penh
Laos → Vientiane
Qatar → Doha
Kuwait → Kuwait City
Bahrain → Manama
Oman → Muscat
Yemen → Sana'a
Jordan → Amman
Lebanon → Beirut
Syria → Damascus
Azerbaijan → Baku
Georgia → Tbilisi
Armenia → Yerevan
Uzbekistan → Tashkent
Costa Rica → San José
Panama → Panama City
Guatemala → Guatemala City
Honduras → Tegucigalpa
El Salvador → San Salvador
Nicaragua → Managua
Jamaica → Kingston
Dominican Republic → Santo Domingo
Haiti → Port-au-Prince
Trinidad and Tobago → Port of Spain
Ecuador → Quito
Bolivia → La Paz
Paraguay → Asunción
Uruguay → Montevideo
Bulgaria → Sofia
Serbia → Belgrade
Croatia → Zagreb
Slovenia → Ljubljana
Slovakia → Bratislava
Albania → Tirana
North Macedonia → Skopje
Bosnia and Herzegovina → Sarajevo

Level 3: Lesser Known Countries (85 countries)
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
Luxembourg → Luxembourg City
Malta → Valletta
Iceland → Reykjavik
Estonia → Tallinn
Latvia → Riga
Lithuania → Vilnius
Moldova → Chișinău
Belarus → Minsk
Montenegro → Podgorica
Kosovo → Pristina
Cyprus → Nicosia
Liechtenstein → Vaduz
Andorra → Andorra la Vella
Monaco → Monaco
San Marino → San Marino
Vatican City → Vatican City
Tajikistan → Dushanbe
Turkmenistan → Ashgabat
Kyrgyzstan → Bishkek
Bhutan → Thimphu
Maldives → Malé
Brunei → Bandar Seri Begawan
Timor-Leste → Dili
North Korea → Pyongyang
Papua New Guinea → Port Moresby
Fiji → Suva
Solomon Islands → Honiara
Vanuatu → Port Vila
Samoa → Apia
Tonga → Nuku'alofa
Kiribati → Tarawa
Micronesia → Palikir
Marshall Islands → Majuro
Palau → Ngerulmud
Nauru → Yaren
Tuvalu → Funafuti
Mauritius → Port Louis
Seychelles → Victoria
Comoros → Moroni
Madagascar → Antananarivo
Djibouti → Djibouti City
Eritrea → Asmara
Somalia → Mogadishu
Mozambique → Maputo
Malawi → Lilongwe
Zambia → Lusaka
Botswana → Gaborone
Namibia → Windhoek
Lesotho → Maseru
Eswatini → Mbabane
Rwanda → Kigali
Burundi → Gitega
Central African Republic → Bangui
Chad → N'Djamena
Niger → Niamey
Mali → Bamako
Burkina Faso → Ouagadougou
Guinea → Conakry
Guinea-Bissau → Bissau
Sierra Leone → Freetown
Liberia → Monrovia
Gambia → Banjul
Mauritania → Nouakchott
Cape Verde → Praia
São Tomé and Príncipe → São Tomé
Equatorial Guinea → Malabo
Gabon → Libreville
Republic of the Congo → Brazzaville
Democratic Republic of the Congo → Kinshasa
South Sudan → Juba
Benin → Porto-Novo
Togo → Lomé
Suriname → Paramaribo
Guyana → Georgetown
Belize → Belmopan
Barbados → Bridgetown
Bahamas → Nassau
Saint Lucia → Castries
Grenada → St. George's
Saint Vincent and the Grenadines → Kingstown
Antigua and Barbuda → St. John's
Dominica → Roseau
Saint Kitts and Nevis → Basseterre

PROGRESSION LOGIC:
━━━━━━━━━━━━━━━━

PATH 1: DEFAULT PROGRESSION (No Preferred Difficulty)
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
✓ Start: Child begins at Level 1 (Well Known)
✓ After 2 Correct Answers in a row → Unlock Level 2 (Moderately Known)
✓ After 2 More Correct Answers in a row at Level 2 → Unlock Level 3 (Lesser Known)
✗ Incorrect Answer → Reset streak counter, stay at current level
⤵ If struggling (multiple incorrect at same level) → Offer to restart at Level 1

PATH 2: PREFERRED DIFFICULTY START (Child Has Preference)
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
If child's profile specifies a preferred difficulty level, start there:
  • ""easy"" → Start at Level 1 (Well Known)
  • ""medium"" → Start at Level 2 (Moderately Known)
  • ""hard"" → Start at Level 3 (Lesser Known)

Once started at preferred level, same progression rules apply:
✓ After 2 Correct Answers in a row → Unlock next level (if available)
✗ Incorrect Answer → Reset streak counter, stay at current level
⤵ Can move up from preferred level, but not forced to start lower unless performance requires it

EXAMPLE SCENARIOS:
━━━━━━━━━━━━━━━━
Scenario A (Default): 
  Child with no preference → Start Level 1 → 2 correct → Level 2 → 2 correct → Level 3

Scenario B (Preferred = ""medium""): 
  Child prefers medium → Start Level 2 → 2 correct → Level 3 (skip Level 1 entirely)

Scenario C (Preferred = ""hard""): 
  Child prefers hard → Start Level 3 (already at highest, stays there)

COUNTRY SELECTION STRATEGY:
━━━━━━━━━━━━━━━━━━━━━━━━━
1. **Avoid Mastered Countries**: For countries where child has >80% success rate, reduce selection frequency by 70%
2. **Prioritize Learning Opportunities**: Focus on countries with <80% success or countries not yet tested
3. **Variety Within Level**: Rotate through available countries at current level to maintain engagement
4. **Performance-Based Exclusion**: If child has answered a specific country correctly 3+ times consecutively, skip it for next 5 sessions
5. **Reset After Level Up**: When advancing to new level, all countries in that level are fresh opportunities regardless of past performance

IMPLEMENTATION:
━━━━━━━━━━━━━━
1. Track consecutive correct answers per session
2. When count reaches 2 → Increment level
3. Reset counter to 0 on any incorrect answer
4. Check child's historical performance data before selecting countries
   - Fetch progress history to determine success rates and streaks for each country
5. Filter out countries with >80% success rate or recent perfect streaks
6. Display current level and available countries to AI Agent
7. Provide encouraging feedback when level changes:
   - Example: ""Great job! You're moving to the next level. The next question will be a bit more challenging, but I know you can do it!""
8. Safe progression ensures child always learns before advancing");
    }
}
