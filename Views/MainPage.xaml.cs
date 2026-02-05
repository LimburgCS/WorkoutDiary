using System.Collections.ObjectModel;
using WorkoutDiary.data;
using WorkoutDiary.Model;
using WorkoutDiary.ViewModels;

namespace WorkoutDiary.Views
{
    public partial class MainPage : ContentPage
    {
        public TodoItemDatabase database;
        public ObservableCollection<BodyParts> bodyParts {  get; set; }

        public MainPage()
        {
            InitializeComponent();
            database = new TodoItemDatabase();
            BindingContext = new MainPageViewModel(database);
            bodyParts = new ObservableCollection<BodyParts>();
            OnAppearing();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();


            if (BindingContext is MainPageViewModel viewModel)
            {
                await viewModel.loadExercise();
                //await viewModel.LoadPartsAsync();
                //await viewModel.LoadGymAsync();
            }
        }
        //private async void LoadPart()
        //{
        //    var db = await database.GetInvoiceAsync();
        //    var bodypartsDB = db.Select(x => x.Part).Distinct().ToList();
        //    namePicker.ItemsSource = bodypartsDB; // Ustawienie listy w Picker

        //}

        //resetowanie zaznaczenia po powrocie na strone
        private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
        {
            ExerciseCollectionView.SelectedItem = null;
        }

        private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            if (BindingContext is MainPageViewModel viewModel)
            {
                DateTime date = datepicker.Date;
                viewModel.ChooseDate(date);
            }
            
            
        }



    }

}
