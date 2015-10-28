using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeloteServer
{
    class Bonus
    {
        // Метод создания бонуса - ему присваиваются тип, младшая карта, а также опционно для бонусов типа "последовательность" - масть
        public Bonus(BonusType Type, CardType LowCard, CardSuit Suit = CardSuit.C_NONE)
        {
            this.Type = Type;
            this.LowCard = LowCard;
            this.Suit = Suit;
        }

        // Создание бонуса из строки
        public Bonus(string BonusString)
        {
            // Если длина строки не равна трем - то это никакой и не бонус
            if (BonusString.Length != 3)
            {
                Type = BonusType.BONUS_NONE;
                LowCard = CardType.C_UNDEFINED;
                Suit = CardSuit.C_NONE;
            }    
            else
            {
                // Считываем из строки значения всех необходимых параметров бонуса
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

        // Бонус преобразуется к строке следующим образом: Тип + Младшая карта + Масть
        public override string ToString()
        {
            return ((int)Type).ToString() + ((int)LowCard).ToString() + Helpers.SuitToString(Suit);
        }
    }
}
