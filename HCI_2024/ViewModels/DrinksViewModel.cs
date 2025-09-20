using HCI_2024.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HCI_2024.ViewModels
{
    class DrinksViewModel
    {
        public ObservableCollection<Models.Item> Drinks { get; set; }
        public ICommand AddDrinkCommand { get; }

        public DrinksViewModel()
        {
            Drinks = DatabaseHelper.LoadItems(Models.ItemType.Drink);
            AddDrinkCommand = new RelayCommand<Models.Item>(AddDrink);
        }

        private void AddDrink(Models.Item drink)
        {
            if (drink != null)
            {
                HomeWindowViewModel.Instance.AddToOrder(drink.ItemName, drink.Price, drink.ItemId, drink.Type, null);
            }
        }
    }
}
