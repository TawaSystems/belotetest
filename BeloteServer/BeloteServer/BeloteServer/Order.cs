using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeloteServer
{
    class Order
    {
        public Order(OrderType Type, int Size, CardSuit Trump)
        {
            this.Type = Type;
            this.Size = Size;
            this.Trump = Trump;
        }

        public OrderType Type
        {
            get;
            private set;
        }

        public int Size
        {
            get;
            private set;
        }

        public CardSuit Trump
        {
            get;
            private set;
        }
    }
}
