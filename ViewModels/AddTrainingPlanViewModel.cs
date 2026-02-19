using LiveChartsCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkoutDiary.data;
using WorkoutDiary.Model;
using WorkoutDiary.Service;
using WorkoutDiary.Views;

namespace WorkoutDiary.ViewModels
{
    public class AddTrainingPlanViewModel : INotifyPropertyChanged, IQueryAttributable
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public TrainingPlanDataBase _database;
        private TrainingPlan _trainingPlan;
        public ReadyTrainingPlan SelectedPlan { get; set; }

        private string _sundayParts;
        public string SundayParts
        {
            get => _sundayParts;
            set
            {
                _sundayParts = value;
                OnPropertyChanged();
            }
        }
        private string _saturdayParts;
        public string SaturdayParts
        {
            get => _saturdayParts;
            set
            {
                _saturdayParts = value;
                OnPropertyChanged();
            }
        }
        private string _fridayParts;
        public string FridayParts
        {
            get => _fridayParts;
            set
            {
                _fridayParts = value;
                OnPropertyChanged();
            }
        }
        private string _thursdayParts;
        public string ThursdayParts
        {
            get => _thursdayParts;
            set
            {
                _thursdayParts = value;
                OnPropertyChanged();
            }
        }
        private string _wednesdayParts;
        public string WednesdayParts
        {
            get => _wednesdayParts;
            set
            {
                _wednesdayParts = value;
                OnPropertyChanged();
            }
        }
        private string _tuesdayParts;
        public string TuesdayParts
        {
            get => _tuesdayParts;
            set
            {
                _tuesdayParts = value;
                OnPropertyChanged();
            }
        }
        private string _mondayParts;
        public string MondayParts
        {
            get => _mondayParts;
            set
            {
                _mondayParts = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveCommand { get; }
        public AddTrainingPlanViewModel(TrainingPlanDataBase database)
        {
            _database = database;
            SaveCommand = new Command(async () => await Save());
        }



        private async Task Save()
        {
            // Jeśli edytujemy istniejący plan
            if (_trainingPlan != null && _trainingPlan.Id != 0)
            {
                _trainingPlan.MondayParts = MondayParts;
                _trainingPlan.TuesdayParts = TuesdayParts;
                _trainingPlan.WednesdayParts = WednesdayParts;
                _trainingPlan.ThursdayParts = ThursdayParts;
                _trainingPlan.FridayParts = FridayParts;
                _trainingPlan.SaturdayParts = SaturdayParts;
                _trainingPlan.SundayParts = SundayParts;

                await _database.UpdateTrainingAsync(_trainingPlan);
            }
            else
            {
                // Tworzymy nowy plan
                var newPlan = new TrainingPlan
                {

                    MondayParts = MondayParts,
                    TuesdayParts = TuesdayParts,
                    WednesdayParts = WednesdayParts,
                    ThursdayParts = ThursdayParts,
                    FridayParts = FridayParts,
                    SaturdayParts = SaturdayParts,
                    SundayParts = SundayParts
                };


                await _database.SaveTrainingAsync(newPlan);

                // Zapisz ID nowego planu do pamięci urządzenia
                SettingsService.SelectPlan = newPlan.Id;
            }

            await Shell.Current.GoToAsync("..");
        }

        public async Task LoadPlan(int planId)
        {
            _trainingPlan = await _database.GetTrainingIDAsync(planId);
            if (_trainingPlan != null)
            {
                
                MondayParts = _trainingPlan.MondayParts;
                TuesdayParts = _trainingPlan.TuesdayParts;
                WednesdayParts = _trainingPlan.WednesdayParts;
                ThursdayParts = _trainingPlan.ThursdayParts;
                FridayParts = _trainingPlan.FridayParts;
                SaturdayParts = _trainingPlan.SaturdayParts;
                SundayParts = _trainingPlan.SundayParts;

                OnPropertyChanged(nameof(MondayParts));
                OnPropertyChanged(nameof(TuesdayParts));
                OnPropertyChanged(nameof(WednesdayParts));
                OnPropertyChanged(nameof(ThursdayParts));
                OnPropertyChanged(nameof(FridayParts));
                OnPropertyChanged(nameof(SaturdayParts));
                OnPropertyChanged(nameof(SundayParts));

            }
        }

        public async Task LoadReadyPlan(int planId)
        {
            // Pobieramy plan z listy gotowych planów
            var plan = ReadyTrainingPlan.DefaultPlans.FirstOrDefault(p => p.Id == planId);
            if (plan == null)
                return;

            // Generujemy tygodniowy plan
            //GenerateWeeklyPlan(plan);

            var map = DayMapping[plan.DaysPerWeek];

            var newPlan = new TrainingPlan
            {
                MondayParts = map.Contains(0) ? ConvertDayToString(plan.WeeklySplit[Array.IndexOf(map, 0)]) : "",
                TuesdayParts = map.Contains(1) ? ConvertDayToString(plan.WeeklySplit[Array.IndexOf(map, 1)]) : "",
                WednesdayParts = map.Contains(2) ? ConvertDayToString(plan.WeeklySplit[Array.IndexOf(map, 2)]) : "",
                ThursdayParts = map.Contains(3) ? ConvertDayToString(plan.WeeklySplit[Array.IndexOf(map, 3)]) : "",
                FridayParts = map.Contains(4) ? ConvertDayToString(plan.WeeklySplit[Array.IndexOf(map, 4)]) : "",
                SaturdayParts = map.Contains(5) ? ConvertDayToString(plan.WeeklySplit[Array.IndexOf(map, 5)]) : "",
                SundayParts = ""
            };


            await _database.SaveTrainingAsync(newPlan);

            // Zapisujesz ID do pamięci
            SettingsService.SelectPlan = newPlan.Id;

            // Ustawiasz _trainingPlan, żeby Save() wiedziało, że to edycja
            _trainingPlan = newPlan;
        }

        private string ConvertDayToString(TrainingDay day)
        {
            var lines = new List<string>();

            foreach (var part in day.Parts)
            {
                lines.Add($"{part.Part.ToUpper()}: {string.Join(", ", part.Exercises)}");
            }

            return string.Join("\n", lines);
        }

        private void GenerateWeeklyPlan(ReadyTrainingPlan plan)
        {
            if (plan == null)
                return;

            MondayParts = plan.DaysPerWeek > 0 ? ConvertDayToString(plan.WeeklySplit[0]) : "";
            TuesdayParts = plan.DaysPerWeek > 1 ? ConvertDayToString(plan.WeeklySplit[1]) : "";
            WednesdayParts = plan.DaysPerWeek > 2 ? ConvertDayToString(plan.WeeklySplit[2]) : "";
            ThursdayParts = plan.DaysPerWeek > 3 ? ConvertDayToString(plan.WeeklySplit[3]) : "";
            FridayParts = plan.DaysPerWeek > 4 ? ConvertDayToString(plan.WeeklySplit[4]) : "";
            SaturdayParts = plan.DaysPerWeek > 5 ? ConvertDayToString(plan.WeeklySplit[5]) : "";
            SundayParts = "";
        }

        private static readonly Dictionary<int, int[]> DayMapping = new()
{
    { 2, new[] { 0, 3 } },             // Poniedziałek, Czwartek
    { 3, new[] { 0, 2, 4 } },          // Poniedziałek, Środa, Piątek
    { 4, new[] { 0, 1, 3, 4 } },       // Pon, Wt, Czw, Pt
    { 5, new[] { 0, 1, 2, 3, 4 } },    // Pon–Pt
    { 6, new[] { 0, 1, 2, 3, 4, 5 } }  // Pon–Sob
};
        //public async Task LoadTrainingPlan(int planId)
        //{
        //    _trainingPlan = await _database.GetTrainingIDAsync(planId);

        //    if (_trainingPlan != null)
        //    {
        //        MondayParts = _trainingPlan.MondayParts;
        //        TuesdayParts = _trainingPlan.TuesdayParts;
        //        WednesdayParts = _trainingPlan.WednesdayParts;
        //        ThursdayParts = _trainingPlan.ThursdayParts;
        //        FridayParts = _trainingPlan.FridayParts;
        //        SaturdayParts = _trainingPlan.SaturdayParts;
        //        SundayParts = _trainingPlan.SundayParts;

        //        OnPropertyChanged(nameof(MondayParts));
        //        OnPropertyChanged(nameof(TuesdayParts));
        //        OnPropertyChanged(nameof(WednesdayParts));
        //        OnPropertyChanged(nameof(ThursdayParts));
        //        OnPropertyChanged(nameof(FridayParts));
        //        OnPropertyChanged(nameof(SaturdayParts));
        //        OnPropertyChanged(nameof(SundayParts));
        //    }
        //}


        //public void ApplyQueryAttributes(IDictionary<string, object> query)
        //{
        //    if (query.ContainsKey("planId") && int.TryParse(query["planId"].ToString(), out int planId))
        //    {
        //        _ = LoadPlan(planId);
        //    }
        //}
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("planId"))
            {
                int id = int.Parse(query["planId"].ToString());

                // Jeśli ID <= liczba gotowych planów → to gotowy plan
                if (ReadyTrainingPlan.DefaultPlans.Any(p => p.Id == id))
                {
                    _ = LoadReadyPlan(id);
                }
                else
                {
                    _ = LoadPlan(id);
                }
            }

        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
