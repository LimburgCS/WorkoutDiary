using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutDiary.data;
using WorkoutDiary.ViewModels;

namespace WorkoutDiary.Views
{
    public partial class AddTrainingPlanPage : ContentPage
    {
        public TrainingPlanDataBase database;
        public AddTrainingPlanPage(AddTrainingPlanViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        //public async void ApplyQueryAttributes(IDictionary<string, object> query)
        //{
        //    if (query.ContainsKey("planId") && int.TryParse(query["planId"].ToString(), out int planId))
        //    {
        //        await _viewmodel.LoadPlan(planId);
        //    }
        //}

    }
}