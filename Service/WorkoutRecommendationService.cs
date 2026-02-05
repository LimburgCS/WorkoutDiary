using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutDiary.data;
using WorkoutDiary.Helper;
using WorkoutDiary.Model;

namespace WorkoutDiary.Service
{
    public class WorkoutRecommendationService
    {
        private readonly TodoItemDatabase _database;
        private readonly WorkoutRuleEngine _engine;

        public WorkoutRecommendationService(TodoItemDatabase database, WorkoutRuleEngine engine)
        {
            _database = database;
            _engine = engine;
        }

        public async Task<List<List<string>>> GetLastTwoParsedTrainingsAsync()
        {
            var lastTwo = await _database.GetLastTwoTrainingDaysAsync()
                          ?? new List<List<BodyParts>>();

            var result = new List<List<string>>();

            foreach (var day in lastTwo.Where(d => d != null))
            {
                var parsed = day
                    .Where(x => x != null && !string.IsNullOrWhiteSpace(x.Part))
                    .Select(x => BodyPartParser.ExtractMainPart(x.Part))
                    .Where(p => !string.IsNullOrWhiteSpace(p) && p != "unknown")
                    .ToList();

                result.Add(parsed);

            }

            return result;
        }

        public async Task<List<string>> GetRecommendationAsync()
        {
            var lastTwoParsed = await GetLastTwoParsedTrainingsAsync();

            var lastTwoRaw = await _database.GetLastTwoTrainingDaysAsync();

            var allRecords = await _database.GetInvoiceAsync();
            var allParts = allRecords
                .Select(r => ExtractMainPart(r.Part))
                .Distinct()
                .ToList();

            if (lastTwoParsed.Count < 2)
                return new List<string> { "Za mało danych — wykonaj więcej treningów" };

            var yesterday = lastTwoParsed[0];
            var dayBeforeYesterday = lastTwoParsed[1];

            return _engine.Recommend(allParts, dayBeforeYesterday, yesterday);

        }



        public async Task<List<List<BodyParts>>> GetLastTwoTrainingDaysAsync()
        {
            return await _database.GetLastTwoTrainingDaysAsync();
        }
        string ExtractMainPart(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return "unknown";

            // rozbijamy po myślniku, plusie i spacji
            var cleaned = raw
                .Split(new[] { '-', '+', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .First()
                .Trim()
                .ToLower();

            return cleaned;
        }

    }

}
