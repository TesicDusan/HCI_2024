using HCI_2024.Models;
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
    public class AddItemDialogViewModel : INotifyPropertyChanged
    {
        private string _name;
        private string _pictureUrl;
        private decimal _price;
        private List<ItemType> _itemTypes;
        private ItemType _selectedItemType;

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public string PictureUrl
        {
            get => _pictureUrl;
            set
            {
                if (_pictureUrl != value)
                {
                    _pictureUrl = value;
                    OnPropertyChanged(nameof(PictureUrl));
                }
            }
        }

        public decimal Price
        {
            get => _price;
            set
            {
                if (_price != value)
                {
                    _price = value;
                    OnPropertyChanged(nameof(Price));
                }
            }
        }

        public List<ItemType> ItemTypes
        {             
            get => _itemTypes;
            set
            {
                if (_itemTypes != value)
                {
                    _itemTypes = value;
                    OnPropertyChanged(nameof(ItemTypes));
                }
            }
        }

        public ItemType SelectedItemType
        {
            get => _selectedItemType;
            set
            {
                if (_selectedItemType != value)
                {
                    _selectedItemType = value;
                    OnPropertyChanged(nameof(SelectedItemType));
                }
            }
        }

        public ICommand AddItemCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public ISnackbarMessageQueue SnackbarMessageQueue { get; set; }

        public AddItemDialogViewModel()
        {
            Name = string.Empty;
            PictureUrl = string.Empty;
            Price = 0;
            ItemTypes = Enum.GetValues(typeof(ItemType)).Cast<ItemType>().ToList();
            ItemTypes.Remove(ItemType.Ticket);
            SelectedItemType = ItemTypes.First();

            SnackbarMessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(3));
            AddItemCommand = new RelayCommand(AddItem);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void AddItem(object obj)
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(PictureUrl) || Price <= 0)
            {
                SnackbarMessageQueue.Enqueue("Please fill in all fields with valid data.");
                return;
            }

            DialogHost.CloseDialogCommand.Execute(new List<object>{ Name, PictureUrl, Price, SelectedItemType }, null);
        }

        private void Cancel(object obj)
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
