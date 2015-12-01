using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeloteClient
{
    public class GameProcess 
    {
        private ServerActions serverActions;

        public GameProcess(ServerActions actions)
        {
            serverActions = actions;
            Place = -1;
            Status = GameStatus.NON_GAME;
        }

        // Смена текущего стола
        public void ChangeTable(Table newTable, int newPlace = -1)
        {
            CurrentTable = newTable;
            Place = (CurrentTable == null) ? -1 : newPlace;
            Status = (CurrentTable == null) ? GameStatus.NON_GAME : GameStatus.WAITING;
        }

        // Добавление бота на текущий стол
        public bool AddBot(int BotPlace)
        {
            if (CurrentTable == null)
                return false;
            if (serverActions.Tables.AddBotToTable(BotPlace))
            {
                CurrentTable.SetPlayerAtPlace(-BotPlace, BotPlace);
                serverActions.Tables.TestFullfillTable();
                return true;
            }
            else
            {
                return false;
            }
        }

        // Удаление бота с игрового стола
        public void DeleteBot(int BotPlace)
        {
            serverActions.Tables.DeleteBotFromTable(BotPlace);
        }

        // Выход игрока со стола. IsSelf - сам ли игрок вышел со стола
        public void ExitFromTable(bool IsSelf)
        {
            if (IsSelf)
            {
                if (Status == GameStatus.WAITING)
                    serverActions.Tables.ExitPlayerFromTable(Place);
                else
                    serverActions.Game.PlayerQuitFromTable(Place);
            }
            //SetPreGameHandlers(false);
            ChangeTable(null);
        }

        // Текущий игровой стол. Если игрок находится на каком-то столе, то существует. Иначе = NULL
        public Table CurrentTable
        {
            get;
            private set;
        }

        // Текущее место игрока - 1, 2, 3, 4. Если игрок не находится на столе, то -1
        public int Place
        {
            get;
            private set;
        }

        public GameStatus Status
        {
            get;
            private set;
        }
    }
}
