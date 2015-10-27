using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeloteServer
{
    // Тип игровой карты: 7, 8...
    public enum CardType
    {
        C_A = 1,
        C_J = 2,
        C_Q = 3,
        C_K = 4,
        C_7 = 7,
        C_8 = 8, 
        C_9 = 9,
        C_10 = 0
    } 

    // Масть игровой карты
    public enum CardSuit
    {
        C_NONE = 0,
        C_HEARTS = 1,
        C_CLUBS = 2,
        C_SPADES = 3,
        С_DIAMONDS = 4
    }

    // Класс, представляющий собой карту
    class Card
    {
        public Card(CardType type, CardSuit suit)
        {
            Suit = suit;
            Type = type;
            IsTrump = false;
            IsGameWithTrump = false;
        }

        public Card(string card)
        {
            IsTrump = false;
            IsGameWithTrump = false;
            switch (card[0])
            {
                case 'H':
                    {
                        Suit = CardSuit.C_HEARTS;
                        break;
                    }
                case 'C':
                    {
                        Suit = CardSuit.C_CLUBS;
                        break;
                    }
                case 'S':
                    {
                        Suit = CardSuit.C_SPADES;
                        break;
                    }
                case 'D':
                    {
                        Suit = CardSuit.С_DIAMONDS;
                        break;
                    }
                default:
                    break;
            }
            Type = (CardType)Int32.Parse(card.Substring(1, 1));
        }

        public CardType Type
        {
            get;
            private set;
        }

        public CardSuit Suit
        {
            get;
            private set;
        }

        public bool IsTrump
        {
            get;
            set;
        }

        public bool IsGameWithTrump
        {
            get;
            set;
        }

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

        public override string ToString()
        {
            string Res = "";
            switch (Suit)
            {
                case CardSuit.C_HEARTS:
                    {
                        Res = "H";
                        break;
                    }
                case CardSuit.C_CLUBS:
                    {
                        Res = "C";
                        break;
                    }
                case CardSuit.C_SPADES:
                    {
                        Res = "S";
                        break;
                    }
                case CardSuit.С_DIAMONDS:
                    {
                        Res = "D";
                        break;
                    }
                default:
                    break;
            }
            Res += Type.ToString();
            return Res;
        }
    }
}
