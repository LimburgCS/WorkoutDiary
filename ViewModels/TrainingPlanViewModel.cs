using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    public class TrainingPlanViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public TrainingPlanDataBase _database;
        private ObservableCollection<TrainingPlan> _trainingPlan;
        public ObservableCollection<TrainingPlan> TrainingPlan
        {
            get => _trainingPlan;
            set
            {
                _trainingPlan = value;
                OnPropertyChanged(nameof(TrainingPlan));

            }
        }
        public ICommand NewPlan { get; }
        public ICommand Delete { get; }
        public ICommand SelectPlan { get; }
        public TrainingPlanViewModel(TrainingPlanDataBase database)
        {
            _database = database;
            TrainingPlan = new ObservableCollection<TrainingPlan>();
            SelectPlan = new Command<TrainingPlan>(async (TrainingPlan) => await SelectPlanAsync(TrainingPlan));
            Delete = new Command<TrainingPlan>(async (TrainingPlan) => await DeletePlan(TrainingPlan));
            NewPlan = new AsyncRelayCommand(NewPlanAsync);
            _ = LoadTrainingPlans();
        }

        private async Task SelectPlanAsync(TrainingPlan TrainingPlan)
        {
            if (TrainingPlan != null)
            {
                // Użyj identyfikatora lub innego klucza zamiast całego obiektu
                await Shell.Current.GoToAsync($"{nameof(AddTrainingPlanPage)}?planId={TrainingPlan.Id}");
            }
        }
        private async Task NewPlanAsync()
        {
            await Shell.Current.GoToAsync(nameof(AddTrainingPlanPage));
        }

        public async Task LoadTrainingPlans()
        {
            var plans = await _database.GetTrainingAsync();

            if (plans == null)
                return;


            TrainingPlan = new ObservableCollection<TrainingPlan>(plans);

        }

        private async Task DeletePlan(TrainingPlan plans)
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert("Potwierdzenie", "Czy na pewno chcesz usunąć plan?", "Tak", "Nie");

            if (confirm)
            {
                await _database.DeletePlanAsync(plans);
                TrainingPlan.Remove(plans);
                //await _supabase.From<ShopList>().Where(x => x.Id == shopList.Id).Delete();
            }
            _ = LoadTrainingPlans();

        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
