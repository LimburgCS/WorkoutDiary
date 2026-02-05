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
    public class BodyParts : INotifyPropertyChanged

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
        public string NameGym { get; set; }

        private bool _showSeparator;
        public bool ShowSeparator
        {
            get => _showSeparator;
            set
            {
                if (_showSeparator != value)
                {
                    _showSeparator = value;
                    OnPropertyChanged();
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));


    }
}
