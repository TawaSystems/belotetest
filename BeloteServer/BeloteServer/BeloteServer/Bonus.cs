﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeloteServer
{
    class Bonus
    {
        // Метод создания бонуса - ему присваиваются тип, младшая карта, а также опционно для бонусов типа "последовательность" - масть
        public Bonus(BonusType Type, CardType LowCard, bool IsTrump, CardSuit Suit = CardSuit.C_NONE)
        {
            this.Type = Type;
            this.LowCard = LowCard;
            this.Suit = Suit;
            this.IsTrump = IsTrump;
            Cost = CalculateCost();
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
                        switch (LowCard)
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
            // Если длина строки не равна четырем - то это никакой и не бонус
            if (BonusString.Length != 4)
            {
                Type = BonusType.BONUS_NONE;
                LowCard = CardType.C_UNDEFINED;
                Suit = CardSuit.C_NONE;
                IsTrump = false;
            }    
            else
            {
                // Считываем из строки значения всех необходимых параметров бонуса
                Type = (BonusType)Int32.Parse(BonusString.Substring(0, 1));
                LowCard = (CardType)Int32.Parse(BonusString.Substring(1, 1));
                Suit = Helpers.StringToSuit(BonusString.Substring(2, 1));
                IsTrump = Helpers.StringToBool(BonusString.Substring(3, 1));
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

        // Бонус преобразуется к строке следующим образом: Тип + Младшая карта + Масть + Козырь
        public override string ToString()
        {
            return ((int)Type).ToString() + ((int)LowCard).ToString() + Helpers.SuitToString(Suit) + Helpers.BoolToString(IsTrump);
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
    }
}
