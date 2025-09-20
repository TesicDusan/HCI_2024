using HCI_2024.Resources.Localization;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HCI_2024.ViewModels.Dialogs
{
    public class AddMovieDialogViewModel : INotifyPropertyChanged
    {
        private string _name;
        private string _posterUrl;

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public string PosterUrl
        {
            get => _posterUrl;
            set
            {
                if (_posterUrl != value)
                {
                    _posterUrl = value;
                    OnPropertyChanged(nameof(PosterUrl));
                }
            }
        }

        public ICommand AddMovieCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public ISnackbarMessageQueue snackbarMessageQueue { get; set; }

        public AddMovieDialogViewModel()
        {
            _name = string.Empty;
            _posterUrl = string.Empty;
            AddMovieCommand = new RelayCommand(AddMovie);
            CancelCommand = new RelayCommand(Cancel);
            snackbarMessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(3));
        }

        private void AddMovie(object? obj)
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(PosterUrl))
            {
                snackbarMessageQueue.Enqueue(Localizer.Instance["AddMovieFailMessage"]);
                return;
            }

            DialogHost.CloseDialogCommand.Execute(new List<string> { Name, PosterUrl }, null);
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
