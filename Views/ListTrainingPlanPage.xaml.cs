using WorkoutDiary.ViewModels;

namespace WorkoutDiary.Views;

public partial class ListTrainingPlanPage : ContentPage
{
	public ListTrainingPlanPage(ChoiceTrainingViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
    }
}