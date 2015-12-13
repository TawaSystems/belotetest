using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeloteClient
{
    public class AuthorizationActions : BaseActionsType
    {
        public AuthorizationActions(ServerConnection connection) : base (connection)
        {
        }

        // Регистрация с помощью электронной почты
        public bool RegistrationEmail(string Email, string Password, string Nickname, string Sex, string Country)
        {
            Message regMessage = new Message(Messages.MESSAGE_AUTORIZATION_REGISTRATION_EMAIL,
                String.Format("Nickname={0},Email={1},Password={2},Country={3},Sex={4}", Nickname, Email, Password, Country, Sex));
            MessageResult res = ServerConnection.ExecuteMessage(regMessage);
            return (res["Registration"] == "1");
        }

        // Авторизация с помощью электронной почты
        public bool AutorizationEmail(string Email, string Password, out int PlayerID)
        {
            Message autMessage = new Message(Messages.MESSAGE_AUTORIZATION_AUTORIZATION_EMAIL,
                String.Format("Email={0},Password={1}", Email, Password));
            MessageResult res = ServerConnection.ExecuteMessage(autMessage);
            PlayerID = Int32.Parse(res["PlayerID"]);
            return (PlayerID != -1);
        }
    }
}
