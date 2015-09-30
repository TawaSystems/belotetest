using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeloteServer
{
    class Autorization
    {
        private Game game;

        public Autorization(Game game)
        {
            this.game = game;
        }

        public bool Enter(string Email, string Password)
        {
            int Count = Int32.Parse(game.DataBase.SelectScalar(String.Format("SELECT Count(*) FROM Players WHERE Email = {0}", Email)));
            if (Count == 0)
                return false;
            string dbPassword = game.DataBase.SelectScalar(String.Format("SELECT Password FROM Players WHERE Emaul = {0}", Email));
            if (dbPassword == Password)
                return true;
            else
                return false;
        }
    }
}
