using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeloteClient
{
    public class MessageHandlers
    {
        private ServerConnection ServerConnection;

        public MessageHandlers(ServerConnection connection)
        {
            ServerConnection = connection;
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
