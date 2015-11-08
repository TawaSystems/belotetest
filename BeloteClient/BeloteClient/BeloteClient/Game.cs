using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;
using System.Threading;

namespace BeloteClient
{
    public class Game
    {
        private MainGuestForm guestForm;
        private MainUserForm userForm;
        private WaitingForm waitingForm;

        public Game()
        {
            Dispatcher = Dispatcher.CurrentDispatcher;
            AppDomain.CurrentDomain.ProcessExit += ProcessExit;
            try
            {
                ServerConnection = new ServerConnection(this);
                guestForm = new MainGuestForm(this);
                guestForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Environment.Exit(0);
            }
        }

        // Событие при выходе из приложения
        public void ProcessExit(Object Sender, EventArgs e)
        {
            if (ServerConnection != null)
                ServerConnection.Disconnect();
        }

        // Регистрация с помощью электронной почты
        public void RegistrationEmail(string Email, string Password, string Nickname, string Sex, string Country)
        {
            Message regMessage = new Message(Messages.MESSAGE_AUTORIZATION_REGISTRATION_EMAIL,
                String.Format("Nickname={0},Email={1},Password={2},Country={3},Sex={4}", Nickname, Email, Password, Country, Sex));
            Dictionary<string, string> res = ServerConnection.ExecuteMessage(regMessage);
            if (res["Registration"] == "1")
            {
                MessageBox.Show("Регистрация прошла успешно!");
            }
            else
            {
                MessageBox.Show("Регистрация не удалась");
            }
        }

        // Авторизация с помощью электронной почты
        public void AutorizationEmail(string Email, string Password)
        {
            Message autMessage = new Message(Messages.MESSAGE_AUTORIZATION_AUTORIZATION_EMAIL,
                String.Format("Email={0},Password={1}", Email, Password));
            Dictionary<string, string> res = ServerConnection.ExecuteMessage(autMessage);
            int ID = Int32.Parse(res["PlayerID"]);
            if (ID != -1)
            {
                MessageBox.Show("Вход успешен!");
                //Player = new Player(this, ID);
                //ServerConnection.SendDataToServer(String.Format("{0}PlayerID={1}", Messages.MESSAGE_PLAYER_GET_INFORMATION, ID));
                guestForm.Close();
                guestForm = null;
                userForm = new MainUserForm(this);
                userForm.Show();
            }
            else
            {
                MessageBox.Show("Не удалось войти");
            }
        }
        
        public ServerConnection ServerConnection
        {
            get;
            private set;
        }

        public Player Player
        {
            get;
            private set;
        }

        public Dispatcher Dispatcher
        {
            get;
            private set;
        }
    }
}
