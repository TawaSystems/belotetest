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
        private Autorization autorization;
        public Game()
        {
            db = new Database();
            autorization = new Autorization(this);
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

        public Autorization Autorization
        {
            get
            {
                return autorization;
            }
        }
    }
}
