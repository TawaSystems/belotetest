using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BeloteServer
{
    class BaseBonusList
    {
        protected List<Bonus> list;

        public BaseBonusList(CardList cards)
        {
#if DEBUG
            Debug.WriteLine("{0} Создание списка бонусов из списка карт: {1}", DateTime.Now, cards.ToString());
#endif
        }

        // Создание списка бонусов из строки
        public BaseBonusList(string bonusString)
        {
#if DEBUG
            Debug.WriteLine("{0} Создание списка бонусов из бонусной строки: {1}", DateTime.Now, bonusString);
#endif
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
    }
}
