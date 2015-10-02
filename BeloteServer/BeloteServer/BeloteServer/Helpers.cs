using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;

namespace BeloteServer
{
    // класс с вспомогательными функциями
    public static class Helpers
    {
        // Функция разделяющая входную от клиента строку с командами на список ключ-значение
        public static Dictionary<string, string> SplitCommandString(string command)
        {
            if (command == null)
                return null;
            try
            {
                Dictionary<string, string> result = new Dictionary<string, string>();
                string[] pairs = command.Split(',');
                foreach (string s in pairs)
                {
                    string[] keyvalue = s.Split('=');
                    try
                    {
                        result.Add(keyvalue[0], keyvalue[1]);
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        // Данная функция вытаскивает из строки, полученной от клиента, команду в формате XXX
        public static string CommandFromStr(string Str)
        {
            try
            {
                return Str.Substring(0, 3);
            }
            catch (Exception)
            {
                return null;
            }
        }

        // Данная функция вытаскивает из строки, полученной от клиента, сообщение
        public static string MessageFromStr(string Str)
        {
            try
            {
                return Str.Substring(3, Str.Length - 3);
            }
            catch (Exception)
            {
                return null;
            }
        }

        // Функция отправки сообщения электронной почты, необходимо настроить
        public static bool SendEmail(string Email, string Subject, string Message)
        {
            var fromAddress = new MailAddress("tawasystems@gmail.com", "BLOT-ONLINE");

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                Timeout = 500,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, "password")
            };
            var msg = new MailMessage(fromAddress, new MailAddress(Email));
            msg.Subject = Subject;
            msg.Body = Message;
            try
            {
                smtp.Send(msg);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

    }
}
