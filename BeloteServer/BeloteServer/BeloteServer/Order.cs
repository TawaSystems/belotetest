using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeloteServer
{
    public enum OrderType
    {
        ORDER_PASS = 0,
        ORDER_BET = 1,
        ORDER_CAPOT = 2,
        ORDER_COINCHE = 3,
        ORDER_SURCOINCHE = 4
    }

    class Order
    {
        public Order(OrderType Type, int Size)
        {
            this.Type = Type;
            this.Size = Size;
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
    }
}
