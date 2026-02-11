using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkoutDiary.data;
using WorkoutDiary.Model;
using WorkoutDiary.Service;
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
        public ObservableCollection<string> TitleGym { get; set; } = new();
        private List<string> _allParts;
        public TodoItemDatabase _database;
        private BodyParts _bodyPart;
        private string _pageTitle;
        private string _part;
        private string _partPicker;
        private string _gym;
        private string _gymPicker;
        private DateTime? _datetime;
        private DateTime _datetimeEntr;
        private int? _series;
        private int? _repetitions;
        private float? _weight;
        private int _numberweek;
        private int _datePickerNumberWeek;
        private string _comment;

        public string PageTitle
        {
            get => _pageTitle;
            set
            {
                _pageTitle = value;
                OnPropertyChanged(nameof(PageTitle));
            }
        }
        private string _selectedGym;
        public string SelectedGym // wybieranie siłowni
        {
            get => _selectedGym;
            set
            {
                if (_selectedGym != value)
                {
                    _selectedGym = value;
                    OnPropertyChanged(nameof(_selectedGym));
                   // Preferences.Set("_selectedGym", value);
                    _ = FindNameGymPlace();
                }
            }
        }
        public string Part
        {
            get => _part;
            set
            {
                _part = value;
                OnPropertyChanged(nameof(Part));
                ValidatePart(value);
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
        public string Gym //dodawanie siłowni 
        {
            get => _gym;
            set
            {
                _gym = value;
                OnPropertyChanged(nameof(Gym));
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
        public string GymPicker
        {
            get => _gymPicker;
            set
            {
                _gymPicker = value;
                OnPropertyChanged(nameof(_gymPicker));
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

        private bool _isPartDuplicate;
        public bool IsPartDuplicate
        {
            get => _isPartDuplicate;
            set
            {
                if (_isPartDuplicate != value)
                {
                    _isPartDuplicate = value;
                    OnPropertyChanged(nameof(IsPartDuplicate));
                }
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
            _ = LoadNameGymPlace();
            LoadPartsAsync();
        }

        private async Task ButtonInfoCommand()
        {
            await Application.Current.MainPage.DisplayAlert("Informacja", "Dodane ćwiczenia są zapisywane, a przy kolejnym trenigu należy wybrać ćwiczenie z listy. Dzięki temu progress danej partii można sprawdzić w oknie 'statystyka' ", "Ok!");
        }

        private async void LoadPartsAsync()
        {
            var db = await _database.GetInvoiceAsync();

            _allParts = db
                .Select(x => x.Part)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
        }


        public async Task LoadExercise(int exerciseId)
        {
            _ = LoadNameGymPlace();
            _bodyPart = await _database.GetInvoiceIDAsync(exerciseId);

            if (_bodyPart != null)
            {
                
                Part = _bodyPart.Part;
                GymPicker = _bodyPart.NameGym;
                Comment = _bodyPart.Comment;
                Series = _bodyPart.Series;
                Repetitions = _bodyPart.Repetitions;
                Weight = _bodyPart.Weight;
                DatetimeEntry = _bodyPart.DateTime;

                OnPropertyChanged(nameof(Series));
                OnPropertyChanged(nameof(GymPicker));
                OnPropertyChanged(nameof(Repetitions));
                OnPropertyChanged(nameof(Weight));
                OnPropertyChanged(nameof(DatetimeEntry));
            }
        }

        public async void ChooseDate(DateTime date)
        {
            DatetimeEntry = date;
            Datetime = date;
            var data = await _database.GetInvoiceAsync();
            if (data != null && data.Any())
            {
                var db = await _database.GetInvoiceAsync();
                //var firstweek = db.FirstOrDefault();
                //int FirstNumberWeek = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(
                //firstweek.DateTime, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

                //int datePickerNumberWeek = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(
                //                date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

                ////int FirstNumberWeek = 1;
                //int reduceNumberWeek = (datePickerNumberWeek - FirstNumberWeek) + 1;
                //DatePickerNumberWeek = reduceNumberWeek;

                var first = db.First().DateTime.Date;
                var now = date.Date;

                // znajdź poniedziałek tygodnia startowego
                var startWeekMonday = first.AddDays(-(int)first.DayOfWeek + (int)DayOfWeek.Monday);

                // znajdź poniedziałek tygodnia aktualnej daty
                var nowWeekMonday = now.AddDays(-(int)now.DayOfWeek + (int)DayOfWeek.Monday);

                // różnica tygodni (ciągła, bez resetu w nowym roku)
                int weekNumber = ((nowWeekMonday - startWeekMonday).Days / 7) + 1;

                DatePickerNumberWeek = weekNumber;


            }
            else
            {
                DatePickerNumberWeek = 1;
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
                    existingBodyPart.NameGym = GymPicker;
                    existingBodyPart.Comment = Comment;
                    existingBodyPart.Series = (int)Series;
                    existingBodyPart.NumberWeek = (DatePickerNumberWeek > 0) ? DatePickerNumberWeek : NumberWeek;
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

                if (TitleGym.Count < 1 && Gym == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Błąd", "Proszę wpisać nazwę siłowni", "OK");
                    return;
                }

                var newExercise = new BodyParts()
                {
                    Id = (await _database.GetInvoiceAsync()).Count + 1, // lub zastosuj logikę generowania ID
                    Part = Part ?? PartPicker,
                    NameGym = Gym ?? GymPicker, // Gym ma pierwszenstwo
                    Comment = Comment,
                    Series = (int)Series,
                    Repetitions = (int)Repetitions,
                    Weight = (float)Weight,
                    DateTime = Datetime ?? DateTime.Today,
                    NumberWeek = (DatePickerNumberWeek > 0) ? DatePickerNumberWeek : NumberWeek


                };
                SettingsService.SelectedGym = Gym ?? GymPicker;
                await _database.SaveInvoiceAsync(newExercise);
            }

            await Shell.Current.GoToAsync("..");
        }
        private async void LoadNumberWeek()
        {
            var date = DateTime.Today;
            var data = await _database.GetInvoiceAsync();
            if (data != null && data.Any())
            {
                var db = await _database.GetInvoiceAsync();
                //var firstweek = db.FirstOrDefault();
                //int currentNumberWeek = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(
                //DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                //int firstWorkNumberWeek = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(
                //                    firstweek.DateTime, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

                //int weekDisplay = currentNumberWeek - firstWorkNumberWeek;
                //NumberWeek = weekDisplay + 1;
                var first = db.FirstOrDefault().DateTime.Date;
                var now = date.Date;

                //// Różnica dni
                var difference = (now - first).TotalDays;

                //// Ile pełnych tygodni minęło
                int weekNumber = (int)(difference / 7d) + 1;

                DatePickerNumberWeek = weekNumber;


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
        //public void loadPickerGym(string selectedGym)
        //{
        //    GymPicker = selectedGym;

        //}
        private async Task LoadNameGymPlace()
        {
            try
            {
                var db = await _database.GetInvoiceAsync();
                var bodypartsDB = db
                    .Select(x => x.NameGym)
                    .Where(x => !string.IsNullOrWhiteSpace(x))   // ⬅️ usuwa null, "", "   "
                    .Distinct()
                    .OrderBy(x => x, StringComparer.CurrentCultureIgnoreCase)
                    .ToList();

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    TitleGym.Clear();
                    foreach (var part in bodypartsDB)
                        TitleGym.Add(part);
                });
            }
            catch (Exception ex)
            {
                // opcjonalnie obsługa błędów
                Console.WriteLine($"Błąd przy wczytywaniu siłowni: {ex.Message}");
            }
            _ = FindNameGymPlace();




        }
        private async Task FindNameGymPlace()
        {
            //// var savedGym = Preferences.Get("_pageTitle", "Wszystko");
            //var savedGym = SettingsService.SelectedGym;
            // var db = await _database.GetInvoiceAsync();

            // var bodypartsDB = db
            //     .Select(x => x.NameGym)
            //     .Where(x => !string.IsNullOrWhiteSpace(x))
            //     .Distinct()
            //     .ToList();

            // // Jeśli zapisany element istnieje w liście — ustaw go
            // if (TitleGym.Contains(savedGym))
            // {
            //     _pageTitle = savedGym;
            //     GymPicker = savedGym;
            //     SelectedGym = savedGym;
            // }
            // else
            // {
            //     _pageTitle = "nie wybrano siłowni";
            // }
            // PageTitle = _pageTitle == "nie wybrano siłowni"
            //  ? "nie wybrano siłowni" : $" {savedGym}";
            var savedGym = SettingsService.SelectedGym;   // ostatnio wybrana siłownia
            var db = await _database.GetInvoiceAsync();

            var bodypartsDB = db
                .Select(x => x.NameGym)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct()
                .ToList();

            // Jeśli nie ma żadnej siłowni
            if (bodypartsDB.Count == 0)
            {
                _pageTitle = "nie wybrano siłowni";
                PageTitle = _pageTitle;
                return;
            }

            // Jeśli użytkownik nigdy nic nie wybrał → wybierz pierwszą
            if (string.IsNullOrWhiteSpace(savedGym))
            {
                savedGym = bodypartsDB.First();
                SettingsService.SelectedGym = savedGym;   // zapisz w pamięci
            }

            // Jeśli zapisany element istnieje w liście — ustaw go
            if (bodypartsDB.Contains(savedGym))
            {
                _pageTitle = savedGym;
                GymPicker = savedGym;
                SelectedGym = savedGym;
            }
            else
            {
                // zapisany nie istnieje → wybierz pierwszą
                savedGym = bodypartsDB.First();
                SettingsService.SelectedGym = savedGym;

                _pageTitle = savedGym;
                GymPicker = savedGym;
                SelectedGym = savedGym;
            }

            PageTitle = $" {savedGym}";

        }

        private void ValidatePart(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                IsPartDuplicate = false;
                return;
            }

            IsPartDuplicate = _allParts
                .Any(x => x.Equals(value, StringComparison.OrdinalIgnoreCase));
        }




        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("exerciseId") && int.TryParse(query["exerciseId"].ToString(), out int exerciseId))
            {
                _ = LoadExercise(exerciseId);
            }
        }
    }
}
