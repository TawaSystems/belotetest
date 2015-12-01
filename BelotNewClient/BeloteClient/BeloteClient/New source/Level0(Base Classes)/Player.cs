using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BeloteClient
{
    public class Player
    {
        public Player() : this (-1)
        {
        }

        // Конструктор - создание по ID и основному игровому объекту
        public Player(int Id)
        {
            Statistics = new Statistics();
            Profile = new Profile();
            Profile.Id = Id;
        }

        public Player(MessageResult pParams)
        {
            Statistics = new Statistics();
            Profile = new Profile();
            this.Profile.Id = Int32.Parse(pParams["PlayerID"]);
            DateTime d;
            Profile.Nickname = pParams["Nickname"];
            Profile.Name = pParams["Name"];
            Profile.Surname = pParams["Surname"];
            Profile.Email = pParams["Email"];
            Profile.Phone = pParams["Phone"];
            Profile.VK = pParams["VK"];
            Profile.FB = pParams["FB"];
            Profile.OK = pParams["OK"];
            Profile.Country = pParams["Country"];
            Profile.Address = pParams["Address"];
            Profile.ZipCode = pParams["ZipCode"];
            Profile.Language = pParams["Language"];
            Profile.Sex = Helpers.StringToBool(pParams["Sex"]);
            Profile.TimeZone = pParams["TimeZone"];
            if (DateTime.TryParse(pParams["BirthDate"], out d))
                Profile.BirtDate = d;
            if (DateTime.TryParse(pParams["VIPExperies"], out d))
                Profile.VIPExperies = d;
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

        public int ID
        {
            get
            {
                return Profile.Id;
            }
        }
        
    }
}
