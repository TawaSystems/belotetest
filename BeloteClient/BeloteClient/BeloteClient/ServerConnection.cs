﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using System.Net.Sockets;

namespace BeloteClient
{
    public class ServerConnection
    {
        private TcpClient client;
        private Thread worker;
        private NetworkStream stream;
        private GameClient game;

        public ServerConnection(GameClient Game)
        {
            if (!Connect())
            {
                throw new Exception("Не удалось подключиться к серверу");
            }
            game = Game;
            stream = client.GetStream();
            worker = new Thread(ProcessClient);
            worker.Start();
        }

        private bool Connect()
        {
            try
            {
                client = new TcpClient(Constants.SERVER_LOCAL_IP, Constants.SERVER_PORT);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void SendDataToServer(string message)
        {
            byte[] data = Encoding.Unicode.GetBytes(message);
            stream.Write(data, 0, data.Length);
        }

        private void ProcessAutorization(string command, string message)
        {
            Dictionary<string, string> regParams = Helpers.SplitCommandString(message);
            switch (command[1])
            {
                case 'R':
                    {
                        //if (regParams["Registration"] == "1")
                            //MessageBox.Show("Регистрация прошла успешно");
                        //else
                           // MessageBox.Show("В регистрации отказано");
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        private void ProcessCommand(string message)
        {
            string command = Helpers.CommandFromStr(message);
            string msg = Helpers.MessageFromStr(message);

            switch (command)
            {
                // Обработка команд авторизации
                case Messages.MESSAGE_AUTORIZATION_AUTORIZATION_EMAIL:
                case Messages.MESSAGE_AUTORIZATION_AUTORIZATION_FB:
                case Messages.MESSAGE_AUTORIZATION_AUTORIZATION_OK:
                case Messages.MESSAGE_AUTORIZATION_AUTORIZATION_PHONE:
                case Messages.MESSAGE_AUTORIZATION_AUTORIZATION_VK:
                case Messages.MESSAGE_AUTORIZATION_REGISTRATION_EMAIL:
                case Messages.MESSAGE_AUTORIZATION_REGISTRATION_FB:
                case Messages.MESSAGE_AUTORIZATION_REGISTRATION_OK:
                case Messages.MESSAGE_AUTORIZATION_REGISTRATION_PHONE:
                case Messages.MESSAGE_AUTORIZATION_REGISTRATION_VK:
                case Messages.MESSAGE_AUTORIZATION_REMINDPASSWORD_EMAIL:
                case Messages.MESSAGE_AUTORIZATION_REMINDPASSWORD_PHONE:
                case Messages.MESSAGE_AUTORIZATION_TEST_EMAIL:
                case Messages.MESSAGE_AUTORIZATION_TEST_FB:
                case Messages.MESSAGE_AUTORIZATION_TEST_NICKNAME:
                case Messages.MESSAGE_AUTORIZATION_TEST_OK:
                case Messages.MESSAGE_AUTORIZATION_TEST_PHONE:
                case Messages.MESSAGE_AUTORIZATION_TEST_VK:
                case Messages.MESSAGE_AUTORIZATION_USER_EXIT:
                    {
                        ProcessAutorization(command, msg);
                        break;
                    }
                // Обработка отключения клиента
                case Messages.MESSAGE_CLIENT_DISCONNECT:
                    {
                        break;
                    }
                // Обработка модификации стола
                case Messages.MESSAGE_TABLE_MODIFY_CREATE:
                case Messages.MESSAGE_TABLE_MODIFY_CREATORLEAVE:
                case Messages.MESSAGE_TABLE_MODIFY_VISIBILITY:
                case Messages.MESSAGE_TABLE_PLAYERS_ADD:
                case Messages.MESSAGE_TABLE_PLAYERS_DELETE:
                case Messages.MESSAGE_TABLE_PLAYERS_QUIT:
                case Messages.MESSAGE_TABLE_SELECT_TABLES:
                    {
                        break;
                    }
                // Обработка процесса игры
                case Messages.MESSAGE_GAME_BAZAR_BET:
                case Messages.MESSAGE_GAME_BAZAR_END:
                case Messages.MESSAGE_GAME_BAZAR_NEXTBETPLAYER:
                case Messages.MESSAGE_GAME_BAZAR_SAYBET:
                case Messages.MESSAGE_GAME_BONUSES_ALL:
                case Messages.MESSAGE_GAME_BONUSES_ANNOUNCE:
                case Messages.MESSAGE_GAME_BONUSES_TYPES:
                case Messages.MESSAGE_GAME_BONUSES_WINNER:
                case Messages.MESSAGE_GAME_DISTRIBUTIONCARDS:
                case Messages.MESSAGE_GAME_END:
                case Messages.MESSAGE_GAME_GAMING_NEXTPLAYER:
                case Messages.MESSAGE_GAME_GAMING_PLAYERMOVE:
                case Messages.MESSAGE_GAME_GAMING_REMINDCARD:
                case Messages.MESSAGE_GAME_START:
                    {
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        private void ProcessClient()
        {
            while (true)
            {
                try
                {
                    byte[] data = new byte[64];
                    StringBuilder builder = new StringBuilder();

                    do
                    {
                        int bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    ProcessCommand(builder.ToString());
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                }
            }
        }
    }
}