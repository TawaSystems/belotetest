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

        public int CreateTable(Player Creator, int Bet, bool PlayersVisibility, bool Chat, int MinimalLevel,
            bool TableVisibility, bool VIPOnly, bool Moderation, bool AI)
        {
#if DEBUG
            Debug.WriteLine("Создание нового стола и добавление его в список столов");
            Debug.Indent();
#endif
            Table table = new Table(game, Creator, Bet, PlayersVisibility, Chat, MinimalLevel, TableVisibility, VIPOnly, Moderation, AI);
            tables.Add(table);
#if DEBUG
            Debug.Unindent();
#endif
            return table.ID;
        }

        public void DeleteTable(int ID)
        {
            this.DeleteTable(tables.Find(t => (t.ID == ID)));
        }

        public void DeleteTable(Table table)
        {
            if (table != null)
            {
#if DEBUG
                Debug.WriteLine("Удаление из списка стола");
                Debug.Indent();
                Debug.WriteLine("Идентификатор стола: " + table.ID);
                Debug.Unindent();
#endif
                tables.Remove(table);
            }
        }

        public List<Table> FindTables(int BetFrom, int BetTo, bool PlayersVisibility, bool Chat, int MinimalLevel, bool VIPOnly, bool Moderation, bool AI)
        {
#if DEBUG
            Debug.WriteLine("Выборка столов по параметрам");
            Debug.Indent();
            Debug.WriteLine(String.Format("Ставка от {0} до {1}, видимость игроков - {2}, чат - {3}, минимальный уровень - {4}, VIP - {5}, премодерация - {6}, AI - {7}",
                BetFrom, BetTo, PlayersVisibility, Chat, MinimalLevel, VIPOnly, Moderation, AI));
#endif
            List<Table> res = tables.FindAll(t => ((t.Bet >= BetFrom) && (t.Bet <= BetTo) && (t.PlayersVisibility == PlayersVisibility) &&
                (t.Chat == Chat) && (t.MinimalLevel >= MinimalLevel) && (t.VIPOnly == VIPOnly) && (t.Moderation == Moderation) &&
                (t.AI == AI)));
#if DEBUG 
            Debug.WriteLine("Найдено столов: " + res.Count);
            Debug.Unindent();
#endif
            return res;
        }

        public void MakeTableVisible(int ID)
        {
#if DEBUG
            Debug.WriteLine("Открытие видимости стола с идентификатором - " + ID.ToString());
#endif
            tables[tables.FindIndex(t => (t.ID == ID))].TableVisibility = true;
        }

        public void AddPlayer2(int TableID, Player Player2)
        {
#if DEBUG
            Debug.WriteLine(String.Format("Добавление второго игрока на стол. Идентификатор стола - {0}, идентификатор игрока - {1}", 
                TableID, Player2.Profile.Id));
#endif
            tables[tables.FindIndex(t => (t.ID == TableID))].Player2 = Player2;
        }

        public void RemovePlayer2(int TableID)
        {
#if DEBUG
            Debug.WriteLine("Удаление второго игрока со стола с идентификатором - " + TableID.ToString());
#endif
            tables[tables.FindIndex(t => (t.ID == TableID))].Player2 = null;
        }

        public void AddPlayer3(int TableID, Player Player3)
        {
#if DEBUG
            Debug.WriteLine(String.Format("Добавление третьего игрока на стол. Идентификатор стола - {0}, идентификатор игрока - {1}",
                TableID, Player3.Profile.Id));
#endif
            tables[tables.FindIndex(t => (t.ID == TableID))].Player3 = Player3;
        }

        public void RemovePlayer3(int TableID)
        {
#if DEBUG
            Debug.WriteLine("Удаление третьего игрока со стола с идентификатором - " + TableID.ToString());
#endif
            tables[tables.FindIndex(t => (t.ID == TableID))].Player3 = null;
        }

        public void AddPlayer4(int TableID, Player Player4)
        {
#if DEBUG
            Debug.WriteLine(String.Format("Добавление четвертого игрока на стол. Идентификатор стола - {0}, идентификатор игрока - {1}",
                TableID, Player4.Profile.Id));
#endif
            tables[tables.FindIndex(t => (t.ID == TableID))].Player4 = Player4;
        }

        public void RemovePlayer4(int TableID)
        {
#if DEBUG
            Debug.WriteLine("Удаление четвертого игрока со стола с идентификатором - " + TableID.ToString());
#endif
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
