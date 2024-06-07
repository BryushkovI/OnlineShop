using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop_CL.Model
{
    public class Customer : INotifyPropertyChanged
    {
        public Customer()
        {
                
        }

        public Customer(DataRow row)
        {
            try
            {
                Email = row.ItemArray[0].ToString();
                LastName = row.ItemArray[1].ToString();
                FirstName = row.ItemArray[2].ToString();
                MiddleName = row.ItemArray[3].ToString();
                Phone = row.ItemArray[4].ToString();
            }
            catch (Exception)
            {

            }

        }

        string _email;
        public string Email
        {
            get => _email; set => _email = value;
        }

        string _lastName;
        public string LastName
        {
            get => _lastName; set => _lastName = value;
        }

        string _firstName;
        public string FirstName
        {
            get => _firstName; set => _firstName = value;
        }

        string _middleName;
        public string MiddleName
        {
            get => _middleName; set => _middleName = value;
        }

        string _phone;
        public string Phone
        {
            get => _phone; set => _phone = value;
        }

        public List<Order> Orders { get; set; } = new();

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class Order : INotifyPropertyChanged
    {
        public Order()
        {
            
        }

        public Order(DataRow row)
        {
            try
            {
                if (int.TryParse(row.ItemArray[0].ToString(), out _id))
                {
                    Id = _id;
                }
                Email = row.ItemArray[1].ToString();
                if (int.TryParse(row.ItemArray[2].ToString(), out _code))
                {
                    Code = _code;
                }
                Nameing = row.ItemArray[3].ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }

        int _id;
        public int Id { get => _id; set => _id = value; }

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

        public Customer? Customer { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
