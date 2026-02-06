using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutDiary.Model
{
    public class TrainingDayView
    {
        public DateTime Date { get; set; }
        public List<BodyParts> RawParts { get; set; }
        public List<string> ParsedParts { get; set; }

        public List<string> FullParts => RawParts.Select(x => x.Part).ToList();
    }


}
