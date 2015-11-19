using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeloteClient
{
    // Класс - контейнер для списка игровых карт
    public class CardList
    {
        private List<Card> list;

        public CardList()
        {
            list = new List<Card>();
        }

        // Создаем список карт из строки
        public CardList(string cards)
        {

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
            list.Add(card);
        }

        // Удаление карты из списка
        public void Remove(Card card)
        {
            list.Remove(card);
        }

        // Индесатор для обращения к конкретной карте
        public Card this[int Index]
        {
            get
            {
                return list[Index];
            }
        }

        // Проверяет наличие карты в списке
        public bool Exists(Card card)
        {
            return ((list.Find(c => (c.Type == card.Type) && (c.Suit == card.Suit))) != null);
        }

        // Обращение к карте по ее типу и масти
        public Card this[CardType type, CardSuit suit]
        {
            get
            {
                return list.Find(c => (c.Type == type) && (c.Suit == suit));
            }
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
