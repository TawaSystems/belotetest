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

        public Game()
        {
            try
            {
                Dispatcher = Dispatcher.CurrentDispatcher;
                Player = null;
                ServerConnection = new ServerConnection(this);
                StartWithGuest();    
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Environment.Exit(0);    
            } 

        }

        public void StartWithGuest()
        {
            guestForm = new MainGuestForm(this);
            guestForm.Show();
        }

        public void RegistrationEmail(string Email, string Password, string Nickname, string Sex, string Country)
        {
            ServerConnection.SendDataToServer(String.Format("{0}Nickname={1},Email={2},Password={3},Country={4},Sex={5}",
                Messages.MESSAGE_AUTORIZATION_REGISTRATION_EMAIL, Nickname, Email, Password, Country, Sex));
        }

        public void AutorizationEmail(string Email, string Password)
        {
            ServerConnection.SendDataToServer(String.Format("{0}Email={1},Password={2}", Messages.MESSAGE_AUTORIZATION_AUTORIZATION_EMAIL,
                Email, Password));
        }

        public void RegistrationResult(bool Success)
        {
            if (Success)
            {
                MessageBox.Show("Регистрация прошла успешно!");
            }
            else
            {
                MessageBox.Show("Не удалось зарегистрироваться. Попробуйте ввести другие данные");
            }
        }

        public void AutorizationResult(int ID)
        {
            if (ID != -1)
            {
                MessageBox.Show("Вход успешен!");
                Player = new Player(this, ID);
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
