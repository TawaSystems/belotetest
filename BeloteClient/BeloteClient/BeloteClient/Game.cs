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
        private GameForm gameForm;

        public Game()
        {
            AppDomain.CurrentDomain.ProcessExit += ProcessExit;
            try
            {
                serverActions = new ServerActions();
                Tables = new TablesList();
                CurrentTable = null;
                Player = null;
                Players = new PlayersList();
                Place = -1;
                guestForm = new MainGuestForm(this);
                guestForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// 
        ///                             Вспомогательные методы обновления списка игроков/столов
        /// 
        /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        public void UpdatePlayers()
        {
            Players.Clear();
            Players.Add(Player);
            if (Tables.Count > 0)
            {
                for (var i = 0; i < Tables.Count; i++)
                {
                    Player p = null;
                    Table t = Tables.GetTableAt(i);
                    if (!Players.PlayerExists(t.TableCreator))
                    {
                        p = serverActions.GetPlayer(t.TableCreator);
                        if (p != null)
                            Players.Add(p);
                    }
                    if (t.Player2 >= 0)
                    {
                        if (!Players.PlayerExists(t.Player2))
                        {
                            p = serverActions.GetPlayer(t.Player2);
                            if (p != null)
                                Players.Add(p);
                        }
                    }
                    if (t.Player3 >= 0)
                    {
                        if (!Players.PlayerExists(t.Player3))
                        {
                            p = serverActions.GetPlayer(t.Player3);
                            if (p != null)
                                Players.Add(p);
                        }
                    }
                    if (t.Player4 >= 0)
                    {
                        if (!Players.PlayerExists(t.Player4))
                        {
                            p = serverActions.GetPlayer(t.Player4);
                            if (p != null)
                                Players.Add(p);
                        }
                    }
                }
            }
        }

        // Добавление информации о всех доступных столах и игроках с них в соответствующие списки
        public void UpdatePossibleTables()
        {
            Tables = serverActions.GetAllPossibleTables();
            if (Tables == null)
                Tables = new TablesList();
            UpdatePlayers();
        }

        public void ChangeCurrentTable(Table newCurrentTable)
        {
            Tables.Clear();
            CurrentTable = newCurrentTable;
            if (CurrentTable != null)
            {
                Tables.AddTable(CurrentTable);
            }
            UpdatePlayers();
        }

        public void ChangeCurrentPlace(int newPlace)
        {
            Place = newPlace;
        }

        /// <summary>
        /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// 
        ///                             Системные методы
        /// 
        /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        // Событие при выходе из приложения
        public void ProcessExit(Object Sender, EventArgs e)
        {
            if (serverActions != null)
                serverActions.Disconnect();
        }

        /// <summary>
        /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// 
        ///                             Методы выполнения основных игровых событий: регистрация, авторизация, вход/создание столов, игра
        /// 
        /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        // Регистрация с помощью электронной почты
        public void RegistrationEmail(string Email, string Password, string Nickname, string Sex, string Country)
        {
            if (serverActions.RegistrationEmail(Email, Password, Nickname, Sex, Country))
            {
                MessageBox.Show("Регистрация прошла успешно!");
            }
            else
            {
                MessageBox.Show("Регистрация не удалась");
            }
        }

        // Авторизация с помощью электронной почты
        public void AutorizationEmail(string Email, string Password)
        {
            int PlayerID;
            if (serverActions.AutorizationEmail(Email, Password, out PlayerID))
            {
                //MessageBox.Show("Вход успешен!");
                Player = serverActions.GetPlayer(PlayerID);
                UpdatePlayers();
                guestForm.Close();
                guestForm = null;
                userForm = new MainUserForm(this);
                userForm.UpdateTables();
                userForm.Show();
            }
            else
            {
                MessageBox.Show("Не удалось войти");
            }
        }

        public void CreateTable(int Bet, bool PlayersVisibility, bool Chat, int MinimalLevel, bool TableVisibility,
            bool VIPOnly, bool Moderation, bool AI)
        {
            Table t = serverActions.CreateTable(this.Player.Profile.Id, Bet, PlayersVisibility, Chat, MinimalLevel, TableVisibility,
                VIPOnly, Moderation, AI);
            if (t != null)
            {
                //MessageBox.Show("Создание стола прошло успешно!");
                ChangeCurrentTable(t);
                ChangeCurrentPlace(1);
                if (userForm != null)
                {
                    userForm.Close();
                    userForm = null;
                }
                waitingForm = new WaitingForm(this);
                SetPreGameHandlers(true);
                waitingForm.UpdateLabels();
                waitingForm.Show();
            }
            else
            {
                MessageBox.Show("Не удалось создать игровой стол");
            } 
        }

        // Посадка на игровой стол
        public void EnterTheTable(int PlayerPlace, int TableID)
        {
            if (Tables[TableID] == null)
                return;
            if (serverActions.AddPlayerToTable(TableID, PlayerPlace))
            {
                ChangeCurrentTable(serverActions.GetTable(TableID));
                ChangeCurrentPlace(PlayerPlace);
                if (userForm != null)
                {
                    userForm.Close();
                    userForm = null;
                }
                waitingForm = new WaitingForm(this);
                waitingForm.UpdateLabels();
                SetPreGameHandlers(true);
                waitingForm.Show();
            }
            else
            {
                MessageBox.Show("Не удалось сесть на игровой стол");
                userForm.UpdateTables();
            }
        }

        // Выход игрока со стола. IsSelf - сам ли игрок вышел со стола
        public void ExitFromTable(bool IsSelf)
        {
            if (IsSelf)
                serverActions.ExitPlayerFromTable(Place);
            SetPreGameHandlers(false);
            ChangeCurrentTable(null);
            ChangeCurrentPlace(-1);
            waitingForm.Close();
            waitingForm = null;
            userForm = new MainUserForm(this);
            userForm.UpdateTables();
            userForm.Show();
        }

        // Добавление бота на стол
        public void AddBot(int BotPlace)
        {
            if (serverActions.AddBotToTable(BotPlace))
            {
                MessageBox.Show("Бот успешно добавлен!");
                switch (Place)
                {
                    case 2:
                        {
                            CurrentTable.Player2 = -BotPlace;
                            break;
                        }
                    case 3:
                        {
                            CurrentTable.Player3 = -BotPlace;
                            break;
                        }
                    case 4:
                        {
                            CurrentTable.Player4 = -BotPlace;
                            break;
                        }
                }
                waitingForm.UpdateLabels();
            }
            else
            {
                MessageBox.Show("Не удалось добавить бота на стол!");
            }
        }

        public void DeleteBot(int BotPlace)
        {
            serverActions.DeleteBotFromTable(BotPlace);
        }

        /// <summary>
        /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// 
        ///                             Методы, связанные с обработкой событий до игры: их установка, снятие и сами обработчики
        /// 
        /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        // Устанавливает все необходимые обработчики событий для игры
        public void SetPreGameHandlers(bool IsSet)
        {
            // Установка
            if (IsSet)
            {
                serverActions.SetPreGameHandlers(PlayerAddHandler, PlayerDeleteHandler, CreatorLeaveHandler, StartGameHandler);
            }
            // Снятие
            else
            {
                serverActions.UnsetPreGameHandlers(PlayerAddHandler, PlayerDeleteHandler, CreatorLeaveHandler, StartGameHandler);
            }
        }

        // Обработчик добавления другого игрока на стол
        public void PlayerAddHandler(Message Msg)
        {
            Dictionary<string, string> pParams = Helpers.SplitCommandString(Msg.Msg);
            int PlayerID = Int32.Parse(pParams["Player"]);
            int PlayerPlace = Int32.Parse(pParams["Place"]);
            switch (PlayerPlace)
            {
                case 2:
                    {
                        CurrentTable.Player2 = PlayerID;
                        break;
                    }
                case 3:
                    {
                        CurrentTable.Player3 = PlayerID;
                        break;
                    }
                case 4:
                    {
                        CurrentTable.Player4 = PlayerID;
                        break;
                    }
            }
            if (!Players.PlayerExists(PlayerID))
            {
                Player p = serverActions.GetPlayer(PlayerID);
                if (p != null)
                    Players.Add(p);
            }
            waitingForm.UpdateLabels();
        }

        // Обработчик удаление другого игрока со стола
        public void PlayerDeleteHandler(Message Msg)
        {
            Dictionary<string, string> pParams = Helpers.SplitCommandString(Msg.Msg);
            int PlayerPlace = Int32.Parse(pParams["Place"]);
            int PlayerID = -1;
            switch (PlayerPlace)
            {
                case 2:
                    {
                        PlayerID = CurrentTable.Player2;
                        CurrentTable.Player2 = -1;
                        break;
                    }
                case 3:
                    {
                        PlayerID = CurrentTable.Player3;
                        CurrentTable.Player3 = -1;
                        break;
                    }
                case 4:
                    {
                        PlayerID = CurrentTable.Player4;
                        CurrentTable.Player4 = -1;
                        break;
                    }
            }
            Players.Delete(Players[PlayerID]);
            waitingForm.UpdateLabels();
        }

        // Обработчик выхода со стола создателя
        public void CreatorLeaveHandler(Message Msg)
        {
            ExitFromTable(false);
        }

        // Обработка начала игры
        public void StartGameHandler(Message Msg)
        {
            SetPreGameHandlers(false);
            waitingForm.Close();
            waitingForm = null;
            gameForm = new GameForm();
            gameForm.Show();
            SetGameHandlers(true);
        }


        /// <summary>
        /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// 
        ///                             Методы, связанные с обработкой событий во время игры
        /// 
        /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // Установка обработчиков сообщений во время игры
        public void SetGameHandlers(bool IsSet)
        {
            if (IsSet)
            {
                serverActions.SetGameHandlers(GetCardsHandler, BazarNextPlayerHandler, BazarPlayerSayHandler, BazarEndHandler,
                    NextPlayerHandler, RemindCardHandler, BonusesShowTypesHandler, BonusesShowWinnerHandler, PlayerQuitHandler, GameEndHandler);
            }
            else
            {
                serverActions.UnsetGameHandlers(GetCardsHandler, BazarNextPlayerHandler, BazarPlayerSayHandler, BazarEndHandler,
                    NextPlayerHandler, RemindCardHandler, BonusesShowTypesHandler, BonusesShowWinnerHandler, PlayerQuitHandler, GameEndHandler);
            }
        }

        // Обработчик получения списка карт
        public void GetCardsHandler(Message Msg)
        {
            MessageBox.Show("Игрок №" + Place + ", карты получены: " + Msg.Msg);
        }

        // Переход хода к игроку во время торговли
        public void BazarNextPlayerHandler(Message Msg)
        {
            MessageBox.Show("Игрок №" + Place + " начинает торговлю");
        }

        // Произношение заявки сделанной одним из игроков
        public void BazarPlayerSayHandler(Message Msg)
        {

        } 

        // Завершение процесса торговли
        public void BazarEndHandler(Message Msg)
        {

        }

        // Переход хода к игроку
        public void NextPlayerHandler(Message Msg)
        {

        }

        // Объявление о типах объявленных бонусов одного из игроков
        public void BonusesShowTypesHandler(Message Msg)
        {

        }

        // Объявление победителя по бонусам
        public void BonusesShowWinnerHandler(Message Msg)
        {

        }

        // Ход другого игрока
        public void RemindCardHandler(Message Msg)
        {

        }

        // Завершение игры
        public void GameEndHandler(Message Msg)
        {

        }

        // Выход игрока со стола во время игры
        public void PlayerQuitHandler(Message Msg)
        {
            
        }
        /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// 
        ///                             Свойства
        /// 
        /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>

        public ServerActions serverActions
        {
            get;
            private set;
        }

        public Player Player
        {
            get;
            private set;
        }

        public TablesList Tables
        {
            get;
            private set;
        }

        public Table CurrentTable
        {
            get;
            private set;
        }

        public int Place
        {
            get;
            private set;
        }

        public PlayersList Players
        {
            get;
            private set;
        }
    }
}
