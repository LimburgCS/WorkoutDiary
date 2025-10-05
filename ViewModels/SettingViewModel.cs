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
        public ICommand DeleteExerciseParts { get; }
        public ICommand DeleteExerciseCardio { get; }
        public SettingViewModel()
        {
            _database = new TodoItemDatabase();
            _cardioData = new CardioDataBase();
            BodyParts = new ObservableCollection<BodyParts>();
            DeleteExerciseParts = new AsyncRelayCommand(DeletePartsAsync);
            DeleteExerciseCardio = new AsyncRelayCommand(DeleteCardioAsync);
            
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
    }
}
