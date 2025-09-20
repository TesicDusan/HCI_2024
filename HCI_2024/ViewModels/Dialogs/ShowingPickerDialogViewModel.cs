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
    public class ShowingPickerDialogViewModel : INotifyPropertyChanged
    {
        private DateTime _selectedDate;
        private ObservableCollection<MovieShowing> _showings;
        private MovieShowing? _selectedShowing;

        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                if (_selectedDate != value)
                {
                    _selectedDate = value;
                    OnPropertyChanged(nameof(SelectedDate));
                    UpdateShowings();
                }
            }
        }

        public ObservableCollection<MovieShowing> Showings
        {
            get { return _showings; }
            set
            {
                if (_showings != value)
                {
                    _showings = value;
                    OnPropertyChanged(nameof(Showings));
                }
            }
        }

        public MovieShowing? SelectedShowing
        {
            get { return _selectedShowing; }
            set
            {
                if (_selectedShowing != value)
                {
                    _selectedShowing = value;
                    OnPropertyChanged(nameof(SelectedShowing));
                }
            }
        }

        public ICommand ConfirmCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public ISnackbarMessageQueue MessageQueue { get; set; }

        public ShowingPickerDialogViewModel()
        {
            _selectedDate = DateTime.Today;
            _showings = new ObservableCollection<MovieShowing>();
            _selectedShowing = null;
            ConfirmCommand = new RelayCommand(Confirm);
            CancelCommand = new RelayCommand(Cancel);
            MessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(3));
            UpdateShowings();
        }

        private void Confirm(object? obj)
        {
            if(SelectedShowing != null)
            {
                DialogHost.CloseDialogCommand.Execute(SelectedShowing.ShowingId, null);
            }
            else
            {
                MessageQueue.Enqueue(Localizer.Instance["SelectShowing"]);
            }
        }

        private void Cancel(object? obj)
        {
            DialogHost.CloseDialogCommand.Execute(null, null);
        }

        private void UpdateShowings()
        {
            Showings = DatabaseHelper.LoadShowings(SelectedDate);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
