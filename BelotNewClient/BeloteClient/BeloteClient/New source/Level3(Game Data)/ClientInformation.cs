using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeloteClient
{
    class ClientInformation
    {
        //**********************************************************************************************************************************************************************************
        //                      Поля данных
        //**********************************************************************************************************************************************************************************
        private ServerActions serverActions;
        private Player player;
        private TablesList tablesList;
        private PlayersList playersList;
        private Table currentTable;
        private int place;


        //**********************************************************************************************************************************************************************************
        //                      Конструкторы
        //**********************************************************************************************************************************************************************************
        public ClientInformation()
        {
            try
            {
                serverActions = new ServerActions();
                tablesList = new TablesList();
                playersList = new PlayersList();
                place = -1;
                Status = GameStatus.NON_GAME;
            }
            catch (Exception Ex)
            {
                throw new BeloteClientException("Невозможно начать взаимодействие с сервером", Ex);
            }
        }

        //**********************************************************************************************************************************************************************************
        //                      Вспомогательные закрытые методы для работы со списками игроков и столов
        //**********************************************************************************************************************************************************************************

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
        private void ChangeCurrentTable(Table newCurrentTable, int newPlace = -1)
        {
            tablesList.Clear();
            currentTable = newCurrentTable;
            place = (currentTable == null) ? -1 : newPlace;
            Status = (currentTable == null) ? GameStatus.NON_GAME : GameStatus.WAITING;
            if (newCurrentTable != null)
            {
                tablesList.AddTable(newCurrentTable);
            }
            UpdatePlayers();
        }


        //**********************************************************************************************************************************************************************************
        //                      Методы авторизации
        //**********************************************************************************************************************************************************************************

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


        //**********************************************************************************************************************************************************************************
        //                      Методы работы со столами: обновление списка столов, создание, посадка на стол, выход со стола
        //**********************************************************************************************************************************************************************************

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
                    AddBot(i);
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
                if (currentTable == null)
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

        // Добавление бота на текущий стол
        public bool AddBot(int BotPlace)
        {
            if (currentTable == null)
                return false;
            if (serverActions.Tables.AddBotToTable(BotPlace))
            {
                currentTable.SetPlayerAtPlace(-BotPlace, BotPlace);
                serverActions.Tables.TestFullfillTable();
                return true;
            }
            else
            {
                return false;
            }
        }

        // Удаление бота с игрового стола
        public void DeleteBot(int BotPlace)
        {
            serverActions.Tables.DeleteBotFromTable(BotPlace);
        }

        // Выход игрока со стола. IsSelf - сам ли игрок вышел со стола
        public void ExitFromTable(bool IsSelf)
        {
            if (IsSelf)
            {
                if (Status == GameStatus.WAITING)
                    serverActions.Tables.ExitPlayerFromTable(place);
                else
                    serverActions.Game.PlayerQuitFromTable(place);
            }
            //SetPreGameHandlers(false);
            ChangeCurrentTable(null);
        }

        //**********************************************************************************************************************************************************************************
        //                      Методы установки и снятия обработчиков игровых событий
        //**********************************************************************************************************************************************************************************

        // Устанавливает все необходимые обработчики событий для игры
        private void SetPreGameHandlers(bool IsSet)
        {
            // Установка
            if (IsSet)
            {
                serverActions.Handlers.SetPreGameHandlers(PlayerAddHandler, PlayerDeleteHandler, CreatorLeaveHandler, StartGameHandler);
            }
            // Снятие
            else
            {
                serverActions.Handlers.UnsetPreGameHandlers(PlayerAddHandler, PlayerDeleteHandler, CreatorLeaveHandler, StartGameHandler);
            }
        }

        //**********************************************************************************************************************************************************************************
        //                      Обработчики доигровых событий: посадка игрока на стол, удаление игрока со стола, старт игры
        //**********************************************************************************************************************************************************************************

        // Обработчик добавления другого игрока на стол
        private void PlayerAddHandler(Message Msg)
        {
            MessageResult pParams = new MessageResult(Msg);
            int PlayerID = Int32.Parse(pParams["Player"]);
            int PlayerPlace = Int32.Parse(pParams["Place"]);
            currentTable.SetPlayerAtPlace(PlayerID, PlayerPlace);
            if (!Players.PlayerExists(PlayerID))
            {
                Player p = serverActions.Players.GetPlayer(PlayerID);
                if (p != null)
                    Players.Add(p);
            }
        }

        // Обработчик удаление другого игрока со стола
        private void PlayerDeleteHandler(Message Msg)
        {
            MessageResult pParams = new MessageResult(Msg);
            int PlayerPlace = Int32.Parse(pParams["Place"]);
            Players.Delete(Players[currentTable[PlayerPlace]]);
            currentTable.SetPlayerAtPlace(-1, PlayerPlace);
        }

        // Обработчик выхода со стола создателя
        private void CreatorLeaveHandler(Message Msg)
        {
            ExitFromTable(false);
        }

        // Обработка начала игры
        private void StartGameHandler(Message Msg)
        {
            SetPreGameHandlers(false);
            Status = GameStatus.GAMING;
            SetGameHandlers(true);
        }

        //**********************************************************************************************************************************************************************************
        //                      Основные доступные извне игровые свойства
        //**********************************************************************************************************************************************************************************

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

        public GameStatus Status
        {
            get;
            private set;
        }
    }
}
