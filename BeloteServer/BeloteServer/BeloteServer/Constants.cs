using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeloteServer
{
    public static class Constants
    {
        // Количество столбцов в таблице Players
        public const int COLS_PLAYERS = 22;

        public const int COLS_STATISTICS = 20;
#if DEBUG
        public const int SERVER_PORT = 8888;

        public const string SERVER_LOCAL_IP = "127.0.0.1";
#else
#endif

#if DEBUG
        public const string EMAIL_ADDRESS = "tawasystems@gmail.com";
        public const string EMAIL_NAME = "BLOT-ONLINE";
        public const string EMAIL_PASSWORD = "password";
        public const string EMAIL_SMPT = "smtp.gmail.com";
        public const int EMAIL_PORT = 587;
        public const int EMAIL_TIMEOUT = 500;
#else
#endif
        public const int GAME_MINIMAL_BET = 10;

        public const int GAME_END_COUNT = 151;
    }
}
