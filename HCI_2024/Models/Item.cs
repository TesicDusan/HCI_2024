using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_2024.Models
{
    public class Item
    {
        public int ItemId { get; set; }

        public required string ItemName { get; set; } = string.Empty;
        public required string PictureUrl { get; set; } = string.Empty;

        public decimal Price { get; set; }
        public ItemType Type { get; set; }
    }

    
}
