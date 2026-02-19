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
            GenerateWeeklyPlan(plan);

            // Tworzysz nowy plan w bazie
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

            // Zapisujesz ID do pamięci
            SettingsService.SelectPlan = newPlan.Id;

            // Ustawiasz _trainingPlan, żeby Save() wiedziało, że to edycja
            _trainingPlan = newPlan;

        }
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

        private void GenerateWeeklyPlan(ReadyTrainingPlan plan)
        {
            if (plan == null)
                return;

            var week = new[]
            {
        nameof(MondayParts),
        nameof(TuesdayParts),
        nameof(WednesdayParts),
        nameof(ThursdayParts),
        nameof(FridayParts),
        nameof(SaturdayParts),
        nameof(SundayParts)
    };

            // Reset
            MondayParts = TuesdayParts = WednesdayParts = "";
            ThursdayParts = FridayParts = SaturdayParts = SundayParts = "";

            for (int i = 0; i < plan.DaysPerWeek; i++)
            {
                var parts = string.Join(", ", plan.WeeklySplit[i]);
                var day = week[i];

                switch (day)
                {
                    case nameof(MondayParts): MondayParts = parts; break;
                    case nameof(TuesdayParts): TuesdayParts = parts; break;
                    case nameof(WednesdayParts): WednesdayParts = parts; break;
                    case nameof(ThursdayParts): ThursdayParts = parts; break;
                    case nameof(FridayParts): FridayParts = parts; break;
                    case nameof(SaturdayParts): SaturdayParts = parts; break;
                    case nameof(SundayParts): SundayParts = parts; break;
                }
            }
        }

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
