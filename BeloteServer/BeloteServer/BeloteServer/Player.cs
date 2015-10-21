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
        // Статистика игрока
        private Statistics statistics;
        // Ссылка на основной игровой объект
        private Game game;
        // Ссылка на профиль
        private Profile profile;

        public Player(Game game) : this (game, -1)
        {
        }

        // Конструктор - создание по ID и основному игровому объекту
        public Player(Game game, int Id)
        {
            this.game = game;
            statistics = new Statistics();
            profile = new Profile();
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
            List<List<string>> playerStatistics = Game.DataBase.Select(String.Format("SELECT * FROM Statistics WHERE PlayerId=\"{0}\";", IdPlayer), Constants.COLS_STATISTICS);
            int value;
            if (Int32.TryParse(playerStatistics[2][0], out value))
            {
                this.statistics.GamesTotal = value;
            }
            if (Int32.TryParse(playerStatistics[3][0], out value))
            {
                this.statistics.WinsCount = value;
            }
            if (Int32.TryParse(playerStatistics[4][0], out value))
            {
                this.statistics.LosesCount = value;
            }
            if (Int32.TryParse(playerStatistics[5][0], out value))
            {
                this.statistics.XP = value;
            }
            if (Int32.TryParse(playerStatistics[6][0], out value))
            {
                this.statistics.Level = value;
            }
            if (Int32.TryParse(playerStatistics[7][0], out value))
            {
                this.statistics.LastSeriesResult = value;
            }
            if (Int32.TryParse(playerStatistics[8][0], out value))
            {
                this.statistics.AbandonedGames = value;
            }
            if (Int32.TryParse(playerStatistics[9][0], out value))
            {
                this.statistics.GamesNotAllowed = value;
            }
            if (Int32.TryParse(playerStatistics[10][0], out value))
            {
                this.statistics.TablesCreated = value;
            }
            if (Int32.TryParse(playerStatistics[11][0], out value))
            {
                this.statistics.TablesCreatedNotPlayed = value;
            }
            if (Int32.TryParse(playerStatistics[12][0], out value))
            {
                this.statistics.DistributionsTotal = value;
            }
            if (Int32.TryParse(playerStatistics[13][0], out value))
            {
                this.statistics.DistributionsWithTrump = value;
            }
            if (Int32.TryParse(playerStatistics[14][0], out value))
            {
                this.statistics.DistributionsAnnouncedWin = value;
            }
            if (Int32.TryParse(playerStatistics[15][0], out value))
            {
                this.statistics.DistributionsWins = value;
            }
            if (Int32.TryParse(playerStatistics[16][0], out value))
            {
                this.statistics.ContraAnnounced = value;
            }
            if (Int32.TryParse(playerStatistics[17][0], out value))
            {
                this.statistics.ContraJustified = value;
            }
            if (Int32.TryParse(playerStatistics[18][0], out value))
            {
                this.statistics.CapotAnnounced = value;
            }
            if (Int32.TryParse(playerStatistics[19][0], out value))
            {
                this.statistics.CapotJustified = value;
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
                this.profile.Id = value;
            }
            this.profile.Nickname = playerData[1][0];
            this.profile.Name = playerData[2][0];
            this.profile.Surname = playerData[3][0];
            this.profile.Password = playerData[4][0];
            this.profile.Email = playerData[5][0];
            this.profile.Phone = playerData[6][0];
            this.profile.VK = playerData[7][0];
            this.profile.FB = playerData[8][0];
            this.profile.OK = playerData[9][0];
            this.profile.Country = playerData[10][0];
            this.profile.Address = playerData[11][0];
            this.profile.ZipCode = playerData[12][0];
            this.profile.Language = playerData[13][0];
            if (Int32.TryParse(playerData[14][0], out value))
            {
                this.profile.Sex = (value > 0);
            }
            this.profile.TimeZone = playerData[15][0];
            if (DateTime.TryParse(playerData[16][0], out d))
                this.profile.BirtDate = d;
            this.profile.AvatarFile = playerData[17][0];
            if (DateTime.TryParse(playerData[18][0], out d))
            {
                this.profile.VIPExperies = d;
            }
            if (Int32.TryParse(playerData[19][0], out value))
            {
                this.profile.USD = value;
            }
            if (Int32.TryParse(playerData[20][0], out value))
            {
                this.profile.BUSD = value;
            }
            if (Int32.TryParse(playerData[21][0], out value))
            {
                this.profile.Chips = value;
            }
            ReadStatisticsFromaDataBase(this.Profile.Id);
#if DEBUG
            Debug.Unindent();
#endif
        }

        public Game Game
        {
            get
            {
                return game;
            }
        }

        public Statistics Statistics
        {
            get
            {
                return statistics;
            }
        }

        public Profile Profile
        {
            get
            {
                return profile;
            }
        }
        
    }
}
