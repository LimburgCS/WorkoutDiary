

using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkoutDiary.data;
using WorkoutDiary.Model;
using WorkoutDiary.Views;

namespace WorkoutDiary.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public TodoItemDatabase _database;
        private ObservableCollection<BodyParts> _bodyParts;
        private int _displayNumberWeek;
        public ObservableCollection<BodyParts> BodyParts
        {
            get => _bodyParts;
            set
            {
                _bodyParts = value;

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
                    loadExercise(); // Asynchroniczne odświeżenie listy
                }
            }
        }

        public string DisplayNumberWeekText => $"Tydzień: {DisplayNumberWeek}";
        public ICommand NewExercise { get; }
        public ICommand Back {  get; }
        public ICommand Next { get; }
        public ICommand SelectDayCommand { get; }
        public ICommand Delete { get; }
        public MainPageViewModel(TodoItemDatabase database)
        {
            _database = database;
            BodyParts = new ObservableCollection<BodyParts>();
            NewExercise = new AsyncRelayCommand(NewExerciseAsync);
            SelectDayCommand = new Command<BodyParts>(async (BodyParts) => await SelectExerciseAsync(BodyParts));
            Back = new Command(BackMethod);
            Next = new Command(NextMethod);
            Delete = new Command<BodyParts>(async (BodyParts) => await DeleteExercise(BodyParts));
            DisplayNumberWeek = 1;
            loadExercise();
            //RefreshData();
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

        public async void loadExercise()
        {
            //standardowe ładowanie danych
            var exerciseFromDb = await _database.GetInvoiceAsync();

            if (exerciseFromDb == null)
                return;

            var filteredData = exerciseFromDb.Where(x => x.NumberWeek == DisplayNumberWeek).ToList(); // Dodano ToList()

            BodyParts = new ObservableCollection<BodyParts>(filteredData);
            OnPropertyChanged(nameof(BodyParts));

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
                    loadExercise();
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
            loadExercise();

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
