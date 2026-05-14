using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutDiary.Views
{
	public partial class StartPage : ContentPage
	{
		public StartPage ()
		{
			InitializeComponent();
            
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await Task.Delay(1500); // czas ekranu startowego
            Application.Current.MainPage = new AppShell();
        }
    }
}