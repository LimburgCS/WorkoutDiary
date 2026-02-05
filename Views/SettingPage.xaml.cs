using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutDiary.data;
using WorkoutDiary.ViewModels;

namespace WorkoutDiary.Views
{
    public partial class SettingPage : ContentPage
    {
        public SettingPage()
        {
            InitializeComponent();
            BindingContext = new SettingViewModel();
        }

        private void ChangeThemeButton_Clicked(object sender, EventArgs e)
        {

            ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            if (mergedDictionaries != null)
            {
                mergedDictionaries.Clear();

                // Pobranie zapisanej wartości
                string savedThemeString = Preferences.Get("AppTheme", "false"); // Pobranie jako string
                bool darkTheme = bool.TryParse(savedThemeString, out bool isDarkTheme) && isDarkTheme;


                // Zmiana motywu
                darkTheme = !darkTheme;
                Preferences.Set("AppTheme", darkTheme.ToString()); // Zapisanie wyboru

                if (darkTheme)
                {
                    mergedDictionaries.Add(new DarkTheme());
                }
                else
                {
                    mergedDictionaries.Add(new LightTheme());
                }


            }

        }
    }

}