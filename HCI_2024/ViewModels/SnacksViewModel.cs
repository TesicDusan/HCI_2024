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
    class SnacksViewModel
    {
        public ObservableCollection<Models.Item> Snacks { get; set; }
        public ICommand AddSnackCommand { get; }

        public SnacksViewModel()
        {
            Snacks = DatabaseHelper.LoadItems(Models.ItemType.Snack);
            AddSnackCommand = new RelayCommand<Models.Item>(AddSnack);
        }

        private void AddSnack(Models.Item snack)
        {
            if (snack != null)
            {
                HomeWindowViewModel.Instance.AddToOrder(snack.ItemName, snack.Price, snack.ItemId, snack.Type, null);
            }
        }
    }
}
