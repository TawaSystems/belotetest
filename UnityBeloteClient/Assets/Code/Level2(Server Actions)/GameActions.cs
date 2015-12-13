using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeloteClient
{
    public class GameActions : BaseActionsType
    {
        public GameActions(ServerConnection connection) : base(connection)
        {
        }
        // Выход игрока со стола во время игры
        public void PlayerQuitFromTable(int Place)
        {
            string SendingMsg;
            if (Place == 1)
            {
                SendingMsg = Messages.MESSAGE_TABLE_MODIFY_CREATORLEAVE;
            }
            else
            {
                SendingMsg = Messages.MESSAGE_TABLE_PLAYERS_QUIT;
            }
            ServerConnection.ExecuteMessageWithoutResult(new Message(SendingMsg, ""));
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
    }
}
