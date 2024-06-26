﻿using OnlineShop.Model;
using OnlineShop.Services;
using Org.BouncyCastle.Asn1.X509.Qualified;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace OnlineShop.ViewModel
{
    internal class MainWindowVM : ViewModel
    {
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
        public DataTable DataTableNewCutomer
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
            _logs = new ObservableCollection<string>();
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
                        dataProvider.DeleteCustomer(e.Row, out message);
                    }
                    break;
                case DataRowAction.Change:
                    if (!string.IsNullOrEmpty(e.Row.ItemArray[0].ToString()))
                    {
                        dataProvider.UpdateCustomer(e.Row, out message);
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

        private void DataTableNewCutomer_RowAdded(object sender, DataRowChangeEventArgs e)
        {
            string message = string.Empty;

            switch (e.Action)
            {
                case DataRowAction.Add:
                    if (!string.IsNullOrEmpty(e.Row.ItemArray[0].ToString()))
                    {
                        dataProvider.AddCustomer(e.Row, out message);
                        if (_dataTableNewCutomer.Rows != null || message != string.Empty)
                        {
                            _dataTableCustomers.Rows.Add(_dataTableNewCutomer.Rows[0].ItemArray);
                            OnPropertyChanged(nameof(DataTableCustomers));
                            _dataTableNewCutomer.Rows.Clear();
                            OnPropertyChanged(nameof(DataTableNewCutomer));
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
                        dataProvider.DeleteOrder(e.Row, out message);
                    }
                    break;
                case DataRowAction.Change:
                    if (!string.IsNullOrEmpty(e.Row.ItemArray[1].ToString()))
                    {
                        dataProvider.UpdateOrder(e.Row, out message);
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
                        dataProvider.AddOrder(e.Row, out message);
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

        private void Login()
        {
            MainWindowsVisibility = Visibility.Visible;
            dataProvider = EnteranceVM.dataProvider;
            loggable.OnLog("Подключено со строкой: {0}\n {1}", dataProvider.GetConnectionString());
            EnteranceVM = null;
            _dataTableOrders = dataProvider.GetOrders();
            _dataTableNewOrder = _dataTableOrders.Clone();
            OnPropertyChanged(nameof(DataTableOrders));
            OnPropertyChanged(nameof(DataTableNewOrder));
            _dataTableCustomers = dataProvider.GetCustomers();
            _dataTableNewCutomer = _dataTableCustomers.Clone();
            OnPropertyChanged(nameof(DataTableCustomers));
            OnPropertyChanged(nameof(DataTableNewCutomer));
            DataTableCustomers.RowChanged += DataTableCustomers_RowChanged;
            DataTableNewCutomer.RowChanged += DataTableNewCutomer_RowAdded;
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
