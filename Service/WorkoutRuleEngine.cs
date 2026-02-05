using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutDiary.data;

namespace WorkoutDiary.Service
{
    public class WorkoutRuleEngine
    {
        private static readonly Dictionary<string, int> Priority = new()
    {
        { "klata", 3 },
        { "plecy", 3 },
        { "nogi", 3 },
        { "barki", 2 },
        { "triceps", 1 },
        { "biceps", 1 },
        { "przedramiona", 1 },
        { "brzuch", 1 }
    };

        private int GetPriority(string part)
            => Priority.TryGetValue(part, out var p) ? p : 1; // domyślny priorytet

        public List<string> Recommend(List<string> allParts, List<string> dayBeforeYesterday, List<string> yesterday)
        {
            // 1. Brak danych → wybierz 2 największe partie z historii
            if (!dayBeforeYesterday.Any() && !yesterday.Any())
            {
                return allParts
                    .OrderByDescending(GetPriority)
                    .Take(2)
                    .ToList();
            }

            // 2. Zbierz trenowane partie
            var trained = dayBeforeYesterday
                .Concat(yesterday)
                .Distinct()
                .ToList();

            // 3. Partie, których NIE trenowałeś
            var missing = allParts
                .Where(p => !trained.Contains(p))
                .OrderByDescending(GetPriority)
                .ToList();

            // 4. Jeśli są pominięte → wybierz 2 najważniejsze
            if (missing.Any())
                return missing.Take(3).ToList();

            // 5. Jeśli trenowałeś wszystko → rotacja (wybierz 2, których nie było wczoraj)
            var rotation = allParts
                .Where(p => !yesterday.Contains(p))
                .OrderByDescending(GetPriority)
                .Take(2)
                .ToList();

            if (rotation.Any())
                return rotation;

            // 6. Ostateczność: wybierz 2 z dnia sprzed wczoraj
            return dayBeforeYesterday
                .OrderByDescending(GetPriority)
                .Take(2)
                .ToList();
        }

    }


}
