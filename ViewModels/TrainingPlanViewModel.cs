using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;
using WorkoutDiary.data;
using WorkoutDiary.Model;
using WorkoutDiary.Views;

namespace WorkoutDiary.ViewModels
{
    public partial class TrainingPlanViewModel : ObservableObject
    {
        private readonly TrainingPlanDataBase _database;

        [ObservableProperty]
        private ObservableCollection<TrainingPlan> trainingPlan = new();


        public ICommand SelectPlan { get; }
        public ICommand Delete { get; }
        public TrainingPlanViewModel(TrainingPlanDataBase database)
        {
            _database = database;
            SelectPlan = new Command<TrainingPlan>(async (TrainingPlan) => await SelectPlanAsync(TrainingPlan));
            Delete = new Command<TrainingPlan>(async (TrainingPlan) => await DeletePlanAsync(TrainingPlan));
            _ = LoadTrainingPlansAsync();
        }


        [RelayCommand]
        public async Task LoadTrainingPlansAsync()
        {
            var plans = await _database.GetTrainingAsync();

            if (plans is null)
                return;

            TrainingPlan = new ObservableCollection<TrainingPlan>(plans);



        }


        private async Task SelectPlanAsync(TrainingPlan plan)
        {
            if (plan is null)
                return;

            await Shell.Current.GoToAsync($"{nameof(AddTrainingPlanPage)}?planId={plan.Id}");
            // opcjonalnie: wyczyść zaznaczenie po kliknięciu
            // TrainingPlanCollectionView.SelectedItem = null; ← to już w code-behind, jeśli chcesz
        }



        [RelayCommand]
        private async Task NewPlanAsync()
        {
            await Shell.Current.GoToAsync(nameof(AddTrainingPlanPage));
        }


        private async Task DeletePlanAsync(TrainingPlan plan)
        {
            if (plan is null)
                return;

            bool confirm = await Application.Current.MainPage.DisplayAlert(
                "Potwierdzenie",
                "Czy na pewno chcesz usunąć plan?",
                "Tak",
                "Nie");

            if (confirm)
            {
                await _database.DeletePlanAsync(plan);
                TrainingPlan.Remove(plan);
            }

            await LoadTrainingPlansAsync();
        }
    }
}