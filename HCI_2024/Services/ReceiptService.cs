using HCI_2024.Models;
using HCI_2024.Resources.Localization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_2024.Services
{
    public static class ReceiptService
    {
        private static readonly string receiptsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Receipts");

        public static void SaveReceipt(int orderId, string userId, ObservableCollection<OrderItem> items, decimal totalPrice)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"------- {Localizer.Instance["Receipt"]} -------");
            sb.AppendLine($"{Localizer.Instance["OrderID"]}: {orderId}");
            sb.AppendLine($"{Localizer.Instance["CashierID"]}: {userId}");
            sb.AppendLine($"{Localizer.Instance["Date"]}: {DateTime.Now}");
            sb.AppendLine();
            foreach (var item in items)
            {
                sb.AppendLine($"{item.Name} x{item.Quantity} - {item.TotalPrice:C}");
            }
            sb.AppendLine("-----------------------");
            sb.AppendLine($"{Localizer.Instance["TotalPrice"]}: {totalPrice:C}");
            sb.AppendLine("-----------------------");
            if (!Directory.Exists(receiptsDirectory))
            {
                Directory.CreateDirectory(receiptsDirectory);
            }
            string fileName = Path.Combine(receiptsDirectory, $"Receipt_{orderId}_{DateTime.Now:yyyyMMddHHmmss}.txt");
            System.IO.File.WriteAllText(fileName, sb.ToString());
        }

        public static ObservableCollection<string> GetReceiptFiles(DateTime date)
        {
            if (!Directory.Exists(receiptsDirectory))
            {
                Directory.CreateDirectory(receiptsDirectory);
            }
            var files = Directory.GetFiles(receiptsDirectory, $"Receipt_*_{date:yyyyMMdd}*.txt")
                                 .Select(Path.GetFileName)
                                 .OrderByDescending(f => f)
                                 .ToList();
            return new ObservableCollection<string>(files);
        }

        public static void OpenReceipt(string fileName)
        {
            string filePath = Path.Combine(receiptsDirectory, fileName);
            if (File.Exists(filePath))
            {
                var psi = new ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
        }
    }
}
