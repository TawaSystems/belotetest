using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeloteServer
{
    // Класс, представляющий собой карту
    class Card : IComparable<Card>
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
        }

        public int CompareTo(Card other)
        {
            int thisSuit = (int)this.Suit;
            int otherSuit = (int)other.Suit;
            if (thisSuit < otherSuit)
            {
                return -1;
            }
            else
            if (thisSuit > otherSuit)
            {
                return 1;
            }
            else
            {
                int thisType = (int)this.Type;
                int otherType = (int)other.Type;
                if (this.IsTrump)
                {
                    int[] weight = new int[8];
                    weight[(int)CardType.C_7] = 0;
                    weight[(int)CardType.C_8] = 1;
                    weight[(int)CardType.C_Q] = 2;
                    weight[(int)CardType.C_K] = 3;
                    weight[(int)CardType.C_10] = 4;
                    weight[(int)CardType.C_A] = 5;
                    weight[(int)CardType.C_9] = 6;
                    weight[(int)CardType.C_J] = 7;
                    if (weight[thisType] < weight[otherType])
                        return 1;
                    else
                        return -1;
                }
                else
                {
                    if (thisType < otherType)
                        return 1;
                    else
                        return -1;
                }
            }
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
            return Helpers.SuitToString(Suit);
        }
    }
}
