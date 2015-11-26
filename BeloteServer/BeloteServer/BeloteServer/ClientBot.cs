using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BeloteServer
{
    // Класс клиента, отвечающий за AI
    class ClientBot : Client
    {
        public ClientBot(int Place, Table Table)
        {
            this.ActivePlace = Place;
            this.ActiveTable = Table;
        }

        // Обработка сообщений
        private void ProcessMessage(string command, string message)
        {
            Dictionary<string, string> bParams = Helpers.SplitCommandString(message);
            switch (command)
            {
                // Обработка раздачи карт
                case Messages.MESSAGE_GAME_DISTRIBUTIONCARDS:
                    {
                        string cardsStr = bParams["Cards"];
                        //int totalScore1 = Int32.Parse(bParams["Scores1"]);
                        //int totalScore2 = Int32.Parse(bParams["Scores2"]);
                        break;
                    }
                // Обработка перехода хода к данному боту
                case Messages.MESSAGE_GAME_BAZAR_NEXTBETPLAYER:
                    {
                        BetType bType = (BetType)Int32.Parse(bParams["Type"]);
                        int minSize = Int32.Parse(bParams["MinSize"]);
                        break;
                    }
                // Название ставки другого игрока
                case Messages.MESSAGE_GAME_BAZAR_SAYBET:
                    {
                        // НЕ НУЖНО
                        break;
                    }
                // Завершение процесса торговли
                case Messages.MESSAGE_GAME_BAZAR_END:
                    {
                        // НЕ НУЖНО
                        break;
                    }
                // Завершение процесса торговли без выбора козыря
                case Messages.MESSAGE_GAME_BAZAR_PASS:
                    {
                        // НЕ НУЖНО
                        break;
                    }
                // Переход хода к боту
                case Messages.MESSAGE_GAME_GAMING_NEXTPLAYER:
                    {
                        BaseCardList PossibleCards = new BaseCardList(bParams["Cards"]);
                        break;
                    }
                // Извещение о ходе картой другим игроком
                case Messages.MESSAGE_GAME_GAMING_REMINDCARD:
                    {
                        int cardPlace = Int32.Parse(bParams["Place"]);
                        Card newCard = new Card(bParams["Card"]);
                        // int LocalScore1 = Int32.Parse(bParams["Scores1"]);
                        // int LocalScore2 = Int32.Parse(bParams["Scores2"]);
                        // int beloteRemind = Int32.Parse(bParams["Belote"]);
                        break;
                    }
                // Получение списка возможных бонусов
                case Messages.MESSAGE_GAME_BONUSES_ALL:
                    {
                        BaseBonusList Bonuses = new BaseBonusList(message);
                        break;
                    }
                // Оглашение типов бонусов другими игроками
                case Messages.MESSAGE_GAME_BONUSES_TYPES:
                    {
                        // НЕ НУЖНО
                        break;
                    }
                // Оглашение победителя бонусов
                case Messages.MESSAGE_GAME_BONUSES_WINNER:
                    {
                        // НЕ НУЖНО
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        // Совершение ботом заказа
        private void MakeOrder(Order Order)
        {
            if (ActiveTable != null)
            {
                lock (ActiveTable)
                {
                    try
                    {
                        ActiveTable.AddOrder(Order);
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine(ex.Message);
#endif
                    }
                }
            }
        }

        // Оглашение ботом бонусов
        private void AnnounceBonuses(BonusList Bonuses)
        {
            if (ActiveTable != null)
            {
                lock (ActiveTable)
                {
                    try
                    {
                        ActiveTable.AnnounceBonuses(ActivePlace, Bonuses);
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine(ex.Message);
#endif
                    }
                }
            }
        }

        // Совершение ботом хода
        private void MakeMove(Card MovedCard)
        {
            if (ActiveTable != null)
            {
                lock (ActiveTable)
                {
                    try
                    {
                        ActiveTable.PlayerMove(ActivePlace, MovedCard);
                    }
                    catch (Exception Ex)
                    {
#if DEBUG
                        Debug.WriteLine(Ex.Message);
#endif
                    }
                }
            }
        }

        // Отправка сообщения клиенту-боту
        public override void SendMessage(string message)
        {
            string command = Helpers.CommandFromStr(message);
            string msg = Helpers.MessageFromStr(message);
            ProcessMessage(command, msg);
        }

        public override int ID
        {
            get
            {
                return -ActivePlace;
            }
        }
    }
}
