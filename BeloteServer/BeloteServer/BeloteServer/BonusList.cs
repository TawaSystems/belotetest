using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeloteServer
{
    class BonusList
    {
        private List<Bonus> list;

        public BonusList(CardList cards)
        {
            list = new List<Bonus>();
            if (cards.Count != 8)
                return;
            // Выборка всех бонусов типа "последовательность"
            int streak = 1;
            for (var i = 1; i < cards.Count; i++)
            {
                if (Math.Abs(((int)cards[i].Type) - ((int)cards[i - 1].Type)) == 1)
                {
                    if (cards[i].Suit == cards[i - 1].Suit)
                    {
                        streak++;
                    }
                }
                else
                {
                    if (streak >= 3)
                    {
                        AddStreakBonus(cards, i, streak);   
                    }
                    streak = 1;
                }
            }
            if (streak >= 3)
            {
                AddStreakBonus(cards, 8, streak);
            }

            // Выборка всех бонусов типа 4Х
            for (var i = 0; i < 5; i++)
            {
                int c = 1;
                for (var j = i + 1; j < 8; j++)
                {
                    if (cards[j].Type == cards[i].Type)
                        c++;
                }
                if (c == 4)
                {
                    list.Add(new Bonus(BonusType.BONUS_4X, cards[i].Type));
                }
            }
        }

        private void AddStreakBonus(CardList cards, int Position, int Streak)
        {
            BonusType bonusType;
            if (Streak == 3)
                bonusType = BonusType.BONUS_TERZ;
            else
            if (Streak == 4)
                bonusType = BonusType.BONUS_50;
            else
                bonusType = BonusType.BONUS_100;
            list.Add(new Bonus(bonusType, cards[Position - Streak].Type, cards[Position - Streak].Suit));
        }

        public BonusList(string cards) : this(new CardList(cards))
        {
        }

        public int Count
        {
            get
            {
                return list.Count;
            }
        }
    }
}
