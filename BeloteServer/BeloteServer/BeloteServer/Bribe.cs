using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeloteServer
{
    // Класс, представляющий каждую конкретную взятку
    class Bribe
    {
        public Bribe()
        {
            Player1 = null;
            Player2 = null;
            Player3 = null;
            Player4 = null;
            BrideSuit = CardSuit.C_NONE;
        }

        // Ищет выигрывшего раздачу игрока
        private int FindWinningPlayer()
        {
            List<Card> list = new List<Card>();
            list.Add(Player1);
            list.Add(Player2);
            list.Add(Player3);
            list.Add(Player4);
            list.Sort();
            if (list[0] == Player1)
                return 1;
            else
            if (list[0] == Player2)
                return 2;
            else
            if (list[0] == Player3)
                return 3;
            else
                return 4;
        }

        // Ищет выигрывшую раздачу команду
        private BeloteTeam FindWinningTeam()
        {
            int p = FindWinningPlayer();
            if ((p == 1) || (p == 3))
                return BeloteTeam.TEAM1_1_3;
            else
                return BeloteTeam.TEAM2_2_4;
        }

        // Метод помещает карту на указанное место
        public void PutCard(int place, Card card)
        {
            if (IsEmpty)
            {
                BrideSuit = card.Suit;
            }
            switch (place)
            {
                case 1:
                    {
                        Player1 = card;
                        break;
                    }
                case 2:
                    {
                        Player2 = card;
                        break;
                    }
                case 3:
                    {
                        Player3 = card;
                        break;
                    }
                case 4:
                    {
                        Player4 = card;
                        break;
                    }
            }
        }

        public Card Player1
        {
            get;
            private set;
        }

        public Card Player2
        {
            get;
            private set;
        }

        public Card Player3
        {
            get;
            private set;
        }

        public Card Player4
        {
            get;
            private set;
        }

        // Тестирует, завершена ли взятка (все ли 4 игрока походили)
        public bool IsEnded
        {
            get
            {
                return ((Player1 != null) && (Player2 != null) && (Player3 != null) && (Player4 != null));
            }
        }

        // Является ли взятка не начатой
        public bool IsEmpty
        {
            get
            {
                return ((Player1 == null) && (Player2 == null) && (Player3 == null) && (Player4 == null));
            }
        }

        public CardSuit BrideSuit
        {
            get;
            private set;
        }

        // Выигрывшая раздачу команда
        public BeloteTeam WinningTeam
        {
            get
            {
                if (!IsEnded)
                    return BeloteTeam.TEAM_NONE;
                return FindWinningTeam();
            }
        }

        // Выигрывший раздачу игрок (номер)
        public int WinnerNumber
        {
            get
            {
                if (!IsEnded)
                    return -1;
                return FindWinningPlayer();
            }
        }
    }
}
