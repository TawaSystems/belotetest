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
        private GameForm gameForm;
        private BetFormType4 betForm4;
        private BetFromType123 betForm123;

        private ClientInformation clientInformation;

        public Game()
        {
            AppDomain.CurrentDomain.ProcessExit += ProcessExit;
            try
            {
                clientInformation = new ClientInformation();
                clientInformation.OnGameStart = StartGameHandler;
                if (!clientInformation.TestVersion())
                {
                    MessageBox.Show("У вас устаревшая версия приложения! Скачайте новую");
                    Environment.Exit(0);
                }
                guestForm = new MainGuestForm(this);
                guestForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Environment.Exit(0);
            }
        }



        /// <summary>
        /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// 
        ///                             Системные методы
        /// 
        /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        // Событие при выходе из приложения
        public void ProcessExit(Object Sender, EventArgs e)
        {
            if (clientInformation != null)
                clientInformation.Disconnect();
        }

        /// <summary>
        /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// 
        ///                             Методы выполнения основных игровых событий: регистрация, авторизация, вход/создание столов, игра
        /// 
        /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        // Регистрация с помощью электронной почты
        public void RegistrationEmail(string Email, string Password, string Nickname, string Sex, string Country)
        {
            if (clientInformation.RegistrationEmail(Email, Password, Nickname, Sex, Country))
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
            if (clientInformation.AutorizationEmail(Email, Password))
            {
                guestForm.Close();
                guestForm = null;
                userForm = new MainUserForm(this);
                userForm.UpdateTables();
                userForm.Show();
            }
            else
            {
                MessageBox.Show("Не удалось войти");
            }
        }

        // Создание тренировочного стола
        public void CreateTrainingTable()
        {
            if (clientInformation.CreateTrainingTable())
            {
                if (userForm != null)
                {
                    userForm.Close();
                    userForm = null;
                }
            }
            else
            {
                MessageBox.Show("Не удалось создать игровой стол");
            }
        }

        public void CreateTable(int Bet, bool PlayersVisibility, bool Chat, int MinimalLevel, bool TableVisibility,
            bool VIPOnly, bool Moderation, bool AI)
        {
            if (clientInformation.CreateTable(Bet, PlayersVisibility, Chat, MinimalLevel, TableVisibility,
                VIPOnly, Moderation, AI))
            {
                if (userForm != null)
                {
                    userForm.Close();
                    userForm = null;
                }
                waitingForm = new WaitingForm(this);
                waitingForm.UpdateLabels();
                waitingForm.Show();
            }
            else
            {
                MessageBox.Show("Не удалось создать игровой стол");
            }
        }

        // Посадка на игровой стол
        public void EnterTheTable(int PlayerPlace, int TableID)
        {
            if (clientInformation.EnterTheTable(PlayerPlace, TableID))
            {
                if (userForm != null)
                {
                    userForm.Close();
                    userForm = null;
                }
                waitingForm = new WaitingForm(this);
                waitingForm.UpdateLabels();
                waitingForm.Show();
            }
            else
            {
                MessageBox.Show("Не удалось сесть на игровой стол");
                userForm.UpdateTables();
            }
        }

        public void ExitFromWaitingTable()
        {
            clientInformation.QuitTable();
            waitingForm.Close();
            waitingForm = null;
            userForm = new MainUserForm(this);
            userForm.UpdateTables();
            userForm.Show();
        }

        // Добавление бота на стол
        public void AddBot(int BotPlace)
        {
            if (clientInformation.AddBot(BotPlace))
            {
                if (waitingForm != null)
                    waitingForm.UpdateLabels();
            }
            else
            {
                MessageBox.Show("Не удалось добавить бота на стол!");
            }
        }

        // Удаление бота с игрового стола
        public void DeleteBot(int BotPlace)
        {
            clientInformation.DeleteBot(BotPlace);
            //if (waitingForm != null)
            //   waitingForm.UpdateLabels();
        }

        // Игрок совершает ход. Параметр - индекс карты в списке всех карт
        public void MakeMove(int CardIndex)
        {
            clientInformation.MakeMove(CardIndex);
            gameForm.UpdateGraphics();
        }

        // Выход игрока со стола во время игры
        public void QuitTable()
        {
            clientInformation.QuitTable();
            if (betForm123 != null)
                betForm123.Close();
            if (betForm4 != null)
                betForm4.Close();
            MessageBox.Show("Игра завершена. Кто-то вышел со стола");
            gameForm.Close();
            gameForm = null;
            userForm = new MainUserForm(this);
            userForm.UpdateTables();
            userForm.Show();
        }

        // Сделать заказ
        public void MakeOrder(Order order)
        {
            clientInformation.MakeOrder(order);
        }

        // Обработка начала игры
        public void StartGameHandler()
        {
            if (waitingForm != null)
            {
                waitingForm.Close();
                waitingForm = null;
            }
            clientInformation.OnUpdateGraphics = UpdateGraphics;
            clientInformation.OnBazarMakingBet = BazarNextPlayerHandler;
            clientInformation.OnAnnounceBonuses = PlayerShowBonuses;
            clientInformation.OnShowBonusesWinner = BonusesShowWinnerHandler;
            clientInformation.OnPlayerQuit = PlayerQuitHandler;
            clientInformation.OnGameEnd = GameEndHandler;
            gameForm = new GameForm(this);
            betForm123 = new BetFromType123(this);
            gameForm.Show();
        }

        // Событие обновления графики
        public void UpdateGraphics()
        {
            if (gameForm != null)
                gameForm.UpdateGraphics();
        }

        // Событие обновления торговли
        public void BazarNextPlayerHandler()
        {
            try
            {
                if (clientInformation.GameData.Orders.PossibleBetType == BetType.BET_SURCOINCHE)
                {
                    betForm4 = new BetFormType4(this);
                    betForm4.ShowDialog();
                    betForm4 = null;
                }
                else
                {
                    betForm123.ShowForm(clientInformation.GameData.Orders.PossibleBetSize, clientInformation.GameData.Orders.PossibleBetType);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Событие показа бонусов
        public void PlayerShowBonuses()
        {
            // Показываем форму
            BonusAnnounceForm form = new BonusAnnounceForm(this);
            form.ShowDialog();
        }



        // Объявление победителя по бонусам
        public void BonusesShowWinnerHandler()
        {
            if (clientInformation.GameData.AnnouncedBonuses.Winner != BeloteTeam.TEAM_NONE)
            {
                MessageBox.Show(String.Format("Команда №{0} получает {1} очков за бонусы", (int)clientInformation.GameData.AnnouncedBonuses.Winner, clientInformation.GameData.AnnouncedBonuses.Scores));
            }
            else
            {
                //MessageBox.Show("Ни одна команда не получает очки за бонусы");
            }
        }
        
        // Обработчик события завершения игры
        public void GameEndHandler()
        {
            clientInformation.OnUpdateGraphics = null;
            clientInformation.OnBazarMakingBet = null;
            clientInformation.OnAnnounceBonuses = null;
            clientInformation.OnShowBonusesWinner = null;
            clientInformation.OnPlayerQuit = null;
            clientInformation.OnGameEnd = null;
            int TotalScore1 = clientInformation.GameData.TotalScores[BeloteTeam.TEAM1_1_3];
            int TotalScore2 = clientInformation.GameData.TotalScores[BeloteTeam.TEAM2_2_4];
            int Place = clientInformation.Place;
            if (TotalScore1 > TotalScore2)
            {
                if ((Place == 1) || (Place == 3))
                    MessageBox.Show(String.Format("Вы победили! Счет - {0} : {1}", TotalScore1, TotalScore2));
                else
                    MessageBox.Show(String.Format("Вы проиграли! Счет - {0} : {1}", TotalScore1, TotalScore2));
            }
            else
            {
                if ((Place == 2) || (Place == 4))
                    MessageBox.Show(String.Format("Вы победили! Счет - {0} : {1}", TotalScore1, TotalScore2));
                else
                    MessageBox.Show(String.Format("Вы проиграли! Счет - {0} : {1}", TotalScore1, TotalScore2));
            }
            
            gameForm.Close();
            gameForm = null;
            userForm = new MainUserForm(this);
            userForm.UpdateTables();
            userForm.Show();
        }

        // Выход игрока со стола во время игры
        public void PlayerQuitHandler()
        {
            MessageBox.Show("Игра завершена. Кто-то вышел со стола");
            if (betForm123 != null)
                betForm123.Close();
            if (betForm4 != null)
                betForm4.Close();
            gameForm.Close();
            gameForm = null;
            userForm = new MainUserForm(this);
            userForm.UpdateTables();
            userForm.Show();
        }
        /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// 
        ///                             Свойства
        /// 
        /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>

        public ClientInformation Information
        {
            get
            {
                return clientInformation;
            }
        }
    }
}
