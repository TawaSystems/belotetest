using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeloteServer
{
    class BonusList
    {
        private List<Bonus> list;

        public BonusList(CardList cards)
        {
            list = new List<Bonus>();
        }

        public BonusList(string cards) : this(new CardList(cards))
        {
        }

        public int Count
        {
            get
            {
                return list.Count;
            }
        }
    }
}
