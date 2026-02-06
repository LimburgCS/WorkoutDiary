using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutDiary.Helper
{
    public static class BodyPartParser
    {
        public static string ExtractMainPart(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return "unknown";

            raw = raw.Trim().ToLowerInvariant();

            var tokens = raw.Split(new[] { ' ', '-', '–', '_' }, StringSplitOptions.RemoveEmptyEntries);

            if (tokens.Length == 0)
                return "unknown";

            var first = tokens[0];

            if (first.Contains("dipy", StringComparison.OrdinalIgnoreCase))
                return "triceps";


            return BodyPartDictionary.AllowedParts.TryGetValue(first, out var normalized)
                ? normalized
                : "unknown";
        }
    }

}
