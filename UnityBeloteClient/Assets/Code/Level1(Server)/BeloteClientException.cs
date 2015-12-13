using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeloteClient
{
    class BeloteClientException : Exception
    {
        // Исключение
        public BeloteClientException(string Message) : base (Message)
        {
        }

        public BeloteClientException(string Message, Exception InnerException) : base(Message, InnerException)
        {
        }
    }
}
