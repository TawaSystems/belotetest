using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;
using System.Net;

namespace BeloteServer
{
    class Client
    {
        // Подключение клиента 
        private TcpClient client;
        // Поток, обрабатывающий запросы пользователя
        private Thread worker;
        // Ссылка на игровой объект
        private Game game;
        // Ссылка на объект игрока
        private Player player;

        // Конструктор с инициализацией всех объектов
        public Client(TcpClient tcpClient, Game game)
        {
#if DEBUG
            Debug.WriteLine(DateTime.Now.ToString() + " Подключение нового клиента");
            Debug.Indent();
            Debug.WriteLine("Client IP: " + ((IPEndPoint)tcpClient.Client.RemoteEndPoint).Address.ToString());
            Debug.Unindent();
#endif
            client = tcpClient;
            this.game = game;
            player = null;
            worker = new Thread(Process);
            worker.Start();
        }
   
        // Идентификатор клиента
        public int ID
        {
            get
            {
                return player.Profile.Id;
            }
        }

        // Профиль игрока
        public Player Player
        {
            get
            {
                return player;
            }
        }

        // Отправка сообщения клиенту
        public void SendMessage(string message)
        {
            var data = Encoding.Unicode.GetBytes(message);
            client.GetStream().Write(data, 0, data.Length);
        }

