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
            this.Player2 = -1;
            this.Player3 = -1;
            this.Player4 = -1;
        }

        public Table(Game game, Dictionary<string, string> tParams)
        {
            this.game = game;
            ID = Int32.Parse(tParams["ID"]);
            Bet = Int32.Parse(tParams["Bet"]);
            PlayersVisibility = Helpers.StringToBool(tParams["PlayersVisibility"]);
            Chat = Helpers.StringToBool(tParams["Chat"]);
            MinimalLevel = Int32.Parse(tParams["MinimalLevel"]);
            VIPOnly = Helpers.StringToBool(tParams["VIPOnly"]);
            Moderation = Helpers.StringToBool(tParams["Moderation"]);
            AI = Helpers.StringToBool(tParams["AI"]);
            TableCreator = Int32.Parse(tParams["Creator"]);
            int Player2, Player3, Player4;
            if (!Int32.TryParse(tParams["Player2"], out Player2))
                Player2 = -1;
            this.Player2 = Player2;
            if (!Int32.TryParse(tParams["Player3"], out Player3))
                Player3 = -1;
            this.Player3 = Player3;
            if (!Int32.TryParse(tParams["Player4"], out Player4))
                Player4 = -1;
            this.Player4 = Player4;
            TableVisibility = true;
        }

        public void ChangeID(int NewID)
        {
            ID = NewID;
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
