using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeloteServer
{
    // Класс, представляющий собой заказ
    class Order
    {
        public Order(OrderType Type, int Size, CardSuit Trump)
        {
            this.Type = Type;
            this.Size = Size;
            this.Trump = Trump;
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
    }
}