        // Функция обработки запросов клиента, выполняется в потоке worker
        private void Process()
        {
            NetworkStream stream = null;
            try
            {
                stream = client.GetStream();
                byte[] data = new byte[64];
                while (true)
                {
                    StringBuilder builder = new StringBuilder();
                    do
                    {
                        int bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    string message = builder.ToString();
#if DEBUG
                    Debug.WriteLine(DateTime.Now.ToString() + " Получено сообщение от клиента");
                    Debug.Indent();
                    Debug.WriteLine("Client IP: " + ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString());
                    Debug.WriteLine("Сообщение: " + message);
                    Debug.Unindent();
#endif
                    // Отключение клиента
                    if (message == "EXT")
                    {
                        if (this.ID != -1)
                            this.game.Clients.DeleteClient(this);
                        break;
                    }
                    string result = ProcessCommand(message);
                    if (result != null)
                    {
#if DEBUG
                        Debug.WriteLine(DateTime.Now.ToString() + " Отправка сообщения клиенту");
                        Debug.Indent();
                        Debug.WriteLine("Client IP: " + ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString());
                        Debug.WriteLine("Сообщение: " + result);
                        Debug.Unindent();
#endif
                        data = Encoding.Unicode.GetBytes(result);
                        stream.Write(data, 0, data.Length);
                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine(ex.Message);
#endif
            }
            finally
            {
                if (stream != null)
                    stream.Close();
                if (client != null)
                    client.Close();
#if DEBUG
                Debug.WriteLine(DateTime.Now.ToString() + " Отключение клиента");
                Debug.Indent();
                Debug.WriteLine("Client IP: " + ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString());
                Debug.Unindent();
#endif
            }
        }

        // Функция обработки полученных от клиента комманд
        private string ProcessCommand(string message)
        {
            string command = Helpers.CommandFromStr(message);
            if (command == null)
            {
                return null;
            }
            string msg = Helpers.MessageFromStr(message);
#if DEBUG
            Debug.WriteLine(DateTime.Now.ToString() + " Начало обработки сообщения от клиента");
            Debug.Indent();
            Debug.WriteLine("Client IP: " + ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString());
            Debug.WriteLine("Команда: " + command);
            Debug.WriteLine("Сообщение: " + msg);
            Debug.Unindent();
#endif
            string Result = null;
            switch (command[0])
            {
                // Autorization
                case 'A':
                    {
                        Result = ProcessAutorization(command, msg);
                        break;
                    }
                case 'B':
                    {
                        break;
                    }
                // Обработка команд работы с игровыми столами
                case 'T':
                default:
                    {
                        Result = ProcessTables(command, msg);
                        break;
                    }
            }
#if DEBUG
            Debug.WriteLine(DateTime.Now.ToString() + " Результат обработки сообщения от клиента");
            Debug.Indent();
            Debug.WriteLine("Client IP: " + ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString());
            Debug.WriteLine("Команда: " + command);
            Debug.WriteLine("Сообщение: " + msg);
            Debug.WriteLine("Результат: " + Result);
            Debug.Unindent();
#endif
            return Result;
        }

        // Функция обработки авторизации пользователя - регистрация, вход, напоминание паролей, выход
        private string ProcessAutorization(string command, string message)
        {
            Dictionary<string, string> regParams = Helpers.SplitCommandString(message);
            if (regParams == null)
            {
                return null;
            }
            String Result = null;
            switch (command[1])
            {
                // Регистрация
                case 'R':
                    {
                        switch (command[2])
                        {
                            // Регистрация с помощью электронной почты
                            case 'E':
                                {
                                    if (game.Autorization.RegistrationEmail(regParams["Nickname"], regParams["Email"], regParams["Password"],
                                        regParams["Country"], (regParams["Sex"] == "1")))
                                    {
                                        // Регистрация прошла успешно
                                        player = new Player(this.game);
                                        player.Profile.Email = regParams["Email"];
                                        player.Profile.Nickname = regParams["Nickname"];
                                        player.Profile.Country = regParams["Country"];
                                        player.Profile.Sex = (regParams["Sex"] == "1");
                                        player.Profile.Id = Int32.Parse(game.DataBase.SelectScalar(String.Format("SELECT ID From Players WHERE Email=\"{0}\";", regParams["Email"])));
                                        this.game.Clients.Add(this);
                                        Result = "ARERegistration=1";
                                        break;
                                    }
                                    else
                                    {
                                        // Ошибка в регистрации
                                        Result = "ARERegistration=0";
                                        break;
                                    }
                                }
                            // Регистрация с помощью телефона
                            case 'P':
                                {
                                    break;
                                }
                            // Регистрация с помощью ВКонтакте
                            case 'V':
                                {
                                    break;
                                }
                            // Регистрация с помощью Facebook
                            case 'F':
                                {
                                    break;
                                }
                            // Регистрация с помощью Одноклассников
                            case 'O':
                                {
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }
                        break;
                    }
                // Авторизация
                case 'A':
                    {
                        switch (command[2])
                        {
                            // Авторизация с помощью электронной почты
                            case 'E':
                                {
                                    if (game.Autorization.EnterEmail(regParams["Email"], regParams["Password"]))
                                    {
                                        player = new Player(this.game);
                                        player.ReadPlayerFromDataBase("Email", regParams["Email"]);
                                        this.game.Clients.Add(this);
                                        Result = "AAEAutorization=1";
                                        break;
                                    }
                                    else
                                    {
                                        Result = "AAEAutorization=0";
                                        break;
                                    }
                                }
                            // Авторизация с помощью телефона
                            case 'P':
                                {
                                    break;
                                }
                            // Авторизация с помощью ВКонтакте
                            case 'V':
                                {
                                    break;
                                }
                            // Регистрация с помощью Facebook
                            case 'F':
                                {
                                    break;
                                }
                            // Регистрация с помощью Одноклассников
                            case 'O':
                                {
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }
                        break;
                    }
                // Напоминание пароля
                case 'M':
                    {
                        switch (command[2])
                        {
                            // Напоминание на почту
                            case 'E':
                                {
                                    bool resRemind = game.Autorization.RemindPasswordEmail(regParams["Email"]);
                                    if (resRemind)
                                    {
                                        Result = "AMERemind=1";
                                        break;
                                    }
                                    else
                                    {
                                        Result = "AMERemind=0";
                                        break;
                                    }
                                }
                            // Напоминание на телефон
                            case 'P':
                                {
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }
                        break;
                    }
                // Тест на наличие эл.почты/ника и т.д.
                case 'T':
                    {
                        switch (command[2])
                        {
                            // Адрес электронной почты
                            case 'E':
                                {
                                    if (game.Autorization.EmailExists(regParams["Email"]))
                                    {
                                        Result = "ATEExists=1";
                                    }
                                    else
                                    {
                                        Result = "ATEExists=0";
                                    }
                                    break;
                                }
                            // Ник
                            case 'N':
                                {
                                    if (game.Autorization.NicknameExists(regParams["Nickname"]))
                                    {
                                        Result = "ATNExists=1";
                                    }
                                    else
                                    {
                                        Result = "ATNExists=0";
                                    }
                                    break;
                                }
                            // Телефон
                            case 'P':
                                {
                                    break;
                                }
                            // Вконтакте
                            case 'V':
                                {
                                    break;
                                }
                            // Facebook
                            case 'F':
                                {
                                    break;
                                }
                            // Одноклассники
                            case 'O':
                                {
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }
                        break;
                    }
                // Выход
                case 'E':
                    {
                        if (this.ID != -1)
                        {
                            this.game.Clients.DeleteClient(this);
                        }
                        player = null;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            return Result;
        }

        // Функция обработки команд на создание, изменение, выборку и удаление игровых столов
        private string ProcessTables(string command, string message)
        {
            Dictionary<string, string> tableParams = Helpers.SplitCommandString(message);
            if (tableParams == null)
            {
                return null;
            }
            string Result = null;
            switch (command[1])
            {
                // Модификация - создание, изменение свойств, удаление
                case 'M':
                    {
                        switch (command[2])
                        {
                            // Создание игрового стола
                            case 'C':
                                {
                                    int tableID = this.game.Tables.CreateTable(this, Int32.Parse(tableParams["Bet"]),
                                        Helpers.StringToBool(tableParams["PlayersVisibility"]), Helpers.StringToBool(tableParams["Chat"]), Int32.Parse(tableParams["MinimalLevel"]),
                                        Helpers.StringToBool(tableParams["TableVisibility"]), Helpers.StringToBool(tableParams["VIPOnly"]), Helpers.StringToBool(tableParams["Moderation"]),
                                        Helpers.StringToBool(tableParams["AI"]));
                                    Result = "ID=" + tableID.ToString();
                                    break;
                                }
                            // Покидание игрового стола создателем
                            case 'L':
                                {
                                    Table closingTable = this.game.Tables[Int32.Parse(tableParams["ID"])];
                                    closingTable.SendMessageToClientsWithoutCreator("TML");
                                    closingTable.CloseTable();
                                    Result = "TML";
                                    break;
                                }
                            // Успешное завершение игры на столе
                            case 'E':
                                {
                                    break;
                                }
                            // Открытие стола для всех игроков (TableVisibility = true)
                            case 'V':
                                {
                                    this.game.Tables[Int32.Parse(tableParams["ID"])].TableVisibility = true;
                                    Result = "TMV";
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }
                        break;
                    }
                // Выборка
                case 'S':
                    {
                        switch (command[2])
                        {
                            // Выборка списка игровых столов по выбранным параметрам
                            case 'T':
                                {
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }
                        break;
                    }
                // Работа с игроками
                case 'P':
                    {
                        switch (command[2])
                        {
                            // Добавление игрока на стол в режиме ожидания
                            case 'A':
                                {
                                    break;
                                }
                            // Удаление игрока со стола в режиме ожидания
                            case 'D':
                                {
                                    break;
                                }
                            // Выход игрока со стола в режиме игры
                            case 'Q':
                                {
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }
                        break;
                    }
                default:
                    {
                        break;
                    }

            }
            return Result;
        }


    }
}
