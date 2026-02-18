using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutDiary.Model
{
    public class ReadyTrainingPlan
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DaysPerWeek { get; set; }
        public List<List<string>> WeeklySplit
        {
            get; set;


        }
        public static List<ReadyTrainingPlan> DefaultPlans => new()
    {
        new ReadyTrainingPlan
        {
            Id = 2,
            Name = "Plan 2 dni – Full Body",
            DaysPerWeek = 2,
            WeeklySplit = new()
            {
                new() { "klata", "plecy", "nogi", "barki", "biceps", "triceps", "brzuch" },
                new() { "plecy", "klata", "nogi", "barki", "triceps", "biceps", "przedramiona" }
            }
        },

        new ReadyTrainingPlan
        {
            Id = 3,
            Name = "Plan 3 dni – Push/Pull/Legs",
            DaysPerWeek = 3,
            WeeklySplit = new()
            {
                new() { "klata", "barki", "triceps" },              // Push
                new() { "plecy", "biceps", "przedramiona" },        // Pull
                new() { "nogi", "brzuch" }                          // Legs
            }
        },

        new ReadyTrainingPlan
        {
            Id = 4,
            Name = "Plan 4 dni – Upper/Lower",
            DaysPerWeek = 4,
            WeeklySplit = new()
            {
                new() { "klata", "plecy", "barki", "biceps", "triceps" }, // Upper A
                new() { "nogi", "brzuch" },                               // Lower A
                new() { "plecy", "klata", "barki", "triceps", "przedramiona" }, // Upper B
                new() { "nogi", "brzuch" }                                // Lower B
            }
        },

        new ReadyTrainingPlan
        {
            Id = 5,
            Name = "Plan 5 dni – PPL + Upper/Lower",
            DaysPerWeek = 5,
            WeeklySplit = new()
            {
                new() { "klata", "barki", "triceps" },                    // Push
                new() { "plecy", "biceps", "przedramiona" },              // Pull
                new() { "nogi", "brzuch" },                               // Legs
                new() { "klata", "plecy", "barki", "biceps", "triceps" }, // Upper
                new() { "nogi", "brzuch" }                                // Lower
            }
        },

        new ReadyTrainingPlan
        {
            Id = 6,
            Name = "Plan 6 dni – PPL × 2",
            DaysPerWeek = 6,
            WeeklySplit = new()
            {
                new() { "klata", "barki", "triceps" },                    // Push A
                new() { "plecy", "biceps", "przedramiona" },              // Pull A
                new() { "nogi", "brzuch" },                               // Legs A
                new() { "klata", "barki", "triceps" },                    // Push B
                new() { "plecy", "biceps", "przedramiona" },              // Pull B
                new() { "nogi", "brzuch" }                                // Legs B
            }
        }
    };



    }
}