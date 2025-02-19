using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkoutDiary.data;
using WorkoutDiary.Model;

namespace WorkoutDiary.ViewModels
{
    public class AddExerciseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        readonly TodoItemDatabase _database;
        private BodyParts _bodyPart;
        private string _part;
        private int _series;
        private int _repetitions;
        private double _weight;

        public string Part
        {
            get => _part;
            set
            {
                _part = value;
                OnPropertyChanged(nameof(Part));
            }
        }
        public int Series
        {
            get => _series;
            set
            {
                _series = value;
                OnPropertyChanged(nameof(Series));
            }
        }
        public int Repetitions
        {
            get => _repetitions;
            set
            {
                _repetitions = value;
                OnPropertyChanged(nameof(Repetitions));
            }
        }
        public double Weight
        {
            get => _weight;
            set
            {
                _weight = value;
                OnPropertyChanged(nameof(Weight));
            }
        }
        public ICommand SaveCommand { get; }
        public AddExerciseViewModel(TodoItemDatabase database)
        {
            _database = database;
            SaveCommand = new Command(async () => await Save());
        }


        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("exerciseId") && int.TryParse(query["exerciseId"].ToString(), out int exerciseId))
            {
                LoadInvoice(exerciseId);
            }
        }
        public async void LoadInvoice(int exerciseId)
        {
            _bodyPart = await _database.GetInvoiceIDAsync(exerciseId); // Zakładam, że masz taką metodę w klasie Database

            if (_bodyPart != null)
            {
                Part = _bodyPart.Part;
                Series = _bodyPart.Series;
                Repetitions = _bodyPart.Repetitions;
                Weight = _bodyPart.Weight;

                // Notyfikacja o zmianie właściwości, aby zaktualizować widok
                OnPropertyChanged(nameof(Series));
                OnPropertyChanged(nameof(Repetitions));
                OnPropertyChanged(nameof(Weight));
            }
        }


        private async Task Save()
        {
            if (_bodyPart != null && _bodyPart.Id != 0)
            {
                // Aktualizuj istniejącą fakturę
                var existingBodyPart = await _database.GetInvoiceIDAsync(_bodyPart.Id);

                if (existingBodyPart != null)
                {
                    existingBodyPart.Part = Part;
                    existingBodyPart.Series = Series;
                    existingBodyPart.Repetitions = Repetitions;
                    existingBodyPart.Weight = Weight;
                    existingBodyPart.DateTime = DateTime.Today;

                    await _database.UpdateInvoiceAsync(existingBodyPart);
                }
            }
            else
            {
                // Tworzenie nowej faktury
                var newExercise = new BodyParts()
                {
                    Id = (await _database.GetInvoiceAsync()).Count + 1, // lub zastosuj logikę generowania ID
                    Part = Part,
                    Series = Series,
                    Repetitions = Repetitions,
                    Weight = Weight,
                    DateTime = DateTime.Today,

                };

                await _database.SaveInvoiceAsync(newExercise);
            }
            await Shell.Current.GoToAsync("..");
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
