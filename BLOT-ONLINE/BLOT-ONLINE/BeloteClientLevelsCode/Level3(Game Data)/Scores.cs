using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeloteClient
{
    public class Scores
    {
        private int team1Score;
        private int team2Score;

        public Scores()
        {
            team1Score = 0;
            team2Score = 0;
        }

        public int this[BeloteTeam Team]
        {
            get
            {
                if (Team == BeloteTeam.TEAM1_1_3)
                    return team1Score;
                if (Team == BeloteTeam.TEAM2_2_4)
                    return team2Score;
                return 0;
            }
            set
            {
                if (Team == BeloteTeam.TEAM1_1_3)
                    team1Score = value;
                if (Team == BeloteTeam.TEAM2_2_4)
                    team2Score = value;
            }
        }
    }
}
