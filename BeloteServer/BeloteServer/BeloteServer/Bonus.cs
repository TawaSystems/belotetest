using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BeloteServer
{
    class Bonus
    {
        // Метод создания бонуса - ему присваиваются тип, младшая карта, а также опционно для бонусов типа "последовательность" - масть
        public Bonus(BonusType Type, CardType HighCard, bool IsTrump, CardSuit Suit = CardSuit.C_NONE)
        {
            Cards = new BaseCardList();
            this.Type = Type;
            this.HighCard = HighCard;
            this.Suit = Suit;
            this.IsTrump = IsTrump;
            Cost = CalculateCost();
#if DEBUG 
            Debug.WriteLine("{0} Создан бонус: Тип - {1}, Старшая карта - {2}, Масть - {3}, Козырь - {4}, Стоимость - {5}",
                DateTime.Now.ToString(), Type, HighCard, Suit, IsTrump, Cost);
#endif 
        }

        // Рассчитывает стоимость бонусов
        private int CalculateCost()
        {
            switch (Type)
            {
                case BonusType.BONUS_TERZ:
                    {
                        return Constants.BONUS_TERZ;
                    }
                case BonusType.BONUS_50:
                    {
                        return Constants.BONUS_50;
                    }
                case BonusType.BONUS_100:
                    {
                        return Constants.BONUS_100;
                    }
                case BonusType.BONUS_4X:
                    {
                        switch (HighCard)
                        {
                            case CardType.C_7:
                            case CardType.C_8:
                                {
                                    return 0;
                                }
                            case CardType.C_Q:
                            case CardType.C_K:
                            case CardType.C_10:
                                {
                                    return Constants.BONUS_4X_10_Q_K;
                                }
                            case CardType.C_9:
                                {
                                    return Constants.BONUS_4X_9;
                                }
                            case CardType.C_J:
                                {
                                    return Constants.BONUS_4X_J;
                                }
                            case CardType.C_A:
                                {
                                    if (IsTrump)
                                        return Constants.BONUS_4X_A_TRUMP;
                                    else
                                        return Constants.BONUS_4X_A;
                                }
                            default:
                                {
                                    return 0;
                                }
                        }
                    }
                default:
                    {
                        return 0;
                    }
            }
        }

        // Создание бонуса из строки
        public Bonus(string BonusString)
        {
            Cards = new BaseCardList();
            // Если длина строки не равна четырем - то это никакой и не бонус
            if (BonusString.Length < 4)
            {
                Type = BonusType.BONUS_NONE;
                HighCard = CardType.C_UNDEFINED;
                Suit = CardSuit.C_NONE;
                IsTrump = false;
                Cost = 0;
            }    
            else
            {
                // Считываем из строки значения всех необходимых параметров бонуса
                Type = (BonusType)Int32.Parse(BonusString.Substring(0, 1));
                HighCard = (CardType)Int32.Parse(BonusString.Substring(1, 1));
                Suit = Helpers.StringToSuit(BonusString.Substring(2, 1));
                IsTrump = Helpers.StringToBool(BonusString.Substring(3, 1));
                Cost = CalculateCost();
                if (BonusString.Length > 4)
                {
                    Cards = new BaseCardList(BonusString.Substring(4, BonusString.Length - 4));
                }
            }
#if DEBUG
            Debug.WriteLine("{0} Создан бонус: Тип - {1}, Старшая карта - {2}, Масть - {3}, Козырь - {4}, Стоимость - {5}",
                DateTime.Now.ToString(), Type, HighCard, Suit, IsTrump, Cost);
#endif
        }

        public int CompareTo(Bonus Other)
        {
            // Если текущий бонус старше
            if (((int)this.Type) > ((int)Other.Type))
            {
                return 1;
            }
            else
            // Если текущий бонус младше
            if (((int)this.Type) < ((int)Other.Type))
            {
                return -1;
            }
            // Если тип бонусов равен
            else
            {
                // Если это бонус типа "Последовательность", то определяем старшенство по младшей карте карте
                if ((this.Type == BonusType.BONUS_TERZ) || (this.Type == BonusType.BONUS_50) || (this.Type == BonusType.BONUS_100))
                {
                    // Последовательность старше
                    if (this.HighCard > Other.HighCard)
                    {
                        return 1;
                    }
                    else
                    // Последовательность младше
                    if (this.HighCard < Other.HighCard)
                    {
                        return -1;
                    }
                    // Если старшинство карт в последовательностях одинаково
                    else
                    {
                        // Если текущая последовательность козырная - она старше
                        if (this.IsTrump)
                        {
                            return 1;
                        }
                        else
                        // Если другая последовательность козырная - то старше она
                        if (Other.IsTrump)
                        {
                            return -1;
                        }
                        // Последовательности равны
                        else
                        {
                            return 0;
                        }
                    }
                }
                // Если это последовательность 4X
                else
                {
                    int[] weights = new int[8];
                    weights[(int)CardType.C_Q] = 0;
                    weights[(int)CardType.C_K] = 1;
                    weights[(int)CardType.C_10] = 2;
                    weights[(int)CardType.C_A] = 3;
                    weights[(int)CardType.C_9] = 4;
                    weights[(int)CardType.C_J] = 5;
                    // Если карта старше по весу, то и бонус старше. Равны быть не могут
                    if (weights[(int)this.HighCard] > (weights[(int)Other.HighCard]))
                    {
                        return 1;
                    }
                    else
                    {
                        return -1;
                    }
                }

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

        // Бонус преобразуется к строке следующим образом: Тип + Младшая карта + Масть + Козырь + Карты
        public override string ToString()
        {
            return ((int)Type).ToString() + ((int)HighCard).ToString() + Helpers.SuitToString(Suit) + Helpers.BoolToString(IsTrump) + Cards.ToString();
        }

        // Стоимость бонуса
        public int Cost
        {
            get;
            private set;
        }

        // Козырные ли карты в бонусе
        public bool IsTrump
        {
            get;
            private set;
        }

        // Карты содержащиеся в бонусе
        public BaseCardList Cards
        {
            get;
            private set;
        }
    }
}
