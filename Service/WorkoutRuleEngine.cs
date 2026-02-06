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

        private int GetPriority(string part, List<string> dayBeforeYesterday)
        {
            var basePriority = Priority.TryGetValue(part, out var p) ? p : 1;

            // ⭐ jeśli partia była trenowana przedwczoraj → podbij priorytet
            if (dayBeforeYesterday.Contains(part))
                basePriority += 2;

            return basePriority;
        }


        public List<string> Recommend(List<string> allParts, List<string> dayBeforeYesterday, List<string> yesterday)
        {
            allParts = Normalize(allParts);
            dayBeforeYesterday = Normalize(dayBeforeYesterday);
            yesterday = Normalize(yesterday);

            // 1. Brak danych → wybierz 2 największe partie z historii
            if (!dayBeforeYesterday.Any() && !yesterday.Any())
            {
                return allParts
                    .OrderByDescending(p => GetPriority(p, dayBeforeYesterday))
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
               .OrderByDescending(p => GetPriority(p, dayBeforeYesterday))
                .ToList();

            // 4. Jeśli są pominięte → wybierz 2 najważniejsze
            if (missing.Count >= 2)
                return missing.Take(3).ToList();

            if (missing.Count == 1)
            {
                var rotation1 = allParts
                    .Where(p => !yesterday.Contains(p) && p != missing[0])
                    .OrderByDescending(p => GetPriority(p, dayBeforeYesterday))
                    .Take(1)
                    .ToList();

                return missing.Concat(rotation1).ToList();
            }


            // 5. Jeśli trenowałeś wszystko → rotacja (wybierz 2, których nie było wczoraj)
            var rotation = allParts
                .Where(p => !yesterday.Contains(p))
                .OrderByDescending(p => GetPriority(p, dayBeforeYesterday))
                .Take(2)
                .ToList();

            if (rotation.Any())
                return rotation;

            // 6. Ostateczność: wybierz 2 z dnia sprzed wczoraj
            return dayBeforeYesterday
                .OrderByDescending(p => GetPriority(p, dayBeforeYesterday))
                .Take(2)
                .ToList();
        }

        private List<string> Normalize(List<string> parts)
        {
            return parts
                .Select(p =>
                {
                    if (string.IsNullOrWhiteSpace(p))
                        return p;

                    var lower = p.ToLowerInvariant();

                    if (lower.Contains("dipy") || lower.Contains("dip") || lower.Contains("dips"))
                        return "triceps";

                    return p;
                })
                .ToList();
        }

    }


}
