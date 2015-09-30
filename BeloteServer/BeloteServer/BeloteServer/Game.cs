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

        private void Start()
        {
            Server server = new Server(this);
            server.Start();
        }

        public Database DataBase
        {
            get
            {
                return db;
            }
        }      


    }
}
