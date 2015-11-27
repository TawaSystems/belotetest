using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

namespace BeloteServer
{
    // Класс клиента, отвечающий за AI
    class ClientBot : Client
    {
        // Карьы и бонусы бота на раздаче
        private CardList botCards;
        private BonusList botBonuses;
        // Использованные за раздачу карты
        private BaseCardList usedCards;
        // Неиспользованные карты
        private CardList dontUsedCards;

        // Флаг, отвечающий за то, делал ли бот заказ на данной раздаче или нет
        private bool IsMakingOrder;

        public ClientBot(int Place, Table Table)
        {
            this.ActivePlace = Place;
            this.ActiveTable = Table;
            usedCards = new BaseCardList();
            dontUsedCards = new CardList();
        }

        // Заново заполняет неиспользованные карты
        private void RenewDontUsedCards()
        {
            dontUsedCards.Clear();
            foreach (CardSuit s in Enum.GetValues(typeof(CardSuit)))
            {
                if (s == CardSuit.C_NONE)
                    continue;
                foreach (CardType t in Enum.GetValues(typeof(CardType)))
                {
                    if (t == CardType.C_UNDEFINED)
                        continue;
                    dontUsedCards.Add(new Card(t, s));
                }
            }
        }

        // Получение максимального возможного заказа для одной масти
        private Order GetOrderForSuit(CardSuit Suit)
        {

            int KCount = 0;
            int Aces = 0;
            int AceAnd10 = 0;
            int Ace10K = 0;
            int Ace10KQ = 0;
            CardList tempCardList = new CardList(botCards.ToString());
            tempCardList.SetTrump(Suit);

            // Подсчет количества козырных карт, некозырных тузов и пар некозырных туз-10, туз-10-к, туз-10-к-д
            for (var i = 0; i < tempCardList.Count; i++)
            {
                if (tempCardList[i].IsTrump)
                {
                    KCount++;
                }
                else
                {
                    if (tempCardList[i].Type == CardType.C_A)
                    {
                        Aces++;
                        if (tempCardList.Exists(new Card(CardType.C_10, tempCardList[i].Suit)))
                        {
                            AceAnd10++;
                            if (tempCardList.Exists(new Card(CardType.C_K, tempCardList[i].Suit)))
                            {
                                Ace10K++;
                                if (tempCardList.Exists(new Card(CardType.C_Q, tempCardList[i].Suit)))
                                {
                                    Ace10KQ++;
                                }
                            }
                        }
                    }
                }
            }

            int orderSize = 0;
            // Выбираем размер ставки при игре с козырем
            if (Suit != CardSuit.C_NONE)
            {
                bool JackK = tempCardList.Exists(new Card(CardType.C_J, Suit));
                bool NineK = tempCardList.Exists(new Card(CardType.C_9, Suit));
                
                if ((JackK) && (NineK) && ((KCount + Aces) >= 4))
                    orderSize = 80;
                if ((JackK) && (NineK) && ((KCount + Aces) >= 5))
                    orderSize = 90;
                if ((JackK) && (NineK) && ((KCount + Aces) >= 5) && (AceAnd10 > 0))
                    orderSize = 100;
                if ((JackK) && (NineK) && (KCount >= 4) && (Aces >= 2))
                    orderSize = 110;
                if ((JackK) && (NineK) && (KCount >= 5) && (Aces >= 1))
                    orderSize = 120;
                if ((JackK) && (NineK) && (KCount >= 5) && (Aces >= 2))
                    orderSize = 130;
                if ((JackK) && (NineK) && (KCount >= 5) && (Aces >= 2) && (AceAnd10 > 0))
                    orderSize = 140;
                if ((JackK) && (KCount >= 6) && (Aces >= 1))
                    orderSize = 150;
                if ((JackK) && (KCount >= 7))
                    orderSize = 160;
                if (KCount == 8)
                    orderSize = 250;
                if ((tempCardList.IsBelote) && (orderSize != 0))
                    orderSize += 20;
            }
            // Выбираем размер ставки при игре без козыря
            else
            {
                if (Aces >= 3)
                    orderSize = 80;
                if ((Aces >= 3) && (AceAnd10 >= 1))
                    orderSize = 90;
                if ((Aces >= 3) && (AceAnd10 >= 2))
                    orderSize = 100;
                if ((Aces >= 3) && (AceAnd10 >= 3))
                    orderSize = 110;
                if ((Aces >= 3) && (AceAnd10 >= 2) && (Ace10K > 0))
                    orderSize = 120;
                if ((Aces >= 3) && (AceAnd10 >= 2) && (Ace10KQ > 0))
                    orderSize = 130;
                if ((Aces >= 4) && (AceAnd10 >= 1))
                    orderSize = 140;
                if ((Aces >= 4) && (AceAnd10 >= 2))
                    orderSize = 150;
                if ((Aces >= 4) && (AceAnd10 >= 3))
                    orderSize = 160;
                if ((Aces >= 4) && (AceAnd10 >= 4))
                    orderSize = 250;
            }
            Order result;
            if (orderSize == 0)
            {
                result = new Order(OrderType.ORDER_PASS, orderSize, CardSuit.C_NONE);
            }
            else
            {
                if (orderSize >= 250)
                {
                    result = new Order(OrderType.ORDER_CAPOT, orderSize, Suit);
                }
                else
                {
                    result = new Order(OrderType.ORDER_BET, orderSize, Suit);
                }
            }
            return result;
        }

