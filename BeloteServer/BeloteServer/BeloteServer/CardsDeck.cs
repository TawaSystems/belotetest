using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeloteServer
{
    // Класс, содержащий в себе колоду карт
    class CardsDeck
    {
        private CardList list;
        private Random random;

        // Создание колоды для игры в блот
        public CardsDeck()
        {
            list = new CardList();
            random = new Random();
            foreach (CardSuit s in Enum.GetValues(typeof(CardSuit)))
            {
                if (s == CardSuit.C_NONE)
                    continue;
                foreach (CardType t in Enum.GetValues(typeof(CardType)))
                {
                    list.Add(new Card(t, s));
                }
            }
        }

        // Создание раздачи на четверых игроков
        public void Distribution(CardList p1, CardList p2, CardList p3, CardList p4)
        {
            if ((p1 == null) || (p2 == null) || (p3 == null) || (p4 == null))
            {
                return;
            }
            for (var i = 0; i < 8; i++)
            {
                p1.Add(GetRandomCard());
                p2.Add(GetRandomCard());
                p3.Add(GetRandomCard());
                p4.Add(GetRandomCard());
            }

        }

        // Взятие случайной карты из колоды
        private Card GetRandomCard()
        {
            Card c = list[random.Next(list.Count)];
            list.Remove(c);
            return c;
        }
    }
}
