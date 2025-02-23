using WorkoutDiary.ViewModels;
using WorkoutDiary.Views;

namespace WorkoutDiary
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(AddExercisePage), typeof(AddExercisePage));
            Routing.RegisterRoute(nameof(StatisticPage), typeof(StatisticPage));
            BindingContext = new AppShellViewModel();
        }
    }
}
