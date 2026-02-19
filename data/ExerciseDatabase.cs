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
            "Wyciskanie sztangi na ławce poziomej",
            "Wyciskanie hantli na skosie dodatnim"
        },

            ["plecy"] = new()
        {
            "Podciąganie nachwytem",
            "Wiosłowanie sztangą"
        },

            ["nogi"] = new()
        {
            "Przysiady ze sztangą",
            "Wykroki z hantlami"
        },

            ["barki"] = new()
        {
            "Wyciskanie hantli nad głowę (OHP)",
            "Unoszenie bokiem"
        },

            ["biceps"] = new()
        {
            "Uginanie ramion ze sztangą",
            "Uginanie z hantlami naprzemiennie"
        },

            ["triceps"] = new()
        {
            "Wyciskanie francuskie sztangi",
            "Prostowanie ramion na wyciągu"
        },

            ["brzuch"] = new()
        {
            "Plank",
            "Unoszenie nóg w zwisie"
        },

            ["przedramiona"] = new()
        {
            "Uginanie nadgarstków podchwytem",
            "Farmer walk"
        }
        };
    }
}
