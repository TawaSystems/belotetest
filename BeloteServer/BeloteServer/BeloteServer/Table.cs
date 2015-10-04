using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }

        private void CreateTableInDatabase()
        {

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
