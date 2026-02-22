

using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using WorkoutDiary.data;
using WorkoutDiary.Model;
using WorkoutDiary.Service;
using WorkoutDiary.Views;

namespace WorkoutDiary.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public TodoItemDatabase _database;
        private readonly PersonDataBase _NoteDb;
        private readonly System.Timers.Timer _timer;
        public ObservableCollection<string> Parts { get; set; } = new();
        public ObservableCollection<string> Gym { get; set; } = new();
        private ObservableCollection<BodyParts> _bodyParts;
        public bool HasNoData => BodyParts == null || BodyParts.Count == 0;
        private string _pageTitle;
        private string _saveNameGym;
        private bool _isBottomGridVisible;
        private float _lastWeight;
        private int _displayNumberWeek;

        private string _notes;
        private string _lastSavedNotes = string.Empty;
        private bool _isSavedVisible;

        public ObservableCollection<BodyParts> BodyParts
        {
            get => _bodyParts;
            set
            {
                _bodyParts = value;
                OnPropertyChanged();
            }
        }

        public string Notes
        {
            get => _notes;
            set
            {
                if (_notes != value)
                {
                    _notes = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsSavedVisible
        {
            get => _isSavedVisible;
            set
            {
                if (_isSavedVisible != value)
                {
                    _isSavedVisible = value;
                    OnPropertyChanged();
                }
            }
        }

        public string PageTitle
        {
            get => _pageTitle;
            set
            {
                _pageTitle = value;
                OnPropertyChanged(nameof(PageTitle));

            }
        }
        public int DisplayNumberWeek
        {
            get => _displayNumberWeek;
            set
            {
                if (_displayNumberWeek != value) // Zapobiega zbędnym aktualizacjom
                {
                    _displayNumberWeek = value;
                    OnPropertyChanged(nameof(DisplayNumberWeek));
                    OnPropertyChanged(nameof(DisplayNumberWeekText));
                    _ = loadExercise(); // Asynchroniczne odświeżenie listy
                }
            }
        }

        public string SaveNameGym
        {
            get => _saveNameGym;
            set
            {
                if (_saveNameGym != value)
                {
                    _saveNameGym = value;
                    OnPropertyChanged(nameof(SaveNameGym));
                }
            }
        }
        private string _selectedPart;
        public string SelectedPart
        {
            get => _selectedPart;
            set
            {
                if (_selectedPart != value)
                {
                    _selectedPart = value;

                    IsBottomGridVisible = !string.IsNullOrWhiteSpace(value) &&
                                          value != "Wszystko";

                    OnPropertyChanged();



                    // _ = loadExercise();
                    //if (_selectedPart == null || _selectedPart == "Wszystko")
                    //{
                    //    IsBottomGridVisible = false;
                    //}
                    //else
                    //{
                    //    IsBottomGridVisible = true;
                    //    //UpdateLastWeight();
                    //}

                }
            }
        }

        private string _selectedGym;
        public string SelectedGym
        {
            get => _selectedGym;
            set
            {
                if (_selectedGym != value)
                {
                    _selectedGym = value;
                    OnPropertyChanged();
                    SettingsService.SelectedGym = value;

                    //Preferences.Set("_selectedGym", value); LoadNameGymPlace();

                    // _ = loadExercise();


                }
            }

        }


        public float LastWeight
        {
            get => _lastWeight;
            set
            {

                _lastWeight = value;
                OnPropertyChanged(nameof(_lastWeight));

            }
        }
        public bool IsBottomGridVisible
        {
            get => _isBottomGridVisible;
            set
            {
                if (_isBottomGridVisible != value)
                {
                    _isBottomGridVisible = value;
                    OnPropertyChanged(nameof(IsBottomGridVisible));
                }
            }
        }

        public string DisplayNumberWeekText => $"Tydzień: {DisplayNumberWeek}";
        public ICommand NewExercise { get; }
        public ICommand Back { get; }
        public ICommand Next { get; }
        public ICommand SelectDayCommand { get; }
        public ICommand Delete { get; }
        public MainPageViewModel(TodoItemDatabase database, PersonDataBase _notes)
        {
            _database = database;
            _NoteDb = _notes;
            BodyParts = new ObservableCollection<BodyParts>();
            NewExercise = new AsyncRelayCommand(NewExerciseAsync);
            SelectDayCommand = new Command<BodyParts>(async (BodyParts) => await SelectExerciseAsync(BodyParts));
            Back = new Command(BackMethod);
            Next = new Command(NextMethod);
            Delete = new Command<BodyParts>(async (BodyParts) => await DeleteExercise(BodyParts));
            //DisplayNumberWeek = 1;
            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(SelectedPart))
                {
                    _ = loadExercise();
                }
                if (e.PropertyName == nameof(SelectedGym))
                {
                    _ = loadExercise();
                }
                //if (e.PropertyName == nameof(BodyParts))
                //{
                //    _ = LoadGymAsync(); problem po wyborze silowni, powtarzaa sie wybor selecttedGym
                //}
            };

            LoadDisplayNumberWeek();
            _ = loadExercise();
            _ = LoadPartsAsync();
            _ = LoadGymAsync();
            RefreshData();
            _ = LoadNotes();
            _timer = new System.Timers.Timer(2000);
            _timer.Elapsed += AutoSave;
            _timer.AutoReset = true;
            _timer.Start();

        }

        public async void ChooseDate(DateTime date)
        {
            var data = await _database.GetInvoiceAsync();
            if (data != null && data.Any())
            {
                //var db = await _database.GetInvoiceAsync();
                //var firstweek = db.FirstOrDefault();
                //int currentNumberWeek = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(
                //date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

                //int firstWorkNumberWeek = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(
                //                    firstweek.DateTime, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

                //int weekDisplay = currentNumberWeek - firstWorkNumberWeek;

                //DisplayNumberWeek = weekDisplay + 1;
                var first = data.First().DateTime.Date;
                var now = date.Date;

                // Różnica dni
                var difference = (now - first).TotalDays;

                // Ile pełnych tygodni minęło
                int weekNumber = (int)(difference / 7d) + 1;

                DisplayNumberWeek = weekNumber;

            }
            else
            {
                DisplayNumberWeek = 1;
            }


        }

        private void BackMethod()
        {
            if (DisplayNumberWeek > 1)
            {
                DisplayNumberWeek--;
            }
        }

        private void NextMethod()
        {
            DisplayNumberWeek++;
        }


        //private async void LoadNumberWeek()
        //{
        //    var db = await _database.GetInvoiceAsync();
        //    var firstweek = db.FirstOrDefault();
        //    int obecnynumerTygodnia = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(
        //                        DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        //    int firstWorkNumberWeek = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(
        //                        firstweek.DateTime, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

        //    int weekDisplay = (firstWorkNumberWeek + 1) - obecnynumerTygodnia;

        //    NumberWeek = $"Tydzień {weekDisplay}";
        //}

        public async Task loadExercise()
        {
            var exerciseFromDb = await _database.GetInvoiceAsync();
            if (exerciseFromDb == null)
                return;






            exerciseFromDb = exerciseFromDb
                .OrderBy(x => x.DateTime)
                .ToList();




 
            // Startujemy od pełnej listy
            var query = exerciseFromDb.AsQueryable();

            // 1. Filtr po tygodniu (gdy Wszystko → filtrujemy tylko po tygodniu)
            // 1. Filtr po partii + tygodniu
            if (string.IsNullOrWhiteSpace(SelectedPart) || SelectedPart == "Wszystko")
            {
                query = query.Where(x => x.NumberWeek == DisplayNumberWeek);
            }
            else
            {
                query = query.Where(x => x.Part == SelectedPart &&
                                         x.NumberWeek == DisplayNumberWeek);

                _lastWeight = query.Any() ? query.Max(x => x.Weight) : 0;
                OnPropertyChanged(nameof(LastWeight));
            }

            // 2. Filtr po siłowni TYLKO gdy SelectedPart = "Wszystko"
            //if(!exerciseFromDb.Any())
            //{
            //    SettingsService.SelectedGym = "Wszystko";
            //}
            var savedGym = SettingsService.SelectedGym;

            if (SelectedPart == null || SelectedPart == "Wszystko")
            {
                if (savedGym != "Wszystko")
                {
                    query = query.Where(x => x.NameGym == savedGym &&
                                             x.NumberWeek == DisplayNumberWeek);
                 }
            }
            var filtered = query.ToList();
            for (int i = 0; i < filtered.Count; i++)
            {
                if (i == 0)
                    filtered[i].ShowSeparator = true;
                else
                    filtered[i].ShowSeparator =
                        filtered[i].DateTime.Date != filtered[i - 1].DateTime.Date;
            }

            PageTitle = savedGym == "Wszystko" ? "Dziennik ćwiczeń" : $"Dziennik ćwiczeń - {savedGym}";

            // Finalizacja
            BodyParts = new ObservableCollection<BodyParts>(query.ToList());
            

            OnPropertyChanged(nameof(BodyParts));
            OnPropertyChanged(nameof(HasNoData));


        }

        private async Task LoadNotes()
        {
            var existing = await _NoteDb.LoadNotes();

            if (existing != null)
            {
                Notes = existing.Notes;
                _lastSavedNotes = existing.Notes;
            }
        }


        public async Task LoadPartsAsync()
        {
            try
            {
                var db = await _database.GetInvoiceAsync();
                var bodypartsDB = db
                    .Select(x => x.Part)
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Distinct().OrderBy(x => x, StringComparer.CurrentCultureIgnoreCase)
                    .ToList();

                bodypartsDB.Insert(0, "Wszystko");
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Parts.Clear();
                    foreach (var part in bodypartsDB)
                        Parts.Add(part);
                });
            }
            catch (Exception ex)
            {
                // opcjonalnie obsługa błędów
                Console.WriteLine($"Błąd przy wczytywaniu danych: {ex.Message}");
            }
        }

        public async Task LoadGymAsync()
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


                bodypartsDB.Insert(0, "Wszystko");
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Gym.Clear();
                    foreach (var Name in bodypartsDB)
                        Gym.Add(Name);
                });
            }
            catch (Exception ex)
            {
                // opcjonalnie obsługa błędów
                Console.WriteLine($"Błąd przy wczytywaniu siłowni: {ex.Message}");
            }
        }

        private async void LoadDisplayNumberWeek()
        {
            var exerciseFromDb = await _database.GetInvoiceAsync();

            if (exerciseFromDb == null || exerciseFromDb.Count() == 0)
            {
                DisplayNumberWeek = 1;
            }
            else
            {
                var filteredData = exerciseFromDb.OrderBy(x => x.NumberWeek).ToList();
                DisplayNumberWeek = filteredData.Last().NumberWeek;
            }



        }

        private async void AutoSave(object sender, ElapsedEventArgs e)
        {
            if (!string.Equals(Notes, _lastSavedNotes))
            {
                await SaveNotesAsync();
            }
        }
        private async Task SaveNotesAsync()
        {
            _lastSavedNotes = Notes;

            await _NoteDb.SaveNotesAsync(Notes);

            IsSavedVisible = true;

            await Task.Delay(1000);

            IsSavedVisible = false;
        }

        private void RefreshData()
        {
            //odświeżenie listy po usunięciu danych w AppShellViewModel
            var appShellViewModel = App.Current.MainPage.BindingContext as AppShellViewModel;
            if (appShellViewModel != null)
            {
                appShellViewModel.DataDeleted += (sender, e) =>
                {
                    // Odśwież dane
                    _ = loadExercise();
                };
            }
        }
        private async Task SelectExerciseAsync(BodyParts selectedExercise)
        {
            if (selectedExercise != null)
            {
                // Użyj identyfikatora lub innego klucza zamiast całego obiektu
                await Shell.Current.GoToAsync($"{nameof(AddExercisePage)}?exerciseId={selectedExercise.Id}");
            }
        }
        private async Task DeleteExercise(BodyParts bodyParts)
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert("Potwierdzenie", "Czy na pewno chcesz usunąć ćwiczenie?", "Tak", "Nie");

            if (confirm)
            {
                await _database.DeleteInvoiceAsync(bodyParts);
                BodyParts.Remove(bodyParts);
                //await _supabase.From<ShopList>().Where(x => x.Id == shopList.Id).Delete();
            }
           _ = loadExercise();

        }

        private async Task NewExerciseAsync()
        {
            
            await Shell.Current.GoToAsync(nameof(AddExercisePage));
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
