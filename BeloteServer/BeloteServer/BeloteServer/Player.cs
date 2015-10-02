using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            List<List<string>> playerStatistics = Game.DataBase.Select(String.Format("SELECT * FROM Statistics WHERE PlayerId=\"{0}\";", IdPlayer), Constants.COLS_STATISTICS);
            this.statistics.GamesTotal = Int32.Parse(playerStatistics[2][0]);
            this.statistics.WinsCount = Int32.Parse(playerStatistics[3][0]);
            this.statistics.LosesCount = Int32.Parse(playerStatistics[4][0]);
            this.statistics.XP = Int32.Parse(playerStatistics[5][0]);
            this.statistics.Level = Int32.Parse(playerStatistics[6][0]);
            this.statistics.LastSeriesResult = Int32.Parse(playerStatistics[7][0]);
            this.statistics.AbandonedGames = Int32.Parse(playerStatistics[8][0]);
            this.statistics.GamesNotAllowed = Int32.Parse(playerStatistics[9][0]);
            this.statistics.TablesCreated = Int32.Parse(playerStatistics[10][0]);
            this.statistics.TablesCreatedNotPlayed = Int32.Parse(playerStatistics[11][0]);
            this.statistics.DistributionsTotal = Int32.Parse(playerStatistics[12][0]);
            this.statistics.DistributionsWithTrump = Int32.Parse(playerStatistics[13][0]);
            this.statistics.DistributionsAnnouncedWin = Int32.Parse(playerStatistics[14][0]);
            this.statistics.DistributionsWins = Int32.Parse(playerStatistics[15][0]);
            this.statistics.ContraAnnounced = Int32.Parse(playerStatistics[16][0]);
            this.statistics.ContraJustified = Int32.Parse(playerStatistics[17][0]);
            this.statistics.CapotAnnounced = Int32.Parse(playerStatistics[18][0]);
            this.statistics.CapotJustified = Int32.Parse(playerStatistics[19][0]);
        }

        //Чтение игрока из базы данных
        public void ReadPlayerFromDataBase(string parameterName, object parameterValue)
        {
            List<List<string>> playerData = Game.DataBase.Select(String.Format("SELECT * FROM Players WHERE {0}=\"{1}\";", parameterName, parameterValue), Constants.COLS_PLAYERS);
            this.profile.Id = Int32.Parse(playerData[0][0]);
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
            this.profile.Sex = (Int32.Parse(playerData[14][0]) > 0) ? true : false;
            this.profile.TimeZone = playerData[15][0];
            this.profile.BirtDate = DateTime.Parse(playerData[16][0]);
            this.profile.AvatarFile = playerData[17][0];
            this.profile.VIPExperies = DateTime.Parse(playerData[18][0]);
            this.profile.USD = Int32.Parse(playerData[19][0]);
            this.profile.BUSD = Int32.Parse(playerData[20][0]);
            this.profile.Chips = Int32.Parse(playerData[21][0]);
            ReadStatisticsFromaDataBase(this.Profile.Id);
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
