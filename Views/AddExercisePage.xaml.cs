
using WorkoutDiary.data;
using WorkoutDiary.Service;
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
            _ = LoadPart();

        }
        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("exerciseId") && int.TryParse(query["exerciseId"].ToString(), out int exerciseId))
            {
                await _viewmodel.LoadExercise(exerciseId);
            }
        }
        private async Task LoadPart()
        {
            var db = await database.GetInvoiceAsync();
            var bodypartsDB = db.Select(x => x.Part).Distinct().OrderBy(x=>x, StringComparer.CurrentCultureIgnoreCase).ToList();
            var nameGym = db.Select(x => x.NameGym).Distinct().OrderBy(x => x, StringComparer.CurrentCultureIgnoreCase).ToList();
            namePicker.ItemsSource = bodypartsDB; // Ustawienie listy w Picker
            namePickerGym.ItemsSource = nameGym;
            string savedGym = SettingsService.SelectedGym;
            namePickerGym.SelectedItem = savedGym;

        }
        private void PartSelected(object sender, EventArgs e)
        {
            if (namePicker.SelectedIndex != -1)
            {
                string selectedPart = namePicker.SelectedItem.ToString();
                _viewmodel.loadPicker(selectedPart);
            }
        }
        //private void GymSelected(object sender, EventArgs e)
        //{
        //    if (namePickerGym.SelectedIndex != -1)
        //    {
        //        string selectedGym = namePickerGym.SelectedItem.ToString();


        //        _viewmodel.loadPickerGym(selectedGym);

        //    }
        //}

        private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            if (BindingContext is AddExerciseViewModel viewModel)
            {
                DateTime date = datepickerEditor.Date;
                viewModel.ChooseDate(date);
            }


        }

        private void Entry_Completed(object sender, EventArgs e)
        {
            if (sender == SeriesEditor)
                RepetitionsEditor.Focus();
            else if (sender == RepetitionsEditor)
                WeightEditor.Focus();

        }
        
        private void Entry_save_Completed(object sender, EventArgs e)
        {
            save.SendClicked();

        }
    }
}