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
        // Подключение клиента 
        private TcpClient client;
        // Поток, обрабатывающий запросы пользователя
        private Thread worker;
        // Ссылка на игровой объект
        private Game game;
        // Ссылка на объект игрока
        private Player player;

        // Конструктор с инициализацией всех объектов
        public Client(TcpClient tcpClient, Game game)
        {
            client = tcpClient;
            this.game = game;
            player = null;
            worker = new Thread(Process);
            worker.Start();
        }
   
        // Функция обработки запросов клиента, выполняется в потоке worker
        private void Process()
        {
            NetworkStream stream = null;
            try
            {
                stream = client.GetStream();
                byte[] data = new byte[64];
                while (true)
                {
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

        // Функция обработки полученных от клиента комманд
        private string ProcessCommand(string message)
        {
            string command = Helpers.CommandFromStr(message);
            if (command == null)
            {
                return null;
            }
            string msg = Helpers.MessageFromStr(message);
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

        // Функция обработки авторизации пользователя - регистрация, вход, напоминание паролей, выход
        private string ProcessAutorization(string command, string message)
        {
            Dictionary<string, string> regParams = Helpers.SplitCommandString(message);
            if (regParams == null)
            {
                return null;
            }
            switch (command[1])
            {
                // Регистрация
                case 'R':
                    {
                        switch (command[2])
                        {
                            // Регистрация с помощью электронной почты
                            case 'E':
                                {
                                    if (game.Autorization.RegistrationEmail(regParams["Nickname"], regParams["Email"], regParams["Password"],
                                        regParams["Country"], (regParams["Sex"] == "1") ? true : false))
                                    {
                                        // Регистрация прошла успешно
                                        player = new Player(this.game);
                                        player.Profile.Email = regParams["Email"];
                                        player.Profile.Nickname = regParams["Nickname"];
                                        player.Profile.Country = regParams["Country"];
                                        player.Profile.Sex = (regParams["Sex"] == "1");
                                        return "ARERegistration=1";
                                    }
                                    else
                                    {
                                        // Ошибка в регистрации
                                        return "ARERegistration=0";
                                    }
                                }
                            // Регистрация с помощью телефона
                            case 'P':
                                {
                                    break;
                                }
                            // Регистрация с помощью ВКонтакте
                            case 'V':
                                {
                                    break;
                                }
                            // Регистрация с помощью Facebook
                            case 'F':
                                {
                                    break;
                                }
                            // Регистрация с помощью Одноклассников
                            case 'O':
                                {
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }
                        break;
                    }
                // Авторизация
                case 'A':
                    {
                        switch (command[2])
                        {
                            // Авторизация с помощью электронной почты
                            case 'E':
                                {
                                    if (game.Autorization.EnterEmail(regParams["Email"], regParams["Password"]))
                                    {
                                        player = new Player(this.game);
                                        player.ReadPlayerFromDataBase("Email", regParams["Email"]);
                                        return "AAEAutorization=1";
                                    }
                                    else
                                    {
                                        return "AAEAutorization=0";
                                    }
                                }
                            // Авторизация с помощью телефона
                            case 'P':
                                {
                                    break;
                                }
                            // Авторизация с помощью ВКонтакте
                            case 'V':
                                {
                                    break;
                                }
                            // Регистрация с помощью Facebook
                            case 'F':
                                {
                                    break;
                                }
                            // Регистрация с помощью Одноклассников
                            case 'O':
                                {
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }
                        break;
                    }
                // Напоминание пароля
                case 'M':
                    {
                        switch (command[2])
                        {
                            // Напоминание на почту
                            case 'E':
                                {
                                    string pass = game.Autorization.RemindPasswordEmail(regParams["Email"]);
                                    if (pass != null)
                                    {
                                        return "AMERemind=1";
                                    }
                                    else
                                    {
                                        return "AMERemind=0";
                                    }
                                }
                            // Напоминание на телефон
                            case 'P':
                                {
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }
                        break;
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
                                        return "ATEExists=1";
                                    }
                                    else
                                    {
                                        return "ATEExists=0";
                                    }
                                }
                            // Ник
                            case 'N':
                                {
                                    if (game.Autorization.NicknameExists(regParams["Nickname"]))
                                    {
                                        return "ATNExists=1";
                                    }
                                    else
                                    {
                                        return "ATNExists=0";
                                    }
                                }
                            // Телефон
                            case 'P':
                                {
                                    break;
                                }
                            // Вконтакте
                            case 'V':
                                {
                                    break;
                                }
                            // Facebook
                            case 'F':
                                {
                                    break;
                                }
                            // Одноклассники
                            case 'O':
                                {
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }
                        break;
                    }
                // Выход
                case 'E':
                    {
                        player = null;
                        return null;
                    }
                default:
                    {
                        break;
                    }
            }
            return null;
        }

    }
}
