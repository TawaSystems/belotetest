using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeloteServer
{
    class Bonus
    {
        public Bonus(BonusType Type, CardType LowCard, CardSuit Suit = CardSuit.C_NONE)
        {
            this.Type = Type;
            this.LowCard = LowCard;
            this.Suit = Suit;
        }

        public Bonus(string BonusString)
        {
            if (BonusString.Length != 3)
            {
                Type = BonusType.BONUS_NONE;
                LowCard = CardType.C_UNDEFINED;
                Suit = CardSuit.C_NONE;
            }    
            else
            {
                Type = (BonusType)Int32.Parse(BonusString.Substring(0, 1));
                LowCard = (CardType)Int32.Parse(BonusString.Substring(1, 1));
                Suit = Helpers.StringToSuit(BonusString.Substring(2, 1));
            }
        }

        public BonusType Type
        {
            get;
            private set;
        }

        public CardType LowCard
        {
            get;
            private set;
        }

        public CardSuit Suit
        {
            get;
            private set;
        }

        public override string ToString()
        {
            return ((int)Type).ToString() + ((int)LowCard).ToString() + Helpers.SuitToString(Suit);
        }
    }
}
