﻿using System;
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
                    // Отключение клиента
                    if (message == "EXT")
                    {
                        if (this.ID != -1)
                            this.game.Server.Clients.DeleteClient(this);
                        break;
                    }
                    string result = ProcessCommand(message);
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
                if (stream != null)
                    stream.Close();
                if (client != null)
                    client.Close();
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
            Debug.WriteLine("Client ID: " + ID);
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
                    {
                        Result = ProcessTables(command, msg);
                        break;
                    }
                // Обработка команд игрового процесса
                case 'G':
                    {
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
                                        this.game.Server.Clients.Add(this);
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
                                        Player = new Player(this.game);
                                        Player.ReadPlayerFromDataBase("Email", regParams["Email"]);
                                        this.game.Server.Clients.Add(this);
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
                            this.game.Server.Clients.DeleteClient(this);
                        }
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
                                    Client c = this.game.Server.Clients[Int32.Parse(tableParams["Player"])];
                                    int place = Int32.Parse(tableParams["Place"]);
                                    Result = "TPAResult=";
                                    switch (place)
                                    {
                                        case 2:
                                            {
                                                if (this.game.Tables.AddPlayer2(tableID, c))
                                                    Result += "1";
                                                else
                                                    Result += "0";
                                                break;
                                            }
                                        case 3:
                                            {
                                                if (this.game.Tables.AddPlayer3(tableID, c))
                                                    Result += "1";
                                                else
                                                    Result += "0";
                                                break;
                                            }
                                        case 4:
                                            {
                                                if (this.game.Tables.AddPlayer4(tableID, c))
                                                    Result += "1";
                                                else
                                                    Result += "0";
                                                break;
                                            }
                                        default:
                                            {
                                                Result += "0";
                                                break;
                                            }
                                    }
                                    break;
                                }
                            // Удаление игрока со стола в режиме ожидания
                            case 'D':
                                {
                                    int tableID = Int32.Parse(tableParams["ID"]);
                                    int place = Int32.Parse(tableParams["Place"]);
                                    switch (place)
                                    {
                                        case 2:
                                            {
                                                this.game.Tables.RemovePlayer2(tableID);
                                                break;
                                            }
                                        case 3:
                                            {
                                                this.game.Tables.RemovePlayer3(tableID);
                                                break;
                                            }
                                        case 4:
                                            {
                                                this.game.Tables.RemovePlayer4(tableID);
                                                break;
                                            }
                                        default:
                                            {
                                                break;
                                            }
                                    }
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
