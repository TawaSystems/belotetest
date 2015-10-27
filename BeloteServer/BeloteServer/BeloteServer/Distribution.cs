﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeloteServer
{
    public enum BeloteTeam
    {
        TEAM_NONE = 0,
        TEAM1_1_3 = 1,
        TEAM2_2_4 = 2
    }

    public enum DistributionStatus
    {
        D_BAZAR = 1,
        D_GAME = 2,
        D_ENDED = 3
    }

    class Distribution
    {
        private CardList p1, p2, p3, p4;
        private CardSuit trump;
        private int scores1;
        private int scores2;
        private BeloteTeam orderedTeam;
        private bool coinche, surcoinche, capot;
        private DistributionStatus status;

        public Distribution()
        {
            p1 = new CardList();
            p2 = new CardList();
            p3 = new CardList();
            p4 = new CardList();
            trump = CardSuit.C_NONE;
            scores1 = 0;
            scores2 = 0;
            orderedTeam = BeloteTeam.TEAM_NONE;
            coinche = false;
            surcoinche = false;
            capot = false;
            status = DistributionStatus.D_BAZAR;
        }

        public CardList Player1Cards
        {
            get
            {
                return p1;
            }
        }

        public CardList Player2Cards
        {
            get
            {
                return p2;
            }
        }

        public CardList Player3Cards
        {
            get
            {
                return p3;
            }
        }

        public CardList Player4Cards
        {
            get
            {
                return p4;
            }
        }

        public CardSuit Trump
        {
            get
            {
                return trump;
            }
        }

        public BeloteTeam OrderedTeam
        {
            get
            {
                return orderedTeam;
            }
        }

        public bool IsCoinche
        {
            get
            {
                return coinche;
            }
        }

        public bool IsSurcoinche
        {
            get
            {
                return surcoinche;
            }
        }

        public bool IsCapot
        {
            get
            {
                return capot;
            }
        }

        public DistributionStatus Status
        {
            get
            {
                return status;
            }
        }

        public int ScoresTeam1
        {
            get
            {
                return scores1;
            }
        }

        public int ScoresTeam2
        {
            get
            {
                return scores2;
            }
        }
    }
}