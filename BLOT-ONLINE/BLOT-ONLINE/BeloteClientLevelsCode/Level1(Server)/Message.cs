using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    // Результат сообщения
    public class MessageResult
    {
        private Dictionary<string, string> mParams;

        public MessageResult(Message message)
        {
            mParams = Helpers.SplitCommandString(message.Msg);
        }

        public string this[string Key]
        {
            get
            {
                return mParams[Key];
            }
        }
    }
}
