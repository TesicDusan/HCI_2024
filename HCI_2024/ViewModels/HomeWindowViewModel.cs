using HCI_2024.Models;
using HCI_2024.Resources.Localization;
using HCI_2024.Services;
using HCI_2024.Views.Dialogs;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HCI_2024.ViewModels
{
    public class HomeWindowViewModel : INotifyPropertyChanged
    {
        public readonly string UserId;

        private static HomeWindowViewModel _instance;
        public static HomeWindowViewModel Instance => _instance;

        private ObservableCollection<Models.OrderItem> _orderItems;
        private decimal _totalPrice;

        public ObservableCollection<Models.OrderItem> OrderItems
        {
            get => _orderItems;
            set
            {
                if (_orderItems != value)
                {
                    if (_orderItems != null)
                        _orderItems.CollectionChanged -= OrderItems_CollectionChanged;

                    _orderItems = value;
                    _orderItems.CollectionChanged += OrderItems_CollectionChanged;

                    OnPropertyChanged(nameof(OrderItems));
                    UpdateTotalPrice();
                }
            }
        }

        public decimal TotalPrice
        {
            get => _totalPrice;
            private set
            {
                if (_totalPrice != value)
                {
                    _totalPrice = value;
                    OnPropertyChanged(nameof(TotalPrice));
                }
            }
        }

        public string LocalizedTotalPrice => $"{Resources.Localization.Localizer.Instance["TotalPrice"]}: {TotalPrice:C}";

        public ICommand ShowMoviesCommand { get; }
        public ICommand ShowDrinksCommand { get; }
        public ICommand ShowSnacksCommand { get; }
        public ICommand ShowOptionsCommand { get; }
        public ICommand ExitCommand { get; }
        public ICommand ItemDeleteCommand { get; }
        public ICommand CheckoutCommand { get; }

        public ISnackbarMessageQueue MessageQueue { get; }

        public HomeWindowViewModel(string userId)
        {
            _instance = this;

            UserId = userId;

            OrderItems = new ObservableCollection<Models.OrderItem>();

            ShowMoviesCommand = new RelayCommand(o => ShowMovies?.Invoke());
            ShowDrinksCommand = new RelayCommand(o => ShowDrinks?.Invoke());
            ShowSnacksCommand = new RelayCommand(o => ShowSnacks?.Invoke());
            ShowOptionsCommand = new RelayCommand(o => ShowOptions?.Invoke());
            ExitCommand = new RelayCommand(async o => await ShowExitDialog());
            ItemDeleteCommand = new RelayCommand<Models.OrderItem>(ItemDelete);
            CheckoutCommand = new RelayCommand(Checkout);
            MessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(3));

            Resources.Localization.Localizer.Instance.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == null)
                    OnPropertyChanged(nameof(LocalizedTotalPrice));
            };

            UpdateTotalPrice();
        }

        public void AddToOrder(string name, decimal price, int itemId, ItemType type, Seat? seat)
        {
            var existingItem = OrderItems.FirstOrDefault(i => i.Name == name);
            if (existingItem != null)
            {
                existingItem.Quantity++;
                if (type == ItemType.Ticket && seat != null)
                {
                    if (existingItem.SeatIds == null)
                        existingItem.SeatIds = new ObservableCollection<int>();
                    existingItem.SeatIds.Add(seat.SeatId);
                }
            }
            else
            {
                OrderItems.Add(new Models.OrderItem
                {
                    Name = name,
                    Price = price,
                    Quantity = 1,
                    Type = type,
                    ItemId = type != ItemType.Ticket ? itemId : (int?)null,
                    ShowingId = type == ItemType.Ticket ? itemId : (int?)null,
                    SeatIds = type == ItemType.Ticket && seat != null ? new ObservableCollection<int> { seat.SeatId } : null
                });
            }
            UpdateTotalPrice();
        }

        private async void Checkout(object obj)
        {
            if (OrderItems.Count == 0)
            {
                await MaterialDesignThemes.Wpf.DialogHost.Show(
                    new ConfirmationDialog(Localizer.Instance["EmptyOrder"]),
                    "RootDialog"
                );
                return;
            }

            var sb = new StringBuilder();
            foreach (var item in OrderItems)
            {
                sb.AppendLine($"{item.Name} x{item.Quantity} - {item.TotalPrice:C}");
            }
            sb.AppendLine("-----------------------------");
            sb.AppendLine(LocalizedTotalPrice);

            var result = await MaterialDesignThemes.Wpf.DialogHost.Show(
                new ConfirmationDialog(sb.ToString()),
                "RootDialog"
            );

            if (result is bool confirmed && confirmed)
            {
                int orderId = DatabaseHelper.SaveOrderToDatabase(OrderItems, UserId, TotalPrice);
                ReceiptService.SaveReceipt(orderId, UserId, OrderItems, TotalPrice);
                OrderItems.Clear();
                UpdateTotalPrice();
            }
        }

        private void OrderItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdateTotalPrice();
        }

        private void UpdateTotalPrice()
        {
            TotalPrice = OrderItems.Sum(item => item.Price * item.Quantity);
            OnPropertyChanged(nameof(LocalizedTotalPrice));
        }

        public ObservableCollection<int>? GetSelectedSeatIds(int showingId)
        {
            var ticketItem = OrderItems.FirstOrDefault(i => i.Type == ItemType.Ticket && i.ShowingId == showingId);
            return ticketItem?.SeatIds;
        }

        public Boolean IsSeatSelected(int showingId, int seatId)
        {
            var ticketItem = OrderItems.FirstOrDefault(i => i.Type == ItemType.Ticket && i.ShowingId == showingId);
            return ticketItem != null && ticketItem.SeatIds != null && ticketItem.SeatIds.Contains(seatId);
        }

        public ObservableCollection<Seat> GetSelectedSeats(int showingId)
        {
            var ticketItem = OrderItems.FirstOrDefault(i => i.Type == ItemType.Ticket && i.ShowingId == showingId);
            if (ticketItem != null && ticketItem.SeatIds != null)
            {
                var seats = new ObservableCollection<Seat>();
                foreach (var seatId in ticketItem.SeatIds)
                {
                    seats.Add(new Seat { SeatId = seatId });
                }
                return seats;
            }
            return new ObservableCollection<Seat>();
        }

        public void RemoveFromOrder(int showingId, int seatId)
        {
            var ticketItem = OrderItems.FirstOrDefault(i => i.Type == ItemType.Ticket && i.ShowingId == showingId);
            if (ticketItem != null && ticketItem.SeatIds != null && ticketItem.SeatIds.Contains(seatId))
            {
                ticketItem.SeatIds.Remove(seatId);
                if (ticketItem.Quantity > 1)
                {
                    ticketItem.Quantity--;
                }
                else
                {
                    OrderItems.Remove(ticketItem);
                }
                UpdateTotalPrice();
            }
        }

        public event System.Action ShowMovies;
        public event System.Action ShowDrinks;
        public event System.Action ShowSnacks;
        public event System.Action ShowOptions;
        public event System.Action CloseRequested;

        private void ItemDelete(Models.OrderItem item)
        {
            if (item != null)
            {
                if(item.Quantity > 1)
                {
                    item.Quantity--;
                    UpdateTotalPrice();
                }
                else OrderItems.Remove(item);
            }
        }

        private async Task ShowExitDialog()
        {
            var result = await MaterialDesignThemes.Wpf.DialogHost.Show(
                new Views.Dialogs.ConfirmationDialog(Localizer.Instance["ExitQuestion"]),
                "RootDialog"
            );
            if (result is bool confirmed && confirmed)
            {
                CloseRequested?.Invoke();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
