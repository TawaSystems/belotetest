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
        ENDING = 4
    }

    class Table
    {
        private Player tableCreator;
        private Player player2;
        private Player player3;
        private Player player4;
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

        public Table(Game Game, Player Creator) : this(Game, Creator, Constants.GAME_MINIMAL_BET, true, true, 0, true, false, false, true)
        {
        }

        public Table(Game Game, Player Creator, int Bet, bool PlayersVisibility, bool Chat, int MinimalLevel, bool TableVisibility, bool VIPOnly, bool Moderation, bool AI)
        {
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
            CreateTableInDatabase();
        }

        private void CreateTableInDatabase()
        {
#if DEBUG
            Debug.WriteLine(DateTime.Now.ToString() + " Добавление записи о созданном столе в базу данных");
            Debug.Indent();
            Debug.WriteLine("Table Creator ID: " + TableCreator.Profile.Id);
#endif   
            game.DataBase.ExecuteQueryWithoutQueue(String.Format("INSERT INTO Tables {0} VALUES (\"{1}\", \"{2}\", \"{3}\", \"{4}\", \"{5}\", \"{6}\", \"{7}\", \"{8}\", \"{9}\");",
                "(TableCreatorId, Bet, PlayersVisibility, Chat, MinimalLevel, TableVisibility, VIPOnly, Moderation, Ai)",
                TableCreator.Profile.Id, Bet, Helpers.BoolToString(PlayersVisibility), Helpers.BoolToString(Chat), MinimalLevel,
                Helpers.BoolToString(TableVisibility), Helpers.BoolToString(VIPOnly), Helpers.BoolToString(Moderation), Helpers.BoolToString(AI)));
#if DEBUG
            Debug.WriteLine("Получение ID созданного стола");
#endif
            id = Int32.Parse(game.DataBase.SelectScalar(String.Format("SELECT MAX(ID) FROM (SELECT Id FROM Tables WHERE TableCreatorId = \"{0}\") AS A1;")));
            if (id != -1)
                status = TableStatus.WAITING;
#if DEBUG 
            Debug.WriteLine("Идентификатор созданного стола: " + id);
            Debug.Unindent();
#endif
        }

        public Player TableCreator
        {
            get
            {
                return tableCreator;
            }
        }

        public Player Player2
        {
            get
            {
                return player2;
            }
        }

        public Player Player3
        {
            get
            {
                return player3;
            }
        }

        public Player Player4
        {
            get
            {
                return player4;
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
