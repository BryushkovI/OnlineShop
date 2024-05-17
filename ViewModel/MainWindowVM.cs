using OnlineShop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.ViewModel
{
    internal class MainWindowVM : ViewModel
    {
        Customer _customer;
        public Customer Customer
        {
            get => _customer;
            set => Set(ref _customer, value);
        }
    }
}
