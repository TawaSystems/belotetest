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

        private List<ClientMan> clients;

        public ClientsList(Game Game)
        {
            this.game = Game;
            clients = new List<ClientMan>();
        }

        // Добавление клиента в список
        public int Add(ClientMan client)
        {
#if DEBUG
            Debug.WriteLine(DateTime.Now.ToString() + " Добавление клиента в список. ID = " + client.ID);
#endif
            clients.Add(client);
            return client.ID;
        }

        // Обращение-индексатор к списку клиентов. Обращение происходит по идентификатору пользователя
        public ClientMan this[int ID]
        {
            get
            {
                return clients.Find(p => (p.ID == ID));
            }
        }

        // Удаление клиента из списка по ссылке на клиента
        public void DeleteClient(ClientMan client)
        {
            if (client != null)
            {
#if DEBUG
                Debug.WriteLine(DateTime.Now.ToString() + " Удаление клиента из списка. ID = " + client.ID);
#endif
                clients.Remove(client);
            }
        }

        // Удаление клиента из списка по идентификатору игрока
        public void DeleteClient(int ID)
        {
            DeleteClient(this[ID]);
        }

        // Количество подключенных клиентов
        public int Count
        {
            get
            {
                return clients.Count;
            }
        }
    }
}
