using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkoutDiary.data;
using WorkoutDiary.Model;

namespace WorkoutDiary.ViewModels
{
    public class AddExerciseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        readonly TodoItemDatabase _database;
        private BodyParts _bodyPart;
        private string _part;
        private string _partPicker;
        private int? _series;
        private int? _repetitions;
        private float? _weight;
        private int _numberweek;
        public string Part
        {
            get => _part;
            set
            {
                _part = value;
                OnPropertyChanged(nameof(Part));
            }
        }
        public string PartPicker
        {
            get => _partPicker;
            set
            {
                _partPicker = value;
                OnPropertyChanged(nameof(_partPicker));
            }
        }
        public int? Series
        {
            get => _series;
            set
            {
                _series = value;
                OnPropertyChanged(nameof(Series));
            }
        }
        public int? Repetitions
        {
            get => _repetitions;
            set
            {
                _repetitions = value;
                OnPropertyChanged(nameof(Repetitions));
            }
        }
        public float? Weight
        {
            get => _weight;
            set
            {
                _weight = value;
                OnPropertyChanged(nameof(Weight));
            }
        }

        public int NumberWeek
        {
            get => _numberweek;
            set
            {
                _numberweek = value;
            }
        }
        public ICommand SaveCommand { get; }
        public AddExerciseViewModel(TodoItemDatabase database)
        {
            _database = database;
            SaveCommand = new Command(async () => await Save());
            LoadNumberWeek();
        }


        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("exerciseId") && int.TryParse(query["exerciseId"].ToString(), out int exerciseId))
            {
                LoadExercise(exerciseId);
            }
        }
        public async void LoadExercise(int exerciseId)
        {
            _bodyPart = await _database.GetInvoiceIDAsync(exerciseId); 

            if (_bodyPart != null)
            {
                Part = _bodyPart.Part;
                Series = _bodyPart.Series;
                Repetitions = _bodyPart.Repetitions;
                Weight = _bodyPart.Weight;

                OnPropertyChanged(nameof(Series));
                OnPropertyChanged(nameof(Repetitions));
                OnPropertyChanged(nameof(Weight));
            }
        }


        private async Task Save()
        {
            if (_bodyPart != null && _bodyPart.Id != 0)
            {
                var existingBodyPart = await _database.GetInvoiceIDAsync(_bodyPart.Id);

                if (existingBodyPart != null)
                {
                    existingBodyPart.Part = Part;
                    existingBodyPart.Series = (int)Series;
                    existingBodyPart.Repetitions = (int)Repetitions;
                    existingBodyPart.Weight = (float)Weight;

                    await _database.UpdateInvoiceAsync(existingBodyPart);
                }
            }
            else
            {
                // Tworzenie nowej faktury
                var newExercise = new BodyParts()
                {
                    Id = (await _database.GetInvoiceAsync()).Count + 1, // lub zastosuj logikę generowania ID
                    Part = Part ?? PartPicker,
                    Series = (int)Series,
                    Repetitions = (int)Repetitions,
                    Weight = (float)Weight,
                    DateTime = DateTime.Today,
                    NumberWeek = NumberWeek,


                };

                await _database.SaveInvoiceAsync(newExercise);
            }
            await Shell.Current.GoToAsync("..");
        }
        private async void LoadNumberWeek()
        {

            var data = await _database.GetInvoiceAsync();
            if (data != null && data.Any()) 
            {
                var db = await _database.GetInvoiceAsync();
                var firstweek = db.FirstOrDefault();
            int currentNumberWeek = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(
            DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            int firstWorkNumberWeek = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(
                                firstweek.DateTime, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            int weekDisplay = currentNumberWeek - firstWorkNumberWeek;

            NumberWeek = weekDisplay + 1;
            }
            else
            {
                NumberWeek = 1;
            }
            //if (weekDisplay == 0)
            //{
            //    NumberWeek = ;
            //}
            //else if (weekDisplay == 1)
            //{
            //    NumberWeek = 2;
            //}
            //else if (weekDisplay > 2)
            //{
            //    NumberWeek = weekDisplay;
            //}




        }
        public void loadPicker(string selectedPart)
        {
            PartPicker = selectedPart;
        }

    }
}
