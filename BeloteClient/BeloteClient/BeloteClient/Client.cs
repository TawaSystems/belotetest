using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using System.Windows.Forms;

namespace BeloteClient
{
    public class Client
    {
        private TcpClient client;
        private Thread worker;
        private NetworkStream stream;

        public Client()
        {
            if (!Connect())
            {
                MessageBox.Show("Невозможно подключиться к серверу!");
                return;
            }
            stream = client.GetStream();
            worker = new Thread(ProcessClient);
            worker.Start();
        }

        private bool Connect()
        {
            try
            {
                client = new TcpClient(BeloteServer.Constants.SERVER_LOCAL_IP, BeloteServer.Constants.SERVER_PORT);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void SendDataToServer(string message)
        {
            //lock (stream)
            //{
                byte[] data = Encoding.Unicode.GetBytes(message);
                stream.Write(data, 0, data.Length);
            //}
        }

        private void ProcessAutorization(string command, string message)
        {
            Dictionary<string, string> regParams = BeloteServer.Helpers.SplitCommandString(message);
            switch (command[1])
            {
                case 'R':
                    {
                        if (regParams["Registration"] == "1")
                            MessageBox.Show("Регистрация прошла успешно");
                        else
                            MessageBox.Show("В регистрации отказано");
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
            string command = BeloteServer.Helpers.CommandFromStr(message);
            string msg = BeloteServer.Helpers.MessageFromStr(message);

            switch (command[0])
            {
                case 'A':
                    {
                        ProcessAutorization(command, msg);
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
                    MessageBox.Show(ex.Message);
                }
            }
        }



        public void Registration(string Nickname, string Password, string Email, string Country, string Sex)
        {
            string msg = String.Format("AR0Nickname={0},Password={1},Email={2},Country={3},Sex={4}", 
                Nickname, Password, Email, Country, Sex);
            SendDataToServer(msg);
        }
    }
}
