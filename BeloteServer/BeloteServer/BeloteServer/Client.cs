using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeloteServer
{
    // Базовый класс для наследования человеческого клента и AI
    class Client
    {
        public Client()
        {
        }

        public virtual void SendMessage(string message)
        {
            return;
        }

        public virtual int ID
        {
            get
            {
                return -1;
            }
        }

        public Table ActiveTable
        {
            get;
            set;
        }

        public int ActivePlace
        {
            get;
            set;
        }
    }
}
