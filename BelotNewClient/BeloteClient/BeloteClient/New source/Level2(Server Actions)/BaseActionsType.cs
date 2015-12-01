using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeloteClient
{
    // Базовый класс типа действий с сервером
    public class BaseActionsType
    {
        protected ServerConnection ServerConnection;

        public BaseActionsType(ServerConnection connection)
        {
            ServerConnection = connection;
        }
    }
}
