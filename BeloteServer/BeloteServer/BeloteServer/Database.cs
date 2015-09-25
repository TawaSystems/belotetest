using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql;
using MySql.Data.MySqlClient;

namespace BeloteServer
{
    class Database
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        public Database()
        {
            Initialize();
        }

        private void Initialize()
        {
            server = "localhost";
            database = "Belote";
            uid = "root";
            password = "";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);
        }

        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                return false;
            }
        }

        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            { 
                return false;
            }
        }

        public bool ExecuteQuery(string Query)
        {
            if (OpenConnection())
            {
                MySqlCommand cmd = new MySqlCommand(Query, connection);
                try
                {
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (MySqlException ex)
                {
                    return false;
                }
            }
            else
                return false;
        }

        public List<List<String>> Select(string Query, int ColCount)
        {
            if (ColCount == 0)
                return null;
            List<List<String>> resList = new List<List<string>>();
            for (var i = 0; i < ColCount; i++)
                resList.Add(new List<string>());
            
            if (OpenConnection())
            {
                MySqlCommand cmd = new MySqlCommand(Query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                try
                {
                    while (dataReader.Read())
                    {
                        for (var i = 0; i < ColCount; i++)
                            resList[i].Add(dataReader[i].ToString());
                    }

                    dataReader.Close();

                    CloseConnection();

                    return resList;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public string SelectScalar(string Query)
        {
            if (OpenConnection())
            {
                MySqlCommand cmd = new MySqlCommand(Query, connection);

                try
                {
                    string res = cmd.ExecuteScalar().ToString();
                    return res;
                }
                catch (MySqlException ex)
                {
                    return null;
                }
            }
            else
                return null;
        }
    }
}
