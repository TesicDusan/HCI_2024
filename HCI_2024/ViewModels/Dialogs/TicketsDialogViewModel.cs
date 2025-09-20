using HCI_2024.Services;
using MaterialDesignThemes.Wpf;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HCI_2024.ViewModels.Dialogs
{
    public class TicketsDialogViewModel : INotifyPropertyChanged
    {
        private readonly int _showingId;

        private int _ticketQuantity = 1;
        public int TicketQuantity
        {   
            get => _ticketQuantity;
            set
            {
                if (_ticketQuantity != value)
                {
                    _ticketQuantity = value;
                    OnPropertyChanged(nameof(TicketQuantity));
                    UpdateSeatSelectionLimit();
                }
            }
        }

        public ObservableCollection<Models.Seat> Seats { get; set; }

        public ICommand ToggleSeatCommand { get; }
        public ICommand ConfirmCommand { get; }

        public TicketsDialogViewModel(int showingId, ObservableCollection<int>? seatIds)
        {
            _showingId = showingId;
            Seats = DatabaseHelper.LoadSeats(showingId);

            ToggleSeatCommand = new RelayCommand<Models.Seat>(ToggleSeat);
            ConfirmCommand = new RelayCommand<object>(ConfirmSelection);

            foreach (var seat in Seats)
            {
                if (seatIds != null && seatIds.Contains(seat.SeatId))
                {
                    seat.IsSelected = true;
                }
            }
        }

        private void ToggleSeat(Models.Seat seat)
        {
            if (seat == null || !seat.IsAvailable)
                return;

            if (seat.IsSelected)
            {
                seat.IsSelected = false;
                return;
            }
            
            foreach (var s in Seats.Where(s => s.IsSelected))
                s.IsSelected = false;

            int startIndex = Seats.IndexOf(seat);
            
            for (int i = 0; i < TicketQuantity; i++)
            {
                int index = startIndex + i;
                if (index < Seats.Count && Seats[index].IsAvailable)
                {
                    Seats[index].IsSelected = true;
                }
                else
                {
                    break;
                }
            }
        }

        private void UpdateSeatSelectionLimit()
        {
            int selectedCount = Seats.Count(s => s.IsSelected);
            if (selectedCount > TicketQuantity)
            {
                foreach (var seat in Seats.Where(s => s.IsSelected).Skip(TicketQuantity))
                {
                    seat.IsSelected = false;
                }
                OnPropertyChanged(nameof(Seats));
            }
        }

        private void ConfirmSelection(object obj)
        {
            var selectedSeats = Seats.Where(s => s.IsSelected).ToList();
            DialogHost.CloseDialogCommand.Execute(selectedSeats, null);
        }

        

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
