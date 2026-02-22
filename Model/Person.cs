using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutDiary.Model
{
    public class Person
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public float Growth { get; set; }
        public float Weight { get; set; }
        public int Age { get; set; }
        public int Pulse { get; set; }
        public double IndexBmi { get; set; }
        public string RateBmi { get; set; }
        public string Notes { get; set; }

    }
}
