using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BeloteServer
{
    class BonusList : BaseBonusList
    {
        // Создание списка бонусов из списка карт
        public BonusList(CardList cards) : base(cards)
        {
            list = new List<Bonus>();
            SeniorBonus = null;
            // Если это не полная раздача карт на одного игрока - то что-то не так, отсюда нельзя создать список бонусов
            if (cards.Count != 8)
                return;
            // Выборка всех бонусов типа "последовательность"
            int streak = 1;
            for (var i = 1; i < cards.Count; i++)
            {
                // Проверяем, что две следующие друг за другом карты последовательны и одной масти
                if ((Math.Abs(((int)cards[i].Type) - ((int)cards[i - 1].Type)) == 1) && (cards[i].Suit == cards[i - 1].Suit))
                {
                    // Увеличиваем значение последовательности
                    streak++;
                }
                else
                {
                    // Если удалось собрать последовательность из 3 и более карт одной масти, то мы имеем бонус
                    if (streak >= 3)
                    {
                        // Добавляем полученный бонус в список бонусов
                        AddStreakBonus(cards, i, streak);   
                    }
                    streak = 1;
                }
            }
            // После прохода по всем картам, может остаться неучтенной последняя имеющаяся последовательность
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
                    // Добавляем бонус. В качестве козыря берем игру с козырем или без (для расчета стоимость 4XA
                    if ((cards[i].Type != CardType.C_7) && (cards[i].Type != CardType.C_8))
                        AddBonusToList(new Bonus(BonusType.BONUS_4X, cards[i].Type, cards[i].IsGameWithTrump));
                }
            }
        }

        // Добавление бонуса типа "Последовательность" в список бонусов
        private void AddStreakBonus(CardList cards, int Position, int Streak)
        {
            BonusType bonusType;
            if (Streak == 3)
                bonusType = BonusType.BONUS_TERZ;
            else
            if (Streak == 4)
                bonusType = BonusType.BONUS_50;
            else
            {
                bonusType = BonusType.BONUS_100;
            }
            Bonus bonus = new Bonus(bonusType, cards[Position - Streak].Type, cards[Position - Streak].IsTrump, cards[Position - Streak].Suit);
            // Заполнение списка карт бонуса
            for (var i = Position - Streak; i < Position; i++)
                bonus.Cards.Add(cards[i]);
            AddBonusToList(bonus);
        }

        // Добавляет бонус в список, проверяя на старшенство
        private void AddBonusToList(Bonus bonus)
        {
            // Добавляем бонус в список
            list.Add(bonus);
            // Если это первый бонус в списке, то он и становится старшим
            if (SeniorBonus == null)
            {
                SeniorBonus = bonus;
            }
            else
            {
                // Если это не первый бонус, то сравниваем его со страшим, и заменяем, если он старше
                if (SeniorBonus.CompareTo(bonus) < 0)
                    SeniorBonus = bonus;
            }
        }

        // Создание списка бонусов из строки
        public BonusList(string bonusString) : base(bonusString)
        {
            list = new List<Bonus>();
            Dictionary<string, string> bonuses = Helpers.SplitCommandString(bonusString);
            if (bonuses.Count > 0)
            {
                int c;
                if (Int32.TryParse(bonuses["Count"], out c))
                {
                    for (var i = 0; i < c; i++)
                    {
                        AddBonusToList(new Bonus(bonuses["Bonus" + i.ToString()]));
                    }
                }
            }
        }

        // Сумма бонусов в списке
        public int Cost
        {
            get
            {
                if (Count == 0)
                    return 0;
                int s = 0;
                foreach (Bonus b in list)
                {
                    s += b.Cost;
                }
                return s;
            }
        }

        // Старший бонус в раздаче
        public Bonus SeniorBonus
        {
            get;
            private set;
        }

    }
}
