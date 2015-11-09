using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Windows.Threading;

namespace BeloteClient
{
    // Тип - сообщение
    public class Message
    {
        public Message(string messageStr)
        {
            Msg = Helpers.MessageFromStr(messageStr);
            Command = Helpers.CommandFromStr(messageStr);
        }

        public Message(string command, string msg)
        {
            this.Command = command;
            this.Msg = msg;
        }

        public string Msg
        {
            get;
            private set;
        }

        public string Command
        {
            get;
            private set;
        }
    }

    // Делегат - обработчик сообщений
    public delegate void MessageDelegate(Message msg);

    public class ServerConnection
    {
        // Объект подключение к серверу
        private TcpClient client;
        // Поток - обработчик данных от сервера
        private Thread serverWorker;
        // Поток обработчик сообщений
        private Thread messagesWorker;
        // Поток данных сервера
        private NetworkStream stream;
        // Список с необработанными сообщениями
        private List<Message> messagesList;
        // Список с обработчиками сообщений
        private Dictionary<string, List<MessageDelegate>> messageHandlers;
        // Диспетчер для главного потока
        private Dispatcher dispatcher;
         
        // Конструктор - создание всех объектов
        public ServerConnection()
        {
            if (!Connect())
            {
                throw new Exception("Не удалось подключиться к серверу");
            }
            dispatcher = Dispatcher.CurrentDispatcher;
            stream = client.GetStream();
            messagesList = new List<Message>();
            messageHandlers = new Dictionary<string, List<MessageDelegate>>();
            serverWorker = new Thread(ProcessServer);
            serverWorker.Start();
            messagesWorker = new Thread(ProcessMessages);
            messagesWorker.Start();
        }

        // Попытка соединения с сервером
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

        // Посылка сообщения на сервер
        private void SendDataToServer(string message)
        {
            byte[] data = Encoding.Unicode.GetBytes(message);
            stream.Write(data, 0, data.Length);
        }

        // Обработка сообщений от сервера
        private void ProcessServer()
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

                    lock (messagesList)
                    {
                        messagesList.Add(new Message(builder.ToString()));
                    }
                }
                catch (Exception ex)
                {
                    Disconnect();
                }
            }
        }

        // Обработка сообщений
        private void ProcessMessages()
        {
            while (true)
            {
                lock (messagesList)
                {
                    foreach (Message msg in messagesList)
                    {
                        List<MessageDelegate> msgHandlers;
                        if (messageHandlers.TryGetValue(msg.Command, out msgHandlers))
                        {
                            if (msgHandlers.Count > 0)
                            {
                                messagesList.Remove(msg);
                                foreach (MessageDelegate md in msgHandlers)
                                {
                                    dispatcher.BeginInvoke(new Action(() => md(msg)));
                                }
                            }
                        }
                    }
                }
                Thread.Sleep(100);
            }
        }

        // Отключение от сервера
        public void Disconnect()
        {
            try
            {
                SendDataToServer("EXT");
            }
            finally
            {
                serverWorker.Abort();
                client.Close();
            }
        }

        // Добавление обработчика сообщения
        public void AddMessageHandler(string Command, MessageDelegate Handler)
        {
            List<MessageDelegate> msgHandlers;
            if (!messageHandlers.TryGetValue(Command, out msgHandlers))
            {
                msgHandlers = new List<MessageDelegate>();
                messageHandlers.Add(Command, msgHandlers);
            }
            msgHandlers.Add(Handler);
        }

        // Удаление обработчика сообщения
        public void DeleteMessageHandler(string Command, MessageDelegate Handler)
        {
            List<MessageDelegate> msgHandlers;
            if (messageHandlers.TryGetValue(Command, out msgHandlers))
            {
                msgHandlers.Remove(Handler);
                if (msgHandlers.Count == 0)
                {
                    messageHandlers.Remove(Command);
                }
            }
        }

        // Выполнение команды на сервере и получение результата 
        public Dictionary<string, string> ExecuteMessage(Message msg)
        {
            SendDataToServer(msg.Command + msg.Msg);
            Message result = null;
            Thread worker = new Thread(delegate ()
            {
                while (true)
                {
                    lock (messagesList)
                    {
                        result = messagesList.Find(m => m.Command == msg.Command);
                        if (result != null)
                        {
                            messagesList.Remove(result);
                            break;
                        }
                    }
                    Thread.Sleep(20);
                }
            });
            worker.Start();
            worker.Join();
            return Helpers.SplitCommandString(result.Msg);
        }

        // Выполняет сообщение без ожидания результата
        public void ExecuteMessageWithoutResult(Message msg)
        {
            SendDataToServer(msg.Command + msg.Msg);
        }
    }
}
