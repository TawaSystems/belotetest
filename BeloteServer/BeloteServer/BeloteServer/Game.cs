using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeloteServer
{
    class Game
    {
        private Database db;
        public Game()
        {
            db = new Database();
            Start();
        }
          
        public Database DataBase
        {
            get
            {
                return db;
            }
        }

        private void Start()
        {

        }

              
    }
}
