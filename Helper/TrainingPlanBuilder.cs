using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutDiary.data;
using WorkoutDiary.Model;

namespace WorkoutDiary.Helper
{
    public static class TrainingPlanBuilder
    {
        public static List<TrainingDay> Build(List<List<string>> rawSplit)
        {
            var result = new List<TrainingDay>();

            foreach (var dayParts in rawSplit)
            {
                var trainingDay = new TrainingDay
                {
                    Parts = new List<ExercisePart>()
                };

                foreach (var part in dayParts)
                {
                    trainingDay.Parts.Add(new ExercisePart
                    {
                        Part = part,
                        Exercises = ExerciseDatabase.Exercises[part]
                    });
                }

                result.Add(trainingDay);
            }

            return result;
        }
    }
}
