using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.Data;
using OnlineShop_CL.Services;
using System.Reflection;
using OnlineShop_CL.Model;

namespace OnlineShop_CL.Services
{
    public class DataProvider
    {
        Context _context;
        public DataProvider(Context context)
        {
            _context = context;
        }
        public DataTable GetOrders()
        {
            return ToDataTable(_context.Orders.ToList());
        }

        public DataTable GetCustomers()
        {
            return ToDataTable(_context.Customers.ToList());
        }

        public void AddOrder(Order order, out string message)
        {
            message = string.Empty;
            try
            {
                _context.Orders.Add(order);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            
        }

        public void AddCustomer(Customer customer, out string message)
        {
            message = string.Empty;
            try
            {
                _context.Customers.Add(customer);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

        }

        public void UpdateCustomer(Customer customer, out string message)
        {
            message = string.Empty;
            try
            {
                _context.Customers.Update(customer);
            }
            catch (Exception ex)
            {
                message = ex.Message;       
            }
        }

        public void UpdateOrder(Order order, out string message)
        {
            message = string.Empty;
            try
            {
                _context.Orders.Update(order);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
        }

        public void DeleteCustomer(Customer customer, out string message)
        {
            message = string.Empty;
            try
            {
                _context.Customers.Remove(customer);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

        }

        public void DeleteOrder(Order order, out string message)
        {
            message = string.Empty;
            try
            {
                _context.Orders.Remove(order);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
        }

        static DataTable ToDataTable<T>(IEnumerable<T> items)
        {
            var dataTable = new DataTable(typeof(T).Name);
            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo property in properties)
            {
                dataTable.Columns.Add(property.Name, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
            }
            foreach (T item in items)
            {
                var row = dataTable.NewRow();
                foreach (PropertyInfo property in properties)
                {
                    row[property.Name] = property.GetValue(item) ?? DBNull.Value;
                }
                dataTable.Rows.Add(row);
            }
            return dataTable;
        }

    }
}
