using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
                        PlayerID = game.CurrentTable.TableCreator;
                        break;
                    }
                case 2:
                    {
                        PlayerID = game.CurrentTable.Player2;
                        break;
                    }
                case 3:
                    {
                        PlayerID = game.CurrentTable.Player3;
                        break;
                    }
                case 4:
                    {
                        PlayerID = game.CurrentTable.Player4;
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
            return game.Players[PlayerID];
        }

        // Рисует игровые карты и рубашки
        private void DrawPlayersCards()
        {
            if (game.AllCards.Count > 0)
            {
                for (var i = 0; i < game.AllCards.Count; i++)
                {
                    switch (game.AllCards[i].Suit)
                    {
                        case CardSuit.C_CLUBS:
                            {
                                pCards[i].Image = Clubs.Images[(int)game.AllCards[i].Type];
                                break;
                            }
                        case CardSuit.C_HEARTS:
                            {
                                pCards[i].Image = Hearts.Images[(int)game.AllCards[i].Type];
                                break;
                            }
                        case CardSuit.C_SPADES:
                            {
                                pCards[i].Image = Spades.Images[(int)game.AllCards[i].Type];
                                break;
                            }
                        case CardSuit.С_DIAMONDS:
                            {
                                pCards[i].Image = Diamonds.Images[(int)game.AllCards[i].Type];
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
            for (var i = game.AllCards.Count; i < 8; i++)
            {
                ClearPlayerCard(i);
            }
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
            int graphicNumber = game.ServerPlaceToGraphicPlace(serverNumber);
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
                        if ((order == null) || (order.Trump == CardSuit.C_NONE))
                            Player1BetSuit.Image = null;
                        else
                            Player1BetSuit.Image = suitesImageList.Images[((int)order.Trump) - 1];
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
                        if ((order == null) || (order.Trump == CardSuit.C_NONE))
                            Player2BetSuit.Image = null;
                        else
                            Player2BetSuit.Image = suitesImageList.Images[((int)order.Trump) - 1];
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
                        if ((order == null) || (order.Trump == CardSuit.C_NONE))
                            Player3BetSuit.Image = null;
                        else
                            Player3BetSuit.Image = suitesImageList.Images[((int)order.Trump) - 1];
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
                        if ((order == null) || (order.Trump == CardSuit.C_NONE))
                            Player4BetSuit.Image = null;
                        else
                            Player4BetSuit.Image = suitesImageList.Images[((int)order.Trump) - 1];
                        break;
                    }
            }
        }

        // Обновляет графику на экране
        public void UpdateGraphics()
        {
            try
            {
                // Обновление игровых карт
                DrawPlayersCards();
                // Обновление номеров игроков
                Player1Label.Text = "Игрок №" + game.GraphicPlaceToServerPlace(1).ToString();
                Player2Label.Text = "Игрок №" + game.GraphicPlaceToServerPlace(2).ToString();
                Player3Label.Text = "Игрок №" + game.GraphicPlaceToServerPlace(3).ToString();
                Player4Label.Text = "Игрок №" + game.GraphicPlaceToServerPlace(4).ToString();
                // Обновление имен (емейлов) игроков
                Player p1 = GetPlayerFromNumber(game.GraphicPlaceToServerPlace(1));
                Player p2 = GetPlayerFromNumber(game.GraphicPlaceToServerPlace(2));
                Player p3 = GetPlayerFromNumber(game.GraphicPlaceToServerPlace(3));
                Player p4 = GetPlayerFromNumber(game.GraphicPlaceToServerPlace(4));
                Player1Name.Text = (p1 != null) ? p1.Profile.Email : "Бот";
                Player2Name.Text = (p2 != null) ? p2.Profile.Email : "Бот";
                Player3Name.Text = (p3 != null) ? p3.Profile.Email : "Бот";
                Player4Name.Text = (p4 != null) ? p4.Profile.Email : "Бот";
                // Обновление информации по заявкам
                if (game.Status == TableStatus.BAZAR)
                {
                    // Обновление счета
                    UpdatePlayerAddInfoBazar(1, game.Player1Order);
                    UpdatePlayerAddInfoBazar(2, game.Player2Order);
                    UpdatePlayerAddInfoBazar(3, game.Player3Order);
                    UpdatePlayerAddInfoBazar(4, game.Player4Order);
                }
                ScoreLocalLabel.Text = String.Format("C   {0} | {1}", game.LocalScore1, game.LocalScore2);
                ScoreSummLabel.Text = String.Format("S   {0} | {1}", game.TotalScore1, game.TotalScore2);
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
    }
}
