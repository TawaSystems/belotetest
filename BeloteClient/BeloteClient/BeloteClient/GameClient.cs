using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeloteClient
{
    public class GameClient
    {
        public GameClient()
        {
            try
            {
                ServerConnection = new ServerConnection(this);
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


    }
}
