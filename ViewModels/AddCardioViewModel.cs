
using Microcharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkoutDiary.data;
using WorkoutDiary.Model;

namespace WorkoutDiary.ViewModels
{
    public class AddCardioViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        readonly CardioDataBase _database;
        private Cardio _cardio;
        private string _name;
        private float? _distance;
        private string _comment;
        private float? _time;
        private int? _calories;
        private DateTime? _datetime;
        private DateTime _datetimeEntr;
        private string _namePicker;
        private int _datePickerNumberWeek;
        private int _numberweek;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public float? Distance
        {
            get => _distance;
            set
            {
                _distance = value;
                OnPropertyChanged(nameof(Distance));
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
        public float? Time
        {
            get => _time;
            set
            {
                _time = value;
                OnPropertyChanged(nameof(Time));
            }
        }
        public int? Calories
        {
            get => _calories;
            set
            {
                _calories = value;
                OnPropertyChanged(nameof(Calories));
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
        public int NumberWeek
        {
            get => _numberweek;
            set
            {
                _numberweek = value;
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
        public int DatePickerNumberWeek
        {
            get => _datePickerNumberWeek;
            set
            {
                _datePickerNumberWeek = value;
                OnPropertyChanged(nameof(DatePickerNumberWeek));
            }
        }
        public string NamePicker
        {
            get => _namePicker;
            set
            {
                _namePicker = value;
                OnPropertyChanged(nameof(NamePicker));
            }
        }
        public ICommand SaveCommand { get; }
        public ICommand ButtonInfo { get; }
        public AddCardioViewModel(CardioDataBase database)
        {
            _database = database;
            SaveCommand = new Command(async () => await Save());
            ButtonInfo = new Command(async () => await ButtonInfoCommand());
            DatetimeEntry = DateTime.Today;
            LoadNumberWeek();
        }
        private async Task ButtonInfoCommand()
        {
            await Application.Current.MainPage.DisplayAlert("Informacja", "Dodane ćwiczenia są zapisywane, a przy kolejnym trenigu należy wybrać ćwiczenie z listy. Dzięki temu progress danego ćwieczenia można sprawdzić w oknie 'statystyka' ", "Ok!");
        }
        private async Task Save()
        {


            if (_cardio != null && _cardio.Id != 0)
            {
                var existingCardio = await _database.GetCardioIDAsync(_cardio.Id);

                if (existingCardio != null)
                {
                    existingCardio.Name = Name;
                    existingCardio.Comment = Comment;
                    existingCardio.Calories = (int)Calories;
                    existingCardio.Distance = (float)Distance;
                    existingCardio.Time = (float)Time;
                    existingCardio.DateTime = Datetime ?? DateTime.Today;
                    if (Calories == null)
                        existingCardio.Calories = 0;
                    if (Distance == null)
                        existingCardio.Distance = 0;
                    if (Time == null)
                        existingCardio.Time = 0;

                    await _database.UpdateCardioAsync(existingCardio);
                }
            }
            else
            {
                if (Calories == null)
                    Calories = 0;
                if (Distance == null)
                    Distance = 0;
                if (Time == null)
                    Time = 0;

                var newCardio = new Cardio()
                {
                    Id = (await _database.GetCardioAsync()).Count + 1, // lub zastosuj logikę generowania ID
                    Name = Name ?? NamePicker,
                    Comment = Comment,
                    Calories = (int)Calories,
                    Distance = (float)Distance,
                    Time = (float)Time,
                    DateTime = Datetime ?? DateTime.Today,
                    NumberWeek = (DatePickerNumberWeek > 0) ? DatePickerNumberWeek : NumberWeek


                };

                await _database.SaveCardioAsync(newCardio);
            }
            await Shell.Current.GoToAsync("..");
        }

        public async void ChooseDate(DateTime date)
        {
            Datetime = date;
            DatetimeEntry = date;

            var data = await _database.GetCardioAsync();
            if (data != null && data.Any())
            {
                var db = await _database.GetCardioAsync();
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
        public async void LoadCardioExercise(int exerciseId)
        {
            _cardio = await _database.GetCardioIDAsync(exerciseId);

            if (_cardio != null)
            {
                Name = _cardio.Name;
                Comment = _cardio.Comment;
                Distance = _cardio.Distance;
                Time = _cardio.Time;
                Calories = _cardio.Calories;
                DatetimeEntry = _cardio.DateTime;

                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(Comment));
                OnPropertyChanged(nameof(Time));
                OnPropertyChanged(nameof(Calories));
                OnPropertyChanged(nameof(DatetimeEntry));
            }


        }
        private async void LoadNumberWeek()
        {

            var data = await _database.GetCardioAsync();
            if (data != null && data.Any())
            {
                var db = await _database.GetCardioAsync();
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




        }
        public void loadPicker(string selectedPart)
        {
            NamePicker = selectedPart;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("exerciseId") && int.TryParse(query["exerciseId"].ToString(), out int exerciseId))
            {
                LoadCardioExercise(exerciseId);
            }
        }
    }
}
