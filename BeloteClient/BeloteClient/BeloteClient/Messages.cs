using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeloteClient
{
    public static class Messages
    {
        // Сообщения регистрации пользователя
        public const string MESSAGE_AUTORIZATION_REGISTRATION_EMAIL = "ARE";

        public const string MESSAGE_AUTORIZATION_REGISTRATION_PHONE = "ARP";

        public const string MESSAGE_AUTORIZATION_REGISTRATION_VK = "ARV";

        public const string MESSAGE_AUTORIZATION_REGISTRATION_OK = "ARO";

        public const string MESSAGE_AUTORIZATION_REGISTRATION_FB = "ARF";

        // Сообщения авторизации пользователя
        public const string MESSAGE_AUTORIZATION_AUTORIZATION_EMAIL = "AAE";

        public const string MESSAGE_AUTORIZATION_AUTORIZATION_PHONE = "AAP";

        public const string MESSAGE_AUTORIZATION_AUTORIZATION_VK = "AAV";
        
        public const string MESSAGE_AUTORIZATION_AUTORIZATION_OK = "AAO";

        public const string MESSAGE_AUTORIZATION_AUTORIZATION_FB = "AAF";

        // Сообщения напоминания пароля пользователя
        public const string MESSAGE_AUTORIZATION_REMINDPASSWORD_EMAIL = "AME";

        public const string MESSAGE_AUTORIZATION_REMINDPASSWORD_PHONE = "AMP";

        // Сообщения на тестирование наличия пользователя
        public const string MESSAGE_AUTORIZATION_TEST_EMAIL = "ATE";

        public const string MESSAGE_AUTORIZATION_TEST_PHONE = "ATP";

        public const string MESSAGE_AUTORIZATION_TEST_VK = "ATV";

        public const string MESSAGE_AUTORIZATION_TEST_OK = "ATO";

        public const string MESSAGE_AUTORIZATION_TEST_FB = "ATF";

        public const string MESSAGE_AUTORIZATION_TEST_NICKNAME = "ATN";

        // Сообщение выхода пользователя
        public const string MESSAGE_AUTORIZATION_USER_EXIT = "AEO";

        // Сообщение отключения клиента
        public const string MESSAGE_CLIENT_DISCONNECT = "EXT";

        // Сообщения модификации игрового стола создателем
        public const string MESSAGE_TABLE_MODIFY_CREATE = "TMC";

        public const string MESSAGE_TABLE_MODIFY_CREATORLEAVE = "TML";

        public const string MESSAGE_TABLE_MODIFY_VISIBILITY = "TMV";

        // Сообщения добавления и удаления игроков со стола
        public const string MESSAGE_TABLE_PLAYERS_ADD = "TPA";

        public const string MESSAGE_TABLE_PLAYERS_DELETE = "TPD";

        public const string MESSAGE_TABLE_PLAYERS_QUIT = "TPQ";

        public const string MESSAGE_TABLE_PLAYERS_ADDBOT = "TPB";

        public const string MESSAGE_TABLE_PLAYERS_DELETEBOT = "TPW";

        // Сообщение выборки столов
        public const string MESSAGE_TABLE_SELECT_TABLES = "TST";

        public const string MESSAGE_TABLE_SELECT_ALL = "TSA";

        public const string MESSAGE_TABLE_SELECT_CONCRETIC = "TSC";

        // Сообщения начала игры на столе
        public const string MESSAGE_GAME_START = "GTS";

        public const string MESSAGE_GAME_DISTRIBUTIONCARDS = "GDC";

        // Сообщения процесса торговли на столе
        public const string MESSAGE_GAME_BAZAR_BET = "GBB";

        public const string MESSAGE_GAME_BAZAR_SAYBET = "GBS";

        public const string MESSAGE_GAME_BAZAR_NEXTBETPLAYER = "GBN";

        public const string MESSAGE_GAME_BAZAR_END = "GBE";

        // Сообщения объявления бонусов на столе
        public const string MESSAGE_GAME_BONUSES_ALL = "GGB";

        public const string MESSAGE_GAME_BONUSES_ANNOUNCE = "GGA";

        public const string MESSAGE_GAME_BONUSES_WINNER = "GGW";

        public const string MESSAGE_GAME_BONUSES_TYPES = "GGC";

        // Сообщения игрового процесса на столе
        public const string MESSAGE_GAME_GAMING_PLAYERMOVE = "GGH";

        public const string MESSAGE_GAME_GAMING_NEXTPLAYER = "GGP";

        public const string MESSAGE_GAME_GAMING_REMINDCARD = "GGR";

        // Сообщение завершения игры на столе
        public const string MESSAGE_GAME_END = "GEG";

        // Сообщения для получения информации о профиле игрока
        public const string MESSAGE_PLAYER_GET_INFORMATION = "PGI";

        public const string MESSAGE_PLAYER_GET_STATISTICS = "PGS";

        public const string MESSAGE_PLAYER_GET_AVATAR = "PGA";

        public const string MESSAGE_PLAYER_GET_ACCOUNTS = "PGM";
    }
}
