using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeloteServer
{
    class Game
    {
        public Game()
        {
            DataBase = new Database();
            Autorization = new Autorization(this);
            Tables = new TablesList(this);
            Start();
        }

        private void Start()
        {
            Server = new Server(this);
            Server.Start();
        }

        public Database DataBase
        {
            get;
            private set;
        }      

        public Autorization Autorization
        {
            get;
            private set;
        }

        public TablesList Tables
        {
            get;
            private set;
        }

        public Server Server
        {
            get;
            private set;
        }
    }
}
