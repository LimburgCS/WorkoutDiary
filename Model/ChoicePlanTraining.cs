using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutDiary.Model
{
    public class ChoiceTrainingPlan
    {
        public string Name { get; set; }
        public int DaysPerWeek { get; set; }
        public string Description { get; set; }
        public List<string> Split { get; set; }

        // DOMYŚLNE PLANY
        public static List<ChoiceTrainingPlan> DefaultPlans => new()
    {
        new ChoiceTrainingPlan
        {
            Name = "2 dni w tygodniu – Full Body",
            DaysPerWeek = 2,
            Description = "Trening całego ciała dwa razy w tygodniu. Idealny dla początkujących.",
            Split = new List<string>
            {
                "Full Body A",
                "Full Body B"
            }
        },

        new ChoiceTrainingPlan
        {
            Name = "3 dni w tygodniu – Push/Pull/Legs",
            DaysPerWeek = 3,
            Description = "Najpopularniejszy plan dla większości osób.",
            Split = new List<string>
            {
                "Push",
                "Pull",
                "Legs"
            }
        },

        new ChoiceTrainingPlan
        {
            Name = "4 dni w tygodniu – Upper/Lower",
            DaysPerWeek = 4,
            Description = "Bardzo efektywny plan dla osób średniozaawansowanych.",
            Split = new List<string>
            {
                "Upper A",
                "Lower A",
                "Upper B",
                "Lower B"
            }
        },

        new ChoiceTrainingPlan
        {
            Name = "5 dni w tygodniu – PPL + Upper/Lower",
            DaysPerWeek = 5,
            Description = "Duża objętość, szybki progres. Dla zaawansowanych.",
            Split = new List<string>
            {
                "Push",
                "Pull",
                "Legs",
                "Upper",
                "Lower"
            }
        },

        new ChoiceTrainingPlan
        {
            Name = "6 dni w tygodniu – PPL × 2",
            DaysPerWeek = 6,
            Description = "Najbardziej intensywny plan. Wymaga dobrej regeneracji.",
            Split = new List<string>
            {
                "Push A",
                "Pull A",
                "Legs A",
                "Push B",
                "Pull B",
                "Legs B"
            }
        }
    };
    }
}
