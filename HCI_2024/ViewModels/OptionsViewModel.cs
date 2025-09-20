using HCI_2024.Models;
using HCI_2024.Resources.Localization;
using HCI_2024.Services;
using HCI_2024.Views.Dialogs;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HCI_2024.ViewModels
{
    public class OptionsViewModel
    {
        private bool _isAdmin;

        public bool IsAdmin
        {
            get { return _isAdmin; }
            set
            {
                _isAdmin = value;
            }
        }

        public ICommand ChangePasswordCommand { get; set; }

        public ICommand AddUserCommand { get; set; }
        public ICommand DeleteUserCommand { get; set; }
        public ICommand MakeAdminCommand { get; set; }

        public ICommand AddMovieCommand { get; set; }
        public ICommand DeleteMovieCommand { get; set; }

        public ICommand AddItemCommand { get; set; }
        public ICommand DeleteItemCommand { get; set; }

        public ICommand AddShowingCommand { get; set; }
        public ICommand DeleteShowingCommand { get; set; }

        public ICommand ViewReceiptsCommand { get; set; }

        public OptionsViewModel() 
        {
            IsAdmin = DatabaseHelper.IsAdmin(HomeWindowViewModel.Instance.UserId);

            ChangePasswordCommand = new RelayCommand(ChangePassword);

            AddUserCommand = new RelayCommand(AddUser);
            DeleteUserCommand = new RelayCommand(DeleteUser);
            MakeAdminCommand = new RelayCommand(MakeAdmin);

            AddMovieCommand = new RelayCommand(AddMovie);
            DeleteMovieCommand = new RelayCommand(DeleteMovie);

            AddItemCommand = new RelayCommand(AddItem);
            DeleteItemCommand = new RelayCommand(DeleteItem);

            AddShowingCommand = new RelayCommand(AddShowing);
            DeleteShowingCommand = new RelayCommand(DeleteShowing);

            ViewReceiptsCommand = new RelayCommand(ViewReceipts);
        }

        private async void ChangePassword(object? obj)
        {
            var result = await MaterialDesignThemes.Wpf.DialogHost.Show(
                new InputDialog(Localizer.Instance["ChangePassword"]),
                "RootDialog"
            );

            if (result is string newPin && newPin.Length == 4)
            {
                DatabaseHelper.ChangePin(HomeWindowViewModel.Instance.UserId, newPin);
                HomeWindowViewModel.Instance.MessageQueue.Enqueue(Localizer.Instance["ChangePasswordSuccess"]);
            }
            else
            {
                HomeWindowViewModel.Instance.MessageQueue.Enqueue(Localizer.Instance["ChangePasswordFail"]);
            }
        }

        private async void AddUser(object? obj)
        {
            var result = await MaterialDesignThemes.Wpf.DialogHost.Show(
                new AddUserDialog(),
                "RootDialog"
            );
            if (result is List<string> strings && strings.Count == 2)
            {
                DatabaseHelper.AddUser(strings[0], strings[1]);
                HomeWindowViewModel.Instance.MessageQueue.Enqueue(Localizer.Instance["Success"]);
            }
            else
            {
                HomeWindowViewModel.Instance.MessageQueue.Enqueue(Localizer.Instance["Fail"]);
            }
        }

        private async void DeleteUser(object? obj)
        {
            var result = await MaterialDesignThemes.Wpf.DialogHost.Show(
                new TextInputDialog(Localizer.Instance["DeleteUser"]),
                "RootDialog"
            );
            if (result is string userId && !string.IsNullOrEmpty(userId))
            {
                DatabaseHelper.DeleteUser(userId);
                HomeWindowViewModel.Instance.MessageQueue.Enqueue(Localizer.Instance["Success"]);
            }
            else
            {
                HomeWindowViewModel.Instance.MessageQueue.Enqueue(Localizer.Instance["Fail"]);
            }
        }

        private async void MakeAdmin(object? obj)
        {
            var result = await DialogHost.Show(
                new TextInputDialog(Localizer.Instance["MakeAdmin"]),
                "RootDialog"
            );
            if (result is int userId)
            {
                DatabaseHelper.MakeAdmin(userId.ToString());
                HomeWindowViewModel.Instance.MessageQueue.Enqueue(Localizer.Instance["Success"]);
            }
            else
            {
                HomeWindowViewModel.Instance.MessageQueue.Enqueue(Localizer.Instance["Fail"]);
            }
        }

        private async void AddMovie(object? obj)
        {
            var result = await MaterialDesignThemes.Wpf.DialogHost.Show(
                new AddMovieDialog(),
                "RootDialog"
            );
            if (result is List<string> parts && parts.Count() == 2)
            {
                DatabaseHelper.AddMovie(parts[0], parts[1]);
                HomeWindowViewModel.Instance.MessageQueue.Enqueue(Localizer.Instance["Success"]);
            }
            else
            {
                HomeWindowViewModel.Instance.MessageQueue.Enqueue(Localizer.Instance["Fail"]);
            }
        }

        private async void DeleteMovie(object? obj)
        {
            var result = await MaterialDesignThemes.Wpf.DialogHost.Show(
                new TextInputDialog(Localizer.Instance["DeleteMovie"]),
                "RootDialog"
            );
            if (result is string movieName && !string.IsNullOrEmpty(movieName))
            {
                DatabaseHelper.DeleteMovie(movieName);
                HomeWindowViewModel.Instance.MessageQueue.Enqueue(Localizer.Instance["Success"]);
            }
            else
            {
                HomeWindowViewModel.Instance.MessageQueue.Enqueue(Localizer.Instance["Fail"]);
            }
        }

        private async void AddItem(object? obj)
        {
            var result = await MaterialDesignThemes.Wpf.DialogHost.Show(
                new AddItemDialog(),
                "RootDialog"
            );
            if (result is List<object> parts && parts.Count() == 4)
            {
                string name = parts[0] as string;
                string pictureUrl = parts[1] as string;
                decimal price = (decimal)parts[2];
                ItemType type = (ItemType)parts[3];
                DatabaseHelper.AddItem(name, pictureUrl, price, type);
                HomeWindowViewModel.Instance.MessageQueue.Enqueue(Localizer.Instance["Success"]);
            }
            else
            {
                HomeWindowViewModel.Instance.MessageQueue.Enqueue(Localizer.Instance["Fail"]);
            }
        }

        private async void DeleteItem(object? obj)
        {
            var itemName = await DialogHost.Show(
                new TextInputDialog(Localizer.Instance["DeleteItem"]),
                "RootDialog"
            );
            if (itemName is string name && !string.IsNullOrEmpty(name))
            {
                DatabaseHelper.RemoveItem(name);
                HomeWindowViewModel.Instance.MessageQueue.Enqueue(Localizer.Instance["Success"]);
            }
            else
            {
                HomeWindowViewModel.Instance.MessageQueue.Enqueue(Localizer.Instance["Fail"]);
            }
        }

        private async void AddShowing(object? obj)
        {
            var result = await MaterialDesignThemes.Wpf.DialogHost.Show(
                new AddShowingDialog(),
                "RootDialog"
            );
            if (result is List<object> parts && parts.Count() == 3)
            {
                int movieId = DatabaseHelper.GetMovieId(parts[0] as string);
                DateTime dateTime = (DateTime)parts[1];
                decimal price = (decimal)parts[2];
                DatabaseHelper.AddShowing(movieId, dateTime, price);
                HomeWindowViewModel.Instance.MessageQueue.Enqueue(Localizer.Instance["Success"]);
            }
            else
            {
                HomeWindowViewModel.Instance.MessageQueue.Enqueue(Localizer.Instance["Fail"]);
            }
        }

        private async void DeleteShowing(object? obj)
        {
            var result = await DialogHost.Show(
                new ShowingPickerDialog(),
                "RootDialog"
            );
            if (result is int showingId)
            {
                DatabaseHelper.RemoveShowing(showingId);
                HomeWindowViewModel.Instance.MessageQueue.Enqueue(Localizer.Instance["Success"]);
            }
            else
            {
                HomeWindowViewModel.Instance.MessageQueue.Enqueue(Localizer.Instance["Fail"]);
            }
        }

        private async void ViewReceipts(object? obj)
        {
            await MaterialDesignThemes.Wpf.DialogHost.Show(
                new ReceiptsDialog(),
                "RootDialog"
            );
        }
    }
}
