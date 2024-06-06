using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop_CL.Model
{
    internal class Customer : INotifyPropertyChanged
    {
        string _firstName;
        public string FirstName
        {
            get => _firstName; set => _firstName = value;
        }

        string _lastName;
        public string LastName
        {
            get => _lastName; set => _lastName = value;
        }

        string _middleName;
        public string MiddleName
        {
            get => _middleName; set => _middleName = value;
        }

        string _email;
        public string Email
        {
            get => _email; set => _email = value;
        }

        string _phone;
        public string Phone
        {
            get => _phone; set => _phone = value;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    internal class Order : INotifyPropertyChanged
    {
        string _email;
        public string Email
        {
            get => _email; set => _email = value;
        }

        int _code;
        public int Code
        {
            get => _code; set => _code = value;
        }

        string _nameing;
        public string Nameing
        {
            get => _nameing; set => _nameing = value;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
