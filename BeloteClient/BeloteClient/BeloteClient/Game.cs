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
        private GameForm gameForm;
        private BetFormType4 betForm4;
        private BetFromType123 betForm123;

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
                Status = TableStatus.NONE;
                Player1Order = null;
                Player2Order = null;
                Player3Order = null;
                Player4Order = null;
                IsMakingMove = false;
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

        private int NextPlaceNumber(int curPlace)
        {
            if (curPlace < 4)
                return (++curPlace);
            else
                return 1;
        }

        private int PredPlaceNumber(int curPlace)
        {
            if (curPlace > 1)
                return (--curPlace);
            else
                return 4;
        }

        // Переводит координты игрока на сервере в координаты игрока на платформе
        public int ServerPlaceToGraphicPlace(int serverPlace)
        {
            if (serverPlace == Place)
            {
                return 1;
            }
            else
            if (serverPlace == (NextPlaceNumber(Place)))
            {
                return 2;
            }
            else
            if (serverPlace == (NextPlaceNumber(NextPlaceNumber(Place))))
            {
                return 3;
            }
            else
                return 4;
        }

        // Переводит координаты игрока при отрисовке в координаты игрока на сервере
        public int GraphicPlaceToServerPlace(int graphicPlace)
        {
            switch (graphicPlace)
            {
                case 1:
                    {
                        return Place;
                    }
                case 2:
                    {
                        return NextPlaceNumber(Place);
                    }
                case 3:
                    {
                        return NextPlaceNumber(NextPlaceNumber(Place));
                    }
                case 4:
                    {
                        return PredPlaceNumber(Place);
                    }
                default:
                    return graphicPlace;
            }
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
                serverActions.TestFullfillTable();
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
                //MessageBox.Show("Бот успешно добавлен!");
                switch (BotPlace)
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
                serverActions.TestFullfillTable();
            }
            else
            {
                MessageBox.Show("Не удалось добавить бота на стол!");
            }
        }

        // Удаление бота с игрового стола
        public void DeleteBot(int BotPlace)
        {
            serverActions.DeleteBotFromTable(BotPlace);
        }

        // Игрок совершает ход. Параметр - индекс карты в списке всех карт
        public void MakeMove(int CardIndex)
        {
            Card card = AllCards[CardIndex];
            AllCards.Remove(card);
            IsMakingMove = false;
            serverActions.PlayerMakeMove(card);
            gameForm.UpdateGraphics();
        }

        // Выход игрока со стола
        public void QuitTable()
        {
            //SetGameHandlers(false);
            serverActions.PlayerQuitFromTable();
            /*ChangeCurrentTable(null);
            ChangeCurrentPlace(-1);
            if (betForm123 != null)
                betForm123.Close();
            if (betForm4 != null)
                betForm4.Close();
            gameForm.Close();
            gameForm = null;
            userForm = new MainUserForm(this);
            userForm.UpdateTables();
            userForm.Show();*/
        }

        // Сделать заказ
        public void MakeOrder(Order order)
        {
            serverActions.PlayerMakeOrder(order);
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
            gameForm = new GameForm(this);
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
                serverActions.SetGameHandlers(GetCardsHandler, BazarNextPlayerHandler, BazarPlayerSayHandler, BazarEndHandler, BazarPassHandler,
                    NextPlayerHandler, RemindCardHandler, BonusesGetAllHandler, BonusesShowTypesHandler, BonusesShowWinnerHandler, PlayerQuitHandler, GameEndHandler);
            }
            else
            {
                serverActions.UnsetGameHandlers(GetCardsHandler, BazarNextPlayerHandler, BazarPlayerSayHandler, BazarEndHandler, BazarPassHandler,
                    NextPlayerHandler, RemindCardHandler, BonusesGetAllHandler, BonusesShowTypesHandler, BonusesShowWinnerHandler, PlayerQuitHandler, GameEndHandler);
            }
        }

        // Обработчик получения списка карт
        public void GetCardsHandler(Message Msg)
        {
            // MessageBox.Show("Раздача карт игрок " + Place.ToString());
            Dictionary<string, string> cParams = Helpers.SplitCommandString(Msg.Msg);
            string cardsStr = cParams["Cards"];
            TotalScore1 = Int32.Parse(cParams["Scores1"]);
            TotalScore2 = Int32.Parse(cParams["Scores2"]);
            LocalScore1 = 0;
            LocalScore2 = 0;
            Player1Order = null;
            Player2Order = null;
            Player3Order = null;
            Player4Order = null;
            IsMakingMove = false;
            Player1BonusesTypes = null;
            Player2BonusesTypes = null;
            Player3BonusesTypes = null;
            Player4BonusesTypes = null;
            P1Card = null;
            P2Card = null;
            P3Card = null;
            P4Card = null;
            Bonuses = null;
            BelotePlace = 0;
            RebelotePlace = 0;
            AllCards = new CardList(cardsStr);
            PossibleCards = AllCards;
            Status = TableStatus.BAZAR;
            gameForm.UpdateGraphics();
        }

        // Переход хода к игроку во время торговли
        public void BazarNextPlayerHandler(Message Msg)
        {
            //MessageBox.Show(Msg.Msg);
            try
            {
                Dictionary<string, string> bParams = Helpers.SplitCommandString(Msg.Msg);
                BetType bType = (BetType)Int32.Parse(bParams["Type"]);
                int minSize = Int32.Parse(bParams["MinSize"]);
                if (bType == BetType.BET_SURCOINCHE)
                {
                    betForm4 = new BetFormType4(this);
                    betForm4.ShowDialog();
                    betForm4 = null;
                }
                else
                {
                    betForm123 = new BetFromType123(this, minSize, bType);
                    betForm123.ShowDialog();
                    betForm123 = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Произношение заявки сделанной одним из игроков
        public void BazarPlayerSayHandler(Message Msg)
        {
            Dictionary<string, string> bParams = Helpers.SplitCommandString(Msg.Msg);
            int playerNum = Int32.Parse(bParams["Player"]);
            OrderType orderType = (OrderType)Int32.Parse(bParams["Type"]);
            int orderSize = Int32.Parse(bParams["Size"]);
            CardSuit orderSuit = Helpers.StringToSuit(bParams["Trump"]);
            switch (playerNum)
            {
                case 1:
                    {
                        Player1Order = new Order(orderType, orderSize, orderSuit);
                        break;
                    }
                case 2:
                    {
                        Player2Order = new Order(orderType, orderSize, orderSuit);
                        break;
                    }
                case 3:
                    {
                        Player3Order = new Order(orderType, orderSize, orderSuit);
                        break;
                    }
                case 4:
                    {
                        Player4Order = new Order(orderType, orderSize, orderSuit);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            gameForm.UpdateGraphics();
        } 

        // Завершение процесса торговли
        public void BazarEndHandler(Message Msg)
        {
            Player1Order = null;
            Player2Order = null;
            Player3Order = null;
            Player4Order = null;
            Dictionary<string, string> bParams = Helpers.SplitCommandString(Msg.Msg);
            BeloteTeam oTeam = (BeloteTeam)Int32.Parse(bParams["Team"]);
            OrderType oType = (OrderType)Int32.Parse(bParams["Type"]);
            CardSuit oSuit = (CardSuit)Int32.Parse(bParams["Trump"]);
            int oSize = Int32.Parse(bParams["Size"]);
            EndOrder = new Order(oType, oSize, oSuit);
            EndOrder.ChangeTeam(oTeam);
            Status = TableStatus.BONUSES;
            gameForm.UpdateGraphics();
        }

        // Завершение торговли без заявки козыря
        public void BazarPassHandler(Message Msg)
        {
            
        }

        // Переход хода к игроку
        public void NextPlayerHandler(Message Msg)
        {
            // Если это первый ход, то нужно огласить бонусы
            if (Bonuses != null)
            {
                // Если есть неоглашенные бонусы, то предлагаем их огласить
                if (Bonuses.Count != 0)
                {
                    // Показываем форму
                    BonusAnnounceForm form = new BonusAnnounceForm(this);
                    form.ShowDialog();
                    serverActions.PlayerAnnounceBonuses(Bonuses);
                    // Обнуляем бонусы
                    Bonuses = null;
                }
            }
            Dictionary<string, string> cParams = Helpers.SplitCommandString(Msg.Msg);
            // Получаем список возможных карт
            PossibleCards = new CardList(cParams["Cards"]);
            // Разрешаем игроку сделать ход
            IsMakingMove = true;
            gameForm.UpdateGraphics();
        }

        // Объявление о типах объявленных бонусов одного из игроков
        public void BonusesShowTypesHandler(Message Msg)
        {
            Dictionary<string, string> tParams = Helpers.SplitCommandString(Msg.Msg);
            int count = Int32.Parse(tParams["Count"]);
            int place = Int32.Parse(tParams["Place"]);
            if (count == 0)
                return;
            string showstr = "";
            for (var i = 0; i < count; i++)
            {
                BonusType nextB = (BonusType)Int32.Parse(tParams["Type" + i.ToString()]);
                string addstr = "";
                if (i != 0)
                    addstr += " + ";
                switch (nextB)
                {
                    case BonusType.BONUS_TERZ:
                        {
                            addstr += "TERZ";
                            break;
                        }
                    case BonusType.BONUS_50:
                        {
                            addstr += "50";
                            break;
                        }
                    case BonusType.BONUS_100:
                        {
                            addstr += "100";
                            break;
                        }
                    case BonusType.BONUS_4X:
                        {
                            addstr += "4X";
                            break;
                        }
                }
                showstr += addstr;
            }
            switch (place)
            {
                case 1:
                    {
                        Player1BonusesTypes = showstr;
                        break;
                    }
                case 2:
                    {
                        Player2BonusesTypes = showstr;
                        break;
                    }
                case 3:
                    {
                        Player3BonusesTypes = showstr;
                        break;
                    }
                case 4:
                    {
                        Player4BonusesTypes = showstr;
                        break;
                    }
            }
            gameForm.UpdateGraphics();
        }

        // Объявление победителя по бонусам
        public void BonusesShowWinnerHandler(Message Msg)
        {
            Player1BonusesTypes = null;
            Player2BonusesTypes = null;
            Player3BonusesTypes = null;
            Player4BonusesTypes = null;
            gameForm.UpdateGraphics();
            Status = TableStatus.PLAY;
            Dictionary<string, string> wParams = Helpers.SplitCommandString(Msg.Msg);
            BeloteTeam winner = (BeloteTeam)Int32.Parse(wParams["Winner"]);
            int scores = Int32.Parse(wParams["Scores"]);
            if (winner != BeloteTeam.TEAM_NONE)
            {
                MessageBox.Show(String.Format("Команда №{0} получает {1} очков за бонусы", (int)winner, scores));
            }
            else
            {
                //MessageBox.Show("Ни одна команда не получает очки за бонусы");
            }
        }

        // Получение списка возможных бонусов
        public void BonusesGetAllHandler(Message Msg)
        {
            Bonuses = new BonusList(Msg.Msg);
        }

        // Ход другого игрока
        public void RemindCardHandler(Message Msg)
        {
            Dictionary<string, string> cParams = Helpers.SplitCommandString(Msg.Msg);
            int cardPlace = Int32.Parse(cParams["Place"]);
            Card newCard = new Card(cParams["Card"]);
            LocalScore1 = Int32.Parse(cParams["Scores1"]);
            LocalScore2 = Int32.Parse(cParams["Scores2"]);
            int beloteRemind = Int32.Parse(cParams["Belote"]);
            if (beloteRemind == 0)
            {
                BelotePlace = 0;
                RebelotePlace = 0;
            }
            else
            {
                if (beloteRemind == 1)
                    BelotePlace = cardPlace;
                else
                    RebelotePlace = cardPlace;
            }
            switch (cardPlace)
            {
                case 1:
                    {
                        P1Card = newCard;
                        break;
                    }
                case 2:
                    {
                        P2Card = newCard;
                        break;
                    }
                case 3:
                    {
                        P3Card = newCard;
                        break;
                    }
                case 4:
                    {
                        P4Card = newCard;
                        break;
                    }
            }
            gameForm.UpdateGraphics();
            if ((P1Card != null) && (P2Card != null) && (P3Card != null) && (P4Card != null))
            {
                P1Card = null;
                P2Card = null;
                P3Card = null;
                P4Card = null;
                //Thread.Sleep(300);
                //gameForm.UpdateGraphics();
            }
        }

        // Завершение игры
        public void GameEndHandler(Message Msg)
        {
            Dictionary<string, string> gParams = Helpers.SplitCommandString(Msg.Msg);
            TotalScore1 = Int32.Parse(gParams["Scores1"]);
            TotalScore2 = Int32.Parse(gParams["Scores2"]);
            if (TotalScore1 > TotalScore2)
            {
                if ((Place == 1) || (Place == 3))
                    MessageBox.Show(String.Format("Вы победили! Счет - {0} : {1}", TotalScore1, TotalScore2));
                else
                    MessageBox.Show(String.Format("Вы проиграли! Счет - {0} : {1}", TotalScore1, TotalScore2));
            }
            else
            {
                if ((Place == 2) || (Place == 4))
                    MessageBox.Show(String.Format("Вы победили! Счет - {0} : {1}", TotalScore1, TotalScore2));
                else
                    MessageBox.Show(String.Format("Вы проиграли! Счет - {0} : {1}", TotalScore1, TotalScore2));
            }
            SetGameHandlers(false);
            ChangeCurrentTable(null);
            ChangeCurrentPlace(-1);
            gameForm.Close();
            gameForm = null;
            userForm = new MainUserForm(this);
            userForm.UpdateTables();
            userForm.Show();
        }

        // Выход игрока со стола во время игры
        public void PlayerQuitHandler(Message Msg)
        {
            Dictionary<string, string> qParams = Helpers.SplitCommandString(Msg.Msg);
            // Продолжаем игру
            if (qParams["Continue"] == "1")
            {
                /*int BotPlace = Int32.Parse(qParams["Place"]);
                int PlayerID = -1;
                MessageBox.Show(String.Format("Игрок №{0} покинул стол, его заменил бот. Игра продолжается"));
                switch (BotPlace)
                {
                    case 2:
                        {
                            PlayerID = CurrentTable.Player2;
                            CurrentTable.Player2 = -BotPlace;
                            break;
                        }
                    case 3:
                        {
                            PlayerID = CurrentTable.Player3;
                            CurrentTable.Player3 = -BotPlace;
                            break;
                        }
                    case 4:
                        {
                            PlayerID = CurrentTable.Player4;
                            CurrentTable.Player4 = -BotPlace;
                            break;
                        }
                }
                Players.Delete(Players[PlayerID]);*/
            }
            // Завершаем игру
            else
            {
                MessageBox.Show("Игра завершена. Кто-то вышел со стола");
                SetGameHandlers(false);
                ChangeCurrentTable(null);
                ChangeCurrentPlace(-1);
                if (betForm123 != null)
                    betForm123.Close();
                if (betForm4 != null)
                    betForm4.Close();
                gameForm.Close();
                gameForm = null;
                userForm = new MainUserForm(this);
                userForm.UpdateTables();
                userForm.Show();
            }
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

        // Все карты в наличии
        public CardList AllCards
        {
            get;
            private set;
        }

        // Возможные к ходу карты
        public CardList PossibleCards
        {
            get;
            private set;
        }

        public TableStatus Status
        {
            get;
            private set;
        }

        // Счет каждой из команд за всю игру на столе
        public int TotalScore1
        {
            get;
            private set;
        }

        public int TotalScore2
        {
            get;
            private set;
        }

        // Локальный счет каждой из команд во время раздачи
        public int LocalScore1
        {
            get;
            private set;
        }

        public int LocalScore2
        {
            get;
            private set;
        }

        public Order Player1Order
        {
            get;
            private set;
        }

        public Order Player2Order
        {
            get;
            private set;
        }

        public Order Player3Order
        {
            get;
            private set;
        }

        public Order Player4Order
        {
            get;
            private set;
        }

        public Order EndOrder
        {
            get;
            private set;
        }

        public BonusList Bonuses
        {
            get;
            private set;
        }

        public string Player1BonusesTypes
        {
            get;
            private set;
        }
        public string Player2BonusesTypes
        {
            get;
            private set;
        }
        public string Player3BonusesTypes
        {
            get;
            private set;
        }
        public string Player4BonusesTypes
        {
            get;
            private set;
        }

        public bool IsMakingMove
        {
            get;
            private set;
        }

        public Card P1Card
        {
            get;
            private set;
        }

        public Card P2Card
        {
            get;
            private set;
        }

        public Card P3Card
        {
            get;
            private set;
        }

        public Card P4Card
        {
            get;
            private set;
        }

        public int BelotePlace
        {
            get;
            private set;
        }

        public int RebelotePlace
        {
            get;
            private set;
        }
    }
}
