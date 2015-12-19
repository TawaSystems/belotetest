using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeloteClient
{
    public static class Constants
    {
        // Разделитель сообщений
        public const char MESSAGE_DELIMITER = '#';

        // Количество столбцов в таблице Players
        public const int COLS_PLAYERS = 22;

        // Количество столбцов в таблице Statistics
        public const int COLS_STATISTICS = 20;

        public const int SERVER_PORT = 8888;

        //public const string SERVER_LOCAL_IP = "127.0.0.1";
        public const string SERVER_LOCAL_IP = "188.227.18.8";

        public const string CLIENT_ACTUAL_VERSION = "1.0.0.3";

#if DEBUG
        public const string EMAIL_ADDRESS = "tawasystems@gmail.com";
        public const string EMAIL_NAME = "BLOT-ONLINE";
        public const string EMAIL_PASSWORD = "password";
        public const string EMAIL_SMPT = "smtp.gmail.com";
        public const int EMAIL_PORT = 587;
        public const int EMAIL_TIMEOUT = 500;
#else
#endif

#if DEBUG
        // Входные данные для БД
        public const string DATABASE_SERVER = "localhost";
        public const string DATABASE_NAME = "Belote";
        public const string DATABASE_USER = "root";
        public const string DATABASE_PASSWORD = "";
#endif
        // Размер минимальной ставки на столе
        public const int GAME_MINIMAL_BET = 10;

        // Счет, до которого происходит игра на столе
        public const int GAME_END_COUNT = 151;

        // Стоимость бонусов
        public const int BONUS_BELOT = 20;
        public const int BONUS_TERZ = 20;
        public const int BONUS_50 = 50;
        public const int BONUS_100 = 100;
        public const int BONUS_4X_10_Q_K = 100;
        public const int BONUS_4X_9 = 150;
        public const int BONUS_4X_J = 200;
        public const int BONUS_4X_A_TRUMP = 110;
        public const int BONUS_4X_A = 190;
    }
}
