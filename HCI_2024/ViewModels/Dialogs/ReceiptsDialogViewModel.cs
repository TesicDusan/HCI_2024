using HCI_2024.Services;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HCI_2024.ViewModels.Dialogs
{
    public class ReceiptsDialogViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<string> _receipts;
        private string? _selectedReceipt;
        private DateTime _selectedDate;

        public ObservableCollection<string> Receipts
        {
            get => _receipts;
            set
            {
                if (_receipts != value)
                {
                    _receipts = value;
                    OnPropertyChanged(nameof(Receipts));
                }
            }
        }

        public string? SelectedReceipt
        {
            get => _selectedReceipt;
            set
            {
                if (_selectedReceipt != value)
                {
                    _selectedReceipt = value;
                    OnPropertyChanged(nameof(SelectedReceipt));
                    ReceiptService.OpenReceipt(_selectedReceipt!);
                }
            }
        }

        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                if (_selectedDate != value)
                {
                    _selectedDate = value;
                    OnPropertyChanged(nameof(SelectedDate));
                    UpdateReceipts();
                }
            }
        }

        public ICommand CloseCommand { get; set; }

        public ReceiptsDialogViewModel()
        {
            SelectedDate = DateTime.Now;
            Receipts = new ObservableCollection<string>();
            CloseCommand = new RelayCommand(CloseAction);
            UpdateReceipts();
        }

        public void CloseAction(object? obj)
        {
            DialogHost.CloseDialogCommand.Execute(null, null);
        }

        private void UpdateReceipts()
        {
            Receipts = ReceiptService.GetReceiptFiles(SelectedDate);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
