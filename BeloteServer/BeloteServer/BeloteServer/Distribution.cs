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
            BonusesWinner = BeloteTeam.TEAM_NONE;
            Status = DistributionStatus.D_BAZAR;
        }

        // Сравнивает два бонуса
        private Bonus CompareBonuses(Bonus Bonus1, Bonus Bonus2)
        {
            if ((Bonus1 == null) && (Bonus2 == null))
                return null;
            if (Bonus1 == null)
                return Bonus2;
            if (Bonus2 == null)
                return Bonus1;
            if (Bonus1.CompareTo(Bonus2) > 0)
            {
                return Bonus1;
            }
            else
            if (Bonus1.CompareTo(Bonus2) < 0)
            {
                return Bonus2;
            }
            else
                return null;
        }

        // Определяем команду, бонусы которой оказались старше
        public BeloteTeam FindBonusesWinner()
        {
            Bonus SeniorBonusTeam1 = null;
            Bonus SeniorBonusTeam2 = null;
            if ((Player1Bonuses.Count != 0) || (Player3Bonuses.Count != 0))
            {
                SeniorBonusTeam1 = CompareBonuses(Player1Bonuses.SeniorBonus, Player3Bonuses.SeniorBonus);
                if (SeniorBonusTeam1 == null)
                    SeniorBonusTeam1 = Player1Bonuses.SeniorBonus;
            }
            if ((Player2Bonuses.Count != 0) || (Player4Bonuses.Count != 0))
            {
                SeniorBonusTeam2 = CompareBonuses(Player2Bonuses.SeniorBonus, Player4Bonuses.SeniorBonus);
                if (SeniorBonusTeam2 == null)
                    SeniorBonusTeam2 = Player2Bonuses.SeniorBonus;
            }
            Bonus Winner = CompareBonuses(SeniorBonusTeam1, SeniorBonusTeam2);
            if (Winner == null)
            {
                BonusesWinner = BeloteTeam.TEAM_NONE;
            }
            if (Winner == SeniorBonusTeam1)
            {
                BonusesWinner = BeloteTeam.TEAM1_1_3;
            }
            else
            {
                BonusesWinner = BeloteTeam.TEAM2_2_4;
            }

            switch (BonusesWinner)
            {
                case BeloteTeam.TEAM1_1_3:
                    {
                        ClearTeamBonuses(BeloteTeam.TEAM2_2_4);
                        break;
                    }
                case BeloteTeam.TEAM2_2_4:
                    {
                        ClearTeamBonuses(BeloteTeam.TEAM1_1_3);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            return BonusesWinner;
        }

        // Подсчитывает количество бонусных очков каждой команды
        public int BonusSummTeam(BeloteTeam Team)
        {
            switch (Team)
            {
                case BeloteTeam.TEAM1_1_3:
                    {
                        return Player1Bonuses.Cost + Player3Bonuses.Cost;
                    }
                case BeloteTeam.TEAM2_2_4:
                    {
                        return Player2Bonuses.Cost + Player4Bonuses.Cost;
                    }
                default:
                    {
                        return 0;
                    }
            }
        }

        // Очищает бонусы выбранной команды
        private void ClearTeamBonuses(BeloteTeam Team)
        {
            switch (Team)
            {
                case BeloteTeam.TEAM1_1_3:
                    {
                        Player1Bonuses.Clear();
                        Player3Bonuses.Clear();
                        break;
                    }
                case BeloteTeam.TEAM2_2_4:
                    {
                        Player2Bonuses.Clear();
                        Player4Bonuses.Clear();
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
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
            Status = DistributionStatus.D_GAME;
            // Заполняем список бонусов
            Player1Bonuses = new BonusList(Player1Cards);
            Player2Bonuses = new BonusList(Player2Cards);
            Player3Bonuses = new BonusList(Player3Cards);
            Player4Bonuses = new BonusList(Player4Cards);
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

            Status = DistributionStatus.D_ENDED;
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

        // Команда, выигравшая бонусы
        public BeloteTeam BonusesWinner
        {
            get;
            private set;
        }
    }
}
