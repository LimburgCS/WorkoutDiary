using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkoutDiary.data;
using WorkoutDiary.Model;

namespace WorkoutDiary.ViewModels
{
    public class AddPersonViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        readonly PersonDataBase _database;
        private ObservableCollection<Person> _persons;
        private float? _growth;
        private float? _weight;
        private int? _age;
        private int _pulse;
        private Double _indexBmi;
        private string _rateBmi;
        public ObservableCollection<Person> Person
        {
            get => _persons;
            set
            {
                _persons = value;
                OnPropertyChanged(nameof(Person));
            }
        }
        public float? Growth
        {
            get => _growth;
            set
            {
                _growth = value;
                OnPropertyChanged(nameof(Growth));
            }
        }
        public float? Weight
        {
            get => _weight;
            set
            {
                _weight = value;
                OnPropertyChanged(nameof(Weight));
            }
        }
        public int? Age
        {
            get => _age;
            set
            {
                _age = value;
                OnPropertyChanged(nameof(Age));
            }
        }



        public int Pulse
        {
            get => _pulse;
            set
            {
                _pulse = value;
                OnPropertyChanged(nameof(Pulse));
            }
        }
        public Double IndexBmi
        {
            get => _indexBmi;
            set
            {
                _indexBmi = value;
                OnPropertyChanged(nameof(IndexBmi));
                OnPropertyChanged(nameof(RateBmi));
            }
        }
        public string RateBmi
        {
            get => _rateBmi;
    
            set
            {
                _rateBmi = value;

            }
        }
        public ICommand SaveCommand { get; }
        public ICommand DeleteAll { get; }

        public AddPersonViewModel(PersonDataBase datebase)
        {
            _database = datebase;
            SaveCommand = new Command(async () => await Save());
            DeleteAll = new AsyncRelayCommand(DeleteAllAsync);
        }
        private async Task DeleteAllAsync()
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert("Potwierdzenie", "Czy na pewno chcesz usunąć wszystkie dane?", "Tak", "Nie");

            if (confirm)
            {
                await _database.DeleteInvoiceAllAsync();
                Person.Clear();
            }
        }

        private async Task Save()
        {
            var PersonDb = await _database.GetPersonAsync();

            if (PersonDb != null && PersonDb.Any())
            {
                var existingPerson = await _database.GetPersonIDAsync(1);

                if (existingPerson != null)
                {
                    existingPerson.Growth = (float)Growth;
                    existingPerson.Weight = (float)Weight;
                    existingPerson.Age = (int)Age;
                    existingPerson.Pulse = 220 - (int)Age;
                    existingPerson.IndexBmi = (float)Weight / Math.Pow(((float)Growth / 100), 2);
                    existingPerson.RateBmi = rate(existingPerson.IndexBmi);

                    


                    await _database.UpdatePersonAsync(existingPerson);
                }
            }
            else
            {

                var newExercise = new Person()
                {
                    Id = 1, 
                    Growth = (float)Growth,
                    Weight = (float)Weight,
                    Age = (int)Age,
                    Pulse = 220 - (int)Age,
                    IndexBmi = (float)Weight / Math.Pow(((float)Growth / 100), 2),
                    RateBmi = RateBmi


                };

                await _database.SavePersonAsync(newExercise);
            }
            await Shell.Current.GoToAsync("..");
        }

        private string rate(double indexBmi)
        {
            if (indexBmi < 18.5)
                return "Niedowaga";
            else if (indexBmi < 25.0)
                return "Waga prawidłowa";
            else if (indexBmi < 30.0)
                return "Nadwaga";
            else if (indexBmi < 35.0)
                return "Otyłość I stopnia";
            else if (indexBmi < 40.0)
                return "Otyłość II stopnia";
            else
                return "Otyłość III stopnia";
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
