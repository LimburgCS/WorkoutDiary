using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutDiary.data;
using WorkoutDiary.ViewModels;


namespace WorkoutDiary.Views
{
	public partial class AddPersonPage : ContentPage
	{
        //private readonly AddPersonViewModel _viewmodel;
        PersonDataBase database;
        public AddPersonPage ()
		{
            InitializeComponent();

            database = new PersonDataBase();
            BindingContext = new AddPersonViewModel(database);
            //LoadPart();
        }
	}
}