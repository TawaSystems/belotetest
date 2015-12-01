using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeloteClient
{
    class ClientInformation
    {
        private ServerActions serverActions;
        private Player player;
        private TablesList tablesList;
        private PlayersList playersList;

        public ClientInformation()
        {
            try
            {
                serverActions = new ServerActions();
            }
            catch (Exception Ex)
            {
                throw new BeloteClientException("Невозможно начать взаимодействие с сервером", Ex);
            }
        }

        // Составляет список игроков со всех доступных столов для быстрой возможности получения информации
        private void UpdatePlayers()
        {
            playersList.Clear();
            playersList.Add(player);
            if (tablesList != null)
            {
                for (var i = 0; i < tablesList.Count; i++)
                {
                    Table t = tablesList.GetTableAt(i);
                    for (var playerPlace = 1; playerPlace <= 4; playerPlace++)
                    {
                        if (t[playerPlace] >= 0)
                        {
                            if (!playersList.PlayerExists(t[playerPlace]))
                            {
                                Player p = serverActions.Players.GetPlayer(t[playerPlace]);
                                if (p != null)
                                {
                                    playersList.Add(p);
                                }
                            }
                        }
                    }
                }
            }
        }

        // Регистрация с помощью электронной почты
        public bool RegistrationEmail(string Email, string Password, string Nickname, string Sex, string Country)
        {
            return serverActions.Authorization.RegistrationEmail(Email, Password, Nickname, Sex, Country);
        }

        // Авторизация с помощью электронной почты
        public bool AutorizationEmail(string Email, string Password)
        {
            int PlayerID;
            if (serverActions.Authorization.AutorizationEmail(Email, Password, out PlayerID))
            {
                player = serverActions.Players.GetPlayer(PlayerID);
                if (player != null)
                {
                    UpdatePlayers();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        // Добавление информации о всех доступных столах и игроках с них в соответствующие списки
        public void UpdatePossibleTables()
        {
            tablesList = serverActions.Tables.GetAllPossibleTables();
            UpdatePlayers();
        }
    }
}
