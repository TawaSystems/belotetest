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
        private Client tableCreator;
        private Client player2;
        private Client player3;
        private Client player4;
        private int currentPlayer;
        private int bet;
        private bool playersVisibility;
        private bool chat;
        private int minimalLevel;
        private bool tableVisibility;
        private bool vipOnly;
        private bool moderation;
        private bool ai;

        private int id;
        private TableStatus status;
        private Game game;
        private DistributionsList distributions;

        public Table(Game Game, Client Creator) : this(Game, Creator, Constants.GAME_MINIMAL_BET, true, true, 0, true, false, false, true)
        {
        }

        public Table(Game Game, Client Creator, int Bet, bool PlayersVisibility, bool Chat, int MinimalLevel, bool TableVisibility, bool VIPOnly, bool Moderation, bool AI)
        {
            this.game = Game;
            status = TableStatus.CREATING;
            tableCreator = Creator;
            bet = Bet;
            playersVisibility = PlayersVisibility;
            chat = Chat;
            minimalLevel = MinimalLevel;
            tableVisibility = TableVisibility;
            vipOnly = VIPOnly;
            moderation = Moderation;
            ai = AI;
            id = -1;
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
            id = Int32.Parse(game.DataBase.SelectScalar(String.Format("SELECT MAX(ID) FROM (SELECT Id FROM Tables WHERE TableCreatorId = \"{0}\") AS A1;", TableCreator.ID)));
            if (id != -1)
                status = TableStatus.WAITING;
            else
                status = TableStatus.ERROR;
#if DEBUG 
            Debug.WriteLine("Идентификатор созданного стола: " + id);
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
            status = TableStatus.ENDING;
        }

        // Функция тестирует, полностью ли заполнен игровой стол
        public bool TestFullfill()
        {
            bool Result = ((TableCreator != null) && (Player2 != null) && (Player3 != null) && (player4 != null));
#if DEBUG
            Debug.WriteLine("Тестирование на заполненность стола.");
            Debug.Indent();
            Debug.WriteLine("Идентификатор стола: " + ID);
            Debug.WriteLine("Результат: " + Result.ToString());
            Debug.Unindent();
#endif
            return Result;
        }

        private int NextPlayer()
        {
            if (currentPlayer < 4)
                currentPlayer++;
            else
                currentPlayer = 1;
            return currentPlayer;
        }

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

        public void StartGame()
        {
            status = TableStatus.PLAYING;
            SendMessageToClients("GTS");
            NextPlayer();
            NextDistribution();
        }

        private void NextDistribution()
        {
            // Если игра на столе завершена...
            if (IsEndedGame())
            {

            }
            distributions.AddNew();
            CardsDeck cd = new CardsDeck();
            cd.Distribution(distributions.Current.Player1Cards, distributions.Current.Player2Cards, distributions.Current.Player3Cards, distributions.Current.Player4Cards);
            tableCreator.SendMessage(String.Format("GDCCards={0},Scores1={1},Scores2={2}", distributions.Current.Player1Cards.ToString(),
                distributions.ScoresTeam1, distributions.ScoresTeam2));
            player2.SendMessage(String.Format("GDCCards={0},Scores1={1},Scores2={2}", distributions.Current.Player2Cards.ToString(),
                distributions.ScoresTeam1, distributions.ScoresTeam2));
            player3.SendMessage(String.Format("GDCCards={0},Scores1={1},Scores2={2}", distributions.Current.Player3Cards.ToString(),
                distributions.ScoresTeam1, distributions.ScoresTeam2));
            player4.SendMessage(String.Format("GDCCards={0},Scores1={1},Scores2={2}", distributions.Current.Player4Cards.ToString(),
                    distributions.ScoresTeam1, distributions.ScoresTeam2));
            switch (currentPlayer)
            {
                case 1:
                    {
                        tableCreator.SendMessage("GBNType=1,Size=80");
                        break;
                    }
                case 2:
                    {
                        player2.SendMessage("GBNType=1,Size=80");
                        break;
                    }
                case 3:
                    {
                        player3.SendMessage("GBNType=1,Size=80");
                        break;
                    }
                case 4:
                    {
                        player4.SendMessage("GBNType=1,Size=80");
                        break;
                    }
            }
        }

        public int ID
        {
            get
            {
                return id;
            }
        }

        public Client TableCreator
        {
            get
            {
                return tableCreator;
            }
        }

        public Client Player2
        {
            get
            {
                return player2;
            }
            set
            {
                player2 = value;
            }
        }

        public Client Player3
        {
            get
            {
                return player3;
            }
            set
            {
                player3 = value;
            }
        }

        public Client Player4
        {
            get
            {
                return player4;
            }
            set
            {
                player4 = value;
            }
        }

        public int Bet
        {
            get
            {
                return bet;
            }
        }

        public bool PlayersVisibility
        {
            get
            {
                return playersVisibility;
            }
        }

        public bool Chat
        {
            get
            {
                return chat;
            }
        }

        public int MinimalLevel
        {
            get
            {
                return minimalLevel;
            }
        }

        public bool TableVisibility
        {
            get
            {
                return tableVisibility;
            }
            set
            {
                tableVisibility = value;
            }
        }

        public bool VIPOnly
        {
            get
            {
                return vipOnly;
            }
        }

        public bool Moderation
        {
            get
            {
                return moderation;
            }
        }

        public bool AI
        {
            get
            {
                return ai;
            }
        }

        public TableStatus Status
        {
            get
            {
                return status;
            }
        }
    }
}
