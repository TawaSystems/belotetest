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
                StartWithGuest();
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

        // Запускаем приложение с окна гостя
        public void StartWithGuest()
        {
            guestForm = new MainGuestForm(this);
            guestForm.Show();
        }

        // Регистрация с помощью электронной почты
        public void RegistrationEmail(string Email, string Password, string Nickname, string Sex, string Country)
        {
           //ServerConnection.SendDataToServer(String.Format("{0}Nickname={1},Email={2},Password={3},Country={4},Sex={5}",
                //Messages.MESSAGE_AUTORIZATION_REGISTRATION_EMAIL, Nickname, Email, Password, Country, Sex));
        }

        // Авторизация с помощью электронной почты
        public void AutorizationEmail(string Email, string Password)
        {
            //ServerConnection.SendDataToServer(String.Format("{0}Email={1},Password={2}", Messages.MESSAGE_AUTORIZATION_AUTORIZATION_EMAIL,
              //  Email, Password));
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
