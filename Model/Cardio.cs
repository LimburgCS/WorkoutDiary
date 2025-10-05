using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutDiary.Model
{
    public class Cardio
    {
        [PrimaryKey, AutoIncrement]
        public int Id {  get; set; }
        public string Name { get; set; }
        public float Distance { get; set; }
        public int NumberWeek { get; set; }
        public string Comment { get; set; }
        public float Time { get; set; }
        public int Calories { get; set; }
        public DateTime DateTime { get; set; }
    }
}
