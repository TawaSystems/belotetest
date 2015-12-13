using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeloteClient
{
    public class BonusesTypes
    {
        private List<BonusType>[] playersBonuses;

        public BonusesTypes()
        {
            playersBonuses = new List<BonusType>[4];
            for (var i = 0; i < 4; i++)
                playersBonuses[i] = new List<BonusType>();
        }

        public void AddBonus(int Place, BonusType bType)
        {
            playersBonuses[Place - 1].Add(bType);
        }

        public void Clear()
        {
            for (var i = 0; i < 4; i++)
                playersBonuses[i].Clear();
            Winner = BeloteTeam.TEAM_NONE;
            Scores = 0;
        }

        public void SetWinner(BeloteTeam winner, int size)
        {
            Winner = winner;
            Scores = size;
        }

        public List<BonusType> this[int Place]
        {
            get
            {
                return playersBonuses[Place - 1];
            }
        }

        public BeloteTeam Winner
        {
            get;
            private set;
        }

        public int Scores
        {
            get;
            private set;
        }
    }
}
