using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BeloteServer
{
    // Класс, представляющий собой карту. Реализует интерфейс IComaparable для сортировки карт
    class Card : IComparable<Card>
    {
        private bool gamewithtrump;
        // Создаем карту по ее типу и масти
        public Card(CardType type, CardSuit suit)
        {
#if DEBUG
            Debug.WriteLine("{0} Создание новой карты. Тип - {1}, масть - {2}", DateTime.Now, type, suit);
#endif
            Suit = suit;
            Type = type;
            IsTrump = false;
            IsGameWithTrump = false;
        }

        // Создаем карту из строки
        public Card(string card)
        {
            // Если длина строки, то карту нельзя идентифицировать
            if (card.Length != 2)
            {
                Suit = CardSuit.C_NONE;
                Type = CardType.C_UNDEFINED;
            }
            else
            {
                IsTrump = false;
                IsGameWithTrump = false;
                Suit = Helpers.StringToSuit(card.Substring(0, 1));
                Type = (CardType)Int32.Parse(card.Substring(1, 1));
            }
#if DEBUG
            Debug.WriteLine("{0} Создание новой карты из строки: ", DateTime.Now, card);
#endif
        }

        // Метод сравнения карты с другой картой
        public int CompareTo(Card other)
        {
            int thisSuit = (int)this.Suit;
            int otherSuit = (int)other.Suit;
            // Сравнение по масти
            if (thisSuit < otherSuit)
            {
                return -1;
            }
            else
            if (thisSuit > otherSuit)
            {
                return 1;
            }
            // Если масть одна и та же
            else
            {
                int thisType = (int)this.Type;
                int otherType = (int)other.Type;
                // Если карты козырные
                if (this.IsTrump)
                {
                    // Сначала сравниваем карты по их стоимости
                    if (this.Cost < other.Cost)
                        return 1;
                    else
                    if (this.Cost > other.Cost)
                        return -1;
                    // Если стоимость равна (для 7 и 8 например), то сравниваем их в порядке следования
                    else
                    {
                        if (thisType < otherType)
                            return 1;
                        else
                            return -1;
                    }
                }
                // Если карты не козырные 
                else
                {
                    // Сравниваем их в порядке следования
                    if (thisType < otherType)
                        return 1;
                    else
                        return -1;
                }
            }
        }

        // Тип карты - туз, валет и т.д.
        public CardType Type
        {
            get;
            private set;
        }

        // Масть карты
        public CardSuit Suit
        {
            get;
            private set;
        }

        // Является ли карта козырной
        public bool IsTrump
        {
            get;
            set;
        }

        // Происходит ли игра с козырем или нет. Если устанавливается игра без козыря, то и карта становится некозырной
        public bool IsGameWithTrump
        {
            get
            {
                return gamewithtrump;
            }
            set
            {
                gamewithtrump = value;
                if (value == false)
                {
                    IsTrump = false;
                }
            }
        }

        // Рассчет стоимости карты
        public int Cost
        {
            get
            {
                if ((Type == CardType.C_7) || (Type == CardType.C_8))
                    return 0;
                if (Type == CardType.C_Q)
                    return 3;
                if (Type == CardType.C_K)
                    return 4;
                if (Type == CardType.C_10)
                    return 10;
                if (!IsGameWithTrump)
                {
                    // Игра без козыря
                    switch (Type)
                    {
                        case CardType.C_9:
                            {
                                return 0;
                            }
                        case CardType.C_A:
                            {
                                return 19;
                            }
                        case CardType.C_J:
                            {
                                return 2;
                            }
                        default:
                            break;
                    }
                }
                else
                {
                    // Игра с козырем
                    if (Type == CardType.C_A)
                        return 11;
                    if (IsTrump)
                    {
                        // Рассматриваемая карта - козырная
                        switch (Type)
                        {
                            case CardType.C_9:
                                {
                                    return 14;
                                }
                            case CardType.C_J:
                                {
                                    return 20;
                                }
                            default:
                                break;
                        }
                    }
                    else
                    {
                        // Рассматриваемая карта не козырная
                        switch (Type)
                        {
                            case CardType.C_9:
                                {
                                    return 0;
                                }
                            case CardType.C_J:
                                {
                                    return 2;
                                }
                            default:
                                break;
                        }
                    }
                }
                return 0;
            }
        }

        // Метод преобразования карты в строку в формате: масть + тип
        public override string ToString()
        {
            return Helpers.SuitToString(Suit) + ((int)Type).ToString();
        }
    }
}
