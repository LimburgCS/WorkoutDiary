using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutDiary.ViewModels;

namespace WorkoutDiary.Views
{
	public partial class RotationPlanPage : ContentPage
	{
        public RotationPlanPage(RotationPlanViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is RotationPlanViewModel vm)
            {
                await vm.LoadRecommendationAsync();
            }
        }

    }
}