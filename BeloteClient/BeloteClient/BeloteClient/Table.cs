using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BeloteClient
{
    public class Table
    {
        private Game game;

        public Table(Game Game, int ID, int Creator) : this(Game, ID, Creator, Constants.GAME_MINIMAL_BET, true, true, 0, true, false, false, true)
        {
        }

        public Table(Game Game, int ID, int Creator, int Bet, bool PlayersVisibility, bool Chat, int MinimalLevel, bool TableVisibility, bool VIPOnly, bool Moderation, bool AI)
        {
            this.game = Game;
            this.Bet = Bet;
            this.TableCreator = Creator;
            this.PlayersVisibility = PlayersVisibility;
            this.Chat = Chat;
            this.MinimalLevel = MinimalLevel;
            this.TableVisibility = TableVisibility;
            this.VIPOnly = VIPOnly;
            this.Moderation = Moderation;
            this.AI = AI;
            this.ID = ID;
        }

        public int ID
        {
            get;
            private set;
        }

        public int TableCreator
        {
            get;
            private set;
        }

        public int Player2
        {
            get;
            set;
        }

        public int Player3
        {
            get;
            set;
        }

        public int Player4
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
    }
}
