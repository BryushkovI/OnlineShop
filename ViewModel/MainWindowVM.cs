using OnlineShop.Model;
using OnlineShop.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OnlineShop.ViewModel
{
    internal class MainWindowVM : ViewModel
    {
        DataProvider dataProvider;

        ILoggable loggable;

        EnteranceVM _enteranceVM;
        public EnteranceVM EnteranceVM
        {
            get => _enteranceVM;
            set => Set(ref _enteranceVM, value);
        }

        #region Таблицы и строки
        DataTable _dataTableOrders;
        public DataTable DataTableOrders
        {
            get => _dataTableOrders;
            set => Set(ref _dataTableOrders, value);
        }

        DataTable _dataTableCustomers;
        public DataTable DataTableCustomers
        {
            get => _dataTableCustomers;
            set => Set(ref _dataTableCustomers, value);
        }
        ObservableCollection<string> _logs;
        public ObservableCollection<string> Logs
        {
            get => _logs;
            set => Set(ref _logs, value);
        }
        #endregion

        Visibility _mainWindowsVisibility = Visibility.Collapsed;
        public Visibility MainWindowsVisibility
        {
            get => _mainWindowsVisibility;
            set => Set(ref _mainWindowsVisibility, value);
        }



        public MainWindowVM()
        {
            _enteranceVM = new EnteranceVM();
            _enteranceVM.Enter += Login;
            OnPropertyChanged(nameof(EnteranceVM));
            loggable = new Logger();
            loggable.Log += Loggable_Log;
            _logs = new ObservableCollection<string>();
        }

        private void Loggable_Log(string message, string[] args)
        {
            _logs.Add(string.Format(message, args));
            OnPropertyChanged(nameof(Logs));
        }

        private void Login()
        {
            MainWindowsVisibility = Visibility.Visible;
            dataProvider = EnteranceVM.dataProvider;
            loggable.OnLog("Подключено со строкой: {0}\n {1}", dataProvider.GetConnectionString());
            EnteranceVM = null;
            _dataTableOrders = dataProvider.GetOrders();
            OnPropertyChanged(nameof(DataTableOrders));
            _dataTableCustomers = dataProvider.GetCustomers();
            OnPropertyChanged(nameof(DataTableCustomers));
        }
    }
}
