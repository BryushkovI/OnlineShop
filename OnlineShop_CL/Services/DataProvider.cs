using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.Data;

namespace OnlineShop_CL.Services
{
    public class DataProvider
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

        public void AddOrder(DataRow order, out string message)
        {
            message = string.Empty;
            try
            {
                using (SqlConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand()
                    {
                        Connection = connection,
                        CommandText = @"INSERT Orders
                                        VALUES (@Email, @Code, @Nameing)"
                    };
                    command.Parameters.AddWithValue("@Email", order.ItemArray[1].ToString());
                    command.Parameters.AddWithValue("@Code", order.ItemArray[2]);
                    command.Parameters.AddWithValue("@Nameing", order.ItemArray[3].ToString());
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception exc)
            {
                message = exc.Message;
            }
            
        }

        public void AddCustomer(DataRow customer, out string message)
        {
            message = string.Empty;
            try
            {
                using (MySqlConnection connection = new MySqlConnection(mySqlConnectionStringBuilder.ConnectionString))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand()
                    {
                        Connection = connection,
                        CommandText = @"INSERT Users(Email, LastName, FirstName, MiddleName, Phone)
                                             VALUES (@Email, @LastName, @FirstName, @MiddleName, @Phone)"
                    };
                    command.Parameters.AddWithValue("@Email", customer.ItemArray[0].ToString());
                    command.Parameters.AddWithValue("@LastName", customer.ItemArray[1].ToString());
                    command.Parameters.AddWithValue("@FirstName", customer.ItemArray[2].ToString());
                    command.Parameters.AddWithValue("@MiddleName", customer.ItemArray[3].ToString());
                    command.Parameters.AddWithValue("@Phone", customer.ItemArray[4].ToString());
                    command.ExecuteNonQuery();

                }
                
            }
            catch (Exception exc)
            {
                message = exc.Message;
            }

        }

        public void UpdateCustomer(DataRow customer, out string message)
        {
            message = string.Empty;
            try
            {
                using (MySqlConnection connection = new MySqlConnection(mySqlConnectionStringBuilder.ConnectionString))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand()
                    {
                        Connection = connection,
                        CommandText = @"UPDATE Users
                                           SET Email = @Email,
                                               LastName = @LastName,
                                               FirstName = @FirstName,
                                               MiddleName = @MiddleName,
                                               Phone = @Phone
                                         WHERE Email = @Email"
                    };
                    command.Parameters.AddWithValue("@Email", customer.ItemArray[0].ToString());
                    command.Parameters.AddWithValue("@LastName", customer.ItemArray[1].ToString());
                    command.Parameters.AddWithValue("@FirstName", customer.ItemArray[2].ToString());
                    command.Parameters.AddWithValue("@MiddleName", customer.ItemArray[3].ToString());
                    command.Parameters.AddWithValue("@Phone", customer.ItemArray[4].ToString());
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception exc)
            {
                message = exc.Message;
            }
        }

        public void UpdateOrder(DataRow order, out string message)
        {
            message = string.Empty;
            try
            {
                using (SqlConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand()
                    {
                        Connection = connection,
                        CommandText = @"UPDATE Orders
                                       SET Id = @Id,
                                           Email = @Email,
                                           Code = @Code,
                                           Nameing = @Nameing
                                     WHERE Id = @Id
                                       AND Email = @Email"
                    };
                    command.Parameters.AddWithValue("@Id", order.ItemArray[0].ToString());
                    command.Parameters.AddWithValue("@Email", order.ItemArray[1].ToString());
                    command.Parameters.AddWithValue("@Code", order.ItemArray[2].ToString());
                    command.Parameters.AddWithValue("@Nameing", order.ItemArray[3].ToString());
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
        }

        public void DeleteCustomer(DataRow customer, out string message)
        {
            message = string.Empty;
            try
            {
                using (MySqlConnection connection = new MySqlConnection(mySqlConnectionStringBuilder.ConnectionString))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand()
                    {
                        Connection = connection,
                        CommandText = @"DELETE FROM Users WHERE Email = @Email"
                    };
                    command.Parameters.AddWithValue("@Email", customer.ItemArray[0].ToString());
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
        }

        public void DeleteOrder(DataRow order, out string message)
        {
            message = string.Empty;
            try
            {
                using (SqlConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand()
                    {
                        Connection = connection,
                        CommandText = @"DELETE FROM Orders WHERE Id = @Id
                                                             AND Email = @Email"
                    };
                    command.Parameters.AddWithValue("@Id", order.ItemArray[0].ToString());
                    command.Parameters.AddWithValue("@Email", order.ItemArray[1].ToString());
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex) { message = ex.Message; }
        }

    }
}