        // Просчет максимально возможного заказа
        private Order GetMaxPossibleOrder()
        {
            // Список, в который помещаются максимальные заказы для каждой из мастей
            List<Order> suitOrders = new List<Order>();
            suitOrders.Add(GetOrderForSuit(CardSuit.C_HEARTS));
            suitOrders.Add(GetOrderForSuit(CardSuit.С_DIAMONDS));
            suitOrders.Add(GetOrderForSuit(CardSuit.C_CLUBS));
            suitOrders.Add(GetOrderForSuit(CardSuit.С_DIAMONDS));
            suitOrders.Add(GetOrderForSuit(CardSuit.C_NONE));

            // Выбираем максимальный заказ
            Order order = suitOrders[0];
            for (var i = 1; i < suitOrders.Count; i++)
            {
                if (suitOrders[i].Size > order.Size)
                    order = suitOrders[i];
            }
            return order;
        }

        // Выбор карты для хода
        private Card GetMovingCard(CardList PossibleCards)
        {
            int movePlace = (usedCards.Count % 4) + 1;
            // Если игрок ходит первым
            if (movePlace == 1)
            {
                // Если игра с козырем
                if (botCards.CardListTrump != CardSuit.C_NONE)
                {
                    Card higherTrump = PossibleCards.GetHigherCard(botCards.CardListTrump);
                    // Если какой-то козырь найден...
                    if (higherTrump != null)
                    {
                        Card higherTrumpInDeck = dontUsedCards.GetHigherCard(botCards.CardListTrump);
                        // Если в колоде есть еще козыри...
                        if (higherTrumpInDeck != null)
                        {
                            // Если у бота имеется самый старший из оставшихся козырей, то ходим им
                            if ((higherTrump.Cost >= higherTrumpInDeck.Cost) && (higherTrump.Type != CardType.C_7))
                            {
                                return higherTrump;
                            }
                        }
                        // Иначе это самый старший, ходим им
                        else
                        {
                            return higherTrump;
                        }
                    }
                }

                // Если козырем пойти не удалось, то пытаемся найти такую карту, чтобы она была старше всех карт оставшихся в колоде данной масти
                foreach (CardSuit s in Enum.GetValues(typeof(CardSuit)))
                {
                    if ((s == CardSuit.C_NONE) || (s == botCards.CardListTrump))
                        continue;
                    Card possibleCard = PossibleCards.GetHigherCard(s);
                    Card higherCardInDeck = dontUsedCards.GetHigherCard(s);
                    if ((higherCardInDeck != null) && (possibleCard != null))
                    {
                        if (possibleCard.ThisIsBiggerThen(higherCardInDeck))
                            return possibleCard;
                    }
                }

                List<Card> lowCards = new List<Card>();
                // Если не удалось найти старшую карту, то пытаемся найти самую младшую из некозырных
                foreach (CardSuit s in Enum.GetValues(typeof(CardSuit)))
                {
                    if ((s == CardSuit.C_NONE) || (s == botCards.CardListTrump))
                        continue;
                    Card lowCard = PossibleCards.GetLowerCard(s);
                    if (lowCard != null)
                        lowCards.Add(lowCard);
                }
                if (lowCards.Count > 0)
                {
                    if (lowCards.Count == 1)
                        return lowCards[0];
                    Card tempCard = lowCards[0];
                    for (var i = 1; i < lowCards.Count - 1; i++)
                    {
                        if (tempCard.ThisIsBiggerThen(lowCards[i]))
                            tempCard = lowCards[i];
                    }
                    return tempCard;
                }

                // Если неудалось пойти ни одной картой, значит ходим минимальной козырной
                Card lowTrump = PossibleCards.GetLowerCard(botCards.CardListTrump);
                if (lowTrump != null)
                    return lowTrump;
                else
                    return PossibleCards[0];
            }
            else
            {
                // Если ходит бот не первым, то сначала пытается скинуть самую младшую карту из некозырных, потом самую младшую из козырных
                /*List<Card> lowCards = new List<Card>();
                foreach (CardSuit s in Enum.GetValues(typeof(CardSuit)))
                {
                    if ((s == CardSuit.C_NONE) || (s == botCards.CardListTrump))
                        continue;
                    Card lowCard = PossibleCards.GetLowerCard(s);
                    if (lowCard != null)
                        lowCards.Add(lowCard);
                }
                if (lowCards.Count > 0)
                {
                    if (lowCards.Count == 1)
                        return lowCards[0];
                    Card tempCard = lowCards[0];
                    for (var i = 1; i < lowCards.Count - 1; i++)
                    {
                        if (tempCard.ThisIsBiggerThen(lowCards[i]))
                            tempCard = lowCards[i];
                    }
                    return tempCard;
                }

                // Если неудалось пойти ни одной картой, значит ходим минимальной козырной
                Card lowTrump = PossibleCards.GetLowerCard(botCards.CardListTrump);
                if (lowTrump != null)
                    return lowTrump;
                else
                    return PossibleCards[0];*/
                PossibleCards.Sort();
                return PossibleCards[PossibleCards.Count - 1];
            }
        }

        // Обработка сообщений
        private void ProcessMessage(string command, string message)
        {
            Dictionary<string, string> bParams = Helpers.SplitCommandString(message);
            switch (command)
            {
                // Обработка раздачи карт
                case Messages.MESSAGE_GAME_DISTRIBUTIONCARDS:
                    {
                        botCards = new CardList(bParams["Cards"]);
                        IsMakingOrder = false;
                        usedCards.Clear();
                        RenewDontUsedCards();
                        //int totalScore1 = Int32.Parse(bParams["Scores1"]);
                        //int totalScore2 = Int32.Parse(bParams["Scores2"]);
                        break;
                    }
                // Обработка перехода хода к данному боту
                case Messages.MESSAGE_GAME_BAZAR_NEXTBETPLAYER:
                    {
                        BetType bType = (BetType)Int32.Parse(bParams["Type"]);
                        int minSize = Int32.Parse(bParams["MinSize"]);
                        // Расчет и совершение заказа
                        Thread.Sleep(500);
                        Order order;
                        if (!IsMakingOrder)
                        {
                            order = GetMaxPossibleOrder();
                            if (order.Size < minSize)
                                order = new Order(OrderType.ORDER_PASS, 0, CardSuit.C_NONE);
                            IsMakingOrder = true;
                        }
                        else
                            order = new Order(OrderType.ORDER_PASS, 0, CardSuit.C_NONE);
                        MakeOrder(order);
                        break;
                    }
                // Название ставки другого игрока
                case Messages.MESSAGE_GAME_BAZAR_SAYBET:
                    {
                        // НЕ НУЖНО
                        break;
                    }
                // Завершение процесса торговли
                case Messages.MESSAGE_GAME_BAZAR_END:
                    {
                        // НЕ НУЖНО
                        break;
                    }
                // Завершение процесса торговли без выбора козыря
                case Messages.MESSAGE_GAME_BAZAR_PASS:
                    {
                        // НЕ НУЖНО
                        break;
                    }
                // Переход хода к боту
                case Messages.MESSAGE_GAME_GAMING_NEXTPLAYER:
                    {
                        CardList PossibleCards = new CardList(bParams["Cards"]);
                        // Расчет и показ бонусов
                        Thread.Sleep(500);
                        if (botBonuses.Count > 0)
                        {
                            for (var i = botBonuses.Count - 1; i >= 0; i--)
                            {
                                if (botBonuses[i] != botBonuses.SeniorBonus)
                                    botBonuses.Delete(botBonuses[i]);
                            }
                        }
                        AnnounceBonuses(botBonuses);
                        Thread.Sleep(500);
                        // Расчет и совершение хода
                        MakeMove(GetMovingCard(PossibleCards));
                        break;
                    }
                // Извещение о ходе картой другим игроком
                case Messages.MESSAGE_GAME_GAMING_REMINDCARD:
                    {
                        int cardPlace = Int32.Parse(bParams["Place"]);
                        Card newCard = new Card(bParams["Card"]);
                        // Запоминание похоженной карты
                        usedCards.Add(newCard);
                        dontUsedCards.Remove(dontUsedCards[newCard.Type, newCard.Suit]);
                        // int LocalScore1 = Int32.Parse(bParams["Scores1"]);
                        // int LocalScore2 = Int32.Parse(bParams["Scores2"]);
                        // int beloteRemind = Int32.Parse(bParams["Belote"]);
                        break;
                    }
                // Получение списка возможных бонусов
                case Messages.MESSAGE_GAME_BONUSES_ALL:
                    {
                        botBonuses = new BonusList(message);
                        break;
                    }
                // Оглашение типов бонусов другими игроками
                case Messages.MESSAGE_GAME_BONUSES_TYPES:
                    {
                        // НЕ НУЖНО
                        break;
                    }
                // Оглашение победителя бонусов
                case Messages.MESSAGE_GAME_BONUSES_WINNER:
                    {
                        // НЕ НУЖНО
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        // Совершение ботом заказа
        private void MakeOrder(Order Order)
        {
            if (ActiveTable != null)
            {
                lock (ActiveTable)
                {
                    try
                    {
                        ActiveTable.AddOrder(Order);
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine(ex.Message);
#endif
                    }
                }
            }
        }

        // Оглашение ботом бонусов
        private void AnnounceBonuses(BonusList Bonuses)
        {
            if (ActiveTable != null)
            {
                lock (ActiveTable)
                {
                    try
                    {
                        ActiveTable.AnnounceBonuses(ActivePlace, Bonuses);
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine(ex.Message);
#endif
                    }
                }
            }
        }

        // Совершение ботом хода
        private void MakeMove(Card MovedCard)
        {
            if (ActiveTable != null)
            {
                lock (ActiveTable)
                {
                    try
                    {
                        ActiveTable.PlayerMove(ActivePlace, MovedCard);
                    }
                    catch (Exception Ex)
                    {
#if DEBUG
                        Debug.WriteLine(Ex.Message);
#endif
                    }
                }
            }
        }

        // Отправка сообщения клиенту-боту
        public override void SendMessage(string message)
        {
            string command = Helpers.CommandFromStr(message);
            string msg = Helpers.MessageFromStr(message);
            ProcessMessage(command, msg);
        }

        public override int ID
        {
            get
            {
                return -ActivePlace;
            }
        }
    }
}
