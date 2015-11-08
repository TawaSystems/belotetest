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
    public class ServerConnection
    {
        private TcpClient client;
        private Thread worker;
        private NetworkStream stream;
        private Game game;

        public ServerConnection(Game Game)
        {
            if (!Connect())
            {
                throw new Exception("Не удалось подключиться к серверу");
            }
            game = Game;
            stream = client.GetStream();
            worker = new Thread(ProcessServer);
            worker.Start();
        }

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

        public void Disconnect()
        {
            SendDataToServer("EXT");
            worker.Abort();
            client.Close();
        }

        public void SendDataToServer(string message)
        {
            byte[] data = Encoding.Unicode.GetBytes(message);
            stream.Write(data, 0, data.Length);
        }

        private void ProcessPlayer(string command, string message)
        {
            Dictionary<string, string> pParams = Helpers.SplitCommandString(message);
            if (pParams == null)
            {
                return;
            }
            switch (command)
            {
                case Messages.MESSAGE_PLAYER_GET_INFORMATION:
                    {
                        Player p = new Player(this.game, Int32.Parse(pParams["PlayerID"]));
                        DateTime d;
                        p.Profile.Nickname = pParams["Nickname"];
                        p.Profile.Name = pParams["Name"];
                        p.Profile.Surname = pParams["Surname"];
                        p.Profile.Email = pParams["Email"];
                        p.Profile.Phone = pParams["Phone"];
                        p.Profile.VK = pParams["VK"];
                        p.Profile.FB = pParams["FB"];
                        p.Profile.OK = pParams["OK"];
                        p.Profile.Country = pParams["Country"];
                        p.Profile.Address = pParams["Address"];
                        p.Profile.ZipCode = pParams["ZipCode"];
                        p.Profile.Language = pParams["Language"];
                        p.Profile.Sex = Helpers.StringToBool(pParams["Sex"]);
                        p.Profile.TimeZone = pParams["TimeZone"];
                        if (DateTime.TryParse(pParams["BirthDate"], out d))
                            p.Profile.BirtDate = d;
                        if (DateTime.TryParse(pParams["VIPExperies"], out d))
                            p.Profile.VIPExperies = d;
                        this.game.Dispatcher.BeginInvoke(new Action(() => this.game.GetPlayerInformation(p)));
                        break;
                    }
                case Messages.MESSAGE_PLAYER_GET_STATISTICS:
                    {
                        break;
                    }
                case Messages.MESSAGE_PLAYER_GET_AVATAR:
                    {
                        break;
                    }
                case Messages.MESSAGE_PLAYER_GET_ACCOUNTS:
                    {
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        private void ProcessAutorization(string command, string message)
        {
            Dictionary<string, string> aParams = Helpers.SplitCommandString(message);
            if (aParams == null)
            {
                return;
            }
            switch (command)
            {
                case Messages.MESSAGE_AUTORIZATION_AUTORIZATION_EMAIL:
                    {
                        this.game.Dispatcher.BeginInvoke(new Action(() => this.game.AutorizationResult(Int32.Parse(aParams["PlayerID"]))));
                        break;
                    }
                case Messages.MESSAGE_AUTORIZATION_AUTORIZATION_FB:
                    {
                        break;
                    }
                case Messages.MESSAGE_AUTORIZATION_AUTORIZATION_OK:
                    {
                        break;
                    }
                case Messages.MESSAGE_AUTORIZATION_AUTORIZATION_PHONE:
                    {
                        break;
                    }
                case Messages.MESSAGE_AUTORIZATION_AUTORIZATION_VK:
                    {
                        break;
                    }
                case Messages.MESSAGE_AUTORIZATION_REGISTRATION_EMAIL:
                    {
                        this.game.Dispatcher.BeginInvoke(new Action(() => this.game.RegistrationResult(aParams["Registration"] == "1")));
                        break;
                    }
                case Messages.MESSAGE_AUTORIZATION_REGISTRATION_FB:
                    {
                        break;
                    }
                case Messages.MESSAGE_AUTORIZATION_REGISTRATION_OK:
                    {
                        break;
                    }
                case Messages.MESSAGE_AUTORIZATION_REGISTRATION_PHONE:
                    {
                        break;
                    }
                case Messages.MESSAGE_AUTORIZATION_REGISTRATION_VK:
                    {
                        break;
                    }
                case Messages.MESSAGE_AUTORIZATION_REMINDPASSWORD_EMAIL:
                    {
                        break;
                    }
                case Messages.MESSAGE_AUTORIZATION_REMINDPASSWORD_PHONE:
                    {
                        break;
                    }
                case Messages.MESSAGE_AUTORIZATION_TEST_EMAIL:
                    {
                        break;
                    }
                case Messages.MESSAGE_AUTORIZATION_TEST_FB:
                    {
                        break;
                    }
                case Messages.MESSAGE_AUTORIZATION_TEST_NICKNAME:
                    {
                        break;
                    }
                case Messages.MESSAGE_AUTORIZATION_TEST_OK:
                    {
                        break;
                    }
                case Messages.MESSAGE_AUTORIZATION_TEST_PHONE:
                    {
                        break;
                    }
                case Messages.MESSAGE_AUTORIZATION_TEST_VK:
                    {
                        break;
                    }
                case Messages.MESSAGE_AUTORIZATION_USER_EXIT:
                    {
                        break;
                    }
            }
        }

        private void ProcessCommand(string message)
        {
            string command = Helpers.CommandFromStr(message);
            string msg = Helpers.MessageFromStr(message);

            switch (command)
            {
                // Обработка команд авторизации
                case Messages.MESSAGE_AUTORIZATION_AUTORIZATION_EMAIL:
                case Messages.MESSAGE_AUTORIZATION_AUTORIZATION_FB:
                case Messages.MESSAGE_AUTORIZATION_AUTORIZATION_OK:
                case Messages.MESSAGE_AUTORIZATION_AUTORIZATION_PHONE:
                case Messages.MESSAGE_AUTORIZATION_AUTORIZATION_VK:
                case Messages.MESSAGE_AUTORIZATION_REGISTRATION_EMAIL:
                case Messages.MESSAGE_AUTORIZATION_REGISTRATION_FB:
                case Messages.MESSAGE_AUTORIZATION_REGISTRATION_OK:
                case Messages.MESSAGE_AUTORIZATION_REGISTRATION_PHONE:
                case Messages.MESSAGE_AUTORIZATION_REGISTRATION_VK:
                case Messages.MESSAGE_AUTORIZATION_REMINDPASSWORD_EMAIL:
                case Messages.MESSAGE_AUTORIZATION_REMINDPASSWORD_PHONE:
                case Messages.MESSAGE_AUTORIZATION_TEST_EMAIL:
                case Messages.MESSAGE_AUTORIZATION_TEST_FB:
                case Messages.MESSAGE_AUTORIZATION_TEST_NICKNAME:
                case Messages.MESSAGE_AUTORIZATION_TEST_OK:
                case Messages.MESSAGE_AUTORIZATION_TEST_PHONE:
                case Messages.MESSAGE_AUTORIZATION_TEST_VK:
                case Messages.MESSAGE_AUTORIZATION_USER_EXIT:
                    {
                        ProcessAutorization(command, msg);
                        break;
                    }
                // Обработка отключения клиента
                case Messages.MESSAGE_CLIENT_DISCONNECT:
                    {
                        break;
                    }
                // Обработка модификации стола
                case Messages.MESSAGE_TABLE_MODIFY_CREATE:
                case Messages.MESSAGE_TABLE_MODIFY_CREATORLEAVE:
                case Messages.MESSAGE_TABLE_MODIFY_VISIBILITY:
                case Messages.MESSAGE_TABLE_PLAYERS_ADD:
                case Messages.MESSAGE_TABLE_PLAYERS_DELETE:
                case Messages.MESSAGE_TABLE_PLAYERS_QUIT:
                case Messages.MESSAGE_TABLE_SELECT_TABLES:
                case Messages.MESSAGE_TABLE_SELECT_ALL:
                case Messages.MESSAGE_TABLE_SELECT_CONCRETIC:
                    {
                        ProcessTables(command, msg);
                        break;
                    }
                // Обработка процесса игры
                case Messages.MESSAGE_GAME_BAZAR_BET:
                case Messages.MESSAGE_GAME_BAZAR_END:
                case Messages.MESSAGE_GAME_BAZAR_NEXTBETPLAYER:
                case Messages.MESSAGE_GAME_BAZAR_SAYBET:
                case Messages.MESSAGE_GAME_BONUSES_ALL:
                case Messages.MESSAGE_GAME_BONUSES_ANNOUNCE:
                case Messages.MESSAGE_GAME_BONUSES_TYPES:
                case Messages.MESSAGE_GAME_BONUSES_WINNER:
                case Messages.MESSAGE_GAME_DISTRIBUTIONCARDS:
                case Messages.MESSAGE_GAME_END:
                case Messages.MESSAGE_GAME_GAMING_NEXTPLAYER:
                case Messages.MESSAGE_GAME_GAMING_PLAYERMOVE:
                case Messages.MESSAGE_GAME_GAMING_REMINDCARD:
                case Messages.MESSAGE_GAME_START:
                    {
                        break;
                    }
                // Обработка сообщений по работе с профилем пользователя
                case Messages.MESSAGE_PLAYER_GET_INFORMATION:
                case Messages.MESSAGE_PLAYER_GET_STATISTICS:
                case Messages.MESSAGE_PLAYER_GET_AVATAR:
                case Messages.MESSAGE_PLAYER_GET_ACCOUNTS:
                    {
                        ProcessPlayer(command, msg);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        private void ProcessTables(string command, string message)
        {
            Dictionary<string, string> tParams = Helpers.SplitCommandString(message);
            if (tParams == null)
            {
                return;
            }
            switch (command)
            {
                case Messages.MESSAGE_TABLE_MODIFY_CREATE:
                    {
                        int ID = Int32.Parse(tParams["ID"]);
                        this.game.Dispatcher.BeginInvoke(new Action(() => this.game.CreatingTable(ID)));
                        break;
                    }
                case Messages.MESSAGE_TABLE_MODIFY_CREATORLEAVE:
                    {
                        break;
                    }
                case Messages.MESSAGE_TABLE_MODIFY_VISIBILITY:
                    {
                        break;
                    }
                case Messages.MESSAGE_TABLE_PLAYERS_ADD:
                    {
                        string res;
                        // Обработка для клиента отправившего сообщение
                        if (tParams.TryGetValue("Result", out res))
                        {
                            this.game.Dispatcher.BeginInvoke(new Action(() => this.game.EnterTheTable(Int32.Parse(tParams["Result"]))));
                        }
                        // Обработка других клиентов
                        else
                        {
                            this.game.Dispatcher.BeginInvoke(new Action(() => this.game.AddingToTable(Int32.Parse(tParams["Player"]), Int32.Parse(tParams["Place"]))));
                        }
                        break;
                    }
                case Messages.MESSAGE_TABLE_PLAYERS_DELETE:
                    {
                        break;
                    }
                case Messages.MESSAGE_TABLE_PLAYERS_QUIT:
                    {
                        break;
                    }
                case Messages.MESSAGE_TABLE_SELECT_TABLES:
                    {
                        int TableID = Int32.Parse(tParams["ID"]);
                        int Bet = Int32.Parse(tParams["Bet"]);
                        bool PlayersVisibility = Helpers.StringToBool(tParams["PlayersVisibility"]);
                        bool Chat = Helpers.StringToBool(tParams["Chat"]);
                        int MinimalLevel = Int32.Parse(tParams["MinimalLevel"]);
                        bool VIPOnly = Helpers.StringToBool(tParams["VIPOnly"]);
                        bool Moderation = Helpers.StringToBool(tParams["Moderation"]);
                        bool AI = Helpers.StringToBool(tParams["AI"]);
                        int Creator = Int32.Parse(tParams["Creator"]);
                        int Player2, Player3, Player4;
                        if (!Int32.TryParse(tParams["Player2"], out Player2))
                            Player2 = -1;
                        if (!Int32.TryParse(tParams["Player3"], out Player3))
                            Player3 = -1;
                        if (!Int32.TryParse(tParams["Player4"], out Player4))
                            Player4 = -1;
                        this.game.Dispatcher.BeginInvoke(new Action(() => this.game.AddPossibleTable(TableID, Bet, PlayersVisibility, Chat, 
                            MinimalLevel, VIPOnly, Moderation, AI, Creator, Player2, Player3, Player4)));
                        break;
                    }
                case Messages.MESSAGE_TABLE_SELECT_ALL:
                    {
                        break;
                    }
                case Messages.MESSAGE_TABLE_SELECT_CONCRETIC:
                    {
                        int TableID = Int32.Parse(tParams["ID"]);
                        int Bet = Int32.Parse(tParams["Bet"]);
                        bool PlayersVisibility = Helpers.StringToBool(tParams["PlayersVisibility"]);
                        bool Chat = Helpers.StringToBool(tParams["Chat"]);
                        int MinimalLevel = Int32.Parse(tParams["MinimalLevel"]);
                        bool VIPOnly = Helpers.StringToBool(tParams["VIPOnly"]);
                        bool Moderation = Helpers.StringToBool(tParams["Moderation"]);
                        bool AI = Helpers.StringToBool(tParams["AI"]);
                        int Creator = Int32.Parse(tParams["Creator"]);
                        int Player2, Player3, Player4;
                        if (!Int32.TryParse(tParams["Player2"], out Player2))
                            Player2 = -1;
                        if (!Int32.TryParse(tParams["Player3"], out Player3))
                            Player3 = -1;
                        if (!Int32.TryParse(tParams["Player4"], out Player4))
                            Player4 = -1;
                        Table t = new Table(this.game, TableID, Creator,
                            Bet, PlayersVisibility, Chat, MinimalLevel, true, VIPOnly, Moderation, AI);
                        t.Player2 = Player2;
                        t.Player3 = Player3;
                        t.Player4 = Player4;
                        this.game.Dispatcher.BeginInvoke(new Action(() => this.game.ReceiveTableInformation(t)));
                        break;
                    }
                default:
                    {
                        break;
                    }
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

                    ProcessCommand(builder.ToString());
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
