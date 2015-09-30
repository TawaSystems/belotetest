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

        public Client()
        {
            if (!Connect())
            {
                MessageBox.Show("Невозможно подключиться к серверу!");
            }
        }

        public bool Connect()
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

        public void ProcessClient(string message)
        {
            try
            {
                NetworkStream stream = client.GetStream();
                
                byte[] data = Encoding.Unicode.GetBytes(message);
                stream.Write(data, 0, data.Length);
                data = new byte[64]; 
                StringBuilder builder = new StringBuilder();

                do
                {
                    int bytes = stream.Read(data, 0, data.Length);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                }
                while (stream.DataAvailable);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                client.Close();
            }
        }
    }
}
