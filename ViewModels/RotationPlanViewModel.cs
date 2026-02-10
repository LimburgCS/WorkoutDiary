using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using WorkoutDiary.data;
using WorkoutDiary.Model;
using WorkoutDiary.Service;
using WorkoutDiary.Views;

namespace WorkoutDiary.ViewModels
{
    public partial class RotationPlanViewModel : ObservableObject
    {
        private readonly WorkoutRecommendationService _recommendationService;
        // Lista sugerowanych partii
        [ObservableProperty]
        private ObservableCollection<string> recommendedParts = new();

        [ObservableProperty]
        private ObservableCollection<TrainingDayView> lastTrainings;

        public RotationPlanViewModel(WorkoutRecommendationService recommendationService)
        {
            _recommendationService = recommendationService;
        }



        public async Task InitAsync()
        {
            var result = await _recommendationService.GetRecommendationAsync();
            RecommendedParts = new ObservableCollection<string>(result);

            var lastTwo = await _recommendationService.GetLastTwoTrainingDaysForDisplayAsync();
            LastTrainings = new ObservableCollection<TrainingDayView>(lastTwo);
        }


        [RelayCommand]
        public async Task ChangePlanView()
        {
            await Shell.Current.GoToAsync("/TrainingPlanPage");
        }

    }
}