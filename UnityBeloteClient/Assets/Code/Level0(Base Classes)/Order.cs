using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeloteClient
{
    // Класс, представляющий собой заказ
    public class Order
    {
        public Order(OrderType Type, int Size, CardSuit Trump)
        {
            this.Type = Type;
            this.Size = Size;
            this.Trump = Trump;
            this.Team = BeloteTeam.TEAM_NONE;
        }

        public void ChangeTeam(BeloteTeam NewTeam)
        {
            Team = NewTeam;
        }

        // Тип заказа
        public OrderType Type
        {
            get;
            private set;
        }

        // Размер заказа
        public int Size
        {
            get;
            private set;
        }

        // Козырь
        public CardSuit Trump
        {
            get;
            private set;
        }

        // Команда
        public BeloteTeam Team
        {
            get;
            private set;
        }
    }
}
