using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeloteClient
{
    public class GameGraphics
    {
        //**********************************************************************************************************************************************************************************
        //                      Игровые формы
        //**********************************************************************************************************************************************************************************
        private MainGuestForm guestForm;
        private MainUserForm userForm;
        private WaitingForm waitingForm;
        private GameForm gameForm;
        private BetFormType4 betForm4;
        private BetFromType123 betForm123;
        private BonusAnnounceForm bonusesForm;

        private Game Game;

        public GameGraphics(Game Game)
        {
            this.Game = Game;
        }
        //**********************************************************************************************************************************************************************************
        //                      Методы отображения графики
        //**********************************************************************************************************************************************************************************

        // Отображение гостевого экрана
        public void ShowGuestScreen()
        {
            guestForm = new MainGuestForm(this.Game);
            guestForm.Show();
        }

        // Закрытие гостевого экрана
        public void CloseGuestScreen()
        {
            if (guestForm != null)
            {
                guestForm.Close();
                guestForm = null;
            }
        }

        // Отображение экрана регистрации
        public void ShowRegistrationEmailScreen()
        {
            RegistrationEmail regEmailForm = new RegistrationEmail(this.Game);
            regEmailForm.ShowDialog();
        }

        // Отображение экрана входа
        public void ShowAuthorizationEmailScreen()
        {
            EmailAutorizationForm form = new EmailAutorizationForm(this.Game);
            form.ShowDialog();
        }

        // Отображение экрана пользователя
        public void ShowUserScreen()
        {
            userForm = new MainUserForm(this.Game);
            userForm.Show();
        }

        // Закрытие экрана пользователя
        public void CloseUserScreen()
        {
            if (userForm != null)
            {
                userForm.Close();
                userForm = null;
            }
        }

        // Отображение экрана создание игрового стола
        public void ShowCreatingTableScreen()
        {
            CreatingTableForm creatingTable = new CreatingTableForm(this.Game);
            creatingTable.ShowDialog();
        }

        // Отображение экрана ожидания игроков
        public void ShowWaitingPlayersScreen()
        {
            waitingForm = new WaitingForm(this.Game);
            waitingForm.Show();
        }

        // Закрытие экрана отображения игроков
        public void CloseWaitingPlayersScreen()
        {
            if (waitingForm != null)
            {
                waitingForm.Close();
                waitingForm = null;
            }
        }

        // Показ игрового экрана
        public void ShowGameScreen()
        {
            gameForm = new GameForm(this.Game);
            betForm123 = new BetFromType123(this.Game);
            gameForm.Show();
        }

        // Закрытие игрового экрана
        public void CloseGameScreen()
        {
            if (betForm123 != null)
            {
                betForm123.Close();
                betForm123 = null;
            }
            if (betForm4 != null)
            {
                betForm4.Close();
                betForm4 = null;
            }
            if (bonusesForm != null)
            {
                bonusesForm.Close();
                bonusesForm = null;
            }
            if (gameForm != null)
            {
                gameForm.Close();
                gameForm = null;
            }
        }

        // Показ экрана выбора объявляемых бонусов
        public void ShowChooseBonusesScreen()
        {
            bonusesForm = new BonusAnnounceForm(this.Game);
            bonusesForm.ShowDialog();
            bonusesForm = null;
        }

        // Показ экрана совершения ставки
        public void ShowBetScreen(int BetSize, BetType BetType)
        {
            if (BetType == BetType.BET_SURCOINCHE)
            {
                betForm4 = new BetFormType4(this.Game);
                betForm4.ShowDialog();
                betForm4 = null;
            }
            else
            {
                betForm123.ShowForm(BetSize, BetType);
            }
        }

        // Отображение информационного экрана с сообщением
        public void ShowMessage(string Message)
        {
            MessageBox.Show(Message);
        }

        // Обновление списка столов в режиме выбора стола
        public void UpdateTablesList()
        {
            if (userForm != null)
                userForm.UpdateTables();
        }

        // Обновление списка ожидающих игроков
        public void UpdateWaitingPlayers()
        {
            if (waitingForm != null)
                waitingForm.UpdateLabels();
        }

        // Обновление игровго экрана
        public void UpdateGameGraphics()
        {
            if (gameForm != null)
                gameForm.UpdateGraphics();
        }
    }
}
