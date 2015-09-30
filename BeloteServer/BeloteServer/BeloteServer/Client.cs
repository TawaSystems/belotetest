using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;

namespace BeloteServer
{
    class Client
    {
        private TcpClient client;
        private Thread worker;
        private Game game;

        public Client(TcpClient tcpClient, Game game)
        {
            client = tcpClient;
            this.game = game;
            worker = new Thread(Process);
            worker.Start();
        }

        // Разбор авторизации пользователя
        private string ProcessAutorization(string command, string message)
        {
            Dictionary<string, string> regParams = Helpers.SplitCommandString(message);
            switch (command[1])
            {
                // Регистрация
                case 'R':
                    {
                        if (game.Autorization.RegistrationEmail(regParams["Nickname"], regParams["Email"], regParams["Password"],
                            regParams["Country"], (regParams["Sex"] == "1") ? true : false))
                        {
                            // Регистрация прошла успешно
                            return "AR0Registration=1";
                        }
                        else
                        {
                            // Ошибка в регистрации
                            return "AR0Registration=0";
                        }
                    }
                // Авторизация
                case 'A':
                    {
                        if (game.Autorization.EnterEmail(regParams["Email"], regParams["Password"]))
                        {
                            return "AA0Autorization=1";
                        }
                        else
                        {
                            return "AA0Autorization=0";
                        }
                    }
                // Напоминание пароля
                case 'M':
                    {
                        string pass = game.Autorization.RemindPasswordEmail(regParams["Email"]);
                        if (pass != null)
                        {
                            return "AM0Password=" + pass;
                        }
                        else
                        {
                            return "AM0Password= ";
                        }
                    }
                // Тест на наличие эл.почты/ника и т.д.
                case 'T':
                    {
                        switch (command[2])
                        {
                            // Адрес электронной почты
                            case 'E':
                                {
                                    if (game.Autorization.EmailExists(regParams["Email"]))
                                    {
                                        return "ATEEmail=1";
                                    }
                                    else
                                    {
                                        return "ATEEmail=0";
                                    }
                                }
                            // Ник
                            case 'N':
                                {
                                    if (game.Autorization.NicknameExists(regParams["Nickname"]))
                                    {
                                        return "ATNNickname=1";
                                    }
                                    else
                                    {
                                        return "ATNNickname=0";
                                    }
                                }
                            default:
                                {
                                    break;
                                }
                        }
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            return null;
        }

        private string ProcessCommand(string message)
        {
            string command = message.Substring(0, 3);
            string msg = message.Substring(3, message.Length - 3);
            switch (command[0])
            {
                // Autorization
                case 'A':
                    {
                        return ProcessAutorization(command, msg);
                    }
                case 'B':
                    {
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            return null;
        }

        private void Process()
        {
            NetworkStream stream = null;
            try
            {
                stream = client.GetStream();
                byte[] data = new byte[64]; // буфер для получаемых данных
                while (true)
                {
                    // получаем сообщение
                    StringBuilder builder = new StringBuilder();
                    do
                    {
                        int bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    string message = builder.ToString();

                    string result = ProcessCommand(message);
                    if (result != null)
                    {
                        data = Encoding.Unicode.GetBytes(result);
                        stream.Write(data, 0, data.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (stream != null)
                    stream.Close();
                if (client != null)
                    client.Close();
            }
        }
    }
}
