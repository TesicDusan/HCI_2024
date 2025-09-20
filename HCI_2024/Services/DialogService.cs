using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_2024.Services
{
    public static class DialogService
    {
        public static async Task<bool> ShowConfirmation(string message)
        {
            var view = new Views.Dialogs.ConfirmationDialog(message);

            var result = await DialogHost.Show(view, "RootDialog");

            return result is bool b && b;
        }
    }
}
