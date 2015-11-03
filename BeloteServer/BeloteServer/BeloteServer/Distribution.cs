using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeloteServer
{
    // Раздача
    class Distribution
    {
        private List<Bribe> bribes;

        public Distribution()
        {
            Player1Cards = new CardList();
            Player2Cards = new CardList();
            Player3Cards = new CardList();
            Player4Cards = new CardList();
            Orders = new OrdersList();
            bribes = new List<Bribe>();
            ScoresTeam1 = 0;
            ScoresTeam2 = 0;
            IsCapotEnded = false;
            Status = DistributionStatus.D_BAZAR;
        }

        // Заполняем бонусы из имеющихся карт
        public void FillBonuses()
        {
            Player1Bonuses = new BonusList(Player1Cards);
            Player2Bonuses = new BonusList(Player2Cards);
            Player3Bonuses = new BonusList(Player3Cards);
            Player4Bonuses = new BonusList(Player4Cards);
        }

        // Завершение процесса торговли
        public void EndBazar()
        {
            // Установка козыря для всех игроков
            Player1Cards.SetTrump(Orders.Current.Trump);
            Player2Cards.SetTrump(Orders.Current.Trump);
            Player3Cards.SetTrump(Orders.Current.Trump);
            Player4Cards.SetTrump(Orders.Current.Trump);
            // Проверка карт на наличие бонуса Belote
            Player1Cards.FindBelote();
            Player2Cards.FindBelote();
            Player3Cards.FindBelote();
            Player4Cards.FindBelote();
        }

        // Рассчет очков, после завершения раздачи
        public void CalculateScores()
        {

        }

        // Добавление в список новой взятки
        public void AddNewBribe()
        {
            bribes.Add(new Bribe());
        }

        // Карты каждого из четырех игроков
        public CardList Player1Cards
        {
            get;
            private set;
        }

        public CardList Player2Cards
        {
            get;
            private set;
        }

        public CardList Player3Cards
        {
            get;
            private set;
        }

        public CardList Player4Cards
        {
            get;
            private set;
        }

        // Списки бонусов каждого из четырех игроков
        public BonusList Player1Bonuses
        {
            get;
            private set;
        }

        public BonusList Player2Bonuses
        {
            get;
            private set;
        }

        public BonusList Player3Bonuses
        {
            get;
            private set;
        }

        public BonusList Player4Bonuses
        {
            get;
            private set;
        }

        // Текущая взятка
        public Bribe CurrentBribe
        {
            get
            {
                if (BribesCount == 0)
                    return null;
                return bribes[BribesCount - 1];
            }
        }

        // Количество разыгранных взяток
        public int BribesCount
        {
            get
            {
                return bribes.Count;
            }
        }
        // Список заказов
        public OrdersList Orders
        {
            get;
            private set;
        }

        // Статус раздачи
        public DistributionStatus Status
        {
            get;
            private set;
        }

        // Очки первой и второй команды
        public int ScoresTeam1
        {
            get;
            private set;
        }

        public int ScoresTeam2
        {
            get;
            private set;
        }

        // Раздача завершена капутом
        public bool IsCapotEnded
        {
            get;
            private set;
        }
    }
}
