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
        public Card c1, c2, c3, c4;
        public Bribe()
        {
            c1 = null;
            c2 = null;
            c3 = null;
            c4 = null;
        }

        public Card Player1
        {
            get
            {
                return c1;
            }
            set
            {
                c1 = value;
            }
        }

        public Card Player2
        {
            get
            {
                return c2;
            }
            set
            {
                c2 = value;
            }
        }

        public Card Player3
        {
            get
            {
                return c3;
            }
            set
            {
                c3 = value;
            }
        }

        public Card Player4
        {
            get
            {
                return c4;
            }
            set
            {
                c4 = value;
            }
        }

        public bool IsEnded
        {
            get
            {
                return ((c1 != null) && (c2 != null) && (c3 != null) && (c4 != null));
            }
        }
    }
}
