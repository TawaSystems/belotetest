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
            // Добавляем в список все возможные карты в колоде
            foreach (CardSuit s in Enum.GetValues(typeof(CardSuit)))
            {
                if (s == CardSuit.C_NONE)
                    continue;
                foreach (CardType t in Enum.GetValues(typeof(CardType)))
                {
                    if (t == CardType.C_UNDEFINED)
                        continue;
                    list.Add(new Card(t, s));
                }
            }
        }

        // Создание раздачи на четверых игроков
        /*public void Distribution(CardList p1, CardList p2, CardList p3, CardList p4)
        {
            // Если какой то из списков не предоставлен, то и колоду раздать не получится
            if ((p1 == null) || (p2 == null) || (p3 == null) || (p4 == null))
            {
                return;
            }
            // Раздаем по 8 карт каждому игроку
            for (var i = 0; i < 8; i++)
            {
                p1.Add(GetRandomCard());
                p2.Add(GetRandomCard());
                p3.Add(GetRandomCard());
                p4.Add(GetRandomCard());
            }
            // Сортируем карты для всех игроков
            p1.Sort();
            p2.Sort();
            p3.Sort();
            p4.Sort();
        }*/

        // Создание тестовой раздачи на четверых игроков
        public void Distribution(CardList p1, CardList p2, CardList p3, CardList p4)
        {
            // Если какой то из списков не предоставлен, то и колоду раздать не получится
            if ((p1 == null) || (p2 == null) || (p3 == null) || (p4 == null))
            {
                return;
            }
            p1.Add(new Card(CardType.C_K, CardSuit.C_SPADES));
            p1.Add(new Card(CardType.C_Q, CardSuit.C_SPADES));
            p1.Add(new Card(CardType.C_J, CardSuit.C_SPADES));
            p1.Add(new Card(CardType.C_J, CardSuit.C_HEARTS));
            p1.Add(new Card(CardType.C_10, CardSuit.C_HEARTS));
            p1.Add(new Card(CardType.C_9, CardSuit.C_HEARTS));
            p1.Add(new Card(CardType.C_8, CardSuit.C_HEARTS));
            p1.Add(new Card(CardType.C_7, CardSuit.C_HEARTS));


            p2.Add(new Card(CardType.C_K, CardSuit.C_CLUBS));
            p2.Add(new Card(CardType.C_Q, CardSuit.C_CLUBS));
            p2.Add(new Card(CardType.C_J, CardSuit.C_CLUBS));
            p2.Add(new Card(CardType.C_10, CardSuit.C_CLUBS));
            p3.Add(new Card(CardType.C_9, CardSuit.C_CLUBS));
            p3.Add(new Card(CardType.C_8, CardSuit.C_CLUBS));
            p2.Add(new Card(CardType.C_7, CardSuit.C_CLUBS));
            p2.Add(new Card(CardType.C_8, CardSuit.С_DIAMONDS));

            p3.Add(new Card(CardType.C_10, CardSuit.C_SPADES));
            p3.Add(new Card(CardType.C_9, CardSuit.C_SPADES));
            p3.Add(new Card(CardType.C_8, CardSuit.C_SPADES));
            p2.Add(new Card(CardType.C_7, CardSuit.C_SPADES));
            p3.Add(new Card(CardType.C_10, CardSuit.С_DIAMONDS));
            p2.Add(new Card(CardType.C_9, CardSuit.С_DIAMONDS));
            p3.Add(new Card(CardType.C_J, CardSuit.С_DIAMONDS));
            p3.Add(new Card(CardType.C_7, CardSuit.С_DIAMONDS));

            p4.Add(new Card(CardType.C_A, CardSuit.C_CLUBS));
            p4.Add(new Card(CardType.C_A, CardSuit.C_SPADES));
            p4.Add(new Card(CardType.C_A, CardSuit.C_HEARTS));
            p4.Add(new Card(CardType.C_A, CardSuit.С_DIAMONDS));
            p4.Add(new Card(CardType.C_K, CardSuit.C_HEARTS));
            p4.Add(new Card(CardType.C_Q, CardSuit.C_HEARTS));
            p4.Add(new Card(CardType.C_K, CardSuit.С_DIAMONDS));
            p4.Add(new Card(CardType.C_Q, CardSuit.С_DIAMONDS));
            
            // Сортируем карты для всех игроков
            p1.Sort();
            p2.Sort();
            p3.Sort();
            p4.Sort();
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
