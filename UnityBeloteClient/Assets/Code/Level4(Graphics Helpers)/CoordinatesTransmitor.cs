using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeloteClient
{
    public static class CoordinatesTransmitor
    {
        private static int NextPlaceNumber(int curPlace)
        {
            if (curPlace < 4)
                return (++curPlace);
            else
                return 1;
        }

        private static int PredPlaceNumber(int curPlace)
        {
            if (curPlace > 1)
                return (--curPlace);
            else
                return 4;
        }

        // Переводит координты игрока на сервере в координаты игрока на платформе
        public static int ServerPlaceToGraphicPlace(int serverPlace, int Place)
        {
            if (serverPlace == Place)
            {
                return 1;
            }
            else
            if (serverPlace == (NextPlaceNumber(Place)))
            {
                return 2;
            }
            else
            if (serverPlace == (NextPlaceNumber(NextPlaceNumber(Place))))
            {
                return 3;
            }
            else
                return 4;
        }

        // Переводит координаты игрока при отрисовке в координаты игрока на сервере
        public static int GraphicPlaceToServerPlace(int graphicPlace, int Place)
        {
            switch (graphicPlace)
            {
                case 1:
                    {
                        return Place;
                    }
                case 2:
                    {
                        return NextPlaceNumber(Place);
                    }
                case 3:
                    {
                        return NextPlaceNumber(NextPlaceNumber(Place));
                    }
                case 4:
                    {
                        return PredPlaceNumber(Place);
                    }
                default:
                    return graphicPlace;
            }
        }

        // Возвращает название бонуса по его типу
        public static string TextFromBonusType(BonusType bonus)
        {
            switch (bonus)
            {
                case BonusType.BONUS_100:
                    return "100";
                case BonusType.BONUS_4X:
                    return "4X";
                case BonusType.BONUS_50:
                    return "50";
                case BonusType.BONUS_TERZ:
                    return "Terz";
                default:
                    return "";
            }
        }
    }
}
