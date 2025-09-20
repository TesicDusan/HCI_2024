using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_2024.Models
{
    public class MovieShowing
    {
        public int ShowingId { get; set; }
        public int MovieId { get; set; }    

        public string MovieName { get; set; } = string.Empty;
        public string PosterUrl { get; set; } = string.Empty;

        public DateTime StartTime { get; set; }
        public decimal TicketPrice { get; set; }

        public double OccupancyRate { get; set; }

        public string DisplayText => $"{MovieName} - {StartTime:dd.MM / HH:mm}";
    }
}
