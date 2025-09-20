using HCI_2024.Resources.Localization;
using HCI_2024.Resources.Themes;
using HCI_2024.Services;
using HCI_2024.Views;
using HCI_2024.Views.Dialogs;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HCI_2024.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public ICommand ExitCommand { get; }
        public ICommand LoginCommand { get; }
        public ISnackbarMessageQueue MessageQueue { get; }

        private string _userId = string.Empty;
        public string UserId
        {
            get => _userId;
            set 
            { 
                if (_userId != value)
                {
                    _userId = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _pin = string.Empty;
        public string Pin
        {
            get => _pin;
            set 
            { 
                if (_pin != value)
                {
                    _pin = value;
                    OnPropertyChanged();
                }
            }
        }

        public MainWindowViewModel()
        {
            ExitCommand = new RelayCommand(_ => ShowExitDialog());
            LoginCommand = new RelayCommand(_ => Login());
            MessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(3));

            ThemeManager.ApplyTheme("LightTheme");
        }

        private async void ShowExitDialog()
        {
            bool shouldExit = await Services.DialogService.ShowConfirmation(Localizer.Instance["ExitQuestion"]);
            if (shouldExit)
            {
                Application.Current.Shutdown();
            }
        }

        public void Login()
        {
            if (DatabaseHelper.IsUserValid(UserId?.Trim(), Pin?.Trim()))
            {
                string lang = DatabaseHelper.GetLanguagePreference(UserId?.Trim());
                string theme = DatabaseHelper.GetThemePreference(UserId?.Trim());

                Localizer.ChangeLanguage(lang);
                ThemeManager.ApplyTheme(theme);

                var homeWindow = new HomeWindow(UserId);
                homeWindow.Show();

                Application.Current.Windows
                    .OfType<Window>()
                    .FirstOrDefault(w => w is MainWindow)?
                    .Close();
            }
            else
            {
                MessageQueue.Enqueue("Invalid User ID or PIN.");
            }
        }

        
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
