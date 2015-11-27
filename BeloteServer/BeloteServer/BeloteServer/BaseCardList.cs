using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BeloteServer
{
    class BaseCardList
    {
        protected List<Card> list;

        public BaseCardList()
        {
#if DEBUG
            Debug.WriteLine("{0} Создание списка карт", DateTime.Now);
#endif
            list = new List<Card>();
        }

        // Создаем список карт из строки
        public BaseCardList(string cards)
        {
#if DEBUG 
            Debug.WriteLine("{0} Создание списка карт из строки карт - {1}", DateTime.Now, cards);
#endif

            list = new List<Card>();
            // Идем с шагом в 2, вынимая каждую карту из строки
            for (var i = 0; i < cards.Length; i += 2)
            {
                list.Add(new Card(cards.Substring(i, 2)));
            }
        }

        // Добавление карты в список
        public void Add(Card card)
        {
#if DEBUG
            Debug.WriteLine("{0} Добавление карты в список. Карта - {1}", DateTime.Now, card.ToString());
#endif
            list.Add(card);
        }

        // Сортировка списка
        public void Sort()
        {
#if DEBUG
            Debug.WriteLine("{0} Сортировка списка карт", DateTime.Now);
#endif
            list.Sort();
        }

        // Удаление карты из списка
        public void Remove(Card card)
        {
#if DEBUG 
            Debug.WriteLine("{0} Удаление карты из списка - {1}", DateTime.Now, card.ToString());
#endif
            list.Remove(card);
        }

        // Проверяет наличие карты в списке
        public bool Exists(Card card)
        {
            return ((list.Find(c => (c.Type == card.Type) && (c.Suit == card.Suit))) != null);
        }

        // Индесатор для обращения к конкретной карте
        public Card this[int Index]
        {
            get
            {
                return list[Index];
            }
        }

        // Обращение к карте по ее типу и масти
        public Card this[CardType type, CardSuit suit]
        {
            get
            {
                return list.Find(c => (c.Type == type) && (c.Suit == suit));
            }
        }

        // Очистка списка
        public void Clear()
        {
            list.Clear();
        }

        // Преобразование списка к строке - последовательности карт
        public override string ToString()
        {
            string Res = "";
            foreach (Card c in list)
            {
                Res += c.ToString();
            }
            return Res;
        }

        // Количество карт в списке
        public int Count
        {
            get
            {
                return list.Count;
            }
        }
    }
}
