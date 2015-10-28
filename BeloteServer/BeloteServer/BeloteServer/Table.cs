using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BeloteServer
{
    class Table
    {
        // Текущий ходящий игрок
        private int currentPlayer;
        // Игрок, начинающий ход в раздаче
        private int startedPlayer;
        private Game game;
        private DistributionsList distributions;

        public Table(Game Game, Client Creator) : this(Game, Creator, Constants.GAME_MINIMAL_BET, true, true, 0, true, false, false, true)
        {
        }

        public Table(Game Game, Client Creator, int Bet, bool PlayersVisibility, bool Chat, int MinimalLevel, bool TableVisibility, bool VIPOnly, bool Moderation, bool AI)
        {
            this.game = Game;
            Status = TableStatus.CREATING;
            TableCreator = Creator;
            this.Bet = Bet;
            this.PlayersVisibility = PlayersVisibility;
            this.Chat = Chat;
            this.MinimalLevel = MinimalLevel;
            this.TableVisibility = TableVisibility;
            this.VIPOnly = VIPOnly;
            this.Moderation = Moderation;
            this.AI = AI;
            // ID стол получает только после записи в БД
            this.ID = -1;
            startedPlayer = 1;
            distributions = new DistributionsList();
            CreateTableInDatabase();
        }

        // Метод, в котором создается запись об игровом столе внутри базы данных
        private void CreateTableInDatabase()
        {
#if DEBUG
            Debug.WriteLine(DateTime.Now.ToString() + " Добавление записи о созданном столе в базу данных");
            Debug.Indent();
            Debug.WriteLine("Table Creator ID: " + TableCreator.ID);
#endif   
            game.DataBase.ExecuteQueryWithoutQueue(String.Format("INSERT INTO Tables {0} VALUES (\"{1}\", \"{2}\", \"{3}\", \"{4}\", \"{5}\", \"{6}\", \"{7}\", \"{8}\", \"{9}\");",
                "(TableCreatorId, Bet, PlayersVisibility, Chat, MinimalLevel, TableVisibility, VIPOnly, Moderation, Ai)",
                TableCreator.ID, Bet, Helpers.BoolToString(PlayersVisibility), Helpers.BoolToString(Chat), MinimalLevel,
                Helpers.BoolToString(TableVisibility), Helpers.BoolToString(VIPOnly), Helpers.BoolToString(Moderation), Helpers.BoolToString(AI)));
#if DEBUG
            Debug.WriteLine("Получение ID созданного стола");
#endif
            ID = Int32.Parse(game.DataBase.SelectScalar(String.Format("SELECT MAX(ID) FROM (SELECT Id FROM Tables WHERE TableCreatorId = \"{0}\") AS A1;", TableCreator.ID)));
            if (ID != -1)
                Status = TableStatus.WAITING;
            else
                Status = TableStatus.ERROR;
#if DEBUG 
            Debug.WriteLine("Идентификатор созданного стола: " + ID);
            Debug.Unindent();
#endif
        }

        // Посылка сообщения всем клиентам
        public void SendMessageToClients(string Message)
        {
            if (TableCreator != null)
                TableCreator.SendMessage(Message);
            SendMessageToClientsWithoutCreator(Message);
        }

        // Посылка сообщения всем клиентам, кроме создателя стола
        public void SendMessageToClientsWithoutCreator(string Message)
        {
            if (Player2 != null)
                Player2.SendMessage(Message);
            if (Player3 != null)
                Player3.SendMessage(Message);
            if (Player4 != null)
                Player4.SendMessage(Message);
        }

        // Метод при завершении игры на столе
        public void CloseTable()
        {
            Status = TableStatus.ENDING;
        }

        // Функция тестирует, полностью ли заполнен игровой стол
        public bool TestFullfill()
        {
            bool Result = ((TableCreator != null) && (Player2 != null) && (Player3 != null) && (Player4 != null));
#if DEBUG
            Debug.WriteLine("Тестирование на заполненность стола.");
            Debug.Indent();
            Debug.WriteLine("Идентификатор стола: " + ID);
            Debug.WriteLine("Результат: " + Result.ToString());
            Debug.Unindent();
#endif
            return Result;
        }

        // Переход к следующему игроку
        private int NextPlayer(int playerNum)
        {
            if (playerNum < 4)
                return ++playerNum;
            else
                return 1;
        }

        // Возвращает ссылку на игрока(клиента) по его номеру
        private Client PlayerFromNumber(int Number)
        {
            switch (Number)
            {
                case 1:
                    {
                        return TableCreator;
                    }
                case 2:
                    {
                        return Player2;
                    }
                case 3:
                    {
                        return Player3;
                    }
                case 4:
                    {
                        return Player4;
                    }
                default:
                    {
                        return null;
                    }
            }
        }

        // Проверка на завершенность игры
        private bool IsEndedGame()
        {
            // Если раздач - 0, то игра еще даже не началась
            if (distributions.Count == 0)
                return false;
            // Проверяем, превысилали ли какая-то команда конечный счет игры
            if ((distributions.ScoresTeam1 >= Constants.GAME_END_COUNT) || (distributions.ScoresTeam2 >= Constants.GAME_END_COUNT))
                // Проверяем неравенство очков команд и завершенность последней раздачи
                if ((distributions.ScoresTeam1 != distributions.ScoresTeam2) && (distributions.Current.Status == DistributionStatus.D_ENDED))
                    // Проверяем, не закончилась ли последняя раздача капутом
                    if (!distributions.Current.IsCapotEnded)
                        return true;
            return false;
        }

        // Запуск игры на столе
        public void StartGame()
        {
            Status = TableStatus.PLAYING;
            // Посылаем всем игрокам сообщение о старте игры
            SendMessageToClients("GTS");
            // Переходим от раздающего игрока к следующему
            startedPlayer = NextPlayer(startedPlayer);
            // Создаем новую раздачу и запускаем процесс торговли
            NextDistribution();
        }

        // Следующая раздача
        public void NextDistribution()
        {
            // Если игра на столе завершена...
            if (IsEndedGame())
            {

            }
            // Добавление новой раздачи
            currentPlayer = startedPlayer;
            distributions.AddNew();
            // Раздача игровых карт между игроками, сортировка их в порядке "без козыря"
            CardsDeck cd = new CardsDeck();
            cd.Distribution(distributions.Current.Player1Cards, distributions.Current.Player2Cards, distributions.Current.Player3Cards, distributions.Current.Player4Cards);
            // Заполнение списка возможных бонусов
            distributions.Current.FillBonuses();
            // Посылка всем игрокам списка их игровых карт
            TableCreator.SendMessage(String.Format("GDCCards={0},Scores1={1},Scores2={2}", distributions.Current.Player1Cards.ToString(),
                distributions.ScoresTeam1, distributions.ScoresTeam2));
            Player2.SendMessage(String.Format("GDCCards={0},Scores1={1},Scores2={2}", distributions.Current.Player2Cards.ToString(),
                distributions.ScoresTeam1, distributions.ScoresTeam2));
            Player3.SendMessage(String.Format("GDCCards={0},Scores1={1},Scores2={2}", distributions.Current.Player3Cards.ToString(),
                distributions.ScoresTeam1, distributions.ScoresTeam2));
            Player4.SendMessage(String.Format("GDCCards={0},Scores1={1},Scores2={2}", distributions.Current.Player4Cards.ToString(),
                    distributions.ScoresTeam1, distributions.ScoresTeam2));
            // Посылка ходящему игроку тип ставки и ее минимальный размер
            Client p = PlayerFromNumber(currentPlayer);
            p.SendMessage("GBNType=1,Size=80");
        }

        // Метод, добавляющий новый заказ игрока в список заказов
        public void AddOrder(Order order)
        {
            // Выбираем, какая из команд сделала заказ
            BeloteTeam team = (((currentPlayer == 1) || (currentPlayer == 3)) ? BeloteTeam.TEAM1_1_3 : BeloteTeam.TEAM2_2_4);
            // Добавляем заказ в список заказов текущей раздачи
            distributions.Current.Orders.Add(order, team);
            // Посылаем всем клиентам уведомление о том, какой заказ был сделан
            SendMessageToClients(String.Format("GBSPlayer={0},Type={1},Size={2},Trump={3}", currentPlayer, (int)order.Type, order.Size, Helpers.SuitToString(order.Trump)));
            // В случае окончания процесса торговли
            if (distributions.Current.Orders.IsEnded())
            {
                // Завершаем торговлю, назначая козырь
                distributions.Current.EndBazar();
                // Отсылаем все возможные бонус клиентам
                TableCreator.SendMessage("GGB" + distributions.Current.Player1Bonuses.ToString());
                Player2.SendMessage("GGB" + distributions.Current.Player2Bonuses.ToString());
                Player3.SendMessage("GGB" + distributions.Current.Player3Bonuses.ToString());
                Player4.SendMessage("GGB" + distributions.Current.Player4Bonuses.ToString());
                // Ход переходит к первому ходящему на раздаче
                currentPlayer = startedPlayer;
                // Отсылаем всем клиентам сообщение о конце торговли
                SendMessageToClients(String.Format("GBETeam={0},Type={1},Size={2},Trump={3},Player={4}", (int)distributions.Current.Orders.OrderedTeam,
                    (int)distributions.Current.Orders.Current.Type, distributions.Current.Orders.Current.Size, (int)distributions.Current.Orders.Current.Trump, 
                    PlayerFromNumber(currentPlayer).ID));
            }
            else
            {
                // Тип ставки для следующего игрока
                BetType betType;
                currentPlayer = NextPlayer(currentPlayer);
                // Если была оглашена контра
                if (distributions.Current.Orders.IsCoinched)
                {
                    betType = BetType.BET_SURCOINCHE;
                    // Если после контры уже был оглашен пасс, то перескакиваем еще одного игрока
                    if (distributions.Current.Orders.Last.Type == OrderType.ORDER_PASS)
                    {
                        currentPlayer = NextPlayer(currentPlayer);
                    }
                }
                else
                // Если последним был объявлен капут, то ответить на него можно только капутом или контрой
                if (distributions.Current.Orders.IsCapot)
                {
                    betType = BetType.BET_CAPOT;
                }
                // Если уже был сделан хотя бы один заказ, то можно ответить заказом или контрой
                if (distributions.Current.Orders.Current != null)
                {
                    betType = BetType.T_BETABET;
                }
                // Если еще ни одного заказа не сделано, то можно ответить только заказом
                else
                {
                    betType = BetType.T_BET;
                }
                // Минимальный размер ставки для следующего игрока
                int minSize = (distributions.Current.Orders.Current == null) ? 80 : distributions.Current.Orders.Current.Size + 10;
                // Посылка сообщения GBN следующему в торговле игроку
                Client p = PlayerFromNumber(currentPlayer);
                p.SendMessage(String.Format("GBNType={0},MinSize={1}", (int)betType, minSize));
            }
        }

        public int ID
        {
            get;
            private set;
        }

        public Client TableCreator
        {
            get;
            private set;
        }

        public Client Player2
        {
            get;
            set;
        }

        public Client Player3
        {
            get;
            set;
        }

        public Client Player4
        {
            get;
            set;
        }

        public int Bet
        {
            get;
            private set;
        }

        public bool PlayersVisibility
        {
            get;
            private set;
        }

        public bool Chat
        {
            get;
            private set;
        }

        public int MinimalLevel
        {
            get;
            private set;
        }

        public bool TableVisibility
        {
            get;
            set;
        }

        public bool VIPOnly
        {
            get;
            private set;
        }

        public bool Moderation
        {
            get;
            private set;
        }

        public bool AI
        {
            get;
            private set;
        }

        public TableStatus Status
        {
            get;
            private set;
        }
    }
}
