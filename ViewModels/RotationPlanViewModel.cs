using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using WorkoutDiary.data;
using WorkoutDiary.Model;
using WorkoutDiary.Service;

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



        [RelayCommand]
        public async Task LoadRecommendationAsync()
        {
            var result = await _recommendationService.GetRecommendationAsync();
            RecommendedParts = new ObservableCollection<string>(result);

            var lastTwo = await _recommendationService.GetLastTwoTrainingDaysForDisplayAsync();
            LastTrainings = new ObservableCollection<TrainingDayView>(lastTwo);


        }


    }
}