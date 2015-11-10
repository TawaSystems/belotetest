using System;
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
                    }
                    if (t.Player2 >= 0)
                    {
                        if (!Players.PlayerExists(t.Player2))
                        {
                            p = serverActions.GetPlayer(t.Player2);
                        }
                    }
                    if (t.Player3 >= 0)
                    {
                        if (!Players.PlayerExists(t.Player3))
                        {
                            p = serverActions.GetPlayer(t.Player3);
                        }
                    }
                    if (t.Player4 >= 0)
                    {
                        if (!Players.PlayerExists(t.Player4))
                        {
                            p = serverActions.GetPlayer(t.Player4);
                        }
                    }
                    if (p != null)
                        Players.Add(p);
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
                SetGameHandlers(true);
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
                SetGameHandlers(true);
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
            SetGameHandlers(false);
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
        ///                             Методы, связанные с обработкой событий: их установка, снятие и сами обработчики
        /// 
        /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        // Устанавливает все необходимые обработчики событий для игры
        public void SetGameHandlers(bool IsSet)
        {
            // Установка
            if (IsSet)
            {
                serverActions.AddMessageHandler(Messages.MESSAGE_TABLE_PLAYERS_ADD, PlayerAddHandler);
                serverActions.AddMessageHandler(Messages.MESSAGE_TABLE_PLAYERS_DELETE, PlayerDeleteHandler);
                serverActions.AddMessageHandler(Messages.MESSAGE_TABLE_MODIFY_CREATORLEAVE, CreatorLeaveHandler);
            }
            // Снятие
            else
            {
                serverActions.DeleteMessageHandler(Messages.MESSAGE_TABLE_PLAYERS_ADD, PlayerAddHandler);
                serverActions.DeleteMessageHandler(Messages.MESSAGE_TABLE_PLAYERS_DELETE, PlayerDeleteHandler);
                serverActions.DeleteMessageHandler(Messages.MESSAGE_TABLE_MODIFY_CREATORLEAVE, CreatorLeaveHandler);
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

        /// <summary>
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
