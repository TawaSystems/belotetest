using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeloteServer
{
    // Класс, представляющий каждую конкретную взятку
    class Bribe
    {
        public Bribe()
        {
            Player1 = null;
            Player2 = null;
            Player3 = null;
            Player4 = null;
        }

        public Card Player1
        {
            get;
            set;
        }

        public Card Player2
        {
            get;
            set;
        }

        public Card Player3
        {
            get;
            set;
        }

        public Card Player4
        {
            get;
            set;
        }

        public bool IsEnded
        {
            get
            {
                return ((Player1 != null) && (Player2 != null) && (Player3 != null) && (Player4 != null));
            }
        }
    }
}
