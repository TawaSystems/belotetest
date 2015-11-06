using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeloteServer
{
    // Класс клиента, отвечающий за AI
    class ClientBot : Client
    {
        private int tableID;

        public ClientBot(int Place, int TableID)
        {
            this.Place = Place;
            this.tableID = TableID;
        }

        public override void SendMessage(string message)
        {
            return;
        }

        public override int ID
        {
            get
            {
                return -Place;
            }
        }

        public int Place
        {
            get;
            private set;
        }
    }
}
