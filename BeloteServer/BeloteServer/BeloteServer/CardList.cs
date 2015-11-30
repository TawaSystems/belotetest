using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BeloteServer
{
    // Класс - контейнер для списка игровых карт
    class CardList : BaseCardList
    {
        public CardList() : base()
        {
            IsBelote = false;
        }

        public CardList(string cards) : base(cards)
        {
            IsBelote = false;
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
        public bool SuitExists(CardSuit Suit)
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
        public bool TrumpExists()
        {
#if DEBUG
            Debug.WriteLine("{0} Проверка на наличие козыря в списке", DateTime.Now);
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
            CardListTrump = Trump;
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
            Debug.WriteLine("{0} Поиск повзможных карт для хода по текущей взятке ({1}) и номеру игрока - {2}", DateTime.Now, bribe, Place);
#endif
            // В случае если взятка завершена, то игрок будет делать ход на следующей - соответственно он может использовать любую карту
            if ((bribe.IsEnded) || (bribe.IsEmpty))
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
                        // Если у игрока нет запрашиваемой масти, но в то же время первая карта старше
                        if (bribe.FulledCount == 2)
                        {
                            // Если второй игрок пошел некозырной картой другой масти, то можно ходить любой картой
                            if (bribe[0].Suit != bribe[1].Suit)
                                return this;
                            else
                            {
                                // Если первая карта больше второй, то можно пойти любой картой
                                if (bribe[0].ThisIsBiggerThen(bribe[1]))
                                    return this;
                            }
                        }

                        // Если у игрока нет запрашиваемой масти, но в то же время вторая карта старше
                        if (bribe.FulledCount == 3)
                        {
                            // Если второй игрок пошел некозырной картой той же масти что и первый - проверяем дальше
                            if (bribe[0].Suit == bribe[1].Suit)
                            {
                                // Если и третий игрок пошел той же мастью
                                if (bribe[2].Suit == bribe[0].Suit)
                                {
                                    // Если карта второго игрока больше чем карта первого и третьего - то можно пойти любой картой
                                    if ((bribe[1].ThisIsBiggerThen(bribe[0])) && (bribe[1].ThisIsBiggerThen(bribe[2])))
                                        return this;
                                }
                                else
                                {
                                    // Если первый игрок пошел картой младше - то можно ходить любой
                                    if (bribe[1].ThisIsBiggerThen(bribe[0]))
                                        return this;
                                }
                            }
                        }

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
                        /*
                        // Если ходит 4 игрок, то он не обязан перебивать карту своего сокомандника (игрока №2), если она козырная, иначе необходимо перебить, если возможно
                        if (bribe.FulledCount == 3)
                        {
                            if (bribe[1].IsTrump)
                            {
                                // Если же и третий игрок пошел козырем, то нужно проверить, больше ли он чем козырь второго игрока
                                if ((bribe[2].IsTrump) && (bribe[2].Cost >= bribe[1].Cost) && (bribe[2].Type != CardType.C_7))
                                {
                                    // Необходимо попробовать перебить
                                    Card card1 = list.Find(c => (c.IsTrump) && (c.Cost >= bribe.SeniorTrump.Cost) && (c.Type != CardType.C_7));
                                    // В случае, если у игрока имеются козыри, старше чем использовались на раздаче, нужно использовать их
                                    if (card1 != null)
                                    {
                                        foreach (Card c in list)
                                        {
                                            if ((c.IsTrump) && (c.Cost > bribe.SeniorTrump.Cost))
                                                possibleCards.Add(c);
                                        }
                                        return possibleCards;
                                    }
                                    // Если и таких карт нет, то можно использовать любую карту из имеющихся
                                    else
                                    {
                                        return this;
                                    }
                                }
                                else
                                {
                                    return this;
                                }
                            }
                        }
                        */
                        Card card = list.Find(c => (c.IsTrump) && (c.Cost >= bribe.SeniorTrump.Cost) && (c.Type != CardType.C_7));
                        // В случае, если у игрока имеются козыри, старше чем использовались на раздаче, нужно использовать их
                        if (card != null)
                        {
                            foreach (Card c in list)
                            {
                                if ((c.IsTrump) && (c.Cost >= bribe.SeniorTrump.Cost))
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
                    Card card = list.Find(c => (c.IsTrump) && (c.Cost >= bribe.SeniorTrump.Cost) && (c.Type != CardType.C_7));
                    // Если найдены козыри старше, чем использованы на взятке, то необходимо использовать их
                    if (card != null)
                    {
                        foreach (Card c in list)
                        {
                            if ((c.IsTrump) && (c.Cost >= bribe.SeniorTrump.Cost))
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
            else
                IsBelote = false;
#if DEBUG
            Debug.WriteLine("{0} Проверка на наличие бонуса БЛОТ в списке карт. Результат - {1}", DateTime.Now, IsBelote);
#endif
        }

        // Получить саршую карту выбранной масти
        public Card GetHigherCard(CardSuit Suit)
        {
            Card result = null;
            for (var i = 0; i < list.Count; i++)
            {
                if (list[i].Suit == Suit)
                {
                    if (result != null)
                    {
                        // Если мы ищем козырную масть, то сравниваем сначала по стоимости, потом по типу
                        if (result.IsTrump)
                        {
                            // Значит 7 и 8
                            if (result.Cost == list[i].Cost)
                            {
                                if ((int)result.Type < (int)list[i].Type)
                                    result = list[i];
                            }
                            else
                            {
                                if (result.Cost < list[i].Cost)
                                    result = list[i];
                            }
                        }
                        else
                        {
                            if (!result.ThisIsBiggerThen(list[i]))
                                result = list[i];
                        }
                    }
                    else
                    {
                        result = list[i];
                    }
                }
            }
            return result;
        }

        // Получить младшую карту выбранной масти
        public Card GetLowerCard(CardSuit Suit)
        {
            Card result = null;
            for (var i = 0; i < list.Count; i++)
            {
                if (list[i].Suit == Suit)
                {
                    if (result != null)
                    {
                        // Если мы ищем козырную масть, то сравниваем сначала по стоимости, потом по типу
                        if (result.IsTrump)
                        {
                            // Значит 7 и 8
                            if (result.Cost == list[i].Cost)
                            {
                                if ((int)result.Type > (int)list[i].Type)
                                    result = list[i];
                            }
                            else
                            {
                                if (result.Cost > list[i].Cost)
                                    result = list[i];
                            }
                        }
                        else
                        {
                            if (result.ThisIsBiggerThen(list[i]))
                                result = list[i];
                        }
                    }
                    else
                    {
                        result = list[i];
                    }
                }
            }
            return result;
        }

        // Отвечает за комбинацию карт "Блот" в наборе карт
        public bool IsBelote
        {
            get;
            private set;
        }

        // Козырь
        public CardSuit CardListTrump
        {
            get;
            private set;
        }
    }
}
