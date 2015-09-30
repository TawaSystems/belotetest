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

        private string ProcessAutorization(string command, string message)
        {
            switch (command[1])
            {
                // Регистрация
                case 'R':
                    {
                        break;
                    }
                // Авторизация
                case 'A':
                    {
                        break;
                    }
                // Напоминание пароля
                case 'M':
                    {
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
                                    break;
                                }
                            // Ник
                            case 'N':
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
            switch (command[0])
            {
                // Autorization
                case 'A':
                    {
                        return ProcessAutorization(command, message);
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
