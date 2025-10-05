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
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        private DateTime? _datetime;
        private DateTime _datetimeEntr;
        private int? _series;
        private int? _repetitions;
        private float? _weight;
        private int _numberweek;
        private int _datePickerNumberWeek;
        private string _comment;
        public string Part
        {
            get => _part;
            set
            {
                _part = value;
                OnPropertyChanged(nameof(Part));
            }
        }

        public string Comment
        {
            get => _comment;
            set
            {
                _comment = value;
                OnPropertyChanged(nameof(Comment));
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
        public int DatePickerNumberWeek
        {
            get => _datePickerNumberWeek;
            set
            {
                _datePickerNumberWeek = value;
                OnPropertyChanged(nameof(DatePickerNumberWeek));
            }
        }

        public DateTime? Datetime
        {
            get => _datetime;
            set
            {
                _datetime = value;
                OnPropertyChanged(nameof(Datetime));
            }
        }
        public DateTime DatetimeEntry
        {
            get => _datetimeEntr;
            set
            {
                _datetimeEntr = value;
                OnPropertyChanged(nameof(DatetimeEntry));
            }
        }
        public ICommand SaveCommand { get; }
        public ICommand ButtonInfo { get; }

        public AddExerciseViewModel(TodoItemDatabase database)
        {
            _database = database;
            SaveCommand = new Command(async () => await Save());
            ButtonInfo = new Command(async () => await ButtonInfoCommand());
            DatetimeEntry = DateTime.Today;
            LoadNumberWeek();
        }

        private async Task ButtonInfoCommand()
        {
            await Application.Current.MainPage.DisplayAlert("Informacja", "Dodane ćwiczenia są zapisywane, a przy kolejnym trenigu należy wybrać ćwiczenie z listy. Dzięki temu progress danej partii można sprawdzić w oknie 'statystyka' ", "Ok!");
        }


        public async void LoadExercise(int exerciseId)
        {
            _bodyPart = await _database.GetInvoiceIDAsync(exerciseId);

            if (_bodyPart != null)
            {
                Part = _bodyPart.Part;
                Comment = _bodyPart.Comment;
                Series = _bodyPart.Series;
                Repetitions = _bodyPart.Repetitions;
                Weight = _bodyPart.Weight;
                DatetimeEntry = _bodyPart.DateTime;

                OnPropertyChanged(nameof(Series));
                OnPropertyChanged(nameof(Repetitions));
                OnPropertyChanged(nameof(Weight));
                OnPropertyChanged(nameof(DatetimeEntry));
            }
        }

        public async void ChooseDate(DateTime date)
        {
            Datetime = date;
            DatetimeEntry = date;

            var data = await _database.GetInvoiceAsync();
            if (data != null && data.Any())
            {
                var db = await _database.GetInvoiceAsync();
                var firstweek = db.FirstOrDefault();
                int FirstNumberWeek = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(
                firstweek.DateTime, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

                int datePickerNumberWeek = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(
                                date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

                //int FirstNumberWeek = 1;
                int reduceNumberWeek = (datePickerNumberWeek - FirstNumberWeek) + 1;
                DatePickerNumberWeek = reduceNumberWeek;
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
                    existingBodyPart.Comment = Comment;
                    existingBodyPart.Series = (int)Series;
                    existingBodyPart.Repetitions = (int)Repetitions;
                    existingBodyPart.Weight = (float)Weight;
                    existingBodyPart.DateTime = Datetime ?? DateTime.Today;
                    if (Series == null)
                        existingBodyPart.Series = 0;
                    if (Repetitions == null)
                        existingBodyPart.Repetitions = 0;
                    if (Weight == null)
                        existingBodyPart.Weight = 0;

                    await _database.UpdateInvoiceAsync(existingBodyPart);
                }
            }
            else
            {
                if (Series == null)
                    Series = 0;
                if (Repetitions == null)
                    Repetitions = 0;
                if (Weight == null)
                    Weight = 0;

                var newExercise = new BodyParts()
                {
                    Id = (await _database.GetInvoiceAsync()).Count + 1, // lub zastosuj logikę generowania ID
                    Part = Part ?? PartPicker,
                    Comment = Comment,
                    Series = (int)Series,
                    Repetitions = (int)Repetitions,
                    Weight = (float)Weight,
                    DateTime = Datetime ?? DateTime.Today,
                    NumberWeek = (DatePickerNumberWeek > 0) ? DatePickerNumberWeek : NumberWeek


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

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("exerciseId") && int.TryParse(query["exerciseId"].ToString(), out int exerciseId))
            {
                LoadExercise(exerciseId);
            }
        }
    }
}
