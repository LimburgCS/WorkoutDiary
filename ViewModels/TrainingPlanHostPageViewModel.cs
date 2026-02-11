using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutDiary.Model;
using WorkoutDiary.Views;

namespace WorkoutDiary.ViewModels
{
    public partial class TrainingPlanHostViewModel : ObservableObject
    {
        private readonly RotationPlanPage _rotationView;
        private readonly TrainingPlanPage _trainingView;

        private ObservableCollection<TrainingPlan> _trainingPlan;



        [ObservableProperty]
        private string currentTitle;
        [ObservableProperty]
        private string textButtonChngePlan;

        [ObservableProperty]
        private View currentView;

        public TrainingPlanHostViewModel(RotationPlanPage rotationView, TrainingPlanPage trainingView)
        {
            _rotationView = rotationView;
            _trainingView = trainingView;
            var last = Preferences.Get("LastTrainingView", "rotation");

            if (last == "training")
            {
                CurrentView = _trainingView;
                CurrentTitle = "Plan treningowy";
                TextButtonChngePlan = "Plan rotacyjny";

            }
            else
            {
                CurrentView = _rotationView;
                CurrentTitle = "Plan rotacyjny";
                TextButtonChngePlan = "Plan treningowy";
            }





            if (_rotationView.BindingContext is RotationPlanViewModel vm)
                _ = vm.InitAsync();

            if (_trainingView.BindingContext is TrainingPlanViewModel vm2)
                _ = vm2.LoadTrainingPlansAsync();



            
        }

        [RelayCommand]
        private void ToggleView()
        {
            if (CurrentView == _rotationView)
            {
                CurrentView = _trainingView;
                CurrentTitle =  "Plan treningowy"; // tytuł rewersu
                TextButtonChngePlan = "Plan rotacyjny";
                Preferences.Set("LastTrainingView", "training");

                    
            }
            else
            {
                CurrentView = _rotationView;
                CurrentTitle  = "Plan rotacyjny";
                TextButtonChngePlan = "Plan treningowy"; 
                Preferences.Set("LastTrainingView", "rotation");

            }
        }


    }


}
