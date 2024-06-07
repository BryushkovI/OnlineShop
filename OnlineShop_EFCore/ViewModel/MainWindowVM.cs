using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using OnlineShop_CL;
using OnlineShop_CL.Services;
using OnlineShop_CL.Model;

namespace OnlineShop_EFCore.ViewModel
{
    internal class MainWindowVM : ViewModel
    {
        Context context;
        DataProvider dataProvider;

        readonly ILoggable loggable;

        EnteranceVM _enteranceVM;
        public EnteranceVM EnteranceVM
        {
            get => _enteranceVM;
            set => Set(ref _enteranceVM, value);
        }

        #region Таблицы и строки
        #region Заказы
        DataTable _dataTableOrders;
        public DataTable DataTableOrders
        {
            get => _dataTableOrders;
            set => Set(ref _dataTableOrders, value);
        }

        DataTable _dataTableNewOrder;
        public DataTable DataTableNewOrder
        {
            get => _dataTableNewOrder;
            set => Set(ref _dataTableNewOrder, value);
        } 
        #endregion

        #region Покупатели
        DataTable _dataTableCustomers;
        public DataTable DataTableCustomers
        {
            get => _dataTableCustomers;
            set => Set(ref _dataTableCustomers, value);
        }

        DataTable _dataTableNewCutomer;
        public DataTable DataTableNewCustomer
        {
            get => _dataTableNewCutomer;
            set => Set(ref _dataTableNewCutomer, value);
        } 
        #endregion

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
            _logs = [];
        }

        #region Обработка событий таблиц
        private void DataTableCustomers_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            string message = string.Empty;

            switch (e.Action)
            {
                case DataRowAction.Delete:
                    if (!string.IsNullOrEmpty(e.Row.ItemArray[0].ToString()))
                    {
                        dataProvider.DeleteCustomer(new Customer(e.Row), out message);
                        if (string.IsNullOrEmpty(message))
                        {
                            GetOrders();
                        }
                    }
                    break;
                case DataRowAction.Change:
                    if (!string.IsNullOrEmpty(e.Row.ItemArray[0].ToString()))
                    {
                        dataProvider.UpdateCustomer(new Customer(e.Row), out message);
                    }
                    break;
                default:
                    break;
            }
            if (!string.IsNullOrEmpty(message))
            {
                loggable.OnLog(message, []);
                GetCustomers();
            }
        }

        private void DataTableNewCustomer_RowAdded(object sender, DataRowChangeEventArgs e)
        {
            string message = string.Empty;

            switch (e.Action)
            {
                case DataRowAction.Add:
                    if (!string.IsNullOrEmpty(e.Row.ItemArray[0].ToString()))
                    {
                        dataProvider.AddCustomer(new Customer(e.Row), out message);
                        if (_dataTableNewCutomer.Rows != null || message != string.Empty)
                        {
                            _dataTableCustomers.Rows.Add(_dataTableNewCutomer.Rows[0].ItemArray);
                            OnPropertyChanged(nameof(DataTableCustomers));
                            _dataTableNewCutomer.Rows.Clear();
                            OnPropertyChanged(nameof(DataTableNewCustomer));
                        }
                    }
                    break;
                default:
                    break;
            }
            if (!string.IsNullOrEmpty(message))
            {
                loggable.OnLog(message, new string[] { });
                GetCustomers();
            }
        }

        private void DataTableOrders_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            string message = string.Empty;
            switch (e.Action)
            {
                case DataRowAction.Delete:
                    if (!string.IsNullOrEmpty(e.Row.ItemArray[1].ToString()))
                    {
                        dataProvider.DeleteOrder(new Order(e.Row), out message);
                    }
                    break;
                case DataRowAction.Change:
                    if (!string.IsNullOrEmpty(e.Row.ItemArray[1].ToString()))
                    {
                        dataProvider.UpdateOrder(new Order(e.Row), out message);
                    }
                    break;
                default:
                    break;
            }
            if (!string.IsNullOrEmpty(message))
            {
                loggable.OnLog(message, new string[] { });
                GetOrders();
            }
        }


        private void DataTableNewOrder_RowAdded(object sender, DataRowChangeEventArgs e)
        {
            string message = string.Empty;
            switch (e.Action)
            {
                case DataRowAction.Add:
                    if (!string.IsNullOrEmpty(e.Row.ItemArray[1].ToString()))
                    {
                        dataProvider.AddOrder(new Order(e.Row), out message);
                        if (_dataTableNewOrder.Rows != null || message != string.Empty)
                        {
                            _dataTableOrders.Rows.Add(_dataTableNewOrder.Rows[0].ItemArray);
                            OnPropertyChanged(nameof(DataTableOrders));
                            _dataTableNewOrder.Rows.Clear();
                            OnPropertyChanged(nameof(DataTableNewOrder));
                        }
                    }
                    break;
                default:
                    break;
            }
            if (!string.IsNullOrEmpty(message))
            {
                loggable.OnLog(message, new string[] { });
                GetOrders();
            }
        }
        #endregion

        private void Loggable_Log(string message, string[] args)
        {
            _logs.Add(string.Format(message, args));
            OnPropertyChanged(nameof(Logs));
        }

        private void Login(Context context)
        {
            MainWindowsVisibility = Visibility.Visible;
            dataProvider = new(context);
            loggable.OnLog("Подключено со строкой: {0}", [context.Database.GetConnectionString()]);
            EnteranceVM = null;
            _dataTableOrders = dataProvider.GetOrders();
            _dataTableNewOrder = _dataTableOrders.Clone();
            OnPropertyChanged(nameof(DataTableOrders));
            OnPropertyChanged(nameof(DataTableNewOrder));
            _dataTableCustomers = dataProvider.GetCustomers();
            _dataTableNewCutomer = _dataTableCustomers.Clone();
            OnPropertyChanged(nameof(DataTableCustomers));
            OnPropertyChanged(nameof(DataTableNewCustomer));
            DataTableCustomers.RowChanged += DataTableCustomers_RowChanged;
            DataTableNewCustomer.RowChanged += DataTableNewCustomer_RowAdded;
            DataTableOrders.RowChanged += DataTableOrders_RowChanged;
            DataTableNewOrder.RowChanged += DataTableNewOrder_RowAdded;
            DataTableCustomers.RowDeleting += DataTableCustomers_RowChanged;
            DataTableOrders.RowDeleting += DataTableOrders_RowChanged;
        }
        void GetOrders()
        {
            DataTableOrders = dataProvider.GetOrders();
            DataTableOrders.RowChanged += DataTableOrders_RowChanged;
            DataTableOrders.RowDeleting += DataTableOrders_RowChanged;
        }

        void GetCustomers()
        {
            DataTableCustomers = dataProvider.GetCustomers();
            DataTableCustomers.RowChanged += DataTableCustomers_RowChanged;
            DataTableCustomers.RowDeleting += DataTableCustomers_RowChanged;
        }



    }
}
