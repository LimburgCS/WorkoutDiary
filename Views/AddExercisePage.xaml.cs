
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
            LoadPart();
        }
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("exerciseId") && int.TryParse(query["exerciseId"].ToString(), out int exerciseId))
            {
                _viewmodel.LoadExercise(exerciseId);
            }
        }
        private async void LoadPart()
        {
            var db = await database.GetInvoiceAsync();
            var bodypartsDB = db.Select(x => x.Part).Distinct().ToList();
            namePicker.ItemsSource = bodypartsDB; // Ustawienie listy w Picker

        }
        private void OnNameSelected(object sender, EventArgs e)
        {
            if (namePicker.SelectedIndex != -1)
            {
                string selectedPart = namePicker.SelectedItem.ToString();
                _viewmodel.loadPicker(selectedPart);
            }
        }
    }
}