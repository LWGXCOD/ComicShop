using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComicsShop
{
    class UsersDB
    {
        //класс предоставляющий функции для работы с локальной БД.
        MySqlConnection connection = new MySqlConnection("server=localhost;port=3306;User=root;Password=root;database=UsersDB");

        //открытие соединения
        public void openConnection()
        {
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
        }
        //закрытие соединения
        public void closeConnection()
        {
            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
        }
        //получить соединение
        public MySqlConnection getConnection()
        {
            return connection;
        }
    }
}
