using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BeloteClient
{
    public class GameGraphics
    {
        //**********************************************************************************************************************************************************************************
        //                      Игровые формы
        //**********************************************************************************************************************************************************************************

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
        }

        // Закрытие гостевого экрана
        public void CloseGuestScreen()
        {
            
        }

        // Отображение экрана регистрации
        public void ShowRegistrationEmailScreen()
        {
        }

        // Отображение экрана входа
        public void ShowAuthorizationEmailScreen()
        {
            
        }

        // Отображение экрана пользователя
        public void ShowUserScreen()
        {
          
        }

        // Закрытие экрана пользователя
        public void CloseUserScreen()
        {
            
        }

        // Отображение экрана создание игрового стола
        public void ShowCreatingTableScreen()
        {
            
        }

        // Отображение экрана ожидания игроков
        public void ShowWaitingPlayersScreen()
        {
            
        }

        // Закрытие экрана отображения игроков
        public void CloseWaitingPlayersScreen()
        {
            
        }

        // Показ игрового экрана
        public void ShowGameScreen()
        {
            
        }

        // Закрытие игрового экрана
        public void CloseGameScreen()
        {
            
        }

        // Показ экрана выбора объявляемых бонусов
        public void ShowChooseBonusesScreen()
        {
            
        }

        // Показ экрана совершения ставки
        public void ShowBetScreen(int BetSize, BetType BetType)
        {
            
        }

        // Отображение информационного экрана с сообщением
        public void ShowMessage(string Message)
        {
			
        }

        // Обновление списка столов в режиме выбора стола
        public void UpdateTablesList()
        {
            
        }

        // Обновление списка ожидающих игроков
        public void UpdateWaitingPlayers()
        {
            
        }

        // Обновление игровго экрана
        public void UpdateGameGraphics()
        {
            
        }
    }
}
