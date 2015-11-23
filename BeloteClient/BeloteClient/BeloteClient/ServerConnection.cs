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
        }

        // Попытка соединения с сервером
        private bool Connect()
        {
            try
            {
                client = new TcpClient(Constants.SERVER_LOCAL_IP, Constants.SERVER_PORT);
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Посылка сообщения на сервер
        private void SendDataToServer(string message)
        {
            try
            {
                byte[] data = Encoding.Unicode.GetBytes(message + Constants.MESSAGE_DELIMITER);
                stream.Write(data, 0, data.Length);
                stream.Flush();
            }
            catch
            {
                MessageBox.Show("Сервер отключился!");
                Environment.Exit(0);
            }
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
                    // Разделение полученных сообщений
                    string[] messages = builder.ToString().Split(Constants.MESSAGE_DELIMITER);
                    foreach (string str in messages)
                    {
                        if (str != "")
                        {
                            Message msg = new Message(str);
                            lock (messageHandlers)
                            {
                                if (!ProcessMessage(msg))
                                {
                                    lock (messagesList)
                                    {
                                        messagesList.Add(new Message(str));
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception Ex)
                {
                    //Disconnect();
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        // Обработка сообщения
        private bool ProcessMessage(Message Msg)
        {
            List<MessageDelegate> msgHandlers;
            if (Msg.Command == null)
                return true;
            if (messageHandlers.TryGetValue(Msg.Command, out msgHandlers))
            {
                if (msgHandlers.Count > 0)
                {
                    foreach (MessageDelegate md in msgHandlers)
                    {
                        dispatcher.BeginInvoke(new Action(() => md(Msg)));
                    }
                    return true;
                }
            }
            return false;  
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
            if (Handler == null)
                return;
            List<MessageDelegate> msgHandlers;
            lock (messageHandlers)
            {
                if (!messageHandlers.TryGetValue(Command, out msgHandlers))
                {
                    lock (messagesList)
                    {
                        if (messagesList.Count > 0)
                        {
                            for (var i = messagesList.Count - 1; i >= 0; i--)
                            {
                                Message m = messagesList[i];
                                if (m.Command == Command)
                                {
                                    dispatcher.BeginInvoke(new Action(() => Handler(m)));
                                    messagesList.Remove(m);
                                }
                            }
                        }
                    }
                    msgHandlers = new List<MessageDelegate>();
                    messageHandlers.Add(Command, msgHandlers);
                }
                msgHandlers.Add(Handler);
            }
        }

        // Удаление обработчика сообщения
        public void DeleteMessageHandler(string Command, MessageDelegate Handler)
        {
            if (Handler == null)
                return;
            List<MessageDelegate> msgHandlers;
            lock (messageHandlers)
            {
                if (messageHandlers.TryGetValue(Command, out msgHandlers))
                {
                    msgHandlers.Remove(Handler);
                    if (msgHandlers.Count == 0)
                    {
                        messageHandlers.Remove(Command);
                    }
                }
            }
        }

        // Выполнение команды на сервере и возврат результата в виде "сообщения"
        public Message ExecuteMessageGetMessage(Message msg)
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
            return result;
        }

        // Выполнение команды на сервере и получение результата в виде словаря
        public Dictionary<string, string> ExecuteMessage(Message msg)
        {
            return Helpers.SplitCommandString(ExecuteMessageGetMessage(msg).Msg);
        }

        // Выполняет сообщение без ожидания результата
        public void ExecuteMessageWithoutResult(Message msg)
        {
            SendDataToServer(msg.Command + msg.Msg);
        }
    }
}
