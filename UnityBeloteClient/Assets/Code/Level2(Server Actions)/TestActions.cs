using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeloteClient
{
    public class TestActions : BaseActionsType
    {
        public TestActions(ServerConnection connection) : base (connection)
        {
        }

        // Тестирование версии клиента
        public bool TestClientVersion()
        {
            MessageResult vParams = ServerConnection.ExecuteMessage(new Message(Messages.MESSAGE_CLIENT_TEST_VERSION, ""));
            return (vParams["Version"] == Constants.CLIENT_ACTUAL_VERSION);
        }
    }
}
