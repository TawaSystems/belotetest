using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

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
#if DEBUG
            Debug.WriteLine(DateTime.Now.ToString() + " Добавление игрока в список. ID = " + player.Profile.Id);
#endif
            players.Add(player);
            return player.Profile.Id;
        }

        public Player this[int ID]
        {
            get
            {
                return players.Find(p => (p.Profile.Id == ID));
            }
        }

        public void DeletePlayer(Player player)
        {
            if (player != null)
            {
#if DEBUG
                Debug.WriteLine(DateTime.Now.ToString() + " Удаление игрока из списка. ID = " + player.Profile.Id);
#endif
                players.Remove(player);
            }
        }

        public void DeletePlayer(int ID)
        {
            DeletePlayer(this[ID]);
        }
    }
}
