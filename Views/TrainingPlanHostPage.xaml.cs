using Microcharts;
using Microcharts.Maui;
using SkiaSharp;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutDiary.data;
using WorkoutDiary.Model;
using WorkoutDiary.ViewModels;

namespace WorkoutDiary.Views
{

    public partial class TrainingPlanHostPage : ContentPage
    {
        public TrainingPlanHostPage(TrainingPlanHostViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
            
            OnAppearing();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is TrainingPlanHostViewModel hostVm)
            {
                switch (hostVm.CurrentView)
                {
                    case TrainingPlanPage trainingPage
                        when trainingPage.BindingContext is TrainingPlanViewModel trainingVm:
                        _ = trainingVm.LoadTrainingPlansAsync();
                        break;

                    case RotationPlanPage rotationPage
                        when rotationPage.BindingContext is RotationPlanViewModel rotationVm:
                        _ = rotationVm.InitAsync();
                        break;
                }
            }
        }




    }

}