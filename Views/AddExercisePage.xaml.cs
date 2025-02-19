using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutDiary.data;
using WorkoutDiary.ViewModels;


namespace WorkoutDiary.Views
{
    public partial class AddExercisePage : ContentPage, IQueryAttributable
    {
        private readonly AddExerciseViewModel _viewmodel;   
        TodoItemDatabase database;
        public AddExercisePage(AddExerciseViewModel viewmodel)
        {
            InitializeComponent();
            database = new TodoItemDatabase();
            BindingContext = _viewmodel = viewmodel;
        }
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("exerciseId") && int.TryParse(query["exerciseId"].ToString(), out int exerciseId))
            {
                _viewmodel.LoadInvoice(exerciseId);
            }
        }
    }
}