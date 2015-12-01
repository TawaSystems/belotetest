using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeloteClient
{
    public class ServerActions
    {
        private ServerConnection ServerConnection;
       
        public ServerActions()
        {
            try
            {
                ServerConnection = new ServerConnection();
                Test = new TestActions(ServerConnection);
                Authorization = new AuthorizationActions(ServerConnection);
                Players = new PlayersActions(ServerConnection);
                Tables = new TablesActions(ServerConnection);
                Game = new GameActions(ServerConnection);
                Handlers = new MessageHandlers(ServerConnection);
            }
            catch (Exception Ex)
            {
                throw new BeloteClientException("Не удалось создать объект подключения к серверу", Ex);
            }
        }

        public void Disconnect()
        {
            ServerConnection.Disconnect();
        }

        public TestActions Test
        {
            get;
            private set;
        }

        public AuthorizationActions Authorization
        {
            get;
            private set;
        }

        public PlayersActions Players
        {
            get;
            private set;
        }

        public TablesActions Tables
        {
            get;
            private set;
        }

        public GameActions Game
        {
            get;
            private set;
        }

        public MessageHandlers Handlers
        {
            get;
            private set;
        }
    }
}
