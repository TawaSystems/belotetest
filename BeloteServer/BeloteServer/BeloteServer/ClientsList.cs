using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BeloteServer
{
    class ClientsList
    {
        private Game game;

        private List<Client> clients;

        public ClientsList(Game Game)
        {
            this.game = Game;
            clients = new List<Client>();
        }

        public int Add(Client client)
        {
#if DEBUG
            Debug.WriteLine(DateTime.Now.ToString() + " Добавление клиента в список. ID = " + client.ID);
#endif
            clients.Add(client);
            return client.ID;
        }

        public Client this[int ID]
        {
            get
            {
                return clients.Find(p => (p.ID == ID));
            }
        }

        public void DeleteClient(Client client)
        {
            if (client != null)
            {
#if DEBUG
                Debug.WriteLine(DateTime.Now.ToString() + " Удаление клиента из списка. ID = " + client.ID);
#endif
                clients.Remove(client);
            }
        }

        public void DeleteClient(int ID)
        {
            DeleteClient(this[ID]);
        }
    }
}
