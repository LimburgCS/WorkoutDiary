using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutDiary.data
{
    public static class ExerciseDatabase
    {
        public static readonly Dictionary<string, List<string>> Exercises = new()
        {
            ["klata"] = new()
        {
            "Wyciskanie sztangi na ławce poziomej – 4x6-8",
            "Rozpiętki hantlami na ławce skośnej – 3x10-12"
        },

            ["plecy"] = new()
        {
            "Podciąganie nachwytem – 4x6-10",
            "Wiosłowanie na wyciągu siedząc (uchwyt V) – 4x8-10"
        },

            ["nogi"] = new()
        {
            "Przysiady ze sztangą – 4x6-8",
            "Martwy ciąg rumuński – 3x8-10"
        },

            ["barki"] = new()
        {
            "Wyciskanie hantli nad głowę – 4x6-8",
            "Unoszenie hantli bokiem – 3x12-15"
        },

           ["biceps"] = new()
        {
            "Uginanie ramion ze sztangą stojąc – 3x8-12",
            "Uginanie hantli z supinacją – 3x10-12"
        },

            ["triceps"] = new()
        {
            "Wyciskanie francuskie leżąc – 3x8-12",
            "Prostowanie ramion na wyciągu – 3x10-15"
        },

            ["brzuch"] = new()
        {
            "Plank – 3x30-60 sek",
            "Unoszenie nóg w zwisie – 3x10-15"
        },

            ["przedramiona"] = new()
        {
            "Uginanie nadgarstków podchwytem – 3x12-15",
            "Farmer walk – 3x30-40 m / 30-45 sek"
        }
        };
    }
}
