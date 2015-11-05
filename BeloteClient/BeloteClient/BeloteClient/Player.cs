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
        public Player(Game game) : this (game, -1)
        {
        }

        // Конструктор - создание по ID и основному игровому объекту
        public Player(Game game, int Id)
        {
            this.Game = game;
            Statistics = new Statistics();
            Profile = new Profile();
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
