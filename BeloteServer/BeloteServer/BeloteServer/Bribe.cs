﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BeloteServer
{
    // Класс, представляющий каждую конкретную взятку
    class Bribe
    {
        private List<Card> cards;

        public Bribe()
        {
#if DEBUG
            Debug.WriteLine("{0} Создание новой взятки", DateTime.Now);
#endif
            Player1 = null;
            Player2 = null;
            Player3 = null;
            Player4 = null;
            BribeSuit = CardSuit.C_NONE;
            IsTrumpBribe = false;
            BribeTrumped = false;
            SeniorTrump = null;
            cards = new List<Card>();
        }

        // Ищет выигрывшего раздачу игрока
        private int FindWinningPlayer()
        {
            List<Card> list = new List<Card>();
            list.Add(Player1);
            list.Add(Player2);
            list.Add(Player3);
            list.Add(Player4);
            // Если кто то за взятку походил козырной картой, то выбираем победителя из козырей
            if ((Player1.IsTrump) || (Player2.IsTrump) || (Player3.IsTrump) || (Player4.IsTrump))
            {
                for (var i = list.Count - 1; i >= 0; i--)
                {
                    if (!list[i].IsTrump)
                        list.Remove(list[i]);
                }
            }
            // Иначе выбираем победителя среди карт стартовой масти
            else
            {
                for (var i = list.Count - 1; i >= 0; i--)
                {
                    if (list[i].Suit != BribeSuit)
                        list.Remove(list[i]);
                }
            }
            list.Sort();
            // Вариант, со старшинством 10 по отношению к J, Q, K (не козырными)
            if (!list[0].IsTrump)
            {
                if ((list[0].Type == CardType.C_J) || (list[0].Type == CardType.C_Q) || (list[0].Type == CardType.C_K))
                {
                    for(var i = 0; i < list.Count; i++)
                    {
                        if (list[i].Type == CardType.C_10)
                        {
                            list[0] = list[i];
                            break;
                        }
                    }
                }
            }
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
#if DEBUG 
            Debug.WriteLine("{0} Ход картой. Игрок № - {1}, карта - {2}", DateTime.Now, place, card.ToString());
#endif
            // Если во взятку помещается первая карта, то задаем масть взятки, а также является ли она козырной
            if (IsEmpty)
            {
                BribeSuit = card.Suit;
                IsTrumpBribe = card.IsTrump;
            }
            else
            // Если ход во взятке не первый, то проверяем, не сделан ли на некозырную взятку ход козырной картой
            if (!IsTrumpBribe)
            {
                if (card.IsTrump)
                    BribeTrumped = true;
            }

            // Выбор старшего козыря на взятке
            if (card.IsTrump)
            {
                if (SeniorTrump == null)
                {
                    SeniorTrump = card;
                }
                else
                {
                    if (card.Cost > SeniorTrump.Cost)
                        SeniorTrump = card;
                }
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
                default:
                    {
                        break;
                    }
            }
            cards.Add(card);
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

        // Количество похоженных карт во взятке
        public int FulledCount
        {
            get
            {
                return cards.Count;
            }
        }

        public Card this[int Index]
        {
            get
            {
                return cards[Index];
            }
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

        // Заказанная на взятке масть
        public CardSuit BribeSuit
        {
            get;
            private set;
        }

        // Сделан ли первый ход в данной взятки с козырной масти
        public bool IsTrumpBribe
        {
            get;
            private set;
        }

        // Правдиво, если на взятки с запрашиваемой некозырной мастью ход сделан был козырем
        public bool BribeTrumped
        {
            get;
            private set;
        }

        // Старший козырь на взятке
        public Card SeniorTrump
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

        // Стоимость взятки (суммируеся из стоимости карт)
        public int BribeCost
        {
            get
            {
                if (!IsEnded)
                    return 0;
                else
                    return Player1.Cost + Player2.Cost + Player3.Cost + Player4.Cost;
            }
        }
    }
}
