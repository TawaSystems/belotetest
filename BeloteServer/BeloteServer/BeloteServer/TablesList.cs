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

        // Индексатор - обращение к столам по идентификатору
        public Table this[int ID]
        {
            get
            {
                return tables.Find(t => (t.ID == ID));
            }
        }

        // Метод создания игрового стола с заданными параметрами и добавление его в список столов
        public int CreateTable(ClientMan Creator, int Bet, bool PlayersVisibility, bool Chat, int MinimalLevel,
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

        // Метод выборки всех доступных столов
        public List<Table> AllAvailableTables()
        {
#if DEBUG
            Debug.WriteLine(DateTime.Now.ToString() + " Выборка всех доступных столов");
            Debug.Indent();
#endif
            List<Table> res = tables.FindAll(t => ((t.Status == TableStatus.WAITING) && (t.TableVisibility == true)));
#if DEBUG
            Debug.WriteLine("Найдено столов: " + res.Count);
            Debug.Unindent();
#endif
            return res;
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
                (t.AI == AI) && (t.Status == TableStatus.WAITING) && (t.TableVisibility == true)));
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

        public void TestStartGame(int TableID)
        {
            if (this[TableID].TestFullfill())
            {
                this[TableID].StartGame();
            }
        }

        // Метод добавления игрока на стол
        public bool AddPlayer(int TableID, Client Player, int Place)
        {
#if DEBUG
            Debug.WriteLine(DateTime.Now.ToString() + String.Format(" Добавление игрока на стол. Идентификатор стола - {0}, идентификатор игрока - {1}, место - {2}",
                TableID, Player.ID, Place));
#endif
            if (this[TableID] == null)
                return false;
            switch (Place)
            {
                case 2:
                    {
                        if (this[TableID].Player2 != null)
                        {
                            return false;
                        }
                        this[TableID].Player2 = Player;
                        this.game.Server.SendMessageToClients(String.Format("{1}Player={0},Place=2", Player.ID, Messages.MESSAGE_TABLE_PLAYERS_ADD),
                            this[TableID].Player3, this[TableID].Player4, this[TableID].TableCreator);
                        break;
                    }
                case 3:
                    {
                        if (this[TableID].Player3 != null)
                        {
                            return false;
                        }
                        this[TableID].Player3 = Player;
                        this.game.Server.SendMessageToClients(String.Format("{1}Player={0},Place=3", Player.ID, Messages.MESSAGE_TABLE_PLAYERS_ADD),
                            this[TableID].Player2, this[TableID].Player4, this[TableID].TableCreator);
                        break;
                    }
                case 4:
                    {
                        if (this[TableID].Player4 != null)
                        {
                            return false;
                        }
                        this[TableID].Player4 = Player;
                        this.game.Server.SendMessageToClients(String.Format("{1}Player={0},Place=4", Player.ID, Messages.MESSAGE_TABLE_PLAYERS_ADD),
                            this[TableID].Player3, this[TableID].Player2, this[TableID].TableCreator);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            return true;
        }

        // Метод удаления игрока со стола
        public void RemovePlayer(int TableID, int Place)
        {
#if DEBUG
            Debug.WriteLine(String.Format("{0} Удаление игрока со стола. Идентификатор стола: {1}, место игрока: {2}", DateTime.Now.ToString(), TableID, Place));
#endif
            switch (Place)
            {
                case 2:
                    {
                        this[TableID].Player2 = null;
                        break;
                    }
                case 3:
                    {
                        this[TableID].Player3 = null;
                        break;
                    }
                case 4:
                    {
                        this[TableID].Player4 = null;
                        break;
                    }
                default:
                    {
                        return;
                    }
            }
            if (this[TableID].Status == TableStatus.WAITING)
                this[TableID].SendMessageToClients(String.Format("{1}Place={0}", Place, Messages.MESSAGE_TABLE_PLAYERS_DELETE));
        }

        // Количество столов в списке
        public int Count
        {
            get
            {
                return tables.Count;
            }
        }
    }
}
