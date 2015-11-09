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
        public ClientBot(int Place, Table Table)
        {
            this.ActivePlace = Place;
            this.ActiveTable = Table;
        }

        public override void SendMessage(string message)
        {
            return;
        }

        public override int ID
        {
            get
            {
                return -ActivePlace;
            }
        }
    }
}
