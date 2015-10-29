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
            IsBelote = false;
        }

        // Создаем список карт из строки
        public CardList(string cards)
        {
            list = new List<Card>();
            IsBelote = false;
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

        // Сортировка списка
        public void Sort()
        {
            list.Sort();
        }

        // Удаление карты из списка
        public void Remove(Card card)
        {
            list.Remove(card);
        }

        // Установка игры с козырем или без для всех карт списка
        private void PlayWithTrump(bool GameWithTrump)
        {
            foreach (Card c in list)
            {
                c.IsGameWithTrump = GameWithTrump;
            }
        }

        // Метод поиска комбинации Belote в списке карт
        private bool FindBelote()
        {
            Card king = list.Find(c => (c.Type == CardType.C_K) && (c.IsTrump));
            Card queen = list.Find(c => (c.Type == CardType.C_Q) && (c.IsTrump));
            if ((king != null) && (queen != null))
                return true;
            return false;
        }

        // Установка козыря для списка карт
        public void SetTrump(CardSuit Trump)
        {
            // Если игра без козыря, то сбрасываем козырь для всех карт списка
            if (Trump == CardSuit.C_NONE)
            {
                PlayWithTrump(false);
            }
            // Если игра с козырем, то устанавливаем козырем все карты соответствующей масти
            else
            {
                PlayWithTrump(true);
                foreach (Card c in list)
                {
                    if (c.Suit == Trump)
                    {
                        c.IsTrump = true;
                    }
                }
            }
        }

        // Функция возвращает список возможных к ходу карт, полученный из текущего списка карт, по взятке и месту хода
        public CardList PossibleCardsToMove(Bribe bribe, int Place)
        {
            CardList possibleCards = new CardList();

            return possibleCards;
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

        // Отвечает за комбинацию карт "Блот" в наборе карт
        public bool IsBelote
        {
            get;
            private set;
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
