using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeloteClient
{
    public class GameData
    {
        public GameData()
        {
            GameStatus = TableStatus.BAZAR;
            TotalScores = new Scores();
            LocalScores = new Scores();
            LastDistributionScores = new Scores();
            Orders = new Orders();
            Bribes = new Bribes();
            AnnouncedBonuses = new BonusesTypes();
            AllCards = new CardList();
            PossibleCards = new CardList();
            IsMakingMove = false;
        }

        public void ChangeGameStatus(TableStatus newStatus)
        {
            GameStatus = newStatus;
        }

        public void NewDistribution(CardList Cards, int Score1, int Score2)
        {
            AllCards = Cards;
            GameStatus = TableStatus.BAZAR;
            LocalScores[BeloteTeam.TEAM1_1_3] = 0;
            LocalScores[BeloteTeam.TEAM2_2_4] = 0;
            LastDistributionScores[BeloteTeam.TEAM1_1_3] = Score1 - TotalScores[BeloteTeam.TEAM1_1_3];
            LastDistributionScores[BeloteTeam.TEAM2_2_4] = Score2 - TotalScores[BeloteTeam.TEAM2_2_4];
            TotalScores[BeloteTeam.TEAM1_1_3] = Score1;
            TotalScores[BeloteTeam.TEAM2_2_4] = Score2;
            Orders.RenewPlayersOrders();
            IsMakingMove = false;
            Bribes.NewDistribution();
        }

        // Все карты в наличии
        public CardList AllCards
        {
            get;
            private set;
        }

        // Возможные к ходу карты
        public CardList PossibleCards
        {
            get;
            set;
        }

        // Статус игры
        public TableStatus GameStatus
        {
            get;
            private set;
        }

        // Общий игровой счет
        public Scores TotalScores
        {
            get;
            private set;
        }

        // Локальный счет внутри раздачи
        public Scores LocalScores
        {
            get;
            private set;
        }

        // Счет в последней раздачи
        public Scores LastDistributionScores
        {
            get;
            private set;
        }

        // Заказы
        public Orders Orders
        {
            get;
            private set;
        }

        // Бонусы игрока
        public BonusList Bonuses
        {
            get;
            set;
        }

        // Все оглашенные бонусы
        public BonusesTypes AnnouncedBonuses
        {
            get;
            set;
        }

        // Совершает ли игрок ход в данный момент
        public bool IsMakingMove
        {
            get;
            set;
        }

        // Взятки за раздачу
        public Bribes Bribes
        {
            get;
            private set;
        }
    }
}
