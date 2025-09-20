using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_2024.Models
{
    public class OrderItem : INotifyPropertyChanged
    {
        private string _name;
        private decimal _price;
        private int _quantity;

        public string Name { get => _name; set { _name = value; OnPropertyChanged(); } }
        public decimal Price { get => _price; set { _price = value; OnPropertyChanged(); } }
        public int Quantity { get => _quantity; set { _quantity = value; OnPropertyChanged(); } }

        public decimal TotalPrice => Price * Quantity;

        public ItemType Type { get; set; }

        public int? ItemId { get; set; }
        public int? ShowingId { get; set; }
        public ObservableCollection<int>? SeatIds { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
