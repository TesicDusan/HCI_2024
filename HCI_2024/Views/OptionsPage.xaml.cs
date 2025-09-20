using HCI_2024.Resources.Localization;
using HCI_2024.Resources.Themes;
using HCI_2024.Services;
using HCI_2024.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HCI_2024.Views
{
    /// <summary>
    /// Interaction logic for OptionsPage.xaml
    /// </summary>
    public partial class OptionsPage : Page
    {
        public OptionsPage()
        {
            InitializeComponent();
            DataContext = new ViewModels.OptionsViewModel();
        }

        public void LanguageSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.SelectedValue is string cultureCode)
            {
                Localizer.ChangeLanguage(cultureCode);

                DatabaseHelper.SaveLanguagePreference(HomeWindowViewModel.Instance.UserId, cultureCode);
            }
        }

        public void ThemeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.SelectedValue is string theme)
            {
                ThemeManager.ApplyTheme(theme);
                DatabaseHelper.SaveThemePreference(HomeWindowViewModel.Instance.UserId, theme);
            }
        }
    }
}
