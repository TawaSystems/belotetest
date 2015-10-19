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
        private TablesList tablesList;
        private Server server;

        public Game()
        {
            db = new Database();
            autorization = new Autorization(this);
            tablesList = new TablesList(this);
            Start();
        }

        private void Start()
        {
            server = new Server(this);
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

        public TablesList Tables
        {
            get
            {
                return tablesList;
            }
        }

        public Server Server
        {
            get
            {
                return server;
            }
        }
    }
}
