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
    class ClientMan : Client
    {
        // Подключение клиента 
        private TcpClient client;
        // Поток, обрабатывающий запросы пользователя
        private Thread worker;
        // Ссылка на игровой объект
        private Game game;

        // Конструктор с инициализацией всех объектов
        public ClientMan(TcpClient tcpClient, Game game)
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
            ActiveTable = null;
            ActivePlace = -1;
            // Обработка нового клиента происходит в отдельном потоке
            worker = new Thread(Process);
            worker.Start();
        }
   
        // Идентификатор клиента
        public override int ID
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
        public override void SendMessage(string message)
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
                Debug.WriteLine("Количество подключенных клиентов в списке: " + this.game.Server.Clients.Count);
                Debug.Unindent();
#endif
                if (ActiveTable != null)
                {
                    if (ActivePlace == 1)
                    {
                        ProcessCommand(Messages.MESSAGE_TABLE_MODIFY_CREATORLEAVE);
                    }
                    else
                    {
                        if (ActiveTable.Status == TableStatus.WAITING)
                        {
                            ProcessCommand(Messages.MESSAGE_TABLE_PLAYERS_DELETE);
                        }
                        else
                        if (ActiveTable.Status == TableStatus.PLAYING)
                        {
                            ProcessCommand(Messages.MESSAGE_TABLE_PLAYERS_QUIT);
                        }
                    }
                }
                this.game.Server.Clients.DeleteClient(this);
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
                case Messages.MESSAGE_TABLE_SELECT_ALL:
                case Messages.MESSAGE_TABLE_PLAYERS_ADDBOT:
                case Messages.MESSAGE_TABLE_SELECT_CONCRETIC:
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
                case Messages.MESSAGE_PLAYER_GET_INFORMATION:
                case Messages.MESSAGE_PLAYER_GET_STATISTICS:
                case Messages.MESSAGE_PLAYER_GET_AVATAR:
                case Messages.MESSAGE_PLAYER_GET_ACCOUNTS:
                    {
                        Result = ProcessPlayer(command, msg);
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
                            Result = String.Format("{0}PlayerID={1}", Messages.MESSAGE_AUTORIZATION_AUTORIZATION_EMAIL, Player.Profile.Id);
                        }
                        else
                        {
                            Result = String.Format("{0}PlayerID={1}", Messages.MESSAGE_AUTORIZATION_AUTORIZATION_EMAIL, -1);
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
                            Result = Messages.MESSAGE_AUTORIZATION_REGISTRATION_EMAIL + "Registration=1";
                        }
                        else
                        {
                            // Ошибка в регистрации
                            Result = Messages.MESSAGE_AUTORIZATION_REGISTRATION_EMAIL + "Registration=0";
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
                        Result = Messages.MESSAGE_AUTORIZATION_REMINDPASSWORD_EMAIL + ((resRemind) ? "Remind=1" : "Remind=0");
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
                            Result = Messages.MESSAGE_AUTORIZATION_TEST_EMAIL + "Exists=1";
                        }
                        else
                        {
                            Result = Messages.MESSAGE_AUTORIZATION_TEST_EMAIL + "Exists=0";
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
                            Result = Messages.MESSAGE_AUTORIZATION_TEST_NICKNAME + "Exists=1";
                        }
                        else
                        {
                            Result = Messages.MESSAGE_AUTORIZATION_TEST_NICKNAME + "Exists=0";
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
            string Result = null;
            switch (command)
            {
                // Создание игрового стола
                case Messages.MESSAGE_TABLE_MODIFY_CREATE:
                    {
                        int tableID = this.game.Tables.CreateTable(this, Int32.Parse(tableParams["Bet"]),
                                        Helpers.StringToBool(tableParams["PlayersVisibility"]), Helpers.StringToBool(tableParams["Chat"]), Int32.Parse(tableParams["MinimalLevel"]),
                                        Helpers.StringToBool(tableParams["TableVisibility"]), Helpers.StringToBool(tableParams["VIPOnly"]), Helpers.StringToBool(tableParams["Moderation"]),
                                        Helpers.StringToBool(tableParams["AI"]));
                        ActivePlace = 1;
                        ActiveTable = this.game.Tables[tableID];
                        Result = Messages.MESSAGE_TABLE_MODIFY_CREATE + "ID=" + tableID.ToString();
                        break;
                    }
                // Покидание игрового стола создателем
                case Messages.MESSAGE_TABLE_MODIFY_CREATORLEAVE:
                    {
                        ActiveTable.SendMessageToClientsWithoutCreator(Messages.MESSAGE_TABLE_MODIFY_CREATORLEAVE);
                        ActiveTable.CloseTable();
                        ActiveTable = null;
                        ActivePlace = 0;
                        break;
                    }
                // Открытие стола для всех игроков (TableVisibility = true)
                case Messages.MESSAGE_TABLE_MODIFY_VISIBILITY:
                    {
                        ActiveTable.TableVisibility = true;
                        break;
                    }
                // Добавление игрока на стол в режиме ожидания
                case Messages.MESSAGE_TABLE_PLAYERS_ADD:
                    {
                        int tableID = Int32.Parse(tableParams["ID"]);
                        // Добавляем того клиента от кого и пришло сообщение
                        ClientMan c = this;
                        int place = Int32.Parse(tableParams["Place"]);
                        Result = Messages.MESSAGE_TABLE_PLAYERS_ADD + "Result=";
                        if (this.game.Tables.AddPlayer(tableID, c, place))
                        {
                            Result += "1";
                            ActiveTable = this.game.Tables[tableID];
                            ActivePlace = place;
                        }
                        else
                            Result += "0";
                        break;
                    }
                // Удаление игрока со стола в режиме ожидания
                case Messages.MESSAGE_TABLE_PLAYERS_DELETE:
                    {
                        this.game.Tables.RemovePlayer(ActiveTable.ID, ActivePlace);
                        ActivePlace = -1;
                        ActiveTable = null;
                        break;
                    }
                // Добавление бота на стол
                case Messages.MESSAGE_TABLE_PLAYERS_ADDBOT:
                    {
                        int place = Int32.Parse(tableParams["Place"]);
                        ClientBot b = new ClientBot(place, ActiveTable.ID);
                        Result = Messages.MESSAGE_TABLE_PLAYERS_ADDBOT + "Result=";
                        if (this.game.Tables.AddPlayer(ActiveTable.ID, b, place))
                        {
                            Result += "1";
                        }
                        else
                            Result += "0";
                        break;
                    }
                // Выход игрока со стола в режиме игры
                case Messages.MESSAGE_TABLE_PLAYERS_QUIT:
                    {
                        this.game.Tables.RemovePlayer(ActiveTable.ID, ActivePlace);
                        // Если разрешена замена на AI
                        if (ActiveTable.AI)
                        {
                            ClientBot b = new ClientBot(ActivePlace, ActiveTable.ID);
                            switch (ActivePlace)
                            {
                                case 2:
                                    {
                                        ActiveTable.Player2 = b;
                                        break;
                                    }
                                case 3:
                                    {
                                        ActiveTable.Player3 = b;
                                        break;
                                    }
                                case 4:
                                    {
                                        ActiveTable.Player4 = b;
                                        break;
                                    }
                                default:
                                    {
                                        break;
                                    }
                            }
                            string msg = String.Format("{0}Continue=1,Place={1},NewPlayer={2}", Messages.MESSAGE_TABLE_PLAYERS_QUIT, ActivePlace, b.ID);
                            ActiveTable.SendMessageToClients(msg);
                            ActiveTable = null;
                            ActivePlace = -1;
                        }
                        // Если нет, то игра на столе завершается
                        else
                        {
                            string msg = String.Format("{0}Continue=0", Messages.MESSAGE_TABLE_PLAYERS_QUIT);
                            ActiveTable.SendMessageToClients(msg);
                            ActiveTable.CloseTable();
                        }
                        ActiveTable = null;
                        ActivePlace = -1;
                        break;
                    }
                // Выборка списка игровых столов по выбранным параметрам
                case Messages.MESSAGE_TABLE_SELECT_TABLES:
                    {
                        List<Table> list = this.game.Tables.FindTables(Int32.Parse(tableParams["BetFrom"]), Int32.Parse(tableParams["BetTo"]), Helpers.StringToBool(tableParams["PlayersVisibility"]),
                                        Helpers.StringToBool(tableParams["Chat"]), Int32.Parse(tableParams["MinimalLevel"]), Helpers.StringToBool(tableParams["VIPOnly"]),
                                        Helpers.StringToBool(tableParams["Moderation"]), Helpers.StringToBool(tableParams["AI"]));
                        foreach (Table t in list)
                        {
                            string m = String.Format("{12}ID={0},Bet={1},PlayersVisibility={2},Chat={3},MinimalLevel={4},VIPOnly={5},Moderation={6},AI={7},Creator={8},Player2={9},Player3={10},Player4={11}",
                                t.ID, t.Bet, Helpers.BoolToString(t.PlayersVisibility), Helpers.BoolToString(t.Chat), t.MinimalLevel, Helpers.BoolToString(t.VIPOnly),
                                Helpers.BoolToString(t.Moderation), Helpers.BoolToString(t.AI), t.TableCreator.ID, (t.Player2 != null) ? t.Player2.ID : -1, (t.Player3 != null) ? t.Player3.ID : -1,
                                (t.Player4 != null) ? t.Player4.ID : -1, Messages.MESSAGE_TABLE_SELECT_TABLES);
                            this.SendMessage(m);
                        }
                        break;
                    }
                // Выборка всех доступных столов
                case Messages.MESSAGE_TABLE_SELECT_ALL:
                    {
                        List<Table> list = this.game.Tables.AllAvailableTables();
                        foreach (Table t in list)
                        {
                            string m = String.Format("{12}ID={0},Bet={1},PlayersVisibility={2},Chat={3},MinimalLevel={4},VIPOnly={5},Moderation={6},AI={7},Creator={8},Player2={9},Player3={10},Player4={11}",
                                t.ID, t.Bet, Helpers.BoolToString(t.PlayersVisibility), Helpers.BoolToString(t.Chat), t.MinimalLevel, Helpers.BoolToString(t.VIPOnly),
                                Helpers.BoolToString(t.Moderation), Helpers.BoolToString(t.AI), t.TableCreator.ID, (t.Player2 != null) ? t.Player2.ID : -1, (t.Player3 != null) ? t.Player3.ID : -1,
                                (t.Player4 != null) ? t.Player4.ID : -1, Messages.MESSAGE_TABLE_SELECT_TABLES);
                            this.SendMessage(m);
                        }
                        break;
                    }
                case Messages.MESSAGE_TABLE_SELECT_CONCRETIC:
                    {
                        int tableID = Int32.Parse(tableParams["ID"]);
                        Table t = this.game.Tables[tableID];
                        Result = String.Format("{12}ID={0},Bet={1},PlayersVisibility={2},Chat={3},MinimalLevel={4},VIPOnly={5},Moderation={6},AI={7},Creator={8},Player2={9},Player3={10},Player4={11}",
                                t.ID, t.Bet, Helpers.BoolToString(t.PlayersVisibility), Helpers.BoolToString(t.Chat), t.MinimalLevel, Helpers.BoolToString(t.VIPOnly),
                                Helpers.BoolToString(t.Moderation), Helpers.BoolToString(t.AI), t.TableCreator.ID, (t.Player2 != null) ? t.Player2.ID : -1, (t.Player3 != null) ? t.Player3.ID : -1,
                                (t.Player4 != null) ? t.Player4.ID : -1, Messages.MESSAGE_TABLE_SELECT_CONCRETIC);
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
            string Result = null;
            switch (command)
            {
                // Обработка ставки игрока
                case Messages.MESSAGE_GAME_BAZAR_BET:
                    {
                        int orderSize = Int32.Parse(gameParams["Size"]);
                        OrderType type = (OrderType)Int32.Parse(gameParams["Type"]);
                        CardSuit suit = Helpers.StringToSuit(gameParams["Trump"]);
                        // Добавление заявки в список заявок текущего стола
                        ActiveTable.AddOrder(new Order(type, orderSize, suit));
                        break;
                    }
                // Завершение процесса торговли
                case Messages.MESSAGE_GAME_BAZAR_END:
                    {
                        break;
                    }
                // Переход хода к следующему игроку
                case Messages.MESSAGE_GAME_BAZAR_NEXTBETPLAYER:
                    {
                        break;
                    }
                // Объявление сделанной игроком ставки для остальных игроков
                case Messages.MESSAGE_GAME_BAZAR_SAYBET:
                    {
                        break;
                    }
                // Список всех возможных бонусов игрока
                case Messages.MESSAGE_GAME_BONUSES_ALL:
                    {
                        break;
                    }
                // Анонсирование бонусов игроком
                case Messages.MESSAGE_GAME_BONUSES_ANNOUNCE:
                    {
                        int bonusCount = Int32.Parse(gameParams["Count"]);
                        string Str = gameParams["Count"];
                        for (var i = 0; i < bonusCount; i++)
                        {
                            Str += String.Format(",Bonus{0}={1}", i, gameParams["Bonus" + i.ToString()]);
                        }
                        BonusList bList = new BonusList(Str);
                        ActiveTable.AnnounceBonuses(ActivePlace, bList);
                        break;
                    }
                // Уведомление о типах анонсированных бонусов для других игроков
                case Messages.MESSAGE_GAME_BONUSES_TYPES:
                    {
                        break;
                    }
                // Команда-победитель в бонусах
                case Messages.MESSAGE_GAME_BONUSES_WINNER:
                    {
                        break;
                    }
                // Раздача карт
                case Messages.MESSAGE_GAME_DISTRIBUTIONCARDS:
                    {
                        break;
                    }
                // Завершение игры на столе
                case Messages.MESSAGE_GAME_END:
                    {
                        break;
                    }
                // Уведомление следующего игрока о ходе
                case Messages.MESSAGE_GAME_GAMING_NEXTPLAYER:
                    {
                        break;
                    }
                // Ход игрока
                case Messages.MESSAGE_GAME_GAMING_PLAYERMOVE:
                    {
                        Card card = new Card(gameParams["Card"]);
                        ActiveTable.PlayerMove(ActivePlace, card);
                        break;
                    }
                // Уведомление других игроков о карте которой походил игрок
                case Messages.MESSAGE_GAME_GAMING_REMINDCARD:
                    {
                        break;
                    }
                // Начало игры
                case Messages.MESSAGE_GAME_START:
                    {
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            return Result;
        }

        // Обработка действий, связанных с профилем пользователя
        public string ProcessPlayer(string command, string message)
        {
            Dictionary<string, string> playerParams = Helpers.SplitCommandString(message);
            string Result = null;
            switch (command)
            {
                // Получение информации о профиле пользователя
                case Messages.MESSAGE_PLAYER_GET_INFORMATION:
                    {
                        int playerID = Int32.Parse(playerParams["PlayerID"]);
                        Player p;
                        if (playerID != -1)
                        {
                            if (this.ID == playerID)
                            {
                                p = this.Player;
                            }
                            else
                            {
                                p = new Player(this.game, playerID);
                            }
                            Result = String.Format("{17}PlayerID={0},Nickname={1},Name={2},Surname={3},Email={4},Phone={5},VK={6},FB={7},OK={8},Country={9},Address={10},ZipCode={11},Language={12},Sex={13},TimeZone={14},BirthDate={15},VIPExperies={16}",
                                p.Profile.Id, p.Profile.Nickname, p.Profile.Name, p.Profile.Surname, p.Profile.Email, p.Profile.Phone, p.Profile.VK, p.Profile.FB,
                                p.Profile.OK, p.Profile.Country, p.Profile.Address, p.Profile.ZipCode, p.Profile.Language, Helpers.BoolToString(p.Profile.Sex),
                                p.Profile.TimeZone, p.Profile.BirtDate, p.Profile.VIPExperies, Messages.MESSAGE_PLAYER_GET_INFORMATION);
                        }
                        break;
                    }
                // Получение информации о статистике пользователя
                case Messages.MESSAGE_PLAYER_GET_STATISTICS:
                    {
                        break;
                    }
                // Получение аватара пользователя
                case Messages.MESSAGE_PLAYER_GET_AVATAR:
                    {
                        break;
                    }
                // Получение информации о счетах пользователя
                case Messages.MESSAGE_PLAYER_GET_ACCOUNTS:
                    {
                        int playerID = Int32.Parse(playerParams["PlayerID"]);
                        if (playerID != -1)
                        {
                            if (this.ID == playerID)
                            {
                                Result = String.Format("{4}PlayerID={0},USD={1},BUSD={2},Chips={3}", this.ID, this.Player.Profile.USD, this.Player.Profile.BUSD, this.Player.Profile.Chips,
                                    Messages.MESSAGE_PLAYER_GET_ACCOUNTS);
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
