
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
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
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public TodoItemDatabase _database;
        private ObservableCollection<BodyParts> _bodyParts;
        public ObservableCollection<BodyParts> BodyParts
        {
            get => _bodyParts;
            set
            {
                _bodyParts = value;

            }
        }
        public ICommand NewExercise { get; }
        public ICommand SelectDayCommand { get; }
        public MainPageViewModel(TodoItemDatabase database)
        {
            _database = database;
            BodyParts = new ObservableCollection<BodyParts>();
            NewExercise = new AsyncRelayCommand(NewExerciseAsync);
            SelectDayCommand = new Command<BodyParts>(async (BodyParts) => await SelectExerciseAsync(BodyParts));
            loadInvoice();
        }

        public async void loadInvoice()
        {
            var exerciseFromDb = await _database.GetInvoiceAsync();

            BodyParts = new ObservableCollection<BodyParts>(exerciseFromDb);
            OnPropertyChanged(nameof(BodyParts));


        }
        private async Task SelectExerciseAsync(BodyParts selectedExercise)
        {
            if (selectedExercise != null)
            {
                // Użyj identyfikatora lub innego klucza zamiast całego obiektu
                await Shell.Current.GoToAsync($"{nameof(AddExercisePage)}?exerciseId={selectedExercise.Id}");
            }
        }
        private async Task NewExerciseAsync()
        {
            await Shell.Current.GoToAsync(nameof(AddExercisePage));
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
