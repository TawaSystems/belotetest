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

        // Событие при выходе из приложения
        public void ProcessExit(Object Sender, EventArgs e)
        {
            if (serverActions != null)
                serverActions.Disconnect();
        }

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
                MessageBox.Show("Вход успешен!");
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

        public void UpdatePlayers()
        {
            Players.Clear();
            Players.Add(Player);
            if (Tables.Count > 0)
            {
                for (var i = 0; i < Tables.Count; i++)
                {
                    Table t = Tables.GetTableAt(i);
                    if (!Players.PlayerExists(t.TableCreator))
                    {
                        Players.Add(serverActions.GetPlayer(t.TableCreator));
                    }
                    if (t.Player2 >= 0)
                    {
                        if (!Players.PlayerExists(t.Player2))
                        {
                            Players.Add(serverActions.GetPlayer(t.Player2));
                        }
                    }
                    if (t.Player3 >= 0)
                    {
                        if (!Players.PlayerExists(t.Player3))
                        {
                            Players.Add(serverActions.GetPlayer(t.Player3));
                        }
                    }
                    if (t.Player4 >= 0)
                    {
                        if (!Players.PlayerExists(t.Player4))
                        {
                            Players.Add(serverActions.GetPlayer(t.Player4));
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

        public void CreateTable(int Bet, bool PlayersVisibility, bool Chat, int MinimalLevel, bool TableVisibility,
            bool VIPOnly, bool Moderation, bool AI)
        {
            Table t = serverActions.CreateTable(this.Player.Profile.Id, Bet, PlayersVisibility, Chat, MinimalLevel, TableVisibility,
                VIPOnly, Moderation, AI);
            if (t != null)
            {
                MessageBox.Show("Создание стола прошло успешно!");
                ChangeCurrentTable(t);
                ChangeCurrentPlace(1);
                if (userForm != null)
                {
                    userForm.Close();
                    userForm = null;
                }
                waitingForm = new WaitingForm(this);
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
                waitingForm.Show();
            }
            else
            {
                MessageBox.Show("Не удалось сесть на игровой стол");
                userForm.UpdateTables();
            }
        }

        public void ChangeCurrentTable(Table newCurrentTable)
        {
            Tables.Clear();
            CurrentTable = newCurrentTable;
            Tables.AddTable(CurrentTable);
            UpdatePlayers();
        }

        public void ChangeCurrentPlace(int newPlace)
        {
            Place = newPlace;
        }

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
