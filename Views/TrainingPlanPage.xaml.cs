using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutDiary.data;
using WorkoutDiary.ViewModels;

namespace WorkoutDiary.Views
{
    public partial class TrainingPlanPage : ContentPage
    {

        public TrainingPlanPage(TrainingPlanViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm ;
            OnAppearing();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Zakładamy, że ViewModel ma metodę odświeżania danych
            if (BindingContext is TrainingPlanViewModel viewModel)
            {
               await viewModel.LoadTrainingPlans();
            }
        }
        private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
        {
            TrainingPlanCollectionView.SelectedItem = null;
        }
    }
}