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
            Player = null;
            // Обработка нового клиента происходит в отдельном потоке
            worker = new Thread(Process);
            worker.Start();
        }
   
        // Идентификатор клиента
        public int ID
        {
            get
            {
                if (Player != null)
                    return Player.Profile.Id;
                else
                    return -1;
            }
        }

        // Профиль игрока
        public Player Player
        {
            get;
            private set;
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
                    // Считываем данные от клиента, пока они не закончатся
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
                    Debug.WriteLine("Client ID: " + ID);
                    Debug.WriteLine("Сообщение: " + message);
                    Debug.Unindent();
#endif
                    // Обрабатываем полученное сообщение
                    string result = ProcessCommand(message);

                    // Отключение клиента и завершение обработки сообщений от него
                    if (result == Messages.MESSAGE_CLIENT_DISCONNECT)
                    {
                        break;
                    }
                    // Если получен какой то результат обработки сообщения клиента, то отсылаем его клиенту
                    if (result != null)
                    {
#if DEBUG
                        Debug.WriteLine(DateTime.Now.ToString() + " Отправка сообщения клиенту");
                        Debug.Indent();
                        Debug.WriteLine("Client IP: " + ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString());
                        Debug.WriteLine("Client ID: " + ID);
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
#if DEBUG
                Debug.WriteLine(DateTime.Now.ToString() + " Отключение клиента");
                Debug.Indent();
                Debug.WriteLine("Client IP: " + ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString());
                Debug.WriteLine("Client ID: " + ID);
                Debug.Unindent();
#endif
                // Закрытие клиента и потока его данных
                if (stream != null)
                    stream.Close();
                if (client != null)
                    client.Close();
            }
        }

        // Функция обработки полученных от клиента комманд
        private string ProcessCommand(string message)
        {
            // Получаем из строки клиента команду и сообщение
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
            Debug.WriteLine("Client ID: " + ID);
            Debug.WriteLine("Команда: " + command);
            Debug.WriteLine("Сообщение: " + msg);
            Debug.Unindent();
#endif
            string Result = null;

            switch (command)
            {
                // Обработка команд авторизации
                case Messages.MESSAGE_AUTORIZATION_AUTORIZATION_EMAIL:
                case Messages.MESSAGE_AUTORIZATION_AUTORIZATION_FB:
                case Messages.MESSAGE_AUTORIZATION_AUTORIZATION_OK:
                case Messages.MESSAGE_AUTORIZATION_AUTORIZATION_PHONE:
                case Messages.MESSAGE_AUTORIZATION_AUTORIZATION_VK:
                case Messages.MESSAGE_AUTORIZATION_REGISTRATION_EMAIL:
                case Messages.MESSAGE_AUTORIZATION_REGISTRATION_FB:
                case Messages.MESSAGE_AUTORIZATION_REGISTRATION_OK:
                case Messages.MESSAGE_AUTORIZATION_REGISTRATION_PHONE:
                case Messages.MESSAGE_AUTORIZATION_REGISTRATION_VK:
                case Messages.MESSAGE_AUTORIZATION_REMINDPASSWORD_EMAIL:
                case Messages.MESSAGE_AUTORIZATION_REMINDPASSWORD_PHONE:
                case Messages.MESSAGE_AUTORIZATION_TEST_EMAIL:
                case Messages.MESSAGE_AUTORIZATION_TEST_FB:
                case Messages.MESSAGE_AUTORIZATION_TEST_NICKNAME:
                case Messages.MESSAGE_AUTORIZATION_TEST_OK:
                case Messages.MESSAGE_AUTORIZATION_TEST_PHONE:
                case Messages.MESSAGE_AUTORIZATION_TEST_VK:
                case Messages.MESSAGE_AUTORIZATION_USER_EXIT:
                    {
                        Result = ProcessAutorization(command, msg);
                        break;
                    }
                // Обработка отключения клиента
                case Messages.MESSAGE_CLIENT_DISCONNECT:
                    {
                        this.game.Server.Clients.DeleteClient(this);
                        Result = "EXT";
                        break;
                    }
                // Обработка модификации стола
                case Messages.MESSAGE_TABLE_MODIFY_CREATE:
                case Messages.MESSAGE_TABLE_MODIFY_CREATORLEAVE:
                case Messages.MESSAGE_TABLE_MODIFY_VISIBILITY:
                case Messages.MESSAGE_TABLE_PLAYERS_ADD:
                case Messages.MESSAGE_TABLE_PLAYERS_DELETE:
                case Messages.MESSAGE_TABLE_PLAYERS_QUIT:
                case Messages.MESSAGE_TABLE_SELECT_TABLES:
                    {
                        Result = ProcessTables(command, msg);
                        break;
                    }
                // Обработка процесса игры
                case Messages.MESSAGE_GAME_BAZAR_BET:
                case Messages.MESSAGE_GAME_BAZAR_END:
                case Messages.MESSAGE_GAME_BAZAR_NEXTBETPLAYER:
                case Messages.MESSAGE_GAME_BAZAR_SAYBET:
                case Messages.MESSAGE_GAME_BONUSES_ALL:
                case Messages.MESSAGE_GAME_BONUSES_ANNOUNCE:
                case Messages.MESSAGE_GAME_BONUSES_TYPES:
                case Messages.MESSAGE_GAME_BONUSES_WINNER:
                case Messages.MESSAGE_GAME_DISTRIBUTIONCARDS:
                case Messages.MESSAGE_GAME_END:
                case Messages.MESSAGE_GAME_GAMING_NEXTPLAYER:
                case Messages.MESSAGE_GAME_GAMING_PLAYERMOVE:
                case Messages.MESSAGE_GAME_GAMING_REMINDCARD:
                case Messages.MESSAGE_GAME_START:
                    {
                        Result = ProcessGame(command, msg);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
#if DEBUG
            Debug.WriteLine(DateTime.Now.ToString() + " Результат обработки сообщения от клиента");
            Debug.Indent();
            Debug.WriteLine("Client IP: " + ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString());
            Debug.WriteLine("Client ID: " + ID);
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
            // Получаем все параметры от клиента. Если не получено никаких параметров - заканчиваем обработку
            Dictionary<string, string> regParams = Helpers.SplitCommandString(message);
            if (regParams == null)
            {
                return null;
            }
            String Result = null;
            switch (command)
            {
                // Авторизация с помощью элктронной почты
                case Messages.MESSAGE_AUTORIZATION_AUTORIZATION_EMAIL:
                    {
                        if (game.Autorization.EnterEmail(regParams["Email"], regParams["Password"]))
                        {
                            Player = new Player(this.game);
                            Player.ReadPlayerFromDataBase("Email", regParams["Email"]);
                            Result = "AAEAutorization=1";
                        }
                        else
                        {
                            Result = "AAEAutorization=0";
                        }
                        break;
                    }
                // Авторизация с помощью Facebook
                case Messages.MESSAGE_AUTORIZATION_AUTORIZATION_FB:
                    {
                        break;
                    }
                // Авторизация с помощью Одноклассники
                case Messages.MESSAGE_AUTORIZATION_AUTORIZATION_OK:
                    {
                        break;
                    }
                // Авторизация с помощью мобильного телефона
                case Messages.MESSAGE_AUTORIZATION_AUTORIZATION_PHONE:
                    {
                        break;
                    }
                // Авторизация с помощью Вконтакте
                case Messages.MESSAGE_AUTORIZATION_AUTORIZATION_VK:
                    {
                        break;
                    }
                // Регистрация с помощью адреса электронной почты
                case Messages.MESSAGE_AUTORIZATION_REGISTRATION_EMAIL:
                    {
                        int id = game.Autorization.RegistrationEmail(regParams["Nickname"], regParams["Email"], regParams["Password"],
                                        regParams["Country"], (regParams["Sex"] == "1"));
                        if (id != -1)
                        {
                            // Регистрация прошла успешно
                            Player = new Player(this.game);
                            Player.Profile.Email = regParams["Email"];
                            Player.Profile.Nickname = regParams["Nickname"];
                            Player.Profile.Country = regParams["Country"];
                            Player.Profile.Sex = (regParams["Sex"] == "1");
                            Player.Profile.Id = id;
                            Result = "ARERegistration=1";
                        }
                        else
                        {
                            // Ошибка в регистрации
                            Result = "ARERegistration=0";
                        }
                        break;
                    }
                // Регистрация с помощью аккаунта Facebook
                case Messages.MESSAGE_AUTORIZATION_REGISTRATION_FB:
                    {
                        break;
                    }
                // Регистрация с помощью Одноклассники
                case Messages.MESSAGE_AUTORIZATION_REGISTRATION_OK:
                    {
                        break;
                    }
                // Регистрация с помощью мобильного телефона
                case Messages.MESSAGE_AUTORIZATION_REGISTRATION_PHONE:
                    {
                        break;
                    }
                // Регистрация с помощью ВКонтакте
                case Messages.MESSAGE_AUTORIZATION_REGISTRATION_VK:
                    {
                        break;
                    }
                // Напоминание пароля на почту
                case Messages.MESSAGE_AUTORIZATION_REMINDPASSWORD_EMAIL:
                    {
                        bool resRemind = game.Autorization.RemindPasswordEmail(regParams["Email"]);
                        Result = (resRemind) ? "AMERemind=1" : "AMERemind=0";
                        break;
                    }
                // Напоминание пароля на мобильный телефон
                case Messages.MESSAGE_AUTORIZATION_REMINDPASSWORD_PHONE:
                    {
                        break;
                    }
                // Тестирование на наличие почты
                case Messages.MESSAGE_AUTORIZATION_TEST_EMAIL:
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
                // Тестирование на наличие аккаунта Facebook
                case Messages.MESSAGE_AUTORIZATION_TEST_FB:
                    {
                        break;
                    }
                // Тестирование на наличие имени пользователя
                case Messages.MESSAGE_AUTORIZATION_TEST_NICKNAME:
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
                // Тестирование на наличие аккаунта Одноклассники
                case Messages.MESSAGE_AUTORIZATION_TEST_OK:
                    {
                        break;
                    }
                // Тестирование на наличие мобильного телефона
                case Messages.MESSAGE_AUTORIZATION_TEST_PHONE:
                    {
                        break;
                    }
                // Тестирование на наличие аккаунта ВКонтакте
                case Messages.MESSAGE_AUTORIZATION_TEST_VK:
                    {
                        break;
                    }
                // Выход пользователя из аккаунта
                case Messages.MESSAGE_AUTORIZATION_USER_EXIT:
                    {
                        Player = null;
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
                                    List<Table> list = this.game.Tables.FindTables(Int32.Parse(tableParams["BetFrom"]), Int32.Parse(tableParams["BetTo"]), Helpers.StringToBool(tableParams["PlayersVisibility"]),
                                        Helpers.StringToBool(tableParams["Chat"]), Int32.Parse(tableParams["MinimalLevel"]), Helpers.StringToBool(tableParams["VIPOnly"]),
                                        Helpers.StringToBool(tableParams["Moderation"]), Helpers.StringToBool(tableParams["AI"]));
                                    foreach (Table t in list)
                                    {
                                        string m = String.Format("TSTID={0},Bet={1},PlayersVisibility={2},Chat={3},MinimalLevel={4},VIPOnly={5},Moderation={6},AI={7},Creator={8},Player2={9},Player3={10},Player4={11}",
                                            t.ID, t.Bet, Helpers.BoolToString(t.PlayersVisibility), Helpers.BoolToString(t.Chat), t.MinimalLevel, Helpers.BoolToString(t.VIPOnly),
                                            Helpers.BoolToString(t.Moderation), Helpers.BoolToString(t.AI), t.TableCreator.ID, (t.Player2 != null) ? t.Player2.ID : -1, (t.Player3 != null) ? t.Player3.ID : -1,
                                            (t.Player4 != null) ? t.Player4.ID : -1);
                                        this.SendMessage(m);
                                    }
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
                                    int tableID = Int32.Parse(tableParams["ID"]);
                                    // Добавляем того клиента от кого и пришло сообщение
                                    Client c = this;
                                    int place = Int32.Parse(tableParams["Place"]);
                                    Result = "TPAResult=";
                                    if (this.game.Tables.AddPlayer(tableID, c, place))
                                        Result += "1";
                                    else
                                        Result += "0";
                                    break;
                                }
                            // Удаление игрока со стола в режиме ожидания
                            case 'D':
                                {
                                    int tableID = Int32.Parse(tableParams["ID"]);
                                    int place = Int32.Parse(tableParams["Place"]);
                                    this.game.Tables.RemovePlayer(tableID, place);
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

        // Функция обработки команд по игровому процессу
        private string ProcessGame(string command, string message)
        {
            Dictionary<string, string> gameParams = Helpers.SplitCommandString(message);
            if (gameParams == null)
            {
                return null;
            }
            string Result = null;
            switch (command[1])
            {
                // Базар, торговля
                case 'B':
                    {
                        switch (command[2])
                        {
                            // Заявка игрока
                            case 'B':
                                {
                                    int tableID = Int32.Parse(gameParams["ID"]);
                                    int orderSize = Int32.Parse(gameParams["Size"]);
                                    OrderType type = (OrderType)Int32.Parse(gameParams["Type"]);
                                    CardSuit suit = Helpers.StringToSuit(gameParams["Trump"]);
                                    // Добавление заявки в список заявок текущего стола
                                    this.game.Tables[tableID].AddOrder(new Order(type, orderSize, suit));
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }
                        break;
                    }
                // Процесс игры
                case 'G':
                    {
                        switch (command[2])
                        {
                            // Объявление игроком бонусов
                            case 'A':
                                {
                                    int tableID = Int32.Parse(gameParams["ID"]);
                                    int Place = Int32.Parse(gameParams["Place"]);
                                    int bonusCount = Int32.Parse(gameParams["Count"]);
                                    string Str = gameParams["Count"];
                                    for (var i = 0; i < bonusCount; i++)
                                    {
                                        Str += String.Format(",Bonus{0}={1}", i, gameParams["Bonus" + i.ToString()]);
                                    }
                                    BonusList bList = new BonusList(Str);
                                    this.game.Tables[tableID].AnnounceBonuses(Place, bList);
                                    break;
                                }
                            // Обработка хода игрока
                            case 'H':
                                {
                                    int tableID = Int32.Parse(gameParams["ID"]);
                                    int place = Int32.Parse(gameParams["Place"]);
                                    Card card = new Card(gameParams["Card"]);
                                    this.game.Tables[tableID].PlayerMove(place, card);
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
