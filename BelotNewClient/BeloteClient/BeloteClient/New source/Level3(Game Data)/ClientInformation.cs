using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeloteClient
{
    public delegate void GraphicCallbackDelegate();
    public class ClientInformation
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
        private GameData gameData;

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
        //                      Вспомогательные закрытые методы для работы со списками игроков и столов, а также обновления графики
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
        //                      Системные методы связи с сервером
        //**********************************************************************************************************************************************************************************

        public bool TestVersion()
        {
            return serverActions.Test.TestClientVersion();
        }

        public void Disconnect()
        {
            serverActions.Disconnect();
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
                SetPreGameHandlers(true);
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
                SetPreGameHandlers(true);
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
                SetPreGameHandlers(true);
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
        private void ExitFromTable(bool IsSelf)
        {
            if (IsSelf)
            {
                if (Status == GameStatus.WAITING)
                    serverActions.Tables.ExitPlayerFromTable(place);
                else
                    serverActions.Game.PlayerQuitFromTable(place);
            }
            if (Status == GameStatus.WAITING)
            {
                SetPreGameHandlers(false);
                ChangeCurrentTable(null);
            }
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

        // Установка обработчиков сообщений во время игры
        private void SetGameHandlers(bool IsSet)
        {
            if (IsSet)
            {
                serverActions.Handlers.SetGameHandlers(GetCardsHandler, BazarNextPlayerHandler, BazarPlayerSayHandler, BazarEndHandler, BazarPassHandler,
                    NextPlayerHandler, RemindCardHandler, BonusesGetAllHandler, BonusesShowTypesHandler, BonusesShowWinnerHandler, PlayerQuitHandler, GameEndHandler);
            }
            else
            {
                serverActions.Handlers.UnsetGameHandlers(GetCardsHandler, BazarNextPlayerHandler, BazarPlayerSayHandler, BazarEndHandler, BazarPassHandler,
                    NextPlayerHandler, RemindCardHandler, BonusesGetAllHandler, BonusesShowTypesHandler, BonusesShowWinnerHandler, PlayerQuitHandler, GameEndHandler);
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
            if (OnUpdateWaitingTable != null)
                OnUpdateWaitingTable();
        }

        // Обработчик удаление другого игрока со стола
        private void PlayerDeleteHandler(Message Msg)
        {
            MessageResult pParams = new MessageResult(Msg);
            int PlayerPlace = Int32.Parse(pParams["Place"]);
            Players.Delete(Players[currentTable[PlayerPlace]]);
            currentTable.SetPlayerAtPlace(-1, PlayerPlace);
            if (OnUpdateWaitingTable != null)
                OnUpdateWaitingTable();
        }

        // Обработчик выхода со стола создателя
        private void CreatorLeaveHandler(Message Msg)
        {
            ExitFromTable(false);
            if (OnExitFromTable != null)
                OnExitFromTable();
        }

        // Обработка начала игры
        private void StartGameHandler(Message Msg)
        {
            if (OnGameStart != null)
                OnGameStart();
            SetPreGameHandlers(false);
            Status = GameStatus.GAMING;
            gameData = new GameData();
            SetGameHandlers(true);
        }

        //**********************************************************************************************************************************************************************************
        //                      Обработчики основных игровых событийf
        //**********************************************************************************************************************************************************************************

        // Обработчик получения списка карт
        private void GetCardsHandler(Message Msg)
        {
            try
            {
                MessageResult cParams = new MessageResult(Msg);
                string cardsStr = cParams["Cards"];
                int totalScore1 = Int32.Parse(cParams["Scores1"]);
                int totalScore2 = Int32.Parse(cParams["Scores2"]);
                gameData.NewDistribution(new CardList(cardsStr), totalScore1, totalScore2);
                if (OnUpdateGraphics != null)
                    OnUpdateGraphics();
            }
            catch (Exception Ex)
            {
                throw new BeloteClientException("Произошла ошибка при получении списка карт", Ex);
            }
        }

        // Переход хода к игроку во время торговли
        private void BazarNextPlayerHandler(Message Msg)
        {
            try
            {
                gameData.IsMakingMove = true;
                MessageResult bParams = new MessageResult(Msg);
                gameData.Orders.PossibleBetType = (BetType)Int32.Parse(bParams["Type"]);
                gameData.Orders.PossibleBetSize = Int32.Parse(bParams["MinSize"]);
                if (OnBazarMakingBet != null)
                    OnBazarMakingBet();
            }
            catch (Exception Ex)
            {
                throw new BeloteClientException("Произошла ошибка при переходе хода во время торговли к игроку", Ex);
            }
        }

        // Произношение заявки сделанной одним из игроков
        private void BazarPlayerSayHandler(Message Msg)
        {
            try
            {
                MessageResult bParams = new MessageResult(Msg);
                int playerNum = Int32.Parse(bParams["Player"]);
                OrderType orderType = (OrderType)Int32.Parse(bParams["Type"]);
                int orderSize = Int32.Parse(bParams["Size"]);
                CardSuit orderSuit = Helpers.StringToSuit(bParams["Trump"]);
                gameData.Orders[playerNum] = new Order(orderType, orderSize, orderSuit);
                if (OnUpdateGraphics != null)
                    OnUpdateGraphics();
            }
            catch (Exception Ex)
            {
                throw new BeloteClientException("Произошла ошибка во время информирования игрока о ставке другого игрока", Ex);
            }
        }

        // Завершение процесса торговли
        private void BazarEndHandler(Message Msg)
        {
            try
            {
                MessageResult bParams = new MessageResult(Msg);
                BeloteTeam oTeam = (BeloteTeam)Int32.Parse(bParams["Team"]);
                OrderType oType = (OrderType)Int32.Parse(bParams["Type"]);
                CardSuit oSuit = (CardSuit)Int32.Parse(bParams["Trump"]);
                int oSize = Int32.Parse(bParams["Size"]);
                gameData.Orders.SetEndOrder(new Order(oType, oSize, oSuit));
                gameData.Orders.EndOrder.ChangeTeam(oTeam);
                gameData.ChangeGameStatus(TableStatus.BONUSES);
                if (OnUpdateGraphics != null)
                    OnUpdateGraphics();
            }
            catch (Exception Ex)
            {
                throw new BeloteClientException("Произошла ошибка во время завершения процесса торговли", Ex);
            }
        }

        // Завершение торговли без заявки козыря
        private void BazarPassHandler(Message Msg)
        {

        }

        // Получение списка возможных бонусов
        private void BonusesGetAllHandler(Message Msg)
        {
            gameData.Bonuses = new BonusList(Msg.Msg);
        }

        // Переход хода к игроку
        private void NextPlayerHandler(Message Msg)
        {
            try
            {
                // Если это первый ход, то нужно огласить бонусы
                if (gameData.Bonuses != null)
                {
                    // Если есть неоглашенные бонусы, то предлагаем их огласить
                    if (gameData.Bonuses.Count != 0)
                    {
                        if (OnAnnounceBonuses != null)
                            OnAnnounceBonuses();
                        serverActions.Game.PlayerAnnounceBonuses(gameData.Bonuses);
                        // Обнуляем бонусы
                        gameData.Bonuses = null;
                    }
                }
                MessageResult cParams = new MessageResult(Msg);
                // Получаем список возможных карт
                gameData.PossibleCards = new CardList(cParams["Cards"]);
                // Разрешаем игроку сделать ход
                gameData.IsMakingMove = true;
                if (OnUpdateGraphics != null)
                    OnUpdateGraphics();
            }
            catch (Exception Ex)
            {
                throw new BeloteClientException("Произошла ошибка во время перехода хода к игроку", Ex);
            }
        }

        // Объявление о типах объявленных бонусов одного из игроков
        private void BonusesShowTypesHandler(Message Msg)
        {
            try
            {
                MessageResult tParams = new MessageResult(Msg);
                int count = Int32.Parse(tParams["Count"]);
                int place = Int32.Parse(tParams["Place"]);
                if (count == 0)
                    return;
                for (var i = 0; i < count; i++)
                {
                    BonusType nextB = (BonusType)Int32.Parse(tParams["Type" + i.ToString()]);
                    gameData.AnnouncedBonuses.AddBonus(place, nextB);
                }
                if (OnUpdateGraphics != null)
                    OnUpdateGraphics();
            }
            catch (Exception Ex)
            {
                throw new BeloteClientException("Произошла ошибка при получении типов объявленныъ другим игроком бонусов", Ex);
            }
        }

        // Объявление победителя по бонусам
        private void BonusesShowWinnerHandler(Message Msg)
        {
            try
            {
                gameData.ChangeGameStatus(TableStatus.PLAY);
                MessageResult wParams = new MessageResult(Msg);
                BeloteTeam winner = (BeloteTeam)Int32.Parse(wParams["Winner"]);
                int scores = Int32.Parse(wParams["Scores"]);
                gameData.AnnouncedBonuses.SetWinner(winner, scores);
                if (OnShowBonusesWinner != null)
                    OnShowBonusesWinner();
                gameData.AnnouncedBonuses.Clear();
                if (OnUpdateGraphics != null)
                    OnUpdateGraphics();
            }
            catch (Exception Ex)
            {
                throw new BeloteClientException("Произошла ошибка во время объявления команды попедителя по бонусам", Ex);
            }
        }

        // Ход другого игрока
        private void RemindCardHandler(Message Msg)
        {
            try
            {
                MessageResult cParams = new MessageResult(Msg);
                int cardPlace = Int32.Parse(cParams["Place"]);
                Card newCard = new Card(cParams["Card"]);
                int beloteRemind = Int32.Parse(cParams["Belote"]);
                gameData.Bribes.PutCard(newCard, cardPlace);
                gameData.LocalScores[BeloteTeam.TEAM1_1_3] = Int32.Parse(cParams["Scores1"]);
                gameData.LocalScores[BeloteTeam.TEAM2_2_4] = Int32.Parse(cParams["Scores2"]);
                if (beloteRemind == 1)
                    gameData.Bribes.CurrentBribe.BelotePlace = cardPlace;
                else
                if (beloteRemind == 2)
                    gameData.Bribes.CurrentBribe.RebelotePlace = cardPlace;
                if (OnUpdateGraphics != null)
                    OnUpdateGraphics();
            }
            catch (Exception Ex)
            {
                throw new BeloteClientException("Произошла ошибка во время оповещения о карте, которой походил кто-то из игроков", Ex);
            }
        }

        // Выход игрока со стола во время игры
        private void PlayerQuitHandler(Message Msg)
        {
            try
            {
                MessageResult qParams = new MessageResult(Msg);
                // Продолжаем игру
                if (qParams["Continue"] == "1")
                {
                    int BotPlace = Int32.Parse(qParams["Place"]);
                    if (BotPlace != place)
                    {
                        currentTable.SetPlayerAtPlace(-BotPlace, BotPlace);
                        if (OnUpdateGraphics != null)
                            OnUpdateGraphics();
                        return;
                    }
                }
                SetGameHandlers(false);
                ChangeCurrentTable(null);
                if (OnPlayerQuit != null)
                    OnPlayerQuit();
            }
            catch (Exception Ex)
            {
                throw new BeloteClientException("Произошла ошибка во время выхода кого-то со стола", Ex);
            }
        }

        // Завершение игры
        private void GameEndHandler(Message Msg)
        {
            try
            {
                MessageResult gParams = new MessageResult(Msg);
                gameData.TotalScores[BeloteTeam.TEAM1_1_3] = Int32.Parse(gParams["Scores1"]);
                gameData.TotalScores[BeloteTeam.TEAM2_2_4] = Int32.Parse(gParams["Scores2"]);
                SetGameHandlers(false);
                ChangeCurrentTable(null);
                if (OnGameEnd != null)
                    OnGameEnd();
            }
            catch (Exception Ex)
            {
                throw new BeloteClientException("Произошла ошибка при завершении игры", Ex);
            }
        }

        //**********************************************************************************************************************************************************************************
        //                      Совершаемые игроком действия: ход, объявление заявки и т.д.
        //**********************************************************************************************************************************************************************************

        // Игрок совершает ход. Параметр - индекс карты в списке всех карт
        public void MakeMove(int CardIndex)
        {
            try
            {
                Card card = gameData.AllCards[CardIndex];
                gameData.AllCards.Remove(card);
                gameData.IsMakingMove = false;
                serverActions.Game.PlayerMakeMove(card);
            }
            catch (Exception Ex)
            {
                throw new BeloteClientException("Произошла ошибка во время хода игрока", Ex);
            }
        }

        // Выход игрока со стола
        public void QuitTable()
        {
            try
            {
                ExitFromTable(true);
            }
            catch (Exception Ex)
            {
                throw new BeloteClientException("Произошла ошибка при выходе игрока со стола", Ex);
            }
        }

        // Сделать заказ
        public void MakeOrder(Order order)
        {
            try
            {
                serverActions.Game.PlayerMakeOrder(order);
                gameData.IsMakingMove = false;
            }
            catch (Exception Ex)
            {
                throw new BeloteClientException("Произошла ошибка во время совершения заказа", Ex);
            }
        }

        //**********************************************************************************************************************************************************************************
        //                      Обработчики событий
        //**********************************************************************************************************************************************************************************

        // Вызывается в режиме ожидания игроков на стол
        public GraphicCallbackDelegate OnUpdateWaitingTable
        {
            get;
            set;
        }

        // Вызывается во время игры для обновления графики на столе
        public GraphicCallbackDelegate OnUpdateGraphics
        {
            get;
            set;
        }

        // Вызывается при выходе со стола
        public GraphicCallbackDelegate OnExitFromTable
        {
            get;
            set;
        }

        // Вызывается при старте игры
        public GraphicCallbackDelegate OnGameStart
        {
            get;
            set;
        }

        // Вызывается при необходимости совершить ставку
        public GraphicCallbackDelegate OnBazarMakingBet
        {
            get;
            set;
        }

        // Вызывается когда необходимо анонсировать бонусы
        public GraphicCallbackDelegate OnAnnounceBonuses
        {
            get;
            set;
        }

        // Событие объявления победителей по бонусам
        public GraphicCallbackDelegate OnShowBonusesWinner
        {
            get;
            set;
        }

        // Событие завершения игры
        public GraphicCallbackDelegate OnGameEnd
        {
            get;
            set;
        }

        // Событие выхода игрока со стола во время игры
        public GraphicCallbackDelegate OnPlayerQuit
        {
            get;
            set;
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

        // Текущий статус
        public GameStatus Status
        {
            get;
            private set;
        }

        // Текущий стол
        public Table CurrentTable
        {
            get
            {
                return currentTable;
            }
        }

        // Текущий игрок
        public Player CurrentPlayer
        {
            get
            {
                return player;
            }
        }

        // Игровая информация
        public GameData GameData
        {
            get
            {
                return gameData;
            }
        }

        public int Place
        {
            get
            {
                return place;
            }
        }
    }
}
