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
    public class DatePickerDialogViewModel : INotifyPropertyChanged
    {
        private DateTime? _selectedDate;

        public DateTime? SelectedDate
        {
            get => _selectedDate;
            set
            {
                if (_selectedDate != value)
                {
                    _selectedDate = value;
                    OnPropertyChanged(nameof(SelectedDate));
                }
            }
        }

        public ICommand ConfirmCommand { get; }

        public DatePickerDialogViewModel()
        {
            ConfirmCommand = new RelayCommand<object>(ConfirmSelection);
        }

        private void ConfirmSelection(object obj)
        {
            DialogHost.CloseDialogCommand.Execute(SelectedDate, null);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
