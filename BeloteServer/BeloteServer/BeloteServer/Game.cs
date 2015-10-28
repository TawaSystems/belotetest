using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeloteServer
{
    // Игровой объект, объединяющий в себе все основные различные части сервера и игры
    class Game
    {
        public Game()
        {
            DataBase = new Database();
            Autorization = new Autorization(this);
            Tables = new TablesList(this);
            Server = new Server(this);
            Server.Start();
        }

        // Ссылка на БД
        public Database DataBase
        {
            get;
            private set;
        }      

        // Ссылка на объект авторизации
        public Autorization Autorization
        {
            get;
            private set;
        }

        // Ссылка на список игровых столов 
        public TablesList Tables
        {
            get;
            private set;
        }

        // Ссылка на объект сервера
        public Server Server
        {
            get;
            private set;
        }
    }
}
