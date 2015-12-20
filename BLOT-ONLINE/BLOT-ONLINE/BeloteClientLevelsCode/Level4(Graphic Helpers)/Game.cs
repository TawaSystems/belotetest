using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace BeloteClient
{
    public class Game
    {


        //**********************************************************************************************************************************************************************************
        //                      Поля данных
        //**********************************************************************************************************************************************************************************
        private ClientInformation clientInformation;

        //**********************************************************************************************************************************************************************************
        //                      Методы инициализации и завершения
        //**********************************************************************************************************************************************************************************

        // Конструктор
        public Game()
        {
            AppDomain.CurrentDomain.ProcessExit += ProcessExit;
            try
            {
                clientInformation = new ClientInformation();
                Graphics = new GameGraphics(this);

                clientInformation.OnGameStart = StartGameHandler;
                clientInformation.OnUpdateWaitingTable = UpdateWaitingPlayers;

                if (!clientInformation.TestVersion())
                {
                    Graphics.ShowMessage("У вас устаревшая версия приложения! Скачайте новую");
                    Environment.Exit(0);
                }
                Graphics.ShowGuestScreen();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                Environment.Exit(0);
            }
        }

        // Событие при выходе из приложения
        public void ProcessExit(Object Sender, EventArgs e)
        {
            if (clientInformation != null)
                clientInformation.Disconnect();
        }

        //**********************************************************************************************************************************************************************************
        //                      Действия выполняемые пользователем
        //**********************************************************************************************************************************************************************************

        // Регистрация с помощью электронной почты
        public void RegistrationEmail(string Email, string Password, string Nickname, string Sex, string Country)
        {
            if (clientInformation.RegistrationEmail(Email, Password, Nickname, Sex, Country))
            {
                Graphics.ShowMessage("Регистрация прошла успешно!");
            }
            else
            {
                Graphics.ShowMessage("Регистрация не удалась");
            }
        }

        // Авторизация с помощью электронной почты
		public bool AutorizationEmail(string Email, string Password)
        {
			return clientInformation.AutorizationEmail (Email, Password);
            /*if (clientInformation.AutorizationEmail(Email, Password))
            {
                Graphics.CloseGuestScreen();
                Graphics.ShowUserScreen();
                Graphics.UpdateTablesList();
            }
            else
            {
                Graphics.ShowMessage("Не удалось войти");
            }*/
        }

        // Создание тренировочного стола
        public void CreateTrainingTable()
        {
            if (clientInformation.CreateTrainingTable())
            {
                Graphics.CloseUserScreen();
            }
            else
            {
                Graphics.ShowMessage("Не удалось создать игровой стол");
                Graphics.UpdateTablesList();
            }
        }

        // Создание игрового стола
        public void CreateTable(int Bet, bool PlayersVisibility, bool Chat, int MinimalLevel, bool TableVisibility,
            bool VIPOnly, bool Moderation, bool AI)
        {
            if (clientInformation.CreateTable(Bet, PlayersVisibility, Chat, MinimalLevel, TableVisibility,
                VIPOnly, Moderation, AI))
            {
                Graphics.CloseUserScreen();
                Graphics.ShowWaitingPlayersScreen();
                Graphics.UpdateWaitingPlayers();
            }
            else
            {
                Graphics.ShowMessage("Не удалось создать игровой стол");
                Graphics.UpdateTablesList();
            }
        }

        // Посадка на игровой стол
        public void EnterTheTable(int PlayerPlace, int TableID)
        {
            if (clientInformation.EnterTheTable(PlayerPlace, TableID))
            {
                Graphics.CloseUserScreen();
                Graphics.ShowWaitingPlayersScreen();
                Graphics.UpdateWaitingPlayers();
            }
            else
            {
                Graphics.ShowMessage("Не удалось сесть на игровой стол");
                Graphics.UpdateTablesList();
            }
        }

        public void ExitFromWaitingTable()
        {
            clientInformation.QuitTable();
            Graphics.CloseWaitingPlayersScreen();
            Graphics.ShowUserScreen();
            Graphics.UpdateWaitingPlayers();
        }

        // Добавление бота на стол
        public void AddBot(int BotPlace)
        {
            if (clientInformation.AddBot(BotPlace))
            {
                Graphics.UpdateWaitingPlayers();
            }
            else
            {
                Graphics.ShowMessage("Не удалось добавить бота на стол!");
            }
        }

        // Удаление бота с игрового стола
        public void DeleteBot(int BotPlace)
        {
            clientInformation.DeleteBot(BotPlace);
        }

        // Игрок совершает ход. Параметр - индекс карты в списке всех карт
        public void MakeMove(int CardIndex)
        {
            clientInformation.MakeMove(CardIndex);
            Graphics.UpdateGameGraphics();
        }

        // Выход игрока со стола во время игры
        public void QuitTable()
        {
            clientInformation.QuitTable(); 
        }

        // Сделать заказ
        public void MakeOrder(Order order)
        {
            clientInformation.MakeOrder(order);
        }

        //**********************************************************************************************************************************************************************************
        //                      Обработчики доигровых событий
        //**********************************************************************************************************************************************************************************

        // Обработка начала игры
        public void StartGameHandler()
        {
            Graphics.CloseWaitingPlayersScreen();
            clientInformation.OnUpdateGraphics = UpdateGraphics;
            clientInformation.OnBazarMakingBet = BazarNextPlayerHandler;
            clientInformation.OnAnnounceBonuses = PlayerShowBonuses;
            clientInformation.OnShowBonusesWinner = BonusesShowWinnerHandler;
            clientInformation.OnGameEnd = GameEndHandler;
            clientInformation.OnPlayerQuit = PlayerQuitHandler;
            Graphics.ShowGameScreen();
        }

        // Событие обновления списка игроков
        public void UpdateWaitingPlayers()
        {
            Graphics.UpdateWaitingPlayers();
        }

        // Событие обновления графики
        public void UpdateGraphics()
        {
            Graphics.UpdateGameGraphics();
        }

        // Событие обновления торговли
        public void BazarNextPlayerHandler()
        {
            Graphics.ShowBetScreen(clientInformation.GameData.Orders.PossibleBetSize, clientInformation.GameData.Orders.PossibleBetType);
        }

        // Событие показа бонусов
        public void PlayerShowBonuses()
        {
            Graphics.ShowChooseBonusesScreen();
        }

        // Объявление победителя по бонусам
        public void BonusesShowWinnerHandler()
        {
            if (clientInformation.GameData.AnnouncedBonuses.Winner != BeloteTeam.TEAM_NONE)
            {
                Graphics.ShowMessage(String.Format("Команда №{0} получает {1} очков за бонусы", (int)clientInformation.GameData.AnnouncedBonuses.Winner, clientInformation.GameData.AnnouncedBonuses.Scores));
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
            clientInformation.OnGameEnd = null;
            clientInformation.OnPlayerQuit = null;
            int TotalScore1 = clientInformation.GameData.TotalScores[BeloteTeam.TEAM1_1_3];
            int TotalScore2 = clientInformation.GameData.TotalScores[BeloteTeam.TEAM2_2_4];
            int Place = clientInformation.Place;
            if (TotalScore1 > TotalScore2)
            {
                if ((Place == 1) || (Place == 3))
                    Graphics.ShowMessage(String.Format("Вы победили! Счет - {0} : {1}", TotalScore1, TotalScore2));
                else
                    Graphics.ShowMessage(String.Format("Вы проиграли! Счет - {0} : {1}", TotalScore1, TotalScore2));
            }
            else
            {
                if ((Place == 2) || (Place == 4))
                    Graphics.ShowMessage(String.Format("Вы победили! Счет - {0} : {1}", TotalScore1, TotalScore2));
                else
                    Graphics.ShowMessage(String.Format("Вы проиграли! Счет - {0} : {1}", TotalScore1, TotalScore2));
            }
            Graphics.CloseGameScreen();
            Graphics.ShowUserScreen();
            Graphics.UpdateTablesList();
        }

        // Выход игрока со стола во время игры
        public void PlayerQuitHandler()
        {
            Graphics.ShowMessage("Игра завершена. Кто-то вышел со стола: " + clientInformation.CurrentPlayer.Profile.Name);
            Graphics.CloseGameScreen();
            Graphics.ShowUserScreen();
        }

        //**********************************************************************************************************************************************************************************
        //                      Открытые свойства
        //**********************************************************************************************************************************************************************************

        public ClientInformation Information
        {
            get
            {
                return clientInformation;
            }
        }

        public GameGraphics Graphics
        {
            get;
            private set;
        }
    }
}
