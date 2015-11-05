using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeloteClient
{
    class Game
    {
        public Game()
        {
            try
            {
                ServerConnection = new ServerConnection(this);
                Player = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Environment.Exit(0);    
            } 

        }

        public ServerConnection ServerConnection
        {
            get;
            private set;
        }

        public Player Player
        {
            get;
            private set;
        }
    }
}
