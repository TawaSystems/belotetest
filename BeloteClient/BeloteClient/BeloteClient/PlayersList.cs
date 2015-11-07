using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeloteClient
{
    public class PlayersList
    {
        private List<Player> list;
        private Game game;

        public PlayersList(Game Game)
        {
            this.game = Game;
            list = new List<Player>();
        }

        public int Count
        {
            get
            {
                return list.Count;
            }
        }

        public Player this[int ID]
        {
            get
            {
                return list.Find(p => p.Profile.Id == ID);
            }
        }

        public bool PlayerExists(int ID)
        {
            Player p = this[ID];
            return (p != null);
        }

        public void Add(Player Player)
        {
            list.Add(Player);
        }

        public void Delete(Player Player)
        {
            list.Remove(Player);
        }
    }
}
