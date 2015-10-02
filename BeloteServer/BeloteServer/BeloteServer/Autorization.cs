﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeloteServer
{
    class Autorization
    {
        private Game game;

        public Autorization(Game game)
        {
            this.game = game;
        }

        // Проверка наличия игрока с указанным E-mail в базе данных
        public bool EmailExists(string Email)
        {
            string c = game.DataBase.SelectScalar(String.Format("SELECT Count(*) FROM Players WHERE Email = \"{0}\";", Email));
            int Count = (c == null) ? 0 : Int32.Parse(c);
            return (Count != 0);
        }

        // Проверка наличия игрока с указанным ником в базе данных
        public bool NicknameExists(string Nickname)
        {
            string c = game.DataBase.SelectScalar(String.Format("SELECT Count(*) FROM Players WHERE Nickname = \"{0}\";", Nickname));
            int Count = (c == null) ? 0 : Int32.Parse(c);
            return (Count != 0);
        }

        // Вход пользователя с помощью электронной почты - проверка правильности входа (пароль, E-mail)
        public bool EnterEmail(string Email, string Password)
        {
            if (EmailExists(Email))
                return false;    
            string dbPassword = game.DataBase.SelectScalar(String.Format("SELECT Password FROM Players WHERE Email = \"{0}\";", Email));
            return (dbPassword == Password);
        }

        // Регистрация пользователя с помощью электронной почты
        public bool RegistrationEmail(string Nickname, string Email, string Password, string Country, bool Sex)
        {
            if (NicknameExists(Nickname))
                return false;
            if (EmailExists(Email))
                return false;
            game.DataBase.AddQuery(String.Format("INSERT INTO Players (Nickname, Email, Password, Sex, Country) VALUES (\"{0}\", \"{1}\", \"{2}\", \"{3}\", \"{4}\");",
                Nickname, Email, Password, (Sex == true) ? "1" : "0", Country));
            return true;
        }

        // Напоминание пользователю сообщения на электронную почту
        public string RemindPasswordEmail(string Email)
        {
            if (!EmailExists(Email))
                return null;
            string Password = game.DataBase.SelectScalar(String.Format("SELECT Password FROM Players WHERE Email = \"{0}\";", Email));
            return Password;
        }
    }
}