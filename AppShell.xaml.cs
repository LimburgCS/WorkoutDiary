using WorkoutDiary.Views;

namespace WorkoutDiary
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(AddExercisePage), typeof(AddExercisePage));
        }
    }
}
