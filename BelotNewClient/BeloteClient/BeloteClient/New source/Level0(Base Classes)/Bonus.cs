using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BeloteClient
{
    public class Bonus
    {
        // Метод создания бонуса - ему присваиваются тип, младшая карта, а также опционно для бонусов типа "последовательность" - масть
        public Bonus(BonusType Type, CardType HighCard, bool IsTrump, CardSuit Suit = CardSuit.C_NONE)
        {
            this.Type = Type;
            this.HighCard = HighCard;
            this.Suit = Suit;
            this.IsTrump = IsTrump;
        }

        // Создание бонуса из строки
        public Bonus(string BonusString)
        {
            // Если длина строки не равна четырем - то это никакой и не бонус
            if (BonusString.Length != 4)
            {
                Type = BonusType.BONUS_NONE;
                HighCard = CardType.C_UNDEFINED;
                Suit = CardSuit.C_NONE;
                IsTrump = false;
            }    
            else
            {
                // Считываем из строки значения всех необходимых параметров бонуса
                Type = (BonusType)Int32.Parse(BonusString.Substring(0, 1));
                HighCard = (CardType)Int32.Parse(BonusString.Substring(1, 1));
                Suit = Helpers.StringToSuit(BonusString.Substring(2, 1));
                IsTrump = Helpers.StringToBool(BonusString.Substring(3, 1));
            }
        }

        public BonusType Type
        {
            get;
            private set;
        }

        public CardType HighCard
        {
            get;
            private set;
        }

        public CardSuit Suit
        {
            get;
            private set;
        }

        // Бонус преобразуется к строке следующим образом: Тип + Младшая карта + Масть + Козырь
        public override string ToString()
        {
            return ((int)Type).ToString() + ((int)HighCard).ToString() + Helpers.SuitToString(Suit) + Helpers.BoolToString(IsTrump);
        }

        // Козырные ли карты в бонусе
        public bool IsTrump
        {
            get;
            private set;
        }
    }
}
