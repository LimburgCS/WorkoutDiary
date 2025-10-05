using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutDiary.data;
using WorkoutDiary.ViewModels;

namespace WorkoutDiary.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddCardioPage : ContentPage, IQueryAttributable
    {
        private readonly AddCardioViewModel _viewmodel;
        CardioDataBase database;
        public AddCardioPage(AddCardioViewModel viewmodel)
        {
            InitializeComponent();
            database = new CardioDataBase();
            BindingContext = _viewmodel = viewmodel;
            LoadCardio();
        }
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("exerciseId") && int.TryParse(query["exerciseId"].ToString(), out int exerciseId))
            {
                _viewmodel.LoadCardioExercise(exerciseId);
            }
        }
        private async void LoadCardio()
        {
            var db = await database.GetCardioAsync();
            var cardioDB = db.Select(x => x.Name).Distinct().OrderBy(x => x).ToList();
            namePicker.ItemsSource = cardioDB; // Ustawienie listy w Picker

        }
        private void PartSelected(object sender, EventArgs e)
        {
            if (namePicker.SelectedIndex != -1)
            {
                string selectedPart = namePicker.SelectedItem.ToString();
                _viewmodel.loadPicker(selectedPart);
            }
        }
        private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            if (BindingContext is AddCardioViewModel viewModel)
            {
                DateTime date = datepickerEditor.Date;
                viewModel.ChooseDate(date);
            }


        }
    }
}