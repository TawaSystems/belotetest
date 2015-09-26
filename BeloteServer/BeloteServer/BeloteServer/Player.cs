using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeloteServer
{
    class Player
    {
        private Statistics statistics;
        private Game game;
        private Profile profile;

        public Player(Game game)
        {
            this.game = game;
            statistics = new Statistics();
            profile = new Profile();
        }

        public Game Game
        {
            get
            {
                return game;
            }
        }

        public Statistics Statistics
        {
            get
            {
                return statistics;
            }
        }

        public Profile Profile
        {
            get
            {
                return profile;
            }
        }
        
    }
}
