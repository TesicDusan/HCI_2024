using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HCI_2024.Resources.Localization;
using MaterialDesignThemes.Wpf;

namespace HCI_2024.Views
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ViewModels.MainWindowViewModel();
            txtUserID.Focus();
        }

        private void NumericInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.All(char.IsDigit);
        }

        private void UserId_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtUserID.Text.Length == 4)
            {
                if(DataContext is ViewModels.MainWindowViewModel vm)
                {
                    vm.UserId = txtUserID.Text.Trim();
                }
                txtPin.Focus();
            }
        }

        private void Pin_TextChanged(object sender, RoutedEventArgs e)
        {
            if (txtPin.Password.Length == 4)
            {
                if(DataContext is ViewModels.MainWindowViewModel vm)
                {
                    vm.Pin = txtPin.Password.Trim();
                    vm.Login();
                }
            }
        }

        private void Keypad_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                string number = button.Content.ToString();
                if (txtUserID.Text.Length < 4)
                {
                    txtUserID.Text += number;
                }
                else if (txtPin.Password.Length < 4)
                {
                    txtPin.Password += number;
                }
            }
        }

        private void Backspace_Click(object sender, RoutedEventArgs e)
        {

            if (txtPin.Password.Length > 0)
            {
                txtPin.Password = txtPin.Password.Remove(txtPin.Password.Length - 1);
            }
            else if (txtUserID.Text.Length > 0)
            {
                txtUserID.Text = txtUserID.Text.Remove(txtUserID.Text.Length - 1);
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void LanguageSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.SelectedValue is string cultureCode)
            {
                    Localizer.ChangeLanguage(cultureCode);
            }
        }
    }
}