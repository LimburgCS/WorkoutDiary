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

        // Dwa ostatnie dni treningowe
        [ObservableProperty]
        private ObservableCollection<List<BodyParts>> lastTrainings = new();

        public RotationPlanViewModel(WorkoutRecommendationService recommendationService)
        {
            _recommendationService = recommendationService;
        }

        [RelayCommand]
        public async Task LoadRecommendationAsync()
        {
            var result = await _recommendationService.GetRecommendationAsync();
            RecommendedParts = new ObservableCollection<string>(result);

            var lastTwo = await _recommendationService.GetLastTwoTrainingDaysAsync();
            LastTrainings = new ObservableCollection<List<BodyParts>>(lastTwo);


        }


    }
}