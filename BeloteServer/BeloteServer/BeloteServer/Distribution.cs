using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeloteServer
{
    // Тип - игровая команда
    public enum BeloteTeam
    {
        TEAM_NONE = 0,
        TEAM1_1_3 = 1,
        TEAM2_2_4 = 2
    }

    // Тип - статус раздачи: торговля, игра, завершена
    public enum DistributionStatus
    {
        D_BAZAR = 1,
        D_GAME = 2,
        D_ENDED = 3
    }

    // Раздача
    class Distribution
    {
        public Distribution()
        {
            Player1Cards = new CardList();
            Player2Cards = new CardList();
            Player3Cards = new CardList();
            Player4Cards = new CardList();
            Trump = CardSuit.C_NONE;
            ScoresTeam1 = 0;
            ScoresTeam2 = 0;
            OrderedTeam = BeloteTeam.TEAM_NONE;
            IsCoinche = false;
            IsSurcoinche = false;
            IsCapot = false;
            IsCapotEnded = false;
            Status = DistributionStatus.D_BAZAR;
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

        // Козырь
        public CardSuit Trump
        {
            get;
            private set;
        }

        // Команда, сделавшая заказ
        public BeloteTeam OrderedTeam
        {
            get;
            private set;
        }

        public bool IsCoinche
        {
            get;
            private set;
        }

        public bool IsSurcoinche
        {
            get;
            private set;
        }

        public bool IsCapot
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
