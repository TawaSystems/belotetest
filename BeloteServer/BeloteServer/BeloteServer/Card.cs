using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeloteServer
{
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

    public enum CardSuit
    {
        C_HEARTS = 1,
        C_CLUBS = 2,
        C_SPADES = 3,
        С_DIAMONDS = 4
    }

    class Card
    {
        public Card(CardType type, CardSuit suit)
        {
            Suit = suit;
            Type = type;
            IsTrump = false;
            Cost = 0;
        }

        public Card(string card)
        {
            IsTrump = false;
            Cost = 0;
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

        public int Cost
        {
            get;
            set;
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
