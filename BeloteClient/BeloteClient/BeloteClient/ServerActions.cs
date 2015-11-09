﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeloteClient
{
    public class ServerActions
    {
        private ServerConnection ServerConnection;

        public ServerActions()
        {
            ServerConnection = new ServerConnection();
        }

        public void Disconnect()
        {
            ServerConnection.Disconnect();
        }

        // Регистрация с помощью электронной почты
        public bool RegistrationEmail(string Email, string Password, string Nickname, string Sex, string Country)
        {
            Message regMessage = new Message(Messages.MESSAGE_AUTORIZATION_REGISTRATION_EMAIL,
                String.Format("Nickname={0},Email={1},Password={2},Country={3},Sex={4}", Nickname, Email, Password, Country, Sex));
            Dictionary<string, string> res = ServerConnection.ExecuteMessage(regMessage);
            return (res["Registration"] == "1");
        }

        // Авторизация с помощью электронной почты
        public bool AutorizationEmail(string Email, string Password, out int PlayerID)
        {
            Message autMessage = new Message(Messages.MESSAGE_AUTORIZATION_AUTORIZATION_EMAIL,
                String.Format("Email={0},Password={1}", Email, Password));
            Dictionary<string, string> res = ServerConnection.ExecuteMessage(autMessage);
            PlayerID = Int32.Parse(res["PlayerID"]);
            return (PlayerID != -1);
        }

        // Получение информации об игроке
        public Player GetPlayer(int PlayerID)
        {
            if (PlayerID < 0)
                return null;
            Message playerMessage = new Message(Messages.MESSAGE_PLAYER_GET_INFORMATION,
                    String.Format("PlayerID={0}", PlayerID));
            Dictionary<string, string> player = ServerConnection.ExecuteMessage(playerMessage);
            return new Player(player);
        }

        // Выборка всех столов, для этого должны быть созданы обработчики событий
        public TablesList GetAllPossibleTables()
        {
            string resultTables = ServerConnection.ExecuteMessageGetMessage(new Message(Messages.MESSAGE_TABLE_SELECT_ALL, "")).Msg;
            if (resultTables == "")
            {
                return null;
            }
            TablesList tablesList = new TablesList();
            try
            {
                string[] tables = resultTables.Split('|');
                foreach (string s in tables)
                {
                    tablesList.AddTable(new Table(Helpers.SplitCommandString(s)));
                }
                return tablesList;
            }
            catch
            {
                return null;
            }
        }

        // Создание игрового стола
        public Table CreateTable(int Creator, int Bet, bool PlayersVisibility, bool Chat, int MinimalLevel, bool TableVisibility,
            bool VIPOnly, bool Moderation, bool AI)
        {
            Table result = new Table(-1, Creator, Bet, PlayersVisibility, Chat, MinimalLevel, TableVisibility,
                VIPOnly, Moderation, AI);
            result.Player2 = -1;
            result.Player3 = -1;
            result.Player4 = -1;
            Dictionary<string, string> tParams = ServerConnection.ExecuteMessage(new Message(Messages.MESSAGE_TABLE_MODIFY_CREATE,
                String.Format("Bet={0},PlayersVisibility={1},Chat={2},MinimalLevel={3},TableVisibility={4},VIPOnly={5},Moderation={6},AI={7}",
                Bet, Helpers.BoolToString(PlayersVisibility), Helpers.BoolToString(Chat),
                MinimalLevel, Helpers.BoolToString(TableVisibility), Helpers.BoolToString(VIPOnly),
                Helpers.BoolToString(Moderation), Helpers.BoolToString(AI))));
            int ID = Int32.Parse(tParams["ID"]);
            if (ID != -1)
            {
                result.ChangeID(ID);
                return result;
            }
            else
            {
                return null;
            }
        }

        // Добавление обработчика сообщения
        public void AddMessageHandler(string Command, MessageDelegate Handler)
        {
            ServerConnection.AddMessageHandler(Command, Handler);
        }

        // Удаление обработчика сообщения
        public void DeleteMessageHandler(string Command, MessageDelegate Handler)
        {
            ServerConnection.DeleteMessageHandler(Command, Handler);
        }
    }
}
