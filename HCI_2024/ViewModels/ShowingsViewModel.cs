using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Windows.Input;
using HCI_2024.Services;
using MaterialDesignThemes.Wpf;
using HCI_2024.Views.Dialogs;
using HCI_2024.Models;

namespace HCI_2024.ViewModels
{
    public class ShowingsViewModel
    {
        public ObservableCollection<Models.MovieShowing> Showings { get; set; }
        public ICommand AddTicketCommand { get; }
        public ICommand SelectDateCommand { get; }

        public ShowingsViewModel()
        {
            Showings = DatabaseHelper.LoadShowings(DateTime.Now);

            AddTicketCommand = new RelayCommand<Models.MovieShowing>(async showing => await AddTicketAsync(showing));
            SelectDateCommand = new RelayCommand(async o => await SelectDateAsync());

        }

        private async Task AddTicketAsync(Models.MovieShowing showing)
        {
            if (showing != null)
            {
                var ticketDialog = new TicketsDialog(showing.ShowingId, HomeWindowViewModel.Instance.GetSelectedSeatIds(showing.ShowingId));

                var result = await DialogHost.Show(ticketDialog, "RootDialog");

                if (result is List<Models.Seat> selectedSeats && selectedSeats.Any())
                {
                    foreach (var seat in HomeWindowViewModel.Instance.GetSelectedSeats(showing.ShowingId).ToList())
                    {
                        HomeWindowViewModel.Instance.RemoveFromOrder(showing.ShowingId, seat.SeatId);
                    }

                    foreach (var seat in selectedSeats)
                    {
                        if (!HomeWindowViewModel.Instance.IsSeatSelected(showing.ShowingId, seat.SeatId))
                            HomeWindowViewModel.Instance.AddToOrder(showing.MovieName + " " + showing.StartTime.ToString(), showing.TicketPrice, showing.ShowingId, ItemType.Ticket, seat);
                    }
                }
            }
        }

        private async Task SelectDateAsync()
        {
            Showings.Clear();
            var dateDialog = new DatePickerDialog();
            var result = await DialogHost.Show(dateDialog, "RootDialog");

            if (result is DateTime selectedDate)
            {
                var showings = DatabaseHelper.LoadShowings(selectedDate);
                foreach (var showing in showings)
                {
                    Showings.Add(showing);
                }
            }
        }
    }
}
