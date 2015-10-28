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

        // Создание списка бонусов из списка карт
        public BonusList(CardList cards)
        {
            list = new List<Bonus>();
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
                    list.Add(new Bonus(BonusType.BONUS_4X, cards[i].Type));
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
                bonusType = BonusType.BONUS_100;
            list.Add(new Bonus(bonusType, cards[Position - Streak].Type, cards[Position - Streak].Suit));
        }

        // Создание списка бонусов из строки
        public BonusList(string cards) : this(new CardList(cards))
        {
        }

        // Удаление бонуса из списка
        public void Delete(Bonus bonus)
        {
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
    }
}
