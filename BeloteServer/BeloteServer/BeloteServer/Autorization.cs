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
            // Проверяется количество игроков с заданным E-mail адресом
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
            // Если такого электронного адреса не существует, то войти не возможно
            if (!EmailExists(Email))
                return false;    
            // Происходит выборка и сверка введенного пароля с паролем из БД
            string dbPassword = game.DataBase.SelectScalar(String.Format("SELECT Password FROM Players WHERE Email = \"{0}\";", Email));
#if DEBUG
            Debug.WriteLine("Результат: " + ((dbPassword == Password) ? "Вход успешен" : "Войти не удалось"));
            Debug.Unindent();
#endif
            return (dbPassword == Password);
        }

        // Регистрация пользователя с помощью электронной почты
        public int RegistrationEmail(string Nickname, string Email, string Password, string Country, bool Sex)
        {
#if DEBUG 
            Debug.WriteLine(DateTime.Now.ToString() + " Попытка регистрации с помощью Email");
            Debug.Indent();
            Debug.WriteLine(String.Format("Nickname: {0}, Email: {1}, Password: {2}, Country: {3}, Sex: {4}", Nickname, Email, Password, Country, Sex));
#endif
            // Невозможно зарегистрироваться с уже существующим ником и E-mail адресом
            if (NicknameExists(Nickname))
                return -1;
            if (EmailExists(Email))
                return -1;
            // Выполняем запрос моментально
            game.DataBase.ExecuteQueryWithoutQueue(String.Format("INSERT INTO Players (Nickname, Email, Password, Sex, Country) VALUES (\"{0}\", \"{1}\", \"{2}\", \"{3}\", \"{4}\");",
                Nickname, Email, Password, (Sex == true) ? "1" : "0", Country));
            // Получаем идентификатор сделанной записи в реальном времени
            int id = Int32.Parse(game.DataBase.SelectScalar(String.Format("SELECT ID From Players WHERE Email=\"{0}\";", Email)));
            // Если зарегистрироваться удалось, то делаем запись о нулевой статистике игрока
            if (id != -1)
            {
                game.DataBase.AddQuery(String.Format("INSERT INTO Statistics (idPlayer) VALUES ({0});", id));
            }
#if DEBUG
            Debug.WriteLine("Регистрация успешна");
            Debug.Unindent();
#endif
            return id;
        }

        // Напоминание пользователю сообщения на электронную почту
        public bool RemindPasswordEmail(string Email)
        {
#if DEBUG
            Debug.WriteLine(DateTime.Now.ToString() + " Напоминание пароля на Email");
            Debug.Indent();
            Debug.WriteLine("Email: " + Email);
#endif
            // Если такой E-mail не зарегистрирован, то выполнить восстановление пароля невозможно
            if (!EmailExists(Email))
                return false;
            // Если адрес зарегистрирован, то высылаем пароль на адрес электронной почты
            string Password = game.DataBase.SelectScalar(String.Format("SELECT Password FROM Players WHERE Email = \"{0}\";", Email));
            bool Res = Helpers.SendEmail(Email, "Remind password BLOT-ONLINE", "Your password is: " + Password);
#if DEBUG
            Debug.Unindent();
#endif
            // Возвращаем результат - успешно или неуспешно выполнено восстановление пароля
            return Res;
        }
    }
}
