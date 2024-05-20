using OnlineShop.Model;
using OnlineShop.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OnlineShop.ViewModel
{
    internal class MainWindowVM : ViewModel
    {
        DataProvider dataProvider;


        EnteranceVM _enteranceVM;
        public EnteranceVM EnteranceVM
        {
            get => _enteranceVM;
            set => Set(ref _enteranceVM, value);
        }

        #region Таблицы и строки
        Customer _customer;
        public Customer Customer
        {
            get => _customer;
            set => Set(ref _customer, value);
        }

        Order _order;
        public Order Order
        {
            get => _order;
            set => Set(ref _order, value);
        }

        ObservableCollection<Customer> _customers;

        public ObservableCollection<Customer> Customers
        {
            get => _customers;
            set => Set(ref _customers, value);
        }

        ObservableCollection<Order> _orders;
        public ObservableCollection<Order> Orders
        {
            get => _orders;
            set => Set(ref _orders, value);
        }
        #endregion

        Visibility _mainWindowsVisibility = Visibility.Collapsed;
        public Visibility MainWindowsVisibility
        {
            get => _mainWindowsVisibility;
            set => Set(ref _mainWindowsVisibility, value);
        }

        Visibility _enteranceVisibility = Visibility.Visible;
        public Visibility EnteranceVisibility
        {
            get => _enteranceVisibility;
            set => Set(ref _enteranceVisibility, value);
        }


        public MainWindowVM()
        {
           _enteranceVM = new EnteranceVM();
           _enteranceVM.Enter += Login;
           OnPropertyChanged(nameof(EnteranceVM));
        }

        private void Login()
        {
            MainWindowsVisibility = Visibility.Visible;
            EnteranceVisibility = Visibility.Collapsed;
        }
    }
}
