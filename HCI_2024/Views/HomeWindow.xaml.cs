using HCI_2024.Resources.Localization;
using Org.BouncyCastle.Asn1.Cmp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace HCI_2024.Views
{
    /// <summary>
    /// Interaction logic for HomeWindow.xaml
    /// </summary>
    public partial class HomeWindow : Window
    {

        public HomeWindow(string userId)
        {
            InitializeComponent();



            var vm = new ViewModels.HomeWindowViewModel(userId);
            DataContext = vm;

            vm.ShowMovies += () => CategoryFrame.Navigate(new MoviesPage());
            vm.ShowDrinks += () => CategoryFrame.Navigate(new DrinksPage());
            vm.ShowSnacks += () => CategoryFrame.Navigate(new SnacksPage());
            vm.ShowOptions += () => CategoryFrame.Navigate(new OptionsPage());
            vm.CloseRequested += () => Application.Current.Shutdown();

            CategoryFrame.Navigate(new MoviesPage());
        }
    }
}
