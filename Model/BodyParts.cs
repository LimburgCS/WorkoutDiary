using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutDiary.Model
{
    public class BodyParts
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public int NumberWeek {  get; set; }
        public string Part { get; set; }
        public string Comment { get; set; }
        public int Series {  get; set; }
        public int Repetitions { get; set; }
        public float Weight { get; set; }

    }
}
