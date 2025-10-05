using System.Globalization;

namespace WorkoutDiary
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
            Application.Current.UserAppTheme = AppTheme.Light;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("pl-PL");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("pl-PL");
            ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;

            if (mergedDictionaries != null)
            {
                mergedDictionaries.Clear();
                mergedDictionaries.Add(new LightTheme()); // Dodanie jasnego motywu
            }

            //string savedThemeString = Preferences.Get("AppTheme", "false");
            //bool savedTheme = bool.TryParse(savedThemeString, out bool isDarkTheme) && isDarkTheme;

            //ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;

            //if (mergedDictionaries != null)
            //{
            //    mergedDictionaries.Clear();
            //    mergedDictionaries.Add(savedTheme ? new DarkTheme() : new LightTheme());
            //}




        }


    }
}

