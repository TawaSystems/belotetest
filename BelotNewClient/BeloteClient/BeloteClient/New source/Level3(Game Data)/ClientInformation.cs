using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeloteClient
{
    class ClientInformation
    {
        private ServerActions serverActions;
        private Player player;
        private TablesList tablesList;
        private PlayersList playersList;

        public ClientInformation()
        {
            try
            {
                serverActions = new ServerActions();
                tablesList = new TablesList();
                playersList = new PlayersList();
                CurrentGame = new GameProcess(serverActions);
            }
            catch (Exception Ex)
            {
                throw new BeloteClientException("Невозможно начать взаимодействие с сервером", Ex);
            }
        }

        // Составляет список игроков со всех доступных столов для быстрой возможности получения информации
        private void UpdatePlayers()
        {
            playersList.Clear();
            playersList.Add(player);
            if (tablesList.Count > 0)
            {
                for (var i = 0; i < tablesList.Count; i++)
                {
                    Table t = tablesList.GetTableAt(i);
                    for (var playerPlace = 1; playerPlace <= 4; playerPlace++)
                    {
                        if (t[playerPlace] >= 0)
                        {
                            if (!playersList.PlayerExists(t[playerPlace]))
                            {
                                Player p = serverActions.Players.GetPlayer(t[playerPlace]);
                                if (p != null)
                                {
                                    playersList.Add(p);
                                }
                            }
                        }
                    }
                }
            }
        }

        // Смена текущего игрового стола
        private void ChangeCurrentTable(Table newCurrentTable, int newPlace)
        {
            tablesList.Clear();
            CurrentGame.ChangeTable(newCurrentTable, newPlace);
            if (newCurrentTable != null)
            {
                tablesList.AddTable(newCurrentTable);
            }
            UpdatePlayers();
        }

        // Регистрация с помощью электронной почты
        public bool RegistrationEmail(string Email, string Password, string Nickname, string Sex, string Country)
        {
            return serverActions.Authorization.RegistrationEmail(Email, Password, Nickname, Sex, Country);
        }

        // Авторизация с помощью электронной почты
        public bool AutorizationEmail(string Email, string Password)
        {
            int PlayerID;
            if (serverActions.Authorization.AutorizationEmail(Email, Password, out PlayerID))
            {
                player = serverActions.Players.GetPlayer(PlayerID);
                if (player != null)
                {
                    UpdatePlayers();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        // Добавление информации о всех доступных столах и игроках с них в соответствующие списки
        public void UpdatePossibleTables()
        {
            tablesList = serverActions.Tables.GetAllPossibleTables();
            if (tablesList == null)
                tablesList = new TablesList();
            UpdatePlayers();
        }

        // Создание игрового стола
        public bool CreateTable(int Bet, bool PlayersVisibility, bool Chat, int MinimalLevel, bool TableVisibility,
            bool VIPOnly, bool Moderation, bool AI)
        {
            Table t = serverActions.Tables.CreateTable(player.ID, Bet, PlayersVisibility, Chat, MinimalLevel, TableVisibility,
                VIPOnly, Moderation, AI);
            if (t != null)
            {
                ChangeCurrentTable(t, 1);
                //SetPreGameHandlers(true);
                //waitingForm.UpdateLabels();
                return true;
            }
            else
            {
                return false;
            }
        }

        // Создание тренировочного стола
        public bool CreateTrainingTable()
        {
            Table t = serverActions.Tables.CreateTable(player.ID, 0, true, false, 0, false, false, false, false);
            if (t != null)
            {
                ChangeCurrentTable(t, 1);
                //SetPreGameHandlers(true);
                for (var i = 2; i <= 4; i++)
                {
                    CurrentGame.AddBot(i);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        // Посадка на игровой стол
        public bool EnterTheTable(int PlayerPlace, int TableID)
        {
            if (tablesList[TableID] == null)
                return false;
            if (serverActions.Tables.AddPlayerToTable(TableID, PlayerPlace))
            {
                ChangeCurrentTable(serverActions.Tables.GetTable(TableID), PlayerPlace);
                if (CurrentGame.CurrentTable == null)
                    return false;
                //SetPreGameHandlers(true);
                serverActions.Tables.TestFullfillTable();
                return true;
            }
            else
            {
                return false;
            }
        }

        // Список столов
        public TablesList Tables
        {
            get
            {
                return tablesList;
            }
        }

        // Список пользователей
        public PlayersList Players
        {
            get
            {
                return playersList;
            }
        }

        // Текущий игровой процесс
        public GameProcess CurrentGame
        {
            get;
            private set;
        }
    }
}
