using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using OnlineShop.Model;
using System.Data;

namespace OnlineShop.Services
{
    internal class DataProvider
    {
        private readonly MySqlConnectionStringBuilder mySqlConnectionStringBuilder;
        private readonly SqlConnectionStringBuilder sqlConnectionStringBuilder;

        public string[] GetConnectionString() => new string[]
        {
            mySqlConnectionStringBuilder.ConnectionString,
            sqlConnectionStringBuilder.ConnectionString
        };

        public DataProvider(string username, string password)
        {
            mySqlConnectionStringBuilder = new MySqlConnectionStringBuilder()
            {
                Server = "127.0.0.1",
                Port = 3306,
                UserID = username,
                Password = password,
                Database = "onlineshopdb"
            };
            sqlConnectionStringBuilder = new SqlConnectionStringBuilder()
            {
                DataSource = @"(localdb)\MSSQLLocalDB",
                InitialCatalog = "OnlineShop",
                IntegratedSecurity = false,
                Password = password,
                UserID = username
            };
        }

        public bool IsConnected()
        {
            bool result;

            MySqlConnection mysqlconnection = new MySqlConnection(mySqlConnectionStringBuilder.ConnectionString);
            mysqlconnection.Open();
            
            SqlConnection sqlConnection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);
            sqlConnection.Open();

            result = mysqlconnection.State == System.Data.ConnectionState.Open && sqlConnection.State == System.Data.ConnectionState.Open;

            mysqlconnection?.Close();
            sqlConnection?.Close();

            return result;
        }

        public DataTable GetOrders()
        {
            using (SqlConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString))
            {
                string query = @"SELECT * FROM Orders";
                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(query,connection))
                {
                    DataTable dataTable = new DataTable();
                    DataSet dataSet = new DataSet();

                    dataAdapter.Fill(dataSet, "Orders");
                    dataTable = dataSet.Tables["Orders"];
                    return dataTable;
                }

            }
            
        }

        public DataTable GetCustomers()
        {
            using (MySqlConnection connection = new MySqlConnection(mySqlConnectionStringBuilder.ConnectionString))
            {
                string query = @"SELECT * FROM Users";
                using (MySqlDataAdapter dataAdapter = new MySqlDataAdapter(query, connection))
                {
                    DataTable dataTable = new DataTable();
                    DataSet dataSet = new DataSet();

                    dataAdapter.Fill(dataSet, "Customers");
                    dataTable = dataSet.Tables["Customers"];
                    return dataTable;
                }

            }

        }
    }
}
