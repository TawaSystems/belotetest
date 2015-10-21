using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

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
#if DEBUG
            Debug.WriteLine(DateTime.Now.ToString() + " Результат проверки существования Email");
            Debug.Indent();
            Debug.WriteLine("Email: " + Email);
            Debug.WriteLine("Результат: " + ((Count != 0) ? "Зарегистрирован" : "Не зарегистрирован"));
            Debug.Unindent();
#endif
            return (Count != 0);
        }

        // Проверка наличия игрока с указанным ником в базе данных
        public bool NicknameExists(string Nickname)
        {
            string c = game.DataBase.SelectScalar(String.Format("SELECT Count(*) FROM Players WHERE Nickname = \"{0}\";", Nickname));
            int Count = (c == null) ? 0 : Int32.Parse(c);
#if DEBUG
            Debug.WriteLine(DateTime.Now.ToString() + " Результат проверки существования Nickname");
            Debug.Indent();
            Debug.WriteLine("Nickname: " + Nickname);
            Debug.WriteLine("Результат: " + ((Count != 0) ? "Зарегистрирован" : "Не зарегистрирован"));
            Debug.Unindent();
#endif
            return (Count != 0);
        }

        // Вход пользователя с помощью электронной почты - проверка правильности входа (пароль, E-mail)
        public bool EnterEmail(string Email, string Password)
        {
#if DEBUG
            Debug.WriteLine(DateTime.Now.ToString() + " Попытка входа пользователя по EMail");
            Debug.Indent();
            Debug.WriteLine("Email: " + Email);
            Debug.WriteLine("Password: " + Password);
#endif
            if (!EmailExists(Email))
                return false;    
            string dbPassword = game.DataBase.SelectScalar(String.Format("SELECT Password FROM Players WHERE Email = \"{0}\";", Email));
#if DEBUG
            Debug.WriteLine("Результат: " + ((dbPassword == Password) ? "Вход успешен" : "Войти не удалось"));
            Debug.Unindent();
#endif
            return (dbPassword == Password);
        }

        // Регистрация пользователя с помощью электронной почты
        public bool RegistrationEmail(string Nickname, string Email, string Password, string Country, bool Sex)
        {
#if DEBUG 
            Debug.WriteLine(DateTime.Now.ToString() + " Попытка регистрации с помощью Email");
            Debug.Indent();
            Debug.WriteLine(String.Format("Nickname: {0}, Email: {1}, Password: {2}, Country: {3}, Sex: {4}", Nickname, Email, Password, Country, Sex));
#endif
            if (NicknameExists(Nickname))
                return false;
            if (EmailExists(Email))
                return false;
            game.DataBase.ExecuteQueryWithoutQueue(String.Format("INSERT INTO Players (Nickname, Email, Password, Sex, Country) VALUES (\"{0}\", \"{1}\", \"{2}\", \"{3}\", \"{4}\");",
                Nickname, Email, Password, (Sex == true) ? "1" : "0", Country));
#if DEBUG
            Debug.WriteLine("Регистрация успешна");
            Debug.Unindent();
#endif
            return true;
        }

        // Напоминание пользователю сообщения на электронную почту
        public bool RemindPasswordEmail(string Email)
        {
#if DEBUG
            Debug.WriteLine(DateTime.Now.ToString() + " Напоминание пароля на Email");
            Debug.Indent();
            Debug.WriteLine("Email: " + Email);
#endif
            if (!EmailExists(Email))
                return false;
            string Password = game.DataBase.SelectScalar(String.Format("SELECT Password FROM Players WHERE Email = \"{0}\";", Email));
            bool Res = Helpers.SendEmail(Email, "Remind password BLOT-ONLINE", "Your password is: " + Password);
#if DEBUG
            Debug.Unindent();
#endif
            return Res;
        }
    }
}
