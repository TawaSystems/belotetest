using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace BeloteClient
{
    public class BonusList
    {
        private List<Bonus> list;

        // Создание списка бонусов из строки
        public BonusList(string bonusString)
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
                        list.Add(new Bonus(bonuses["Bonus" + i.ToString()]));
                    }
                }
            }
        }

        // Добавляет бонус в список
        public void AddBonus(Bonus bonus)
        {
            list.Add(bonus);
        }

        // Проверка бонуса на наличие в списке
        public bool ExistsBonus(Bonus bonus)
        {
            foreach (Bonus b in list)
            {
                if ((b.Type == bonus.Type) && (b.Suit == bonus.Suit) && (b.HighCard == bonus.HighCard))
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
    }
}
