using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeloteClient
{
    public class TablesActions : BaseActionsType
    {
        public TablesActions(ServerConnection connection) : base(connection)
        {
        }

        // Выборка всех столов, для этого должны быть созданы обработчики событий
        public TablesList GetAllPossibleTables()
        {
            string resultTables = ServerConnection.ExecuteMessageGetMessage(new Message(Messages.MESSAGE_TABLE_SELECT_ALL, "")).Msg;
            if (resultTables == "")
            {
                return null;
            }
            TablesList tablesList = new TablesList();
            try
            {
                string[] tables = resultTables.Split('|');
                foreach (string s in tables)
                {
                    tablesList.AddTable(new Table(new MessageResult(new Message("", s))));
                }
                return tablesList;
            }
            catch
            {
                return null;
            }
        }

        // Создание игрового стола
        public Table CreateTable(int Creator, int Bet, bool PlayersVisibility, bool Chat, int MinimalLevel, bool TableVisibility,
            bool VIPOnly, bool Moderation, bool AI)
        {
            Table result = new Table(-1, Creator, Bet, PlayersVisibility, Chat, MinimalLevel, TableVisibility,
                VIPOnly, Moderation, AI);
            result.Player2 = -1;
            result.Player3 = -1;
            result.Player4 = -1;
            MessageResult tParams = ServerConnection.ExecuteMessage(new Message(Messages.MESSAGE_TABLE_MODIFY_CREATE,
                String.Format("Bet={0},PlayersVisibility={1},Chat={2},MinimalLevel={3},TableVisibility={4},VIPOnly={5},Moderation={6},AI={7}",
                Bet, Helpers.BoolToString(PlayersVisibility), Helpers.BoolToString(Chat),
                MinimalLevel, Helpers.BoolToString(TableVisibility), Helpers.BoolToString(VIPOnly),
                Helpers.BoolToString(Moderation), Helpers.BoolToString(AI))));
            int ID = Int32.Parse(tParams["ID"]);
            if (ID != -1)
            {
                result.ChangeID(ID);
                return result;
            }
            else
            {
                return null;
            }
        }

        // Получает информацию о конкретном игровом столе
        public Table GetTable(int TableID)
        {
            MessageResult tParams = ServerConnection.ExecuteMessage(new Message(Messages.MESSAGE_TABLE_SELECT_CONCRETIC,
                String.Format("ID={0}", TableID)));
            return new Table(tParams);
        }

        // Добавление игрока на стол
        public bool AddPlayerToTable(int TableID, int Place)
        {
            MessageResult pParams = ServerConnection.ExecuteMessage(new Message(Messages.MESSAGE_TABLE_PLAYERS_ADD,
                String.Format("ID={0},Place={1}", TableID, Place)));
            return (pParams["Result"] == "1");
        }

        // Выход игрока со стола
        public void ExitPlayerFromTable(int Place)
        {
            string SendingMsg;
            if (Place == 1)
            {
                SendingMsg = Messages.MESSAGE_TABLE_MODIFY_CREATORLEAVE;
            }
            else
            {
                SendingMsg = Messages.MESSAGE_TABLE_PLAYERS_DELETE;
            }
            ServerConnection.ExecuteMessageWithoutResult(new Message(SendingMsg, ""));
        }

        // Метод добавления бота на стол
        public bool AddBotToTable(int Place)
        {
            MessageResult bParams = ServerConnection.ExecuteMessage(new Message(Messages.MESSAGE_TABLE_PLAYERS_ADDBOT,
                String.Format("Place={0}", Place)));
            return (bParams["Result"] == "1");
        }

        // Удаление бота со стола
        public void DeleteBotFromTable(int Place)
        {
            ServerConnection.ExecuteMessageWithoutResult(new Message(Messages.MESSAGE_TABLE_PLAYERS_DELETEBOT,
                String.Format("Place={0}", Place)));
        }

        // Тестирование стола на заполненность
        public void TestFullfillTable()
        {
            ServerConnection.ExecuteMessageWithoutResult(new Message(Messages.MESSAGE_TABLE_TEST_FULLFILL, ""));
        }
    }
}
