﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.Diagnostics;

namespace BeloteClient
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
                // Если в строке нет ни одного параметра, то это может быть не ошибкой, а всего лишь отсутствием параметров
                if (command.IndexOf('=') < 0)
                {
                    return result;
                }
                // Разделяем всю полученную струку наподстроки формата параметр=значение
                string[] pairs = command.Split(',');
                foreach (string s in pairs)
                {
                    // Делим полученные строки на две части и добавляем их в словарь
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
            if (Str.Length < 3)
                return null;
            return Str.Substring(0, 3);
        }

        // Данная функция вытаскивает из строки, полученной от клиента, сообщение
        public static string MessageFromStr(string Str)
        {
            if (Str.Length <= 3)
                return null;
            return Str.Substring(3, Str.Length - 3);
        }

        // Методы преобразующие булево значение в строку и обратно
        public static string BoolToString(bool Value)
        {
            return ((Value) ? "1" : "0");
        }

        public static bool StringToBool(string Value)
        {
            return ((Value == "1") ? true : false);
        }

        
        // Методы перевода масти к строке и обратно
        public static string SuitToString(CardSuit Suit)
        {
            switch (Suit)
            {
                case CardSuit.C_HEARTS:
                    {
                        return "H";
                    }
                case CardSuit.C_CLUBS:
                    {
                        return "C";
                    }
                case CardSuit.C_SPADES:
                    {
                        return "S";
                    }
                case CardSuit.С_DIAMONDS:
                    {
                        return "D";
                    }
                default:
                    {
                        return "N";
                    }
            }
        }

        // Переводит строку в масть
        public static CardSuit StringToSuit(string Suit)
        {
            if (Suit.Length == 0)
                return CardSuit.C_NONE;
            switch (Suit[0])
            {
                case 'H':
                    {
                        return CardSuit.C_HEARTS;
                    }
                case 'C':
                    {
                        return CardSuit.C_CLUBS;
                    }
                case 'S':
                    {
                        return CardSuit.C_SPADES;
                    }
                case 'D':
                    {
                        return CardSuit.С_DIAMONDS;
                    }
                default:
                    {
                        return CardSuit.C_NONE;
                    }
            }
        }
      
    }
}
