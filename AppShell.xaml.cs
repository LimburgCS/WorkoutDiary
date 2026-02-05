using WorkoutDiary.ViewModels;
using WorkoutDiary.Views;

namespace WorkoutDiary
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(AddExercisePage), typeof(AddExercisePage));
            Routing.RegisterRoute(nameof(AddCardioPage), typeof(AddCardioPage));
            Routing.RegisterRoute(nameof(StatisticPage), typeof(StatisticPage));
            Routing.RegisterRoute(nameof(CardioPage), typeof(CardioPage));
            Routing.RegisterRoute(nameof(TrainingPlanPage), typeof(TrainingPlanPage));
            Routing.RegisterRoute(nameof(StatisticCardioPage), typeof(StatisticCardioPage));
            Routing.RegisterRoute(nameof(AddPersonPage), typeof(AddPersonPage));
            Routing.RegisterRoute(nameof(AddTrainingPlanPage), typeof(AddTrainingPlanPage));
            Routing.RegisterRoute(nameof(SettingPage), typeof(SettingPage));
            Routing.RegisterRoute(nameof(RotationPlanPage), typeof(RotationPlanPage));
            BindingContext = new AppShellViewModel();
            
            //var itemToRemove = this.Items.FirstOrDefault(i => i.Title == "Statystyka-cardio"); // niewidoczny flylayout
            //if (itemToRemove != null)
            //{
            //    this.Items.Remove(itemToRemove);
            //}

        }
    }
}
