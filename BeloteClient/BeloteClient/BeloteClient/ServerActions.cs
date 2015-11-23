using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeloteClient
{
    public class ServerActions
    {
        private ServerConnection ServerConnection;

        public ServerActions()
        {
            ServerConnection = new ServerConnection();
        }

        public void Disconnect()
        {
            ServerConnection.Disconnect();
        }

        // Регистрация с помощью электронной почты
        public bool RegistrationEmail(string Email, string Password, string Nickname, string Sex, string Country)
        {
            Message regMessage = new Message(Messages.MESSAGE_AUTORIZATION_REGISTRATION_EMAIL,
                String.Format("Nickname={0},Email={1},Password={2},Country={3},Sex={4}", Nickname, Email, Password, Country, Sex));
            Dictionary<string, string> res = ServerConnection.ExecuteMessage(regMessage);
            return (res["Registration"] == "1");
        }

        // Авторизация с помощью электронной почты
        public bool AutorizationEmail(string Email, string Password, out int PlayerID)
        {
            Message autMessage = new Message(Messages.MESSAGE_AUTORIZATION_AUTORIZATION_EMAIL,
                String.Format("Email={0},Password={1}", Email, Password));
            Dictionary<string, string> res = ServerConnection.ExecuteMessage(autMessage);
            PlayerID = Int32.Parse(res["PlayerID"]);
            return (PlayerID != -1);
        }

        // Получение информации об игроке
        public Player GetPlayer(int PlayerID)
        {
            if (PlayerID < 0)
                return null;
            Message playerMessage = new Message(Messages.MESSAGE_PLAYER_GET_INFORMATION,
                    String.Format("PlayerID={0}", PlayerID));
            Dictionary<string, string> pParams = ServerConnection.ExecuteMessage(playerMessage);
            if (pParams != null)
            {
                return new Player(pParams);
            }
            else
            {
                return null;
            }
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
                    tablesList.AddTable(new Table(Helpers.SplitCommandString(s)));
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
            Dictionary<string, string> tParams = ServerConnection.ExecuteMessage(new Message(Messages.MESSAGE_TABLE_MODIFY_CREATE,
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
            Dictionary<string, string> tParams = ServerConnection.ExecuteMessage(new Message(Messages.MESSAGE_TABLE_SELECT_CONCRETIC,
                String.Format("ID={0}", TableID)));
            return new Table(tParams);
        }

        // Добавление игрока на стол
        public bool AddPlayerToTable(int TableID, int Place)
        {
            Dictionary<string, string> pParams = ServerConnection.ExecuteMessage(new Message(Messages.MESSAGE_TABLE_PLAYERS_ADD, 
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
            Dictionary<string, string> bParams = ServerConnection.ExecuteMessage(new Message(Messages.MESSAGE_TABLE_PLAYERS_ADDBOT,
                String.Format("Place={0}", Place)));
            return (bParams["Result"] == "1");
        }

        // Удаление бота со стола
        public void DeleteBotFromTable(int Place)
        {
            ServerConnection.ExecuteMessageWithoutResult(new Message(Messages.MESSAGE_TABLE_PLAYERS_DELETEBOT,
                String.Format("Place={0}", Place)));
        }

        // Добавление обработчика сообщения
        public void AddMessageHandler(string Command, MessageDelegate Handler)
        {
            ServerConnection.AddMessageHandler(Command, Handler);
        }

        // Удаление обработчика сообщения
        public void DeleteMessageHandler(string Command, MessageDelegate Handler)
        {
            ServerConnection.DeleteMessageHandler(Command, Handler);
        }

        // Установка всех обработчиков событий перед игрой
        public void SetPreGameHandlers(MessageDelegate PlayerAddToTableHandler, MessageDelegate PlayerDeleteFromTableHandler,
            MessageDelegate CreatorLeaveTableHandler, MessageDelegate StartGameHandler)
        {
            AddMessageHandler(Messages.MESSAGE_TABLE_PLAYERS_ADD, PlayerAddToTableHandler);
            AddMessageHandler(Messages.MESSAGE_TABLE_PLAYERS_DELETE, PlayerDeleteFromTableHandler);
            AddMessageHandler(Messages.MESSAGE_TABLE_MODIFY_CREATORLEAVE, CreatorLeaveTableHandler);
            AddMessageHandler(Messages.MESSAGE_GAME_START, StartGameHandler);
        }

        // Снятие всех обработчиков событий перед игрой
        public void UnsetPreGameHandlers(MessageDelegate PlayerAddToTableHandler, MessageDelegate PlayerDeleteFromTableHandler,
            MessageDelegate CreatorLeaveTableHandler, MessageDelegate StartGameHandler)
        {
            DeleteMessageHandler(Messages.MESSAGE_TABLE_PLAYERS_ADD, PlayerAddToTableHandler);
            DeleteMessageHandler(Messages.MESSAGE_TABLE_PLAYERS_DELETE, PlayerDeleteFromTableHandler);
            DeleteMessageHandler(Messages.MESSAGE_TABLE_MODIFY_CREATORLEAVE, CreatorLeaveTableHandler);
            DeleteMessageHandler(Messages.MESSAGE_GAME_START, StartGameHandler);
        }

        // Выход игрока со стола во время игры
        public void PlayerQuitFromTable()
        {
            ServerConnection.ExecuteMessageWithoutResult(new Message(Messages.MESSAGE_TABLE_PLAYERS_QUIT, ""));
        }

        // Тестирование стола на заполненность
        public void TestFullfillTable()
        {
            ServerConnection.ExecuteMessageWithoutResult(new Message(Messages.MESSAGE_TABLE_TEST_FULLFILL, ""));
        }

        // Игрок делает заказ
        public void PlayerMakeOrder(Order order)
        {
            if (order == null)
                return;
            ServerConnection.ExecuteMessageWithoutResult(new Message(Messages.MESSAGE_GAME_BAZAR_BET,
                String.Format("Size={0},Type={1},Trump={2}", order.Size, (int)order.Type, Helpers.SuitToString(order.Trump))));
        }
        
        // Анонсирование игроком бонусов
        public void PlayerAnnounceBonuses(BonusList Bonuses)
        {
            if (Bonuses == null)
                return;
            ServerConnection.ExecuteMessageWithoutResult(new Message(Messages.MESSAGE_GAME_BONUSES_ANNOUNCE, Bonuses.ToString()));
        }

        // Игрок делает ход
        public void PlayerMakeMove(Card Card)
        {
            if (Card == null)
                return;
            ServerConnection.ExecuteMessageWithoutResult(new Message(Messages.MESSAGE_GAME_GAMING_PLAYERMOVE, 
                String.Format("Card={0}", Card.ToString())));
        }

        // Установка всех обработчиков события для процесса игры
        public void SetGameHandlers(MessageDelegate CardsDistributionHandler, MessageDelegate BazarNextPlayerHandler,
            MessageDelegate BazarPlayerSayHandler, MessageDelegate BazarEndHandler, MessageDelegate BazarPassHandler, 
            MessageDelegate GameNextPlayerHandler, MessageDelegate GameRemindCardHandler, MessageDelegate BonusesGetAllHandler, 
            MessageDelegate BonusesShowTypesHandler, MessageDelegate BonusesShowWinnerHandler, MessageDelegate GamePlayerQuitHandler,
            MessageDelegate GameEndHandler)
        {
            AddMessageHandler(Messages.MESSAGE_GAME_DISTRIBUTIONCARDS, CardsDistributionHandler);
            AddMessageHandler(Messages.MESSAGE_GAME_BAZAR_NEXTBETPLAYER, BazarNextPlayerHandler);
            AddMessageHandler(Messages.MESSAGE_GAME_BAZAR_SAYBET, BazarPlayerSayHandler);
            AddMessageHandler(Messages.MESSAGE_GAME_BAZAR_END, BazarEndHandler);
            AddMessageHandler(Messages.MESSAGE_GAME_BAZAR_PASS, BazarPassHandler);
            AddMessageHandler(Messages.MESSAGE_GAME_GAMING_NEXTPLAYER, GameNextPlayerHandler);
            AddMessageHandler(Messages.MESSAGE_GAME_GAMING_REMINDCARD, GameRemindCardHandler);
            AddMessageHandler(Messages.MESSAGE_GAME_BONUSES_ALL, BonusesGetAllHandler);
            AddMessageHandler(Messages.MESSAGE_GAME_BONUSES_TYPES, BonusesShowTypesHandler);
            AddMessageHandler(Messages.MESSAGE_GAME_BONUSES_WINNER, BonusesShowWinnerHandler);
            AddMessageHandler(Messages.MESSAGE_TABLE_PLAYERS_QUIT, GamePlayerQuitHandler);
            AddMessageHandler(Messages.MESSAGE_GAME_END, GameEndHandler);
        }

        // Снятие всех обработчиков событий процесса игры
        public void UnsetGameHandlers(MessageDelegate CardsDistributionHandler, MessageDelegate BazarNextPlayerHandler,
            MessageDelegate BazarPlayerSayHandler, MessageDelegate BazarEndHandler, MessageDelegate BazarPassHandler,
            MessageDelegate GameNextPlayerHandler, MessageDelegate GameRemindCardHandler, MessageDelegate BonusesGetAllHandler,
            MessageDelegate BonusesShowTypesHandler, MessageDelegate BonusesShowWinnerHandler, MessageDelegate GamePlayerQuitHandler,
            MessageDelegate GameEndHandler)
        {
            DeleteMessageHandler(Messages.MESSAGE_GAME_DISTRIBUTIONCARDS, CardsDistributionHandler);
            DeleteMessageHandler(Messages.MESSAGE_GAME_BAZAR_NEXTBETPLAYER, BazarNextPlayerHandler);
            DeleteMessageHandler(Messages.MESSAGE_GAME_BAZAR_SAYBET, BazarPlayerSayHandler);
            DeleteMessageHandler(Messages.MESSAGE_GAME_BAZAR_END, BazarEndHandler);
            DeleteMessageHandler(Messages.MESSAGE_GAME_BAZAR_PASS, BazarPassHandler);
            DeleteMessageHandler(Messages.MESSAGE_GAME_GAMING_NEXTPLAYER, GameNextPlayerHandler);
            DeleteMessageHandler(Messages.MESSAGE_GAME_GAMING_REMINDCARD, GameRemindCardHandler);
            DeleteMessageHandler(Messages.MESSAGE_GAME_BONUSES_ALL, BonusesGetAllHandler);
            DeleteMessageHandler(Messages.MESSAGE_GAME_BONUSES_TYPES, BonusesShowTypesHandler);
            DeleteMessageHandler(Messages.MESSAGE_GAME_BONUSES_WINNER, BonusesShowWinnerHandler);
            DeleteMessageHandler(Messages.MESSAGE_TABLE_PLAYERS_QUIT, GamePlayerQuitHandler);
            DeleteMessageHandler(Messages.MESSAGE_GAME_END, GameEndHandler);
        }
    }
}
