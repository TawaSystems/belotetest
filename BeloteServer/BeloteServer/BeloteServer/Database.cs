using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql;
using MySql.Data.MySqlClient;
using System.Threading;

namespace BeloteServer
{
    class Database
    {
        // Используются для присоединения к БД
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;
        // Очередь с запросами от клиентов
        private Queue<string> requestQueue;
        // Локер на запросы SELECT
        private object selectLocker;
        // Рабочий поток для записи в БД
        private Thread bdWorker;
        // Флаг завершения работы
        private bool stopped;

        public Database()
        {
            Initialize();
            requestQueue = new Queue<string>();
            selectLocker = new object();
            stopped = false;
            bdWorker = new Thread(DatabaseWorking);
            bdWorker.Start();
        }

        // Метод, записывающий все накопившиеся в очереди потоки в базу данных
        private void WriteAllRequestToDatabase()
        {
            if (OpenConnection())
            {
                while (!(requestQueue.Count == 0))
                {
                    MySqlCommand cmd = new MySqlCommand(requestQueue.Dequeue(), connection);
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (MySqlException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                CloseConnection();
            }
        }

        // Поток обработки запросов и записи их в базу данных
        private void DatabaseWorking()
        {
            while (true)
            {
                lock (requestQueue)
                {
                    lock (selectLocker)
                    {
                        if (requestQueue.Count > 0)
                            WriteAllRequestToDatabase();
                    }
                }
                lock((object)stopped)
                {
                    if (stopped)
                        break;
                }
                Thread.Sleep(5000);
            }
        }

        // Остновка работы с базой данных
        public void Stop()
        {
            lock ((object)stopped)
            {
                stopped = true;
            }
            bdWorker.Join();
            CloseConnection();

        }

        private void Initialize()
        {
            // подключение к базе данных
            server = "localhost";
            database = "Belote";
            uid = "root";
            password = "";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);
            
            if (OpenConnection())
            {
                Console.WriteLine("Database connected");
                CloseConnection();
            }
            
        }

        // Открытие соединения с БД
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

        // закрытие соединения с БД
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

        // Запрос SELECT для возврата множественного результата. ColCount - количество столбцов с таблице
        public List<List<String>> Select(string Query, int ColCount)
        {
            if (ColCount == 0)
                return null;
            List<List<String>> resList = new List<List<string>>();
            for (var i = 0; i < ColCount; i++)
                resList.Add(new List<string>());
            
            if (OpenConnection())
            {
                lock (selectLocker)
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
                    }
                    catch (Exception ex)
                    {
                        return null;
                    }
                    finally
                    {
                        CloseConnection();
                    }
                    return resList;
                }
            }
            else
            {
                return null;
            }
        }

        // Запрос SELECT для возврата единственного результата
        public string SelectScalar(string Query)
        {
            if (OpenConnection())
            {
                lock (selectLocker)
                {
                    MySqlCommand cmd = new MySqlCommand(Query, connection);
                    string res;
                    try
                    {
                        var q = cmd.ExecuteScalar();
                        if (q != null)
                        {
                            res = q.ToString();
                        }
                        else
                            return null;
                    }
                    catch (MySqlException ex)
                    {
                        return null;
                    }
                    finally
                    {
                        CloseConnection();
                    }
                    return res;
                }
            }
            else
                return null;
        }

        // Добавление запроса, не требующего возвращения результата
        public void AddQuery(string Query)
        {
            lock (requestQueue)
            {
                requestQueue.Enqueue(Query);
            }
        }
    }
}
