using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeloteClient
{
    public class PlayersActions : BaseActionsType
    {
        public PlayersActions(ServerConnection connection) : base(connection)
        {
        }

        // Получение информации об игроке
        public Player GetPlayer(int PlayerID)
        {
            if (PlayerID < 0)
                return null;
            Message playerMessage = new Message(Messages.MESSAGE_PLAYER_GET_INFORMATION,
                    String.Format("PlayerID={0}", PlayerID));
            MessageResult pParams = ServerConnection.ExecuteMessage(playerMessage);
            if (pParams != null)
            {
                return new Player(pParams);
            }
            else
            {
                return null;
            }
        }
    }
}
