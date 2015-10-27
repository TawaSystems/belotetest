using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BeloteServer
{
    class Player
    {
        public Player(Game game) : this (game, -1)
        {
        }

        // Конструктор - создание по ID и основному игровому объекту
        public Player(Game game, int Id)
        {
            this.Game = game;
            Statistics = new Statistics();
            Profile = new Profile();
            if (Id >= 0)
            {
                ReadPlayerFromDataBase("ID", Id);
            }
        }

        // Чтение статистики игрока из базы данных
        private void ReadStatisticsFromaDataBase(int IdPlayer)
        {
#if DEBUG
            Debug.WriteLine(DateTime.Now.ToString() + " Считывание статистики игрока из базы данных");
            Debug.Indent();
            Debug.WriteLine("ID игрока: " + IdPlayer);
#endif
            List<List<string>> playerStatistics = Game.DataBase.Select(String.Format("SELECT * FROM Statistics WHERE idPlayer=\"{0}\";", IdPlayer), Constants.COLS_STATISTICS);
            int value;
            if (Int32.TryParse(playerStatistics[2][0], out value))
            {
                this.Statistics.GamesTotal = value;
            }
            if (Int32.TryParse(playerStatistics[3][0], out value))
            {
                this.Statistics.WinsCount = value;
            }
            if (Int32.TryParse(playerStatistics[4][0], out value))
            {
                this.Statistics.LosesCount = value;
            }
            if (Int32.TryParse(playerStatistics[5][0], out value))
            {
                this.Statistics.XP = value;
            }
            if (Int32.TryParse(playerStatistics[6][0], out value))
            {
                this.Statistics.Level = value;
            }
            if (Int32.TryParse(playerStatistics[7][0], out value))
            {
                this.Statistics.LastSeriesResult = value;
            }
            if (Int32.TryParse(playerStatistics[8][0], out value))
            {
                this.Statistics.AbandonedGames = value;
            }
            if (Int32.TryParse(playerStatistics[9][0], out value))
            {
                this.Statistics.GamesNotAllowed = value;
            }
            if (Int32.TryParse(playerStatistics[10][0], out value))
            {
                this.Statistics.TablesCreated = value;
            }
            if (Int32.TryParse(playerStatistics[11][0], out value))
            {
                this.Statistics.TablesCreatedNotPlayed = value;
            }
            if (Int32.TryParse(playerStatistics[12][0], out value))
            {
                this.Statistics.DistributionsTotal = value;
            }
            if (Int32.TryParse(playerStatistics[13][0], out value))
            {
                this.Statistics.DistributionsWithTrump = value;
            }
            if (Int32.TryParse(playerStatistics[14][0], out value))
            {
                this.Statistics.DistributionsAnnouncedWin = value;
            }
            if (Int32.TryParse(playerStatistics[15][0], out value))
            {
                this.Statistics.DistributionsWins = value;
            }
            if (Int32.TryParse(playerStatistics[16][0], out value))
            {
                this.Statistics.ContraAnnounced = value;
            }
            if (Int32.TryParse(playerStatistics[17][0], out value))
            {
                this.Statistics.ContraJustified = value;
            }
            if (Int32.TryParse(playerStatistics[18][0], out value))
            {
                this.Statistics.CapotAnnounced = value;
            }
            if (Int32.TryParse(playerStatistics[19][0], out value))
            {
                this.Statistics.CapotJustified = value;
            }
#if DEBUG
            Debug.Unindent();
#endif
        }

        //Чтение игрока из базы данных
        public void ReadPlayerFromDataBase(string parameterName, object parameterValue)
        {
#if DEBUG
            Debug.WriteLine(DateTime.Now.ToString() + " Считывание игрока из базы данных");
            Debug.Indent();
            Debug.WriteLine(String.Format("Название параметра выборки: {0}, Значение параметра выборки: {1}", parameterName, parameterValue));
#endif
            List<List<string>> playerData = Game.DataBase.Select(String.Format("SELECT * FROM Players WHERE {0}=\"{1}\";", parameterName, parameterValue), Constants.COLS_PLAYERS);
            int value;
            DateTime d;
            if (Int32.TryParse(playerData[0][0], out value))
            {
                this.Profile.Id = value;
            }
            this.Profile.Nickname = playerData[1][0];
            this.Profile.Name = playerData[2][0];
            this.Profile.Surname = playerData[3][0];
            this.Profile.Password = playerData[4][0];
            this.Profile.Email = playerData[5][0];
            this.Profile.Phone = playerData[6][0];
            this.Profile.VK = playerData[7][0];
            this.Profile.FB = playerData[8][0];
            this.Profile.OK = playerData[9][0];
            this.Profile.Country = playerData[10][0];
            this.Profile.Address = playerData[11][0];
            this.Profile.ZipCode = playerData[12][0];
            this.Profile.Language = playerData[13][0];
            if (Int32.TryParse(playerData[14][0], out value))
            {
                this.Profile.Sex = (value > 0);
            }
            this.Profile.TimeZone = playerData[15][0];
            if (DateTime.TryParse(playerData[16][0], out d))
                this.Profile.BirtDate = d;
            this.Profile.AvatarFile = playerData[17][0];
            if (DateTime.TryParse(playerData[18][0], out d))
            {
                this.Profile.VIPExperies = d;
            }
            if (Int32.TryParse(playerData[19][0], out value))
            {
                this.Profile.USD = value;
            }
            if (Int32.TryParse(playerData[20][0], out value))
            {
                this.Profile.BUSD = value;
            }
            if (Int32.TryParse(playerData[21][0], out value))
            {
                this.Profile.Chips = value;
            }
            ReadStatisticsFromaDataBase(this.Profile.Id);
#if DEBUG
            Debug.Unindent();
#endif
        }

        // Ссылка на основной игровой объект
        public Game Game
        {
            get;
            private set;
        }

        // Статистика игрока
        public Statistics Statistics
        {
            get;
            private set;
        }

        // Ссылка на профиль
        public Profile Profile
        {
            get;
            private set;
        }
        
    }
}
