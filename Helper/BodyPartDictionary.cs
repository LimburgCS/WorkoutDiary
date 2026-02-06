using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutDiary.Helper
{
    public static class BodyPartDictionary
    {
        public static readonly Dictionary<string, string> AllowedParts = new()
        {
            { "klata", "klata" },
            { "klatka", "klata" },
            { "barki", "barki" },
            { "ramiona", "barki" },
            { "plecy", "plecy" },
            { "nogi", "nogi" },
            { "biceps", "biceps" },
            { "triceps", "triceps" },
            { "przedramiona", "przedramiona" },
            { "brzuch", "brzuch" },
            { "dipy", "triceps" }

        };
    }

}
