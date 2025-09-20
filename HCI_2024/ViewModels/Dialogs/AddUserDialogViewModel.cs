using HCI_2024.Resources.Localization;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HCI_2024.ViewModels.Dialogs
{
    public class AddUserDialogViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public ISnackbarMessageQueue MessageQueue { get; set; }

        public ICommand AddUserCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public AddUserDialogViewModel()
        {
            UserName = string.Empty;
            Password = string.Empty;

            AddUserCommand = new RelayCommand(AddUser);
            CancelCommand = new RelayCommand(Cancel);

            MessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(3));
        }

        private void AddUser(object? obj)
        {
            if (UserName.Length == 4 && Password.Length == 4)
            {
                DialogHost.CloseDialogCommand.Execute(new ObservableCollection<string> { UserName, Password }, null);
            }
            else
            {
                MessageQueue.Enqueue(Localizer.Instance["AddUserFailMessage"]);
            }
        }

        private void Cancel(object? obj)
        {
            DialogHost.CloseDialogCommand.Execute(null, null);
        }
    }
}
