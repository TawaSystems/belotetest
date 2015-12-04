using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace BeloteClient
{
    public partial class GameForm : Form
    {
        private Game game;
        private PictureBox[] pCards;
        public GameForm(Game game)
        {
            this.game = game;
            InitializeComponent();
            pCards = new PictureBox[8];
            pCards[0] = PlayerCard1PB;
            pCards[1] = PlayerCard2PB;
            pCards[2] = PlayerCard3PB;
            pCards[3] = PlayerCard4PB;
            pCards[4] = PlayerCard5PB;
            pCards[5] = PlayerCard6PB;
            pCards[6] = PlayerCard7PB;
            pCards[7] = PlayerCard8PB;
        }

        private void ClearPlayerCard(int i)
        {
            pCards[i].Image = null;
            pCards[i].Invalidate();
        }

        // Возвращает профиль игрока по его серверному номеру. Если это бот то возвращает 0
        private Player GetPlayerFromNumber(int ServerNum)
        {
            int PlayerID;
            switch (ServerNum)
            {
                case 1:
                    {
                        PlayerID = game.Information.CurrentTable.TableCreator;
                        break;
                    }
                case 2:
                    {
                        PlayerID = game.Information.CurrentTable.Player2;
                        break;
                    }
                case 3:
                    {
                        PlayerID = game.Information.CurrentTable.Player3;
                        break;
                    }
                case 4:
                    {
                        PlayerID = game.Information.CurrentTable.Player4;
                        break;
                    }
                default:
                    {
                        PlayerID = -1;
                        break;
                    }
            }
            if (PlayerID < 0)
                return null;
            return game.Information.Players[PlayerID];
        }

        // Возвращает ссылку на пикчербокс нужной карты
        private PictureBox PictureBoxFromNumber(int num)
        {
            switch (num)
            {
                case 0:
                    return PlayerCard1PB;
                case 1:
                    return PlayerCard2PB;
                case 2:
                    return PlayerCard3PB;
                case 3:
                    return PlayerCard4PB;
                case 4:
                    return PlayerCard5PB;
                case 5:
                    return PlayerCard6PB;
                case 6:
                    return PlayerCard7PB;
                case 7:
                    return PlayerCard8PB;
                default:
                    return null;
            }
        }

        // Рисует игровые карты и рубашки
        private void DrawPlayersCards()
        {
            if (game.Information.GameData.AllCards == null)
                return;
            if (game.Information.GameData.AllCards.Count > 0)
            {
                for (var i = 0; i < game.Information.GameData.AllCards.Count; i++)
                {
                    switch (game.Information.GameData.AllCards[i].Suit)
                    {
                        case CardSuit.C_CLUBS:
                            {
                                pCards[i].Image = Clubs.Images[(int)game.Information.GameData.AllCards[i].Type];
                                break;
                            }
                        case CardSuit.C_HEARTS:
                            {
                                pCards[i].Image = Hearts.Images[(int)game.Information.GameData.AllCards[i].Type];
                                break;
                            }
                        case CardSuit.C_SPADES:
                            {
                                pCards[i].Image = Spades.Images[(int)game.Information.GameData.AllCards[i].Type];
                                break;
                            }
                        case CardSuit.С_DIAMONDS:
                            {
                                pCards[i].Image = Diamonds.Images[(int)game.Information.GameData.AllCards[i].Type];
                                break;
                            }
                    }
                }
                Player2Back.Image = backImageList.Images[0];
                Player3Back.Image = backImageList.Images[0];
                Player4Back.Image = backImageList.Images[0];
            }
            else
            {
                Player2Back.Image = null;
                Player3Back.Image = null;
                Player4Back.Image = null;
            }
            for (var i = game.Information.GameData.AllCards.Count; i < 8; i++)
            {
                ClearPlayerCard(i);
            }
        }

        // Отображение хоженой карты
        public void DrawMovedCard(int ServerPlace, Card Card)
        {
            int graphicPlace = CoordinatesTransmitor.ServerPlaceToGraphicPlace(ServerPlace, game.Information.Place);
            PictureBox cardBox = null;
            switch (graphicPlace)
            {
                case 1:
                    {
                        cardBox = Player1MoveCard;
                        break;
                    }
                case 2:
                    {
                        cardBox = Player2MoveCard;
                        break;
                    }
                case 3:
                    {
                        cardBox = Player3MoveCard;
                        break;
                    }
                case 4:
                    {
                        cardBox = Player4MoveCard;
                        break;
                    }
            }
            if (Card != null)
            {
                switch (Card.Suit)
                {
                    case CardSuit.C_CLUBS:
                        {
                            cardBox.Image = Clubs.Images[(int)Card.Type];
                            break;
                        }
                    case CardSuit.C_HEARTS:
                        {
                            cardBox.Image = Hearts.Images[(int)Card.Type];
                            break;
                        }
                    case CardSuit.С_DIAMONDS:
                        {
                            cardBox.Image = Diamonds.Images[(int)Card.Type];
                            break;
                        }
                    case CardSuit.C_SPADES:
                        {
                            cardBox.Image = Spades.Images[(int)Card.Type];
                            break;
                        }
                }
            }
            else
                cardBox.Image = null;
            cardBox.Invalidate();
        }

        // Отрисовка карт последней взятки
        private void DrawLastBribeCard(int ServerPlace, Card Card)
        {
            int graphicPlace = CoordinatesTransmitor.ServerPlaceToGraphicPlace(ServerPlace, game.Information.Place);
            PictureBox cardBox = null;
            switch (graphicPlace)
            {
                case 1:
                    {
                        cardBox = LastBribePlayer1PB;
                        break;
                    }
                case 2:
                    {
                        cardBox = LastBribePlayer2PB;
                        break;
                    }
                case 3:
                    {
                        cardBox = LastBribePlayer3PB;
                        break;
                    }
                case 4:
                    {
                        cardBox = LastBribePlayer4PB;
                        break;
                    }
            }
            if (Card != null)
            {
                switch (Card.Suit)
                {
                    case CardSuit.C_CLUBS:
                        {
                            cardBox.Image = Clubs.Images[(int)Card.Type];
                            break;
                        }
                    case CardSuit.C_HEARTS:
                        {
                            cardBox.Image = Hearts.Images[(int)Card.Type];
                            break;
                        }
                    case CardSuit.С_DIAMONDS:
                        {
                            cardBox.Image = Diamonds.Images[(int)Card.Type];
                            break;
                        }
                    case CardSuit.C_SPADES:
                        {
                            cardBox.Image = Spades.Images[(int)Card.Type];
                            break;
                        }
                }
            }
            else
                cardBox.Image = null;
            cardBox.Invalidate();
        }

        // Переводит тип заказа в строку для отображения
        private string OrderTypeToString(OrderType o)
        {
            switch (o)
            {
                case OrderType.ORDER_BET:
                    {
                        return "";
                    }
                case OrderType.ORDER_CAPOT:
                    {
                        return "КАПУТ!";
                    }
                case OrderType.ORDER_COINCHE:
                    {
                        return "КОНТРА!";
                    }
                case OrderType.ORDER_SURCOINCHE:
                    {
                        return "РЕКОНТРА!";
                    }
                case OrderType.ORDER_PASS:
                    {
                        return "ПАС!";
                    }
                default:
                    {
                        return "";
                    }
            }
        }
        
        // Отрисовывает информацию о сделанной игроком ставке
        private void UpdatePlayerAddInfoBazar(int serverNumber, Order order)
        {
            int graphicNumber = CoordinatesTransmitor.ServerPlaceToGraphicPlace(serverNumber, game.Information.Place);
            switch (graphicNumber)
            {
                case 1:
                    {
                        Player1AddLabel.Text = (order != null) ? OrderTypeToString(order.Type) : "";
                        if (order != null)
                        {
                            Player1BetLabel.Text = (order.Size != 0) ? order.Size.ToString() : "";
                        }
                        else
                            Player1BetLabel.Text = "";
                        if ((order != null) && ((order.Type == OrderType.ORDER_BET) || (order.Type == OrderType.ORDER_CAPOT)))
                            Player1BetSuit.Image = suitesImageList.Images[((int)order.Trump)];
                        else
                            Player1BetSuit.Image = null;
                        break;
                    }
                case 2:
                    {
                        Player2AddLabel.Text = (order != null) ? OrderTypeToString(order.Type) : "";
                        if (order != null)
                        {
                            Player2BetLabel.Text = (order.Size != 0) ? order.Size.ToString() : "";
                        }
                        else
                            Player2BetLabel.Text = "";
                        if ((order != null) && ((order.Type == OrderType.ORDER_BET) || (order.Type == OrderType.ORDER_CAPOT)))
                            Player2BetSuit.Image = suitesImageList.Images[((int)order.Trump)];
                        else
                            Player2BetSuit.Image = null;
                        break;
                    }
                case 3:
                    {
                        Player3AddLabel.Text = (order != null) ? OrderTypeToString(order.Type) : "";
                        if (order != null)
                        {
                            Player3BetLabel.Text = (order.Size != 0) ? order.Size.ToString() : "";
                        }
                        else
                            Player3BetLabel.Text = "";
                        if ((order != null) && ((order.Type == OrderType.ORDER_BET) || (order.Type == OrderType.ORDER_CAPOT)))
                            Player3BetSuit.Image = suitesImageList.Images[((int)order.Trump)];
                        else
                            Player3BetSuit.Image = null;
                        break;
                    }
                case 4:
                    {
                        Player4AddLabel.Text = (order != null) ? OrderTypeToString(order.Type) : "";
                        if (order != null)
                        {
                            Player4BetLabel.Text = (order.Size != 0) ? order.Size.ToString() : "";
                        }
                        else
                            Player4BetLabel.Text = "";
                        if ((order != null) && ((order.Type == OrderType.ORDER_BET) || (order.Type == OrderType.ORDER_CAPOT)))
                            Player4BetSuit.Image = suitesImageList.Images[((int)order.Trump)];
                        else
                            Player4BetSuit.Image = null;
                        break;
                    }
            }
        }

        // Отображает информацию об оглашенных типах бонусов
        private void UpdateBonusesTypes(int serverNumber, string bonusType)
        {
            int graphicNumber = CoordinatesTransmitor.ServerPlaceToGraphicPlace(serverNumber, game.Information.Place);
            switch (graphicNumber)
            {
                case 1:
                    {
                        Player1AddLabel.Text = (bonusType == null) ? "" : bonusType;
                        break;
                    }
                case 2:
                    {
                        Player2AddLabel.Text = (bonusType == null) ? "" : bonusType;
                        break;
                    }
                case 3:
                    {
                        Player3AddLabel.Text = (bonusType == null) ? "" : bonusType;
                        break;
                    }
                case 4:
                    {
                        Player4AddLabel.Text = (bonusType == null) ? "" : bonusType;
                        break;
                    }
            }
        }

        // Отображение надписей БЛОТ и РЕБЛОТ
        private void UpdateBeloteRebelote(int serverNumber, string beloteText)
        {
            int graphicNumber = CoordinatesTransmitor.ServerPlaceToGraphicPlace(serverNumber, game.Information.Place);
            switch (graphicNumber)
            {
                case 1:
                    {
                        Player1AddLabel.Text = beloteText;
                        break;
                    }
                case 2:
                    {
                        Player2AddLabel.Text = beloteText;
                        break;
                    }
                case 3:
                    {
                        Player3AddLabel.Text = beloteText;
                        break;
                    }
                case 4:
                    {
                        Player4AddLabel.Text = beloteText;
                        break;
                    }
            }
        }

        // Делает изображение черно-белым
        private Bitmap MakeImageBlackWhite(Bitmap bmp)
        {
            ImageAttributes ia = new ImageAttributes();
            ColorMatrix cm = new ColorMatrix();
            cm.Matrix00 = cm.Matrix01 = cm.Matrix02 =
            cm.Matrix10 = cm.Matrix11 = cm.Matrix12 =
            cm.Matrix20 = cm.Matrix21 = cm.Matrix22 = 0.34f;

            ia.SetColorMatrix(cm);

            Graphics g = Graphics.FromImage(bmp);

            g.DrawImage(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, ia);
            g.DrawLine(Pens.Red, new Point(0, 0), new Point(bmp.Width, bmp.Height));
            g.DrawLine(Pens.Red, new Point(bmp.Width, 0), new Point(0, bmp.Height));

            return bmp;
        }

        // Обновляет графику на экране
        public void UpdateGraphics()
        {
            try
            {
                // Обновление игровых карт
                DrawPlayersCards();
                // Обновление номеров игроков
                Player1Label.Text = "Игрок №" + CoordinatesTransmitor.GraphicPlaceToServerPlace(1, game.Information.Place).ToString();
                Player2Label.Text = "Игрок №" + CoordinatesTransmitor.GraphicPlaceToServerPlace(2, game.Information.Place).ToString();
                Player3Label.Text = "Игрок №" + CoordinatesTransmitor.GraphicPlaceToServerPlace(3, game.Information.Place).ToString();
                Player4Label.Text = "Игрок №" + CoordinatesTransmitor.GraphicPlaceToServerPlace(4, game.Information.Place).ToString();
                // Обновление имен (емейлов) игроков
                Player p1 = GetPlayerFromNumber(CoordinatesTransmitor.GraphicPlaceToServerPlace(1, game.Information.Place));
                Player p2 = GetPlayerFromNumber(CoordinatesTransmitor.GraphicPlaceToServerPlace(2, game.Information.Place));
                Player p3 = GetPlayerFromNumber(CoordinatesTransmitor.GraphicPlaceToServerPlace(3, game.Information.Place));
                Player p4 = GetPlayerFromNumber(CoordinatesTransmitor.GraphicPlaceToServerPlace(4, game.Information.Place));
                Player1Name.Text = (p1 != null) ? p1.Profile.Email : "Бот";
                Player2Name.Text = (p2 != null) ? p2.Profile.Email : "Бот";
                Player3Name.Text = (p3 != null) ? p3.Profile.Email : "Бот";
                Player4Name.Text = (p4 != null) ? p4.Profile.Email : "Бот";
                // Обновление информации по заявкам
                UpdatePlayerAddInfoBazar(1, game.Information.GameData.Orders[1]);
                UpdatePlayerAddInfoBazar(2, game.Information.GameData.Orders[2]);
                UpdatePlayerAddInfoBazar(3, game.Information.GameData.Orders[3]);
                UpdatePlayerAddInfoBazar(4, game.Information.GameData.Orders[4]);
                // Обновление счета
                ScoreLocalLabel.Text = String.Format("C   {0} | {1}", game.Information.GameData.LocalScores[BeloteTeam.TEAM1_1_3], game.Information.GameData.LocalScores[BeloteTeam.TEAM2_2_4]);
                ScoreSummLabel.Text = String.Format("S   {0} | {1}", game.Information.GameData.TotalScores[BeloteTeam.TEAM1_1_3], game.Information.GameData.TotalScores[BeloteTeam.TEAM2_2_4]);
                ScoreLastLabel.Text = String.Format("L   {0} | {1}", game.Information.GameData.LastDistributionScores[BeloteTeam.TEAM1_1_3], game.Information.GameData.LastDistributionScores[BeloteTeam.TEAM2_2_4]);
                
            
                // Обновление информациио конечном заказе в раздаче
                EndOrderPanel.Visible = (game.Information.GameData.GameStatus != TableStatus.BAZAR);
                if (game.Information.GameData.GameStatus != TableStatus.BAZAR)
                {
                    Player1AddLabel.Text = "";
                    Player2AddLabel.Text = "";
                    Player3AddLabel.Text = "";
                    Player4AddLabel.Text = "";

                    if (game.Information.GameData.Orders.EndOrder != null)
                    {
                        EndOrderSizeLabel.Text = String.Format("Заказ: {0}", game.Information.GameData.Orders.EndOrder.Size);
                        EndOrderSuit.Image = suitesImageList.Images[(int)game.Information.GameData.Orders.EndOrder.Trump];
                        EndOrderTeam.Text = String.Format("Команда: {0} - {1}", (int)game.Information.GameData.Orders.EndOrder.Team, (game.Information.GameData.Orders.EndOrder.Team == BeloteTeam.TEAM1_1_3) ? "№1,3" : "№2,4");
                        switch (game.Information.GameData.Orders.EndOrder.Type)
                        {
                            case OrderType.ORDER_CAPOT:
                                {
                                    EndOrderTypeLabel.Text = "KA";
                                    break;
                                }
                            case OrderType.ORDER_COINCHE:
                                {
                                    EndOrderTypeLabel.Text = "C";
                                    break;
                                }
                            case OrderType.ORDER_SURCOINCHE:
                                {
                                    EndOrderTypeLabel.Text = "SC";
                                    break;
                                }
                            default:
                                {
                                    EndOrderTypeLabel.Text = "";
                                    break;
                                }
                        }
                    }
                }
                // Обновление картинок последней взятки
                if (game.Information.GameData.GameStatus == TableStatus.BAZAR)
                {
                    LastBribePlayer1PB.Image = null;
                    LastBribePlayer2PB.Image = null;
                    LastBribePlayer3PB.Image = null;
                    LastBribePlayer4PB.Image = null;
                }
                else
                {
                    if (game.Information.GameData.Bribes.PredBribe != null)
                    {
                        for (var i = 1; i <= 4; i++)
                            DrawLastBribeCard(i, game.Information.GameData.Bribes.PredBribe[i]);
                    }
                }
                // Отображение типов объявленных бонусов
                /*if (game.Information.GameData.GameStatus == TableStatus.BONUSES)
                {
                    UpdateBonusesTypes(1, game.Player1BonusesTypes);
                    UpdateBonusesTypes(2, game.Player2BonusesTypes);
                    UpdateBonusesTypes(3, game.Player3BonusesTypes);
                    UpdateBonusesTypes(4, game.Player4BonusesTypes);
                }*/
                // Если игроку позволяется сделать ход, то делаем неактивными карты, которыми нельзя ходить
                MakingMovePanel.Visible = game.Information.GameData.IsMakingMove;
                if ((game.Information.GameData.IsMakingMove) && (game.Information.GameData.GameStatus != TableStatus.BAZAR))
                {
                    for (var i = 0; i < game.Information.GameData.AllCards.Count; i++)
                    {
                        if (!game.Information.GameData.PossibleCards.Exists(game.Information.GameData.AllCards[i]))
                        {
                            PictureBox pb = PictureBoxFromNumber(i);
                            pb.Image = (Image)MakeImageBlackWhite(new Bitmap(pb.Image));
                            pb.Invalidate();
                        }
                    }
                }
                // Обновление похоженных карт
                if (game.Information.GameData.Bribes.CurrentBribe != null)
                {
                    for (var i = 1; i <= 4; i++)
                        DrawMovedCard(i, game.Information.GameData.Bribes.CurrentBribe[i]);
                    // Обновление блот и реблот
                    if (game.Information.GameData.Bribes.CurrentBribe.BelotePlace != 0)
                        UpdateBeloteRebelote(game.Information.GameData.Bribes.CurrentBribe.BelotePlace, "BELOTE");
                    if (game.Information.GameData.Bribes.CurrentBribe.RebelotePlace != 0)
                        UpdateBeloteRebelote(game.Information.GameData.Bribes.CurrentBribe.RebelotePlace, "REBELOTE");

                }
                else
                {
                    for (var i = 1; i <= 4; i++)
                        DrawMovedCard(i, null);
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Ошибка при отрисовки: " + Ex.Message);
            }

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            game.QuitTable();
        }

        private void PlayerCard1PB_Click(object sender, EventArgs e)
        {
            if (!game.Information.GameData.IsMakingMove)
                return;
            if ((sender as PictureBox).Image == null)
                return;
            int cardIndex = Int32.Parse((sender as PictureBox).Tag.ToString());
            if (!game.Information.GameData.PossibleCards.Exists(game.Information.GameData.AllCards[cardIndex]))
                return;
            game.MakeMove(cardIndex);
        }
    }
}
