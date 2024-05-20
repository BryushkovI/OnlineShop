// See https://aka.ms/new-console-template for more information
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
MySqlConnectionStringBuilder mySqlConnectionStringBuilder = new()
{
    Server = "127.0.0.1",
    Port = 3306,
    UserID = "root",
    Password = "12345",
    Database = "onlineshopdb"
}; //подключение к БД MySQL
MySqlConnectionStringBuilder mySqlConnectionStringBuilderDB = new()
{
    Server = "127.0.0.1",
    Port = 3306,
    UserID = "root",
    Password = "12345"
}; // Подключение к серверу MySQL
SqlConnectionStringBuilder sqlConnectionStringBuilder = new()
{
    DataSource=@"(localdb)\MSSQLLocalDB",
    InitialCatalog = "OnlineShop",
    IntegratedSecurity = true
}; // Подключение к БД MSSQLLocalDB
SqlConnectionStringBuilder sqlConnectionStringBuilderDB = new()
{
    DataSource = @"(localdb)\MSSQLLocalDB",
    InitialCatalog = "master",
    IntegratedSecurity = true
}; // Подключение к серверу MSSQLLocalDB

try
{
    //Создание БД в MySQL
    using (MySqlConnection connectionServer = new MySqlConnection(mySqlConnectionStringBuilderDB.ConnectionString))
    {
        await connectionServer.OpenAsync();
        string query = @"CREATE SCHEMA OnlineShopDB";
        MySqlCommand command = new(query, connectionServer);
        await command.ExecuteNonQueryAsync();

        command.CommandText = @"CREATE USER 'worker'@'localhost' identified by '12345'";
        await command.ExecuteNonQueryAsync();

        command.CommandText = @"GRANT SELECT,UPDATE,DELETE,INSERT ON onlineshopdb.* TO 'worker'@'localhost'";
        await command.ExecuteNonQueryAsync();
    }
    //Создание таблицы и наполнение в MySQL
    using (MySqlConnection connection = new(mySqlConnectionStringBuilder.ConnectionString))
    {
        List<Costumer> costumers =
        [
            new()
            {
                Email = "ivanov@pmail.ru",
                LastName = "Иванов",
                FirstName = "Иван",
                MiddleName = "Иванович",
                Phone = "88005553535"
            },
            new()
            {
                Email = "petrov@pmail.ru",
                LastName = "Петров",
                FirstName = "Петр",
                MiddleName = "Петрович",
                Phone = "89999999999"
            },
            new()
            {
                Email = "sergeev@pmail.ru",
                LastName = "Сергеев",
                FirstName = "Сергей",
                MiddleName = "Сергеевич",
                Phone = "89006664646"
            }
        ];
        await connection.OpenAsync();
        string query = @"CREATE TABLE Users
                     ( Id INT NOT NULL AUTO_INCREMENT,
                       Email VARCHAR(30) NOT NULL,
                       LastName NVARCHAR(20) NOT NULL,
                       FirstName NVARCHAR(20) NOT NULL,
                       MiddleName NVARCHAR(20),
                       Phone VARCHAR(20),
                       PRIMARY KEY (Id, Email) )";

        MySqlCommand command = new(query, connection);
        await command.ExecuteNonQueryAsync();

        command.CommandText = @"INSERT Users(Email, LastName, FirstName, MiddleName, Phone)
                            VALUES (@Email, @LastName, @FirstName, @MiddleName, @Phone)";
        foreach (var item in costumers)
        {
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@Email", item.Email);
            command.Parameters.AddWithValue("@LastName", item.LastName);
            command.Parameters.AddWithValue("@FirstName", item.FirstName);
            command.Parameters.AddWithValue("@MiddleName", item.MiddleName);
            command.Parameters.AddWithValue("@Phone", item.Phone);
            await command.ExecuteNonQueryAsync();
        }
        
    }

    //Создание БД в MSSQLLocalDB
    using (SqlConnection sqlConnectionServer = new(sqlConnectionStringBuilderDB.ConnectionString))
    {
        await sqlConnectionServer.OpenAsync();
        string query = @"CREATE DATABASE OnlineShop";
        SqlCommand command = new(query, sqlConnectionServer);
        await command.ExecuteNonQueryAsync();

        command.CommandText = @"CREATE LOGIN worker WITH PASSWORD = '12345'";
        await command.ExecuteNonQueryAsync();

        command.CommandText = @"USE OnlineShop;
                                CREATE USER worker FOR LOGIN worker;";
        await command.ExecuteNonQueryAsync();

        command.CommandText = @"USE OnlineShop;
                                GRANT SELECT TO worker;
                                GRANT INSERT TO worker;
                                GRANT DELETE TO worker;
                                GRANT UPDATE TO worker;";
        await command.ExecuteNonQueryAsync();
    }
    //Создание таблици и наполнение в MSSQLLocalDB
    using (SqlConnection sqlConnection = new(sqlConnectionStringBuilder.ConnectionString))
    {
        List<Order> orders =
        [
            new(){
                Email = "ivanov@pmail.ru",
                Code = 100,
                Nameing = "Nokia 3310"
            },
            new(){
                Email = "ivanov@pmail.ru",
                Code = 105,
                Nameing = "Чехол для Nokia 3310"
            },
            new(){
                Email = "petrov@pmail.ru",
                Code = 102,
                Nameing = "IPhone 9"
            },
            new(){
                Email = "petrov@pmail.ru",
                Code = 103,
                Nameing = "Пленка для IPhone 9"
            },
            new(){
                Email = "petrov@pmail.ru",
                Code = 222,
                Nameing = "Расширинная гарантия на IPhone 9"
            },
            new(){
                Email = "sergeev@pmail.ru",
                Code = 999,
                Nameing = "Опознанный нелетающий объект"
            }
        ];
        await sqlConnection.OpenAsync();
        string query = @"CREATE TABLE Orders
                         (Id INT IDENTITY NOT NULL,
                          Email NVARCHAR(30) NOT NULL,
                          Code INT NOT NULL,
                          Nameing NVARCHAR(50),
                         PRIMARY KEY (Id, Email))";
        SqlCommand command = new(query, sqlConnection);
        await command.ExecuteNonQueryAsync();

        command.CommandText = @"INSERT Orders
                                VALUES (@Email, @Code, @Nameing)";
        foreach (var item in orders)
        {
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@Email", item.Email);
            command.Parameters.AddWithValue("@Code", item.Code);
            command.Parameters.AddWithValue("@Nameing", item.Nameing);
            await command.ExecuteNonQueryAsync();
        }
    }
    Console.WriteLine("Создание завершено успешно!");
    Console.ReadKey();
}
catch (Exception ex)
{
    Console.WriteLine($"Возникла ошибка типа {ex.GetType()}.\n Текст ошибки {ex.Message}");
    Console.ReadKey();
}

struct Order
{
    public string Email { get; set; }
    public int Code { get; set; }
    public string Nameing { get; set; }
}

struct Costumer
{
    public string Email { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string Phone { get; set; }
}