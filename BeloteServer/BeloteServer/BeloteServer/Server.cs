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

        public Server(Game game)
        {
            this.Game = game;
            Clients = new ClientsList(game);
        }

        // Метод запуска сервера по приему сооединений от клиентов
        public void Start()
        {
#if DEBUG
            Debug.WriteLine(DateTime.Now.ToString() + " Запуск обработчика клиентов");
#endif
            try
            {
                listener = new TcpListener(IPAddress.Parse(Constants.SERVER_LOCAL_IP), Constants.SERVER_PORT);
                listener.Start();

                // Принимаем клиентов и запускаем их обработчики
                while (true)
                {
                    TcpClient client = listener.AcceptTcpClient();
                    Clients.Add(new ClientMan(client, Game));
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

        // Отправляет указанное сообщение неограниченному количеству клиентов
        public void SendMessageToClients(String Message, params ClientMan[] clients)
        {
#if DEBUG
            Debug.WriteLine(DateTime.Now.ToString() + " Отправка сообщения нескольким клиентам.");
            Debug.Indent();
            Debug.WriteLine("Сообщение: " + Message);
#endif
            foreach (ClientMan c in clients)
            {
                if (c != null)
                {
#if DEBUG
                    Debug.WriteLine("Отправка клиенту с ID: " + c.ID);
#endif
                    c.SendMessage(Message);
                }
            }
#if DEBUG
            Debug.Unindent();
#endif
        }

        public Game Game
        {
            get;
            private set;
        }

        // Список всех подключенных клиентов
        public ClientsList Clients
        {
            get;
            private set;
        }
    }
}
