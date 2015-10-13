using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BeloteServer
{
    class TablesList
    {
        private Game game;
        private List<Table> tables;

        public TablesList(Game Game)
        {
            game = Game;
            tables = new List<Table>();
        }

        public void CreateTable(Player Creator, int Bet, bool PlayersVisibility, bool Chat, int MinimalLevel,
            bool TableVisibility, bool VIPOnly, bool Moderation, bool AI)
        {
            Table table = new Table(game, Creator, Bet, PlayersVisibility, Chat, MinimalLevel, TableVisibility, VIPOnly, Moderation, AI);
            tables.Add(table);
        }

        public void DeleteTable(int ID)
        {
            this.DeleteTable(tables.Find(t => (t.ID == ID)));
        }

        public void DeleteTable(Table table)
        {
            if (table != null)
                tables.Remove(table);
        }

        public List<Table> FindTables(int BetFrom, int BetTo, bool PlayersVisibility, bool Chat, int MinimalLevel, bool VIPOnly, bool Moderation, bool AI)
        {
            return tables.FindAll(t => ((t.Bet >= BetFrom) && (t.Bet <= BetTo) && (t.PlayersVisibility == PlayersVisibility) &&
                (t.Chat == Chat) && (t.MinimalLevel >= MinimalLevel) && (t.VIPOnly == VIPOnly) && (t.Moderation == Moderation) &&
                (t.AI == AI)));
        }

        public void MakeTableVisible(int ID)
        {
            tables[tables.FindIndex(t => (t.ID == ID))].TableVisibility = true;
        }

        public void AddPlayer2(int TableID, Player Player2)
        {
            tables[tables.FindIndex(t => (t.ID == TableID))].Player2 = Player2;
        }

        public void RemovePlayer2(int TableID)
        {
            tables[tables.FindIndex(t => (t.ID == TableID))].Player2 = null;
        }

        public void AddPlayer3(int TableID, Player Player3)
        {
            tables[tables.FindIndex(t => (t.ID == TableID))].Player3 = Player3;
        }

        public void RemovePlayer3(int TableID)
        {
            tables[tables.FindIndex(t => (t.ID == TableID))].Player3 = null;
        }

        public void AddPlayer4(int TableID, Player Player4)
        {
            tables[tables.FindIndex(t => (t.ID == TableID))].Player4 = Player4;
        }

        public void RemovePlayer4(int TableID)
        {
            tables[tables.FindIndex(t => (t.ID == TableID))].Player4 = null;
        }

        public int Count
        {
            get
            {
                return tables.Count;
            }
        }
    }
}
