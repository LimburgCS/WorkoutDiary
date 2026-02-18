using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;
using WorkoutDiary.data;
using WorkoutDiary.Model;
using WorkoutDiary.Service;
using WorkoutDiary.Views;

namespace WorkoutDiary.ViewModels
{
    public partial class TrainingPlanViewModel : ObservableObject
    {
        private readonly TrainingPlanDataBase _database;
        private int _selectedReadyPlanId;

        public List<ReadyTrainingPlan> Plans { get; set; }
        


        [ObservableProperty]
        private ObservableCollection<TrainingPlan> trainingPlan = new();

        [ObservableProperty]
        private bool isDisplayButton;
        public ICommand SelectPlan { get; }
        public ICommand SelectReadyPlan { get; }
        public ICommand Delete { get; }
        public TrainingPlanViewModel(TrainingPlanDataBase database)
        {
            _database = database;
            SelectPlan = new Command<TrainingPlan>(async (TrainingPlan) => await SelectPlanAsync(TrainingPlan));
            SelectReadyPlan = new Command<ReadyTrainingPlan>(async plan => await SelectTrainingAsync(plan));
            Delete = new Command<TrainingPlan>(async (TrainingPlan) => await DeletePlanAsync(TrainingPlan));
            Plans = ReadyTrainingPlan.DefaultPlans;

            _ = LoadTrainingPlansAsync();
        }


        [RelayCommand]
        public async Task LoadTrainingPlansAsync()
        {
            var plan = await _database.GetTrainingIDAsync(SettingsService.SelectPlan);

            if (plan is null)
            {
                IsDisplayButton = true;
                return;
            }

            IsDisplayButton = false;

            TrainingPlan = new ObservableCollection<TrainingPlan>
            {
                 plan
            };


        }


        private async Task SelectTrainingAsync(ReadyTrainingPlan selectedReadyPlan)
        {
            SettingsService.SelectPlan = selectedReadyPlan.Id;

            await Shell.Current.GoToAsync(
                $"{nameof(AddTrainingPlanPage)}?planId={selectedReadyPlan.Id}");

        }





        private async Task SelectPlanAsync(TrainingPlan plan)
        {
            if (plan is null)
                return;

            // jeśli wcześniej wybrano gotowy plan → użyj jego ID
            int idToUse = SettingsService.SelectPlan != 0 ? SettingsService.SelectPlan : plan.Id;

            await Shell.Current.GoToAsync($"{nameof(AddTrainingPlanPage)}?planId={idToUse}");
        }








        [RelayCommand]
        private async Task NewPlanAsync()
        {
            await Shell.Current.GoToAsync(nameof(AddTrainingPlanPage));
        }

        [RelayCommand]
        public async Task NewTrainingPlanAdded()
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