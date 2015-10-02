using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    catch (Exception ex)
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
    }
}
