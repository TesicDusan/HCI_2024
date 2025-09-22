using HCI_2024.Models;
using HCI_2024.Resources.Localization;
using HCI_2024.Services;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HCI_2024.ViewModels.Dialogs
{
    public class AddShowingDialogViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<string> _movies;
        private string _movieTitle;
        private DateTime _selectedDateTime;
        private decimal _price;

        public ObservableCollection<string> Movies
        {
            get => _movies;
            set
            {
                if (_movies != value)
                {
                    _movies = value;
                    OnPropertyChanged(nameof(Movies));
                }
            }
        }

        public string MovieTitle
        {
            get => _movieTitle;
            set
            {
                if (_movieTitle != value)
                {
                    _movieTitle = value;
                    OnPropertyChanged(nameof(MovieTitle));
                }
            }
        }

        public DateTime SelectedDateTime
        {
            get => _selectedDateTime;
            set
            {
                if (_selectedDateTime != value)
                {
                    _selectedDateTime = value;
                    OnPropertyChanged(nameof(SelectedDateTime));
                }
            }
        }

        public decimal Price
        {
            get => _price;
            set
            {
                if (_price != value)
                {
                    _price = value;
                    OnPropertyChanged(nameof(Price));
                }
            }
        }

        public ICommand AddShowingCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public ISnackbarMessageQueue MessageQueue { get; set; }

        public AddShowingDialogViewModel()
        {
            Movies = DatabaseHelper.LoadMovies();
            MovieTitle = string.Empty;
            SelectedDateTime = DateTime.Now;
            Price = 0.0m;

            MessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(3));

            AddShowingCommand = new RelayCommand(AddShowing);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void AddShowing(object? obj)
        {
            if (!string.IsNullOrWhiteSpace(MovieTitle) && Price > 0)
            {
                DialogHost.CloseDialogCommand.Execute(new List<object> { MovieTitle, SelectedDateTime, Price }, null);
            }
            else
            {
                MessageQueue.Enqueue(Localizer.Instance["AddShowingFailMessage"]);
            }
        }

        private void Cancel(object? obj)
        {
            DialogHost.CloseDialogCommand.Execute(null, null);
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
