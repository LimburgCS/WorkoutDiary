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
        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Zakładamy, że ViewModel ma metodę odświeżania danych
            if (BindingContext is MainPageViewModel viewModel)
            {
                viewModel.loadInvoice();
            }
        }
        private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
        {
            ExerciseCollectionView.SelectedItem = null;
        }
    }

}
