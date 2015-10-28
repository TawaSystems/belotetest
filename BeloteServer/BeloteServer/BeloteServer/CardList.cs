using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeloteServer
{
    // Класс - контейнер для списка игровых карт
    class CardList
    {
        private List<Card> list;

        public CardList()
        {
            list = new List<Card>();
        }

        public CardList(string cards)
        {
            list = new List<Card>();
            for (var i = 0; i < cards.Length; i +=2)
            {
                list.Add(new Card(cards.Substring(i, 2)));
            }
        }

        public void Add(Card card)
        {
            list.Add(card);
        }

        public void Sort(Comparison<Card> comparison)
        {
            list.Sort(comparison);
        }

        public void Remove(Card card)
        {
            list.Remove(card);
        }

        public Card this[int Index]
        {
            get
            {
                return list[Index];
            }
        }

        public override string ToString()
        {
            string Res = "";
            foreach (Card c in list)
            {
                Res += c.ToString();
            }
            return Res;
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
