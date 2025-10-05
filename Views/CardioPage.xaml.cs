using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkoutDiary.data;
using WorkoutDiary.Model;
using WorkoutDiary.ViewModels;

namespace WorkoutDiary.Views
{
	public partial class CardioPage : ContentPage
	{
        public PersonDataBase database;
        public CardioDataBase cardioDataBase;
        public ObservableCollection<Person> person { get; set; }
        public ObservableCollection<Cardio> cardio { get; set; }


        public CardioPage ()
		{
            InitializeComponent();
            database = new PersonDataBase();
            cardioDataBase = new CardioDataBase();
            BindingContext = new CardioViewModel(database, cardioDataBase);
            person = new ObservableCollection<Person>();
            cardio = new ObservableCollection<Cardio>();
            OnAppearing();
            
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Zakładamy, że ViewModel ma metodę odświeżania danych
            if (BindingContext is CardioViewModel viewModel)
            {
                viewModel.loadPersonData();
                viewModel.loadCardioData();
            }
        }
        private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
        {
            CardioCollectionView.SelectedItem = null;
        }
        private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            if (BindingContext is CardioViewModel viewModel)
            {
                DateTime date = datepicker.Date;
                viewModel.ChooseDate(date);
            }


        }

    }
}