using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutDiary.Model
{
    public class TrainingPlan
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string MondayParts { get; set; }
        public string TuesdayParts { get; set; }
        public string WednesdayParts { get; set; }
        public string ThursdayParts { get; set; }
        public string FridayParts { get; set; }
        public string SaturdayParts { get; set; }
        public string SundayParts { get; set; }

        public string Day { get; set; }

    }
}
