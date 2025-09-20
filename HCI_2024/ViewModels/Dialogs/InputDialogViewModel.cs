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
    public class InputDialogViewModel : INotifyPropertyChanged
    {
        private string _message;
        private string _inputText;

        public string Message
        {
            get => _message;
            set
            {
                if (_message != value)
                {
                    _message = value;
                    OnPropertyChanged(nameof(Message));
                }
            }
        }

        public string InputText
        {
            get => _inputText;
            set
            {
                if (_inputText != value)
                {
                    _inputText = value;
                    OnPropertyChanged(nameof(InputText));
                }
            }
        }

        public ICommand ConfirmCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public InputDialogViewModel(string message)
        {
            Message = message;

            ConfirmCommand = new RelayCommand(ConfirmInput);
            CancelCommand = new RelayCommand(CancelInput);
        }

        private void ConfirmInput(object? obj)
        {
            if (!string.IsNullOrWhiteSpace(InputText) && InputText.Length == 4)
            {
                DialogHost.CloseDialogCommand.Execute(InputText, null);
            }
        }

        private void CancelInput(object? obj)
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
