using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkoutDiary.data;
using WorkoutDiary.Model;

namespace WorkoutDiary.ViewModels
{
    public class SettingViewModel
    {
        readonly TodoItemDatabase _database;
        readonly CardioDataBase _cardioData;
        public ObservableCollection<string> Gym { get; set; } = new();
        private ObservableCollection<BodyParts> _bodyParts;
        public ObservableCollection<BodyParts> BodyParts
        {
            get => _bodyParts;
            set
            {
                _bodyParts = value;

            }
        }
        private ObservableCollection<Cardio> _cardio;
        public ObservableCollection<Cardio> Cardio
        {
            get => _cardio;
            set
            {
                _cardio = value;

            }
        }
        private string _selectedGym;
        public string SelectedGym
        {
            get => _selectedGym;
            set
            {
                    _selectedGym = value;
                
            }

        }
        public ICommand DeleteExerciseParts { get; }
        public ICommand DeleteExerciseCardio { get; }
        public ICommand DeleteGymCommand { get; }
        public SettingViewModel()
        {
            _database = new TodoItemDatabase();
            _cardioData = new CardioDataBase();
            BodyParts = new ObservableCollection<BodyParts>();
            DeleteExerciseParts = new AsyncRelayCommand(DeletePartsAsync);
            DeleteExerciseCardio = new AsyncRelayCommand(DeleteCardioAsync);
            DeleteGymCommand = new Command<BodyParts>(async (BodyParts) => await DeleteNameGym(BodyParts));
            _ = LoadGymAsync();
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
                    foreach (var part in bodypartsDB)
                        Gym.Add(part);
                });
            }
            catch (Exception ex)
            {
                // opcjonalnie obsługa błędów
                Console.WriteLine($"Błąd przy wczytywaniu siłowni: {ex.Message}");
            }
        }
        private async Task DeletePartsAsync()
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert("Potwierdzenie", "Czy na pewno chcesz usunąć wszystkie dane ćwiczeń siłowych?", "Tak", "Nie");

            if (confirm)
            {
                await _database.DeleteInvoiceAllAsync();
                BodyParts.Clear();
            }
        }
        private async Task DeleteCardioAsync()
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert("Potwierdzenie", "Czy na pewno chcesz usunąć wszystkie dane cardio?", "Tak", "Nie");

            if (confirm)
            {
                await _cardioData.DeleteCardioAllAsync();
                BodyParts.Clear();
            }
        }
        private async Task DeleteNameGym(BodyParts bodyParts)
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert("Potwierdzenie", "Czy na pewno chcesz usunąć siłownie wraz z ćwiczeniami?", "Tak", "Nie");

            if (confirm)
            {
                var db = await _database.GetInvoiceAsync();
                var DeleteNameGym = db.Where(x => x.NameGym == SelectedGym).ToList();
                foreach (var item in DeleteNameGym)
                {
                    await _database.DeleteInvoiceAsync(item);
                    BodyParts.Remove(item);
                }
            }

        }
    }
}
