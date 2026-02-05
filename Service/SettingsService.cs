using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutDiary.Service
{
    public static class SettingsService
    {
        private const string SelectedGymKey = "_selectedGym";
        private const string DefaultGymValue = "";

        public static string SelectedGym
        {
            get => Preferences.Get(SelectedGymKey, DefaultGymValue);
            set => Preferences.Set(SelectedGymKey, value);
        }
    }

}
