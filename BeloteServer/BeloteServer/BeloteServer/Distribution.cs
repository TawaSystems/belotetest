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
            // Если не разыграны все 8 карт, то расчитывать нечего
            if (bribes.Count != 8)
                return;
            // Суммы очков команд
            int s1 = 0;
            int s2 = 0;
            // Суммы бонусов команд
            int b1 = 0;
            int b2 = 0;
            // Количества взятых командами взяток
            int c1 = 0;
            int c2 = 0;
            // Подсчитываем набранное количество очков командами
            foreach (Bribe b in bribes)
            {
                if (b.WinningTeam == BeloteTeam.TEAM1_1_3)
                {
                    s1 += b.BribeCost;
                    c1++;
                }
                else
                {
                    s2 += b.BribeCost;
                    c2++;
                }
            }
            // Прибавляем 10 бонусных очков за последнюю раздачу
            if (bribes[bribes.Count - 1].WinningTeam == BeloteTeam.TEAM1_1_3)
                s1 += 10;
            else
                s2 += 10;
            // Определяем команду победителя по бонусам

            // Подсчитыаем бонусы каждой из команд, в том числе и Блот

            // Определяем команду, делавшую заказ

            // Если команда выполнила свой заказ
            {
                // Рассмотрим случай контры
                {
                    // Рассмотрим случай реконтры
                    {

                    }
                }
                // Рассмотрим случай объявленного Капута
                {

                }
                // Рассмотрим случай обычного выигрыша
                {

                }

            }
            // Если команда не выполнила свой заказ
            {
                // Рассмотрим случай Контры
                {
                    // Рассмотрим случай реконты
                    {

                    }
                }
                // Рассмотрим случай обычного поражения
                {
                    // Рассмотрим случай, когда вторая команда набрала Капут
                    {

                    }
                }
            }
            // Присвоение Score1 , Score2
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
