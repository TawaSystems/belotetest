﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;
using System.Threading;

namespace BeloteClient
{
    public class Game
    {
        private MainGuestForm guestForm;
        private MainUserForm userForm;
        private WaitingForm waitingForm;

        public Game()
        {
            Dispatcher = Dispatcher.CurrentDispatcher;
            AppDomain.CurrentDomain.ProcessExit += ProcessExit;
            Player = null;
            CurrentTable = null;
            Place = -1;
            Tables = new TablesList(this);
            Players = new PlayersList(this);
            try
            {
                ServerConnection = new ServerConnection(this);
                StartWithGuest();    
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Environment.Exit(0);    
            } 
        }

        // Вход игрока на стол
        public void EnterTheTable(int Result)
        {
            if (Result == 1)
            {
                ServerConnection.SendDataToServer(String.Format("{0}ID={1}", Messages.MESSAGE_TABLE_SELECT_CONCRETIC, CurrentTable.ID));
                MessageBox.Show("Вход на стол выполнен успешно");   
            }
            else
            {
                MessageBox.Show("Не удалось войти на игровой стол");
                CurrentTable = null;
                Place = -1;
            }
        }

        // Получение информации об игровом столе, на который выполняется вход
        public void ReceiveTableInformation(Table t)
        {
            CurrentTable = t;
            userForm.Close();
            userForm = null;
            waitingForm = new WaitingForm(this);
            waitingForm.Show();
        }

        // Добавление игрока на стол в режиме ожидания игроков
        public void AddingToTable(int Player, int Place)
        {
            switch (Place)
            {
                case 2:
                    {
                        CurrentTable.Player2 = Player;
                        break;
                    }
                case 3:
                    {
                        CurrentTable.Player3 = Player;
                        break;
                    }
                case 4:
                    {
                        CurrentTable.Player4 = Player;
                        break;
                    }
                default:
                    break;
            }
            waitingForm.UpdateLabels();
        }

        // Создание игрового стола (получение результата от сервера)
        public void CreatingTable(int ID)
        {
            if (ID != -1)
            {
                MessageBox.Show("Создание игрового стола прошло успешно!");
                CurrentTable.ChangeID(ID);
                if (userForm != null)
                {
                    userForm.Close();
                    userForm = null;
                }
                Tables.Clear();
                Place = 1;
                waitingForm = new WaitingForm(this);
                waitingForm.Show();
            }
            else
            {
                MessageBox.Show("Не удалось создать игровой стол");
                CurrentTable = null;
            }
        }

        // Событие при выходе из приложения
        public void ProcessExit(Object Sender, EventArgs e)
        {
            if (ServerConnection != null)
                ServerConnection.Disconnect();
        }

        // Запускаем приложение с окна гостя
        public void StartWithGuest()
        {
            guestForm = new MainGuestForm(this);
            guestForm.Show();
        }

        // Регистрация с помощью электронной почты
        public void RegistrationEmail(string Email, string Password, string Nickname, string Sex, string Country)
        {
            ServerConnection.SendDataToServer(String.Format("{0}Nickname={1},Email={2},Password={3},Country={4},Sex={5}",
                Messages.MESSAGE_AUTORIZATION_REGISTRATION_EMAIL, Nickname, Email, Password, Country, Sex));
        }

        // Авторизация с помощью электронной почты
        public void AutorizationEmail(string Email, string Password)
        {
            ServerConnection.SendDataToServer(String.Format("{0}Email={1},Password={2}", Messages.MESSAGE_AUTORIZATION_AUTORIZATION_EMAIL,
                Email, Password));
        }

        // Результат регистрации 
        public void RegistrationResult(bool Success)
        {
            if (Success)
            {
                MessageBox.Show("Регистрация прошла успешно!");
            }
            else
            {
                MessageBox.Show("Не удалось зарегистрироваться. Попробуйте ввести другие данные");
            }
        }

        // Добавляет возмыжный к игре стол
        public void AddPossibleTable(int TableID, int Bet, bool PlayersVisibility, bool Chat, int MinimalLevel, bool VIPOnly, bool Moderation,
            bool AI, int Creator, int Player2, int Player3, int Player4)
        {
            Table table = new Table(this, TableID, Creator, Bet, PlayersVisibility, Chat, MinimalLevel, true, VIPOnly, Moderation, AI);
            table.Player2 = Player2;
            if (Player2 >= 0)
                ServerConnection.SendDataToServer(String.Format("{0}PlayerID={1}", Messages.MESSAGE_PLAYER_GET_INFORMATION, Player2));
            table.Player3 = Player3;
            if (Player3 >= 0)
                ServerConnection.SendDataToServer(String.Format("{0}PlayerID={1}", Messages.MESSAGE_PLAYER_GET_INFORMATION, Player3));
            table.Player4 = Player4;
            if (Player4 >= 0)
                ServerConnection.SendDataToServer(String.Format("{0}PlayerID={1}", Messages.MESSAGE_PLAYER_GET_INFORMATION, Player4));
            Tables.AddTable(table);
            if (userForm != null)
            {
                userForm.AddTable(table.ID);
            }
        }

        // Результат авторизации
        public void AutorizationResult(int ID)
        {
            if (ID != -1)
            {
                MessageBox.Show("Вход успешен!");
                Player = new Player(this, ID);
                ServerConnection.SendDataToServer(String.Format("{0}PlayerID={1}", Messages.MESSAGE_PLAYER_GET_INFORMATION, ID));
                guestForm.Close();
                guestForm = null;
                userForm = new MainUserForm(this);
                userForm.Show();
                userForm.UpdateTablesList();
            }
            else
            {
                MessageBox.Show("Не удалось войти");
            }
        }

        // Получение информации об игроке
        public void GetPlayerInformation(Player p)
        {
            if (p == null)
                return;
            if (Player.Profile.Id == p.Profile.Id)
            {
                Player = p;
                Players.Add(p);
            }
            else
            {
                if (!Players.PlayerExists(p.Profile.Id))
                    Players.Add(p);
            }
        }

        public ServerConnection ServerConnection
        {
            get;
            private set;
        }

        public Player Player
        {
            get;
            private set;
        }

        public Dispatcher Dispatcher
        {
            get;
            private set;
        }

        public TablesList Tables
        {
            get;
            private set;
        }

        public PlayersList Players
        {
            get;
            private set;
        }

        public Table CurrentTable
        {
            get;
            set;
        }

        public int Place
        {
            get;
            set;
        }
    }
}
