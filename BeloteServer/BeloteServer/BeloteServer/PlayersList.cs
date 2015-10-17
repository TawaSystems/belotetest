using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeloteServer
{
    class PlayersList
    {
        private Game game;

        private List<Player> players;

        public PlayersList(Game Game)
        {
            this.game = Game;
            players = new List<Player>();
        }

        public int AddPlayer(Player player)
        {
            players.Add(player);
            return player.Profile.Id;
        }

    }
}
