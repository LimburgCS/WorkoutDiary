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
using WorkoutDiary.Views;

namespace WorkoutDiary.ViewModels
{
    public class AddTrainingPlanViewModel : INotifyPropertyChanged, IQueryAttributable
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public TrainingPlanDataBase _database;
        private TrainingPlan _trainingPlan;

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

            if (_trainingPlan != null && _trainingPlan.Id != 0)
            {
                var Planexisting = await _database.GetTrainingIDAsync(1);

                if (Planexisting != null)
                {
                    Planexisting.MondayParts = MondayParts;
                    Planexisting.TuesdayParts = TuesdayParts;
                    Planexisting.WednesdayParts = WednesdayParts;
                    Planexisting.ThursdayParts = ThursdayParts;
                    Planexisting.FridayParts = FridayParts;
                    Planexisting.SaturdayParts = SaturdayParts;
                    Planexisting.SundayParts = SundayParts;




                    await _database.UpdateTrainingAsync(Planexisting);
                }
            }
            else
            {

                var newPlan = new TrainingPlan()
                {
                    Id = 1,
                    MondayParts = MondayParts,
                    TuesdayParts = TuesdayParts,
                    WednesdayParts = WednesdayParts,
                    ThursdayParts = ThursdayParts,
                    FridayParts = FridayParts,
                    SaturdayParts = SaturdayParts,
                    SundayParts = SundayParts



                };

                await _database.SaveTrainingAsync(newPlan);
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
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("planId") && int.TryParse(query["planId"].ToString(), out int planId))
            {
                _ = LoadPlan(planId);
            }
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
