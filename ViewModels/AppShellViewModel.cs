
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkoutDiary.data;
using WorkoutDiary.Model;

namespace WorkoutDiary.ViewModels
{
    public class AppShellViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        readonly TodoItemDatabase _database;
        public MainPageViewModel _mainpage;
        public event EventHandler DataDeleted;

        private void OnDataDeleted()
        {
            DataDeleted?.Invoke(this, EventArgs.Empty);
        }
        public ICommand DeleteAll { get; }
        private ObservableCollection<BodyParts> _bodyParts;
        public ObservableCollection<BodyParts> BodyParts
        {
            get => _bodyParts;
            set
            {
                _bodyParts = value;

            }
        }
        public AppShellViewModel()
        {
            _database = new TodoItemDatabase();
            BodyParts = new ObservableCollection<BodyParts>();
            DeleteAll = new AsyncRelayCommand(DeleteAllAsync);
        }

        private async Task DeleteAllAsync()
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert("Potwierdzenie", "Czy na pewno chcesz usunąć wszystkie dane?", "Tak", "Nie");

            if (confirm)
            {
                await _database.DeleteInvoiceAllAsync();
                BodyParts.Clear();
            }
            OnDataDeleted();
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
