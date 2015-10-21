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

        public Table this[int ID]
        {
            get
            {
                return tables.Find(t => (t.ID == ID));
            }
        }

        // Метод создания игрового стола с заданными параметрами и добавление его в список столов
        public int CreateTable(Client Creator, int Bet, bool PlayersVisibility, bool Chat, int MinimalLevel,
            bool TableVisibility, bool VIPOnly, bool Moderation, bool AI)
        {
#if DEBUG
            Debug.WriteLine(DateTime.Now.ToString() + " Создание нового стола и добавление его в список столов");
            Debug.Indent();
#endif
            Table table = new Table(game, Creator, Bet, PlayersVisibility, Chat, MinimalLevel, TableVisibility, VIPOnly, Moderation, AI);
            tables.Add(table);
#if DEBUG
            Debug.Unindent();
#endif
            return table.ID;
        }

        // Метод удаления игрового стола по его идентификатору
        public void DeleteTable(int ID)
        {
            this.DeleteTable(this[ID]);
        }

        // Метод удаления игрового стола по ссылке на объект
        public void DeleteTable(Table table)
        {
            if (table != null)
            {
#if DEBUG
                Debug.WriteLine(DateTime.Now.ToString() + " Удаление из списка стола");
                Debug.Indent();
                Debug.WriteLine("Идентификатор стола: " + table.ID);
                Debug.Unindent();
#endif
                tables.Remove(table);
            }
        }

        // Метод выборки списка игровых столов по заданным параметрам
        public List<Table> FindTables(int BetFrom, int BetTo, bool PlayersVisibility, bool Chat, int MinimalLevel, bool VIPOnly, bool Moderation, bool AI)
        {
#if DEBUG
            Debug.WriteLine(DateTime.Now.ToString() +  " Выборка столов по параметрам");
            Debug.Indent();
            Debug.WriteLine(String.Format("Ставка от {0} до {1}, видимость игроков - {2}, чат - {3}, минимальный уровень - {4}, VIP - {5}, премодерация - {6}, AI - {7}",
                BetFrom, BetTo, PlayersVisibility, Chat, MinimalLevel, VIPOnly, Moderation, AI));
#endif
            List<Table> res = tables.FindAll(t => ((t.Bet >= BetFrom) && (t.Bet <= BetTo) && (t.PlayersVisibility == PlayersVisibility) &&
                (t.Chat == Chat) && (t.MinimalLevel >= MinimalLevel) && (t.VIPOnly == VIPOnly) && (t.Moderation == Moderation) &&
                (t.AI == AI) && (t.Status == TableStatus.WAITING)));
#if DEBUG 
            Debug.WriteLine("Найдено столов: " + res.Count);
            Debug.Unindent();
#endif
            return res;
        }

        // Метод, делающий игровой стол видимым для всех игроков
        public void MakeTableVisible(int ID)
        {
#if DEBUG
            Debug.WriteLine(DateTime.Now.ToString() +  " Открытие видимости стола с идентификатором - " + ID.ToString());
#endif
            this[ID].TableVisibility = true;
        }

        // Метод добавления игрока на место №2
        public bool AddPlayer2(int TableID, Client Player2)
        {
            if (this[TableID].Player2 != null)
            {
                return false;
            }
#if DEBUG
            Debug.WriteLine(DateTime.Now.ToString() + String.Format(" Добавление второго игрока на стол. Идентификатор стола - {0}, идентификатор игрока - {1}", 
                TableID, Player2.ID));
#endif
            this[TableID].Player2 = Player2;
            this.game.Server.SendMessageToClients(String.Format("TPAID={0},Place=2", TableID), this[TableID].Player3,
                this[TableID].Player4, this[TableID].TableCreator);
            if (this[TableID].TestFullfill())
            {
                this[TableID].SendMessageToClients("GTS");
            }
            return true;
        }

        // Метод удаления игрока с места №2
        public void RemovePlayer2(int TableID)
        {
#if DEBUG
            Debug.WriteLine(DateTime.Now.ToString() + " Удаление второго игрока со стола с идентификатором - " + TableID.ToString());
#endif
            this[TableID].Player2 = null;
            this.game.Server.SendMessageToClients(String.Format("TPDID={0},Place=2", TableID));
        }

        // Метод добавления игрока на место №3
        public bool AddPlayer3(int TableID, Client Player3)
        {
            if (this[TableID].Player3 != null)
            {
                return false;
            }
#if DEBUG
            Debug.WriteLine(DateTime.Now.ToString() + String.Format(" Добавление третьего игрока на стол. Идентификатор стола - {0}, идентификатор игрока - {1}",
                TableID, Player3.ID));
#endif
            this[TableID].Player3 = Player3;
            this.game.Server.SendMessageToClients(String.Format("TPAID={0},Place=3", TableID), this[TableID].Player2,
                this[TableID].Player4, this[TableID].TableCreator);
            if (this[TableID].TestFullfill())
            {
                this[TableID].SendMessageToClients("GTS");
            }
            return true;
        }

        // Метод удаления игрока с места №3
        public void RemovePlayer3(int TableID)
        {
#if DEBUG
            Debug.WriteLine(DateTime.Now.ToString() + " Удаление третьего игрока со стола с идентификатором - " + TableID.ToString());
#endif
            this[TableID].Player3 = null;
            this.game.Server.SendMessageToClients(String.Format("TPDID={0},Place=3", TableID));
        }

        // Метод добавления игрока на место №4
        public bool AddPlayer4(int TableID, Client Player4)
        {
            if (this[TableID].Player4 != null)
            {
                return false;
            }
#if DEBUG
            Debug.WriteLine(DateTime.Now.ToString() + String.Format(" Добавление четвертого игрока на стол. Идентификатор стола - {0}, идентификатор игрока - {1}",
                TableID, Player4.ID));
#endif
            this[TableID].Player4 = Player4;
            this.game.Server.SendMessageToClients(String.Format("TPAID={0},Place=4", TableID), this[TableID].Player2,
                this[TableID].Player3, this[TableID].TableCreator);
            if (this[TableID].TestFullfill())
            {
                this[TableID].SendMessageToClients("GTS");
            }
            return true;
        }

        // Метод удаления игрока с места №4
        public void RemovePlayer4(int TableID)
        {
#if DEBUG
            Debug.WriteLine(DateTime.Now.ToString() + " Удаление четвертого игрока со стола с идентификатором - " + TableID.ToString());
#endif
            this[TableID].Player4 = null;
            this.game.Server.SendMessageToClients(String.Format("TPDID={0},Place=4", TableID));
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
