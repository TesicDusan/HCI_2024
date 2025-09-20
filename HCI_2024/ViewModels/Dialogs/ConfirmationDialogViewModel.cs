using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_2024.ViewModels.Dialogs
{
    class ConfirmationDialogViewModel
    {
        public string Message { get; }

        public ConfirmationDialogViewModel(string message)
        {
            Message = message;
        }
    }
}
