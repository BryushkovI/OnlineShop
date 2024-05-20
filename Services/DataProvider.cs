using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace OnlineShop.Services
{
    internal class DataProvider
    {
        private readonly MySqlConnectionStringBuilder mySqlConnectionStringBuilder;
        private readonly SqlConnectionStringBuilder sqlConnectionStringBuilder;

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

        public bool GetConnectionStatus()
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
    }
}
