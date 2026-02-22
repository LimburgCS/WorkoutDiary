using System.Collections.ObjectModel;
using System.Reflection.Metadata;
using WorkoutDiary.data;
using WorkoutDiary.Model;
using WorkoutDiary.ViewModels;

namespace WorkoutDiary.Views
{
    public partial class MainPage : ContentPage
    {
        public TodoItemDatabase database;
        public ObservableCollection<BodyParts> bodyParts {  get; set; }

        public MainPage(MainPageViewModel vm)
        {
            InitializeComponent();
            database = new TodoItemDatabase();
            BindingContext = vm;
            bodyParts = new ObservableCollection<BodyParts>();
            OnAppearing();


        }
        protected override  void OnAppearing()
        {
            base.OnAppearing();


            if (BindingContext is MainPageViewModel viewModel)
            {
                _ = viewModel.loadExercise();
                _ = viewModel.LoadGymAsync();
                _ = viewModel.LoadPartsAsync();
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
        bool isOpen = false;

        private async void TogglePanel(object sender, EventArgs e)
        {
            if (isOpen)
            {
                Overlay.IsVisible = false;

                await Task.WhenAll(
                    SidePanel.TranslateTo(-260, 0, 250, Easing.CubicOut),
                    Handle.TranslateTo(0, 0, 250, Easing.CubicOut)
                );
            }
            else
            {
                Overlay.IsVisible = true;
                await Task.WhenAll(
                    SidePanel.TranslateTo(0, 0, 250, Easing.CubicOut),
                    Handle.TranslateTo(260, 0, 250, Easing.CubicOut)
                );
            }

            isOpen = !isOpen;
        }
        private async Task HidePanel()
        {
            await Task.WhenAll(
                SidePanel.TranslateTo(-260, 0, 250, Easing.CubicOut),
                Handle.TranslateTo(0, 0, 250, Easing.CubicOut)
            );

            Overlay.IsVisible = false;
            isOpen = false;
        }
        private async void Overlay_Tapped(object sender, EventArgs e)
        {
            await HidePanel();
        }

        private async void SidePanel_Swiped(object sender, SwipedEventArgs e)
        {
            if (e.Direction == SwipeDirection.Left)
            {
                await HidePanel();
            }
        }




    }

}
