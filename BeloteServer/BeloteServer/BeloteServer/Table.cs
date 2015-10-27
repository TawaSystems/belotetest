using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BeloteServer
{
    public enum TableStatus
    {
        CREATING = 1,
        WAITING = 2,
        PLAYING = 3,
        ENDING = 4,
        ERROR = 5
    }

    class Table
    {
        private int currentPlayer;
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
            this.ID = -1;
            currentPlayer = 1;
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
        private int NextPlayer()
        {
            if (currentPlayer < 4)
                currentPlayer++;
            else
                currentPlayer = 1;
            return currentPlayer;
        }

        // Проверка на завершенность игры
        private bool IsEndedGame()
        {
            if (distributions.Count == 0)
                return false;
            if ((distributions.ScoresTeam1 >= 151) || (distributions.ScoresTeam2 >= 151))
                if ((distributions.ScoresTeam1 != distributions.ScoresTeam2) && (distributions.Current.Status == DistributionStatus.D_ENDED))
                    if (!distributions.Current.IsCapotEnded)
                        return true;
            return false;
        }

        // Запуск игры на столе
        public void StartGame()
        {
            Status = TableStatus.PLAYING;
            SendMessageToClients("GTS");
            NextPlayer();
            NextDistribution();
        }

        // Следующая раздача
        public void NextDistribution()
        {
            // Если игра на столе завершена...
            if (IsEndedGame())
            {

            }
            distributions.AddNew();
            CardsDeck cd = new CardsDeck();
            cd.Distribution(distributions.Current.Player1Cards, distributions.Current.Player2Cards, distributions.Current.Player3Cards, distributions.Current.Player4Cards);
            TableCreator.SendMessage(String.Format("GDCCards={0},Scores1={1},Scores2={2}", distributions.Current.Player1Cards.ToString(),
                distributions.ScoresTeam1, distributions.ScoresTeam2));
            Player2.SendMessage(String.Format("GDCCards={0},Scores1={1},Scores2={2}", distributions.Current.Player2Cards.ToString(),
                distributions.ScoresTeam1, distributions.ScoresTeam2));
            Player3.SendMessage(String.Format("GDCCards={0},Scores1={1},Scores2={2}", distributions.Current.Player3Cards.ToString(),
                distributions.ScoresTeam1, distributions.ScoresTeam2));
            Player4.SendMessage(String.Format("GDCCards={0},Scores1={1},Scores2={2}", distributions.Current.Player4Cards.ToString(),
                    distributions.ScoresTeam1, distributions.ScoresTeam2));
            switch (currentPlayer)
            {
                case 1:
                    {
                        TableCreator.SendMessage("GBNType=1,Size=80");
                        break;
                    }
                case 2:
                    {
                        Player2.SendMessage("GBNType=1,Size=80");
                        break;
                    }
                case 3:
                    {
                        Player3.SendMessage("GBNType=1,Size=80");
                        break;
                    }
                case 4:
                    {
                        Player4.SendMessage("GBNType=1,Size=80");
                        break;
                    }
            }
        }

        public void AddOrder(Order order)
        {

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
