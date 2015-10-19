using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Diagnostics;

namespace BeloteServer
{
    class Server
    {
        private static TcpListener listener;
        private Game game;
        private ClientsList autorizedList;

        public Server(Game game)
        {
            this.game = game;
            autorizedList = new ClientsList(game);
        }

        public void Start()
        {
#if DEBUG
            Debug.WriteLine(DateTime.Now.ToString() + " Запуск обработчика клиентов");
#endif
            try
            {
                listener = new TcpListener(IPAddress.Parse(Constants.SERVER_LOCAL_IP), Constants.SERVER_PORT);
                listener.Start();

                while (true)
                {
                    TcpClient client = listener.AcceptTcpClient();
                    Client clientObject = new Client(client, Game);
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine(ex.Message);
#endif
            }
            finally
            {
                if (listener != null)
                    listener.Stop();
            }
        }

        public void SendMessageToClients(String Message, params Client[] clients)
        {
            foreach (Client c in clients)
                c.SendMessage(Message);
        }

        public Game Game
        {
            get
            {
                return game;
            }
        }

        public ClientsList Clients
        {
            get
            {
                return autorizedList;
            }
        }
    }
}
