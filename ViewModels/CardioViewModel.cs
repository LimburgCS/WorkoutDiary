using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkoutDiary.data;
using WorkoutDiary.Model;
using WorkoutDiary.Views;

namespace WorkoutDiary.ViewModels
{
    public class CardioViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public PersonDataBase _database;
        public CardioDataBase _cardiodatabase;
        private ObservableCollection<Person> _person;
        private ObservableCollection<Cardio> _cardio;
        private int _displayNumberWeek;
        public ObservableCollection<Person> Person
        {
            get => _person;
            set
            {
                _person = value;
                OnPropertyChanged(nameof(Person));
            }
        }
        public ObservableCollection<Cardio> Cardio
        {
            get => _cardio;
            set
            {
                _cardio = value;
   
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
                    loadCardioData(); // Asynchroniczne odświeżenie listy
                }
            }
        }
        public string DisplayNumberWeekText => $"Tydzień: {DisplayNumberWeek}";
        public ICommand AddPerson { get; }
        public ICommand AddCardio { get; }
        public ICommand Back { get; }
        public ICommand Next { get; }
        public ICommand SelectDayCommand { get; }
        public ICommand Delete { get; }
        public CardioViewModel(PersonDataBase database, CardioDataBase cardioDataBase)
        {
            _database = database;
            _cardiodatabase = cardioDataBase;
            Person = new ObservableCollection<Person>();
            AddPerson = new AsyncRelayCommand(AddPersonAsync);
            AddCardio = new AsyncRelayCommand(AddCardioAsync);
            Back = new Command(BackMethod);
            Next = new Command(NextMethod);
            SelectDayCommand = new Command<Cardio>(async (Cardio) => await SelectExerciseAsync(Cardio));
            Delete = new Command<Cardio>(async (Cardio) => await DeleteExercise(Cardio));
            loadPersonData();
            loadCardioData();
            LoadDisplayNumberWeek();
        }
        private async Task DeleteExercise(Cardio cardio)
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert("Potwierdzenie", "Czy na pewno chcesz usunąć ćwiczenie?", "Tak", "Nie");

            if (confirm)
            {
                await _cardiodatabase.DeleteCardioAsync(cardio);
                Cardio.Remove(cardio); 
                //await _supabase.From<ShopList>().Where(x => x.Id == shopList.Id).Delete();
            }
            loadCardioData();

        }
        private async Task AddPersonAsync()
        {
            await Shell.Current.GoToAsync(nameof(AddPersonPage));
        }
        private async Task AddCardioAsync()
        {
            await Shell.Current.GoToAsync(nameof(AddCardioPage));
        }
        public async void ChooseDate(DateTime date)
        {
            var data = await _cardiodatabase.GetCardioAsync();
            if (data != null && data.Any())
            {
                var db = await _cardiodatabase.GetCardioAsync();
                var firstweek = db.FirstOrDefault();
                int currentNumberWeek = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(
                date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

                int firstWorkNumberWeek = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(
                                    firstweek.DateTime, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

                int weekDisplay = currentNumberWeek - firstWorkNumberWeek;

                DisplayNumberWeek = weekDisplay + 1;
            }
            else
            {
                DisplayNumberWeek = 1;
            }


        }
        public async void loadPersonData()
        {
            var exerciseFromDb = await _database.GetPersonAsync();

            //if (exerciseFromDb == null)
            //    return;

                var filteredData = exerciseFromDb.Where(x => x.Id == 1).ToList(); // Dodano ToList()
               Person = new ObservableCollection<Person>(filteredData);
            if (exerciseFromDb != null)
            {
                
                    var dataPerson = await _database.GetPersonIDAsync(1);
                //Pulse = 220 - dataPerson.Age;
                //IndexBmi = dataPerson.Weight / Math.Pow((dataPerson.Growth / 100), 2);

            }

        }

        public async void loadCardioData()
        {
            var cardioFromDb = await _cardiodatabase.GetCardioAsync();

            if (cardioFromDb == null)
                return;

            var filteredData = cardioFromDb.Where(x => x.NumberWeek == DisplayNumberWeek).ToList(); // Dodano ToList()
            Cardio = new ObservableCollection<Cardio>(filteredData);
            OnPropertyChanged(nameof(Cardio));

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
        private async Task SelectExerciseAsync(Cardio selectedExercise)
        {
            if (selectedExercise != null)
            {
                // Użyj identyfikatora lub innego klucza zamiast całego obiektu
                await Shell.Current.GoToAsync($"{nameof(AddCardioPage)}?exerciseId={selectedExercise.Id}");
            }
        }
        private async void LoadDisplayNumberWeek()
        {
            var cardioFromDb = await _cardiodatabase.GetCardioAsync();

            if (cardioFromDb == null || cardioFromDb.Count() == 0)
            {
                DisplayNumberWeek = 1;
            }
            else
            {
                var filteredData = cardioFromDb.OrderBy(x => x.NumberWeek).ToList();
                DisplayNumberWeek = filteredData.Last().NumberWeek;
            }



        }
    }
}
    