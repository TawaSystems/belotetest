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

        public Server(Game game)
        {
            this.game = game;
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

        public Game Game
        {
            get
            {
                return game;
            }
        } 
    }
}
