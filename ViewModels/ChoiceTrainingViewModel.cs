using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkoutDiary.Model;
using WorkoutDiary.Views;

namespace WorkoutDiary.ViewModels
{
    public class ChoiceTrainingViewModel : INotifyPropertyChanged

    {
        public List<ChoiceTrainingPlan> Plans { get; set; }

        private ChoiceTrainingPlan _selectedPlan;
        public ChoiceTrainingPlan SelectedPlan
        {
            get => _selectedPlan;
            set
            {
                _selectedPlan = value;
                OnPropertyChanged(nameof(SelectedPlan));
            }
        }

        public ICommand SelectTrainingCommand { get; }
        public ChoiceTrainingViewModel()
        {
            Plans = ChoiceTrainingPlan.DefaultPlans;

            SelectTrainingCommand = new Command<BodyParts>(async (BodyParts) => await SelectTrainingAsync(BodyParts));

        }

        private async Task SelectTrainingAsync(BodyParts SelectedPlan)
        {
            if (SelectedPlan != null)
            {
                // Użyj identyfikatora lub innego klucza zamiast całego obiektu
                await Shell.Current.GoToAsync($"{nameof(TrainingPlanPage)}?exerciseId={SelectedPlan.Id}");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    }
}
