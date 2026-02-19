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

        private const string SelectedPlanKey = "SelectedPlanId";
        private const int DefaultPlanValue = 1;


        public static string SelectedGym
        {
            get => Preferences.Get(SelectedGymKey, DefaultGymValue);
            set => Preferences.Set(SelectedGymKey, value);
        }
        public static int SelectPlan
        {
            get => Preferences.Get(SelectedPlanKey, DefaultPlanValue);
            set => Preferences.Set(SelectedPlanKey, value);

        }
    }

}
