using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BeloteServer
{
    // Класс - контейнер для списка игровых карт
    class CardList
    {
        private List<Card> list;

        public CardList()
        {
#if DEBUG
            Debug.WriteLine("{0} Создание списка карт", DateTime.Now);
#endif
            list = new List<Card>();
            IsBelote = false;
        }

        // Создаем список карт из строки
        public CardList(string cards)
        {
#if DEBUG 
            Debug.WriteLine("{0} Создание списка карт из строки карт - {1}", DateTime.Now, cards);
#endif

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

        // Установка игры с козырем или без для всех карт списка
        private void PlayWithTrump(bool GameWithTrump)
        {
#if DEBUG 
            Debug.WriteLine("{0} Назначение игры с козырем или без. Значение - {1}", DateTime.Now, GameWithTrump);
#endif
            foreach (Card c in list)
            {
                c.IsGameWithTrump = GameWithTrump;
            }
        }

        // Проверяет, присутствует ли в списке карта карта заданной колоды
        private bool SuitExists(CardSuit Suit)
        {
#if DEBUG
            Debug.WriteLine("{0} Проверка на наличие масти в списке - {1}", DateTime.Now, Suit);
#endif
            Card card = list.Find(c => c.Suit == Suit);
            if (card != null)
                return true;
            else
                return false;
        }

        // Проверяет, присутствует ли в списке карт козырная карта
        private bool TrumpExists()
        {
#if DEBUG
            Debug.WriteLine("{0} Проверка на наличие масти в списке - {1}", DateTime.Now);
#endif
            Card card = list.Find(c => c.IsTrump);
            if (card != null)
                return true;
            else
                return false;
        }

        // Установка козыря для списка карт
        public void SetTrump(CardSuit Trump)
        {
#if DEBUG
            Debug.WriteLine("{0} Установка козыря - {1}", DateTime.Now, Trump);
#endif
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
#if DEBUG
#if DEBUG
            Debug.WriteLine("{0} Поиск повзможных карт для хода по текущей взятке ({1}) и номеру игрока - {2}", DateTime.Now, bribe, Place);
#endif
            // В случае если взятка завершена, то игрок будет делать ход на следующей - соответственно он может использовать любую карту
            if (bribe.IsEnded)
                return this;
            CardList possibleCards = new CardList();
            // Если первый игрок пошел не с козырной масти... Случаи 1, 2, 3, 6
            if (!bribe.IsTrumpBribe)
            {
                // В случае если на взятке с запрашиваемой некозырной мастью не было сделано хода с козырной мастью... Случаи 1, 2, 3
                if (!bribe.BribeTrumped)
                {
                    // Если у игрока есть карты этой масти, то можно пойти ими... Случай 1
                    if (SuitExists(bribe.BribeSuit))
                    {
                        foreach (Card c in list)
                        {
                            if (c.Suit == bribe.BribeSuit)
                            {
                                possibleCards.Add(c);
                            }
                        }
                    }
                    else
                    // Если у игрока нет карт запрашиваемой масти, но есть козыри, то нужно пойти одним из них... Случай 2
                    if (TrumpExists())
                    {
                        foreach (Card c in list)
                        {
                            if (c.IsTrump)
                            {
                                possibleCards.Add(c);
                            }
                        }
                    }
                    // Если у игрока нет ни карт запрашиваемой масти, ни козырей, то он может пойти любой картой... Случай 3
                    else
                    {
                        return this;
                    }
                }
                // Если на вязтке с запрашиваемой некозырной мастью был совершен ход козырной мастью... Случай 6
                else
                {
                    // Если имеются карты запрашиваемой масти, то надо ходить ими
                    if (SuitExists(bribe.BribeSuit))
                    {
                        foreach (Card c in list)
                        {
                            if (c.Suit == bribe.BribeSuit)
                            {
                                possibleCards.Add(c);
                            }
                        }
                    }
                    else
                    {
                        Card card = list.Find(c => (c.IsTrump) && (c.Cost > bribe.SeniorTrump.Cost));
                        // В случае, если у игрока имеются козыри, старше чем использовались на раздаче, нужно использовать их
                        if (card != null)
                        {
                            foreach (Card c in list)
                            {
                                if ((c.IsTrump) && (c.Cost > bribe.SeniorTrump.Cost))
                                    possibleCards.Add(c);
                            }
                        }
                        // Если и таких карт нет, то можно использовать любую карту из имеющихся
                        else
                        {
                            return this;
                        }
                    }
                }

            }
            // В случае если заказанная масть на взятке - козырная... Случаи 4, 5
            else
            {
                // Если у ходящего игрока имеются козыри... Случай 4
                if (TrumpExists())
                {
                    Card card = list.Find(c => (c.IsTrump) && (c.Cost > bribe.SeniorTrump.Cost));
                    // Если найдены козыри старше, чем использованы на взятке, то необходимо использовать их
                    if (card != null)
                    {
                        foreach (Card c in list)
                        {
                            if ((c.IsTrump) && (c.Cost > bribe.SeniorTrump.Cost))
                                possibleCards.Add(c);
                        }
                    }
                    // Если козырей старше нет, то можно использовать любой
                    else
                    {
                        foreach (Card c in list)
                        {
                            if (c.IsTrump)
                                possibleCards.Add(c);
                        }
                    }
                }
                // Если козырей нет, то он может пойти любой картой... Случай 5
                else
                {
                    return this;
                }
            }
            return possibleCards;
        }

        // Метод поиска комбинации Belote в списке карт
        public void FindBelote()
        {
            Card king = list.Find(c => (c.Type == CardType.C_K) && (c.IsTrump));
            Card queen = list.Find(c => (c.Type == CardType.C_Q) && (c.IsTrump));
            if ((king != null) && (queen != null))
                IsBelote = true;
            IsBelote = false;
#if DEBUG
            Debug.WriteLine("{0} Проверка на наличие бонуса БЛОТ в списке карт. Результат - {1}", DateTime.Now, IsBelote);
#endif
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
