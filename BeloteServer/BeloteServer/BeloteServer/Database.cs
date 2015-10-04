using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql;
using MySql.Data.MySqlClient;
using System.Threading;
using System.Diagnostics;

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
#if DEBUG
            Debug.WriteLine(DateTime.Now.ToString() + " Запуск потока обработки команд к БД");
#endif
            bdWorker = new Thread(DatabaseWorking);
            bdWorker.Start();
        }

        // Метод, записывающий все накопившиеся в очереди потоки в базу данных
        private void WriteAllRequestToDatabase()
        {
#if DEBUG
            Debug.WriteLine(DateTime.Now.ToString() + " Запись всех накопившихся запросов в базу данных");
            Debug.Indent();
            Debug.WriteLine("Количество запросов: " + requestQueue.Count);
#endif
            if (OpenConnection())
            {
                while (!(requestQueue.Count == 0))
                {
                    MySqlCommand cmd = new MySqlCommand(requestQueue.Dequeue(), connection);
                    try
                    {
#if DEBUG
                        Debug.WriteLine("Выполнение запроса: " + cmd.CommandText);
#endif
                        cmd.ExecuteNonQuery();
                    }
                    catch (MySqlException ex)
                    {
#if DEBUG
                        Debug.WriteLine(ex.Message);
#endif
                    }
                }
                CloseConnection();
            }
#if DEBUG
            Debug.Unindent();
#endif
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
#if DEBUG
            Debug.WriteLine(DateTime.Now.ToString() + " Остановка потока работы с базой данных");
#endif
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
#if DEBUG
            Debug.WriteLine(DateTime.Now.ToString() + " Подключение к базе данных");
            server = "localhost";
            database = "Belote";
            uid = "root";
            password = "";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
#else
#endif
            connection = new MySqlConnection(connectionString);            
        }

        // Открытие соединения с БД
        private bool OpenConnection()
        {
#if DEBUG
            Debug.WriteLine(DateTime.Now.ToString() + " Попытка открытия соединения к БД");
            Debug.Indent();
#endif
            try
            {
                connection.Open();
#if DEBUG
                Debug.WriteLine("Соединение открыто успешно");
                Debug.Unindent();
#endif
                return true;
            }
            catch (MySqlException ex)
            {
#if DEBUG
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
#endif
                return false;
            }
        }

        // закрытие соединения с БД
        private bool CloseConnection()
        {
#if DEBUG
            Debug.WriteLine(DateTime.Now.ToString() + " Попытка закрытия соединения к БД");
            Debug.Indent();
#endif
            try
            {
                connection.Close();
#if DEBUG
                Debug.WriteLine("Соединение закрыто успешно");
                Debug.Unindent();
#endif
                return true;
            }
            catch (MySqlException ex)
            {
#if DEBUG
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
#endif
                return false;
            }
        }

        // Запрос SELECT для возврата множественного результата. ColCount - количество столбцов с таблице
        public List<List<String>> Select(string Query, int ColCount)
        {
            if (ColCount == 0)
                return null;
#if DEBUG
            Debug.WriteLine(DateTime.Now.ToString() + " Попытка выполнения запроса SELECT");
            Debug.Indent();
            Debug.WriteLine(Query);
            Debug.WriteLine("Количество столбцов: " + ColCount);
#endif
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
#if DEBUG
                        Debug.WriteLine("Запрос выполнен успешно");
#endif
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.Write(ex.Message);
#endif
                        return null;
                    }
                    finally
                    {
#if DEBUG
                        Debug.Unindent();
#endif
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
#if DEBUG
            Debug.WriteLine(DateTime.Now.ToString() + " Попытка выполнения запроса SELECT SCALAR");
            Debug.Indent();
            Debug.WriteLine(Query);
#endif
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
                            res = null;
#if DEBUG
                        Debug.WriteLine("Запрос выполнен. Результат: " + res);
#endif
                    }
                    catch (MySqlException ex)
                    {
#if DEBUG
                        Debug.WriteLine(ex.Message);
#endif
                        return null;
                    }
                    finally
                    {
                        CloseConnection();
                    }
#if DEBUG
                    Debug.Unindent();
#endif
                    return res;
                }
            }
            else
                return null;
        }

        // Добавление запроса, не требующего возвращения результата
        public void AddQuery(string Query)
        {
#if DEBUG
            Debug.WriteLine(DateTime.Now.ToString() + " Добавление запроса в очередь запросов");
            Debug.Indent();
            Debug.WriteLine(Query);
            Debug.Unindent();
#endif
            lock (requestQueue)
            {
                requestQueue.Enqueue(Query);
            }
        }
    }
}
