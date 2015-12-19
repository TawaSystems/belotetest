using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeloteClient
{
    public class Orders
    {
        private Order[] orders;

        public Orders()
        {
            orders = new Order[4];
        }

        public void RenewPlayersOrders()
        {
            EndOrder = null;    
        }

        public void SetEndOrder(Order endOrder)
        {
            for (var i = 0; i < 4; i++)
                orders[i] = null;
            EndOrder = endOrder;
        }

        public Order this[int Place]
        {
            get
            {
                if ((Place >= 1) && (Place <= 4))
                    return orders[Place - 1];
                else
                    return null;
            }
            set
            {
                if ((Place >= 1) && (Place <= 4))
                {
                    orders[Place - 1] = value;
                }
            }
        }

        public Order EndOrder
        {
            get;
            private set;
        }

        public BetType PossibleBetType
        {
            get;
            set;
        }

        public int PossibleBetSize
        {
            get;
            set;
        }

    }
}
