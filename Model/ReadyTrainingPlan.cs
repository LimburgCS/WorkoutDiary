using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutDiary.Helper;

namespace WorkoutDiary.Model
{
    public class ReadyTrainingPlan
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int DaysPerWeek { get; set; }
        public List<TrainingDay> WeeklySplit { get; set; }

        public static List<ReadyTrainingPlan> DefaultPlans => new()
    {
        new ReadyTrainingPlan
        {
            Id = 2,
            Name = "Plan 2 dni – Full Body",
            Description = "Trening całego ciała dwa razy w tygodniu. Idealny dla początkujących.",
            DaysPerWeek = 2,
            WeeklySplit = TrainingPlanBuilder.Build(new()
    {
        new() { "klata", "plecy", "nogi", "barki", "biceps", "triceps", "brzuch" },
        new() { "plecy", "klata", "nogi", "barki", "triceps", "biceps", "przedramiona" }
    })

        },

        new ReadyTrainingPlan
        {
            Id = 3,
            Name = "Plan 3 dni – Push/Pull/Legs",
            Description = "Najpopularniejszy plan dla większości osób.",
            DaysPerWeek = 3,
            WeeklySplit = TrainingPlanBuilder.Build(new()
            {
                new() { "klata", "barki", "triceps" },          // PUSH
                new() { "plecy", "biceps", "przedramiona" },    // PULL
                new() { "nogi", "brzuch" }                      // LEGS
            })
        },


        new ReadyTrainingPlan
        {
            Id = 4,
            Name = "Plan 4 dni – Upper/Lower",
            Description = "Bardzo efektywny plan dla osób średniozaawansowanych.",
            DaysPerWeek = 4,
            WeeklySplit = TrainingPlanBuilder.Build(new()
    {
        new() { "klata", "plecy", "barki", "biceps", "triceps" },
        new() { "nogi", "brzuch" },
        new() { "plecy", "klata", "barki", "triceps", "przedramiona" },
        new() { "nogi", "brzuch" }
    })

        },

        new ReadyTrainingPlan
        {
            Id = 5,
            Name = "Plan 5 dni – PPL + Upper/Lower",
            Description = "Duża objętość, szybki progres. Dla zaawansowanych.",
            DaysPerWeek = 5,
            WeeklySplit = TrainingPlanBuilder.Build(new()
    {
        new() { "klata", "barki", "triceps" },
        new() { "plecy", "biceps", "przedramiona" },
        new() { "nogi", "brzuch" },
        new() { "klata", "plecy", "barki", "biceps", "triceps" },
        new() { "nogi", "brzuch" }
    })

        },

        new ReadyTrainingPlan
        {
            Id = 6,
            Name = "Plan 6 dni – PPL × 2",
            Description = "Najbardziej intensywny plan. Wymaga dobrej regeneracji.",
            DaysPerWeek = 6,
    WeeklySplit = TrainingPlanBuilder.Build(new()
    {
        new() { "klata", "barki", "triceps" },
        new() { "plecy", "biceps", "przedramiona" },
        new() { "nogi", "brzuch" },
        new() { "klata", "barki", "triceps" },
        new() { "plecy", "biceps", "przedramiona" },
        new() { "nogi", "brzuch" }
    })


        }
    };



    }
}