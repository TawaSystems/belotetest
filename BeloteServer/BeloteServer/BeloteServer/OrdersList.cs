using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeloteServer
{
    class OrdersList
    {
        private List<Order> list;

        public OrdersList()
        {
            list = new List<Order>();
            Current = null;
            IsCapot = false;
            IsCoinched = false;
            IsSurcoinched = false;
            IsPass = false;
            OrderedTeam = BeloteTeam.TEAM_NONE;
        }

        public void Add(Order order, BeloteTeam Team)
        {
            list.Add(order);
            if ((order.Type == OrderType.ORDER_BET) || (order.Type == OrderType.ORDER_CAPOT))
            {
                Current = order;
                OrderedTeam = Team;
            }
            if (order.Type == OrderType.ORDER_PASS)
            {
                if (Test4Pass())
                    IsPass = true;
            }
            else
            if (order.Type == OrderType.ORDER_CAPOT)
            {
                IsCapot = true;
            }
            else
            if (order.Type == OrderType.ORDER_COINCHE)
            {
                IsCoinched = true;
            }
            else
            if (order.Type == OrderType.ORDER_SURCOINCHE)
            {
                IsSurcoinched = true;
            }
        }

        // Проверка на отказ играть с даной раздачей карт (4 подряд идущих пасса без других заказов)
        private bool Test4Pass()
        {
            if (Count == 4)
            {
                foreach (Order o in list)
                {
                    if (o.Type != OrderType.ORDER_PASS)
                        return false;
                }
                return true;
            }
            else
                return false;
        }

        // Проверка на 4 пасса после ставки
        private bool Test4PassAfterBet()
        {
            if (Count > 4)
            {
                for (var i = Count - 1; i >= Count - 4; i--)
                {
                    if (list[i].Type != OrderType.ORDER_PASS)
                        return false;
                }
                return true;
            }
            else
                return false;
        }

        // Проверка на два пасса после контры
        private bool Test2PassAfterCoinched()
        {
            if (!IsCoinched)
                return false;
            if ((list[Count - 1].Type == OrderType.ORDER_PASS) && (list[Count - 2].Type == OrderType.ORDER_PASS))
            {
                return true;
            }
            else
                return false;
        }

        // Проверка на завершенность процесса торговли
        public bool IsEnded()
        {
            if (IsPass)
                return true;
            if (Test4PassAfterBet())
                return true;
            if (Test2PassAfterCoinched())
                return true;
            if (IsSurcoinched)
                return true;
            return false;
        }

        public int Count
        {
            get
            {
                return list.Count;
            }
        }

        public Order Current
        {
            get;
            private set;
        }

        public bool IsCoinched
        {
            get;
            private set;
        }

        public bool IsSurcoinched
        {
            get;
            private set;
        }

        public bool IsCapot
        {
            get;
            private set;
        }

        public bool IsPass
        {
            get;
            private set;
        }

        public BeloteTeam OrderedTeam
        {
            get;
            private set;
        }

    }
}
