using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeloteClient
{
    // Класс, представляющий собой карту. Реализует интерфейс IComaparable для сортировки карт
    public class Card
    {
        // Создаем карту по ее типу и масти
        public Card(CardType type, CardSuit suit)
        {
            Suit = suit;
            Type = type;
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
                Suit = Helpers.StringToSuit(card.Substring(0, 1));
                Type = (CardType)Int32.Parse(card.Substring(1, 1));
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

        // Метод преобразования карты в строку в формате: масть + тип
        public override string ToString()
        {
            return Helpers.SuitToString(Suit) + ((int)Type).ToString();
        }
    }
}
