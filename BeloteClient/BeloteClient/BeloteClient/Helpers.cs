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
    }
}
