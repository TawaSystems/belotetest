using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BeloteServer
{
    class BonusList
    {
        private List<Bonus> list;

        // Создание списка бонусов из списка карт
        public BonusList(CardList cards)
        {
#if DEBUG 
            Debug.WriteLine("{0} Создание списка бонусов из списка карт: {1}", DateTime.Now, cards.ToString());
#endif
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
                if (Math.Abs(((int)cards[i].Type) - ((int)cards[i - 1].Type)) == 1)
                {
                    if (cards[i].Suit == cards[i - 1].Suit)
                    {
                        // Увеличиваем значение последовательности
                        streak++;
                    }
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
                // Даже если последовательность из 6-7-8 карт, учитываем только старшие 5
                Streak = 5;
            }
            AddBonusToList(new Bonus(bonusType, cards[Position - Streak].Type, cards[Position - Streak].IsTrump, cards[Position - Streak].Suit));
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
        public BonusList(string bonusString)
        {
#if DEBUG
            Debug.WriteLine("{0} Создание списка бонусов из бонусной строки: {1}", DateTime.Now, bonusString);
#endif
            list = new List<Bonus>();
            Dictionary<string, string> bonuses = Helpers.SplitCommandString(bonusString);
            if (bonuses.Count > 0)
            {
                int c;
                if (Int32.TryParse(bonuses["Count"], out c))
                {
                    for (var i = 0; i < c; i++)
                    {
                        list.Add(new Bonus(bonuses["Bonus" + i.ToString()]));
                    }
                }
            }
        }

        // Проверка бонуса на наличие в списке
        public bool ExistsBonus(Bonus bonus)
        {
            foreach (Bonus b in list)
            {
                if ((b.Type == bonus.Type) && (b.Suit == bonus.Suit) && (b.LowCard == bonus.LowCard))
                    return true;
            }
            return false;
        }

        // Очищение списка бонусов
        public void Clear()
        {
            list.Clear();
        }

        // Удаление бонуса из списка
        public void Delete(Bonus bonus)
        {
#if DEBUG 
            Debug.WriteLine("{0} Удаление бонуса из списка - {1}", DateTime.Now, bonus.ToString());
#endif
            list.Remove(bonus);
        }

        // Индексатор для обращения к бонусам
        public Bonus this[int Index]
        {
            get
            {
                return list[Index];
            }
        }

        // Количество бонусов в списке
        public int Count
        {
            get
            {
                return list.Count;
            }
        }

        // Создает строку, состоящую из бонусов
        public override string ToString()
        {
            string Result = "Count=" + Count.ToString();
            for (var i = 0; i < Count; i++)
            {
                Result += String.Format(",Bonus{0}={1}", i.ToString(), list[i].ToString());
            }
            return Result;
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
