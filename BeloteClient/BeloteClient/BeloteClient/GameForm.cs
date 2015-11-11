﻿using System;
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

        public void UpdateGraphics()
        {
            DrawPlayersCards();
            Player1Label.Text = "Игрок №" + game.GraphicPlaceToServerPlace(1).ToString();
            Player2Label.Text = "Игрок №" + game.GraphicPlaceToServerPlace(2).ToString();
            Player3Label.Text = "Игрок №" + game.GraphicPlaceToServerPlace(3).ToString();
            Player4Label.Text = "Игрок №" + game.GraphicPlaceToServerPlace(4).ToString();
            Player p1 = GetPlayerFromNumber(game.GraphicPlaceToServerPlace(1));
            Player p2 = GetPlayerFromNumber(game.GraphicPlaceToServerPlace(2));
            Player p3 = GetPlayerFromNumber(game.GraphicPlaceToServerPlace(3));
            Player p4 = GetPlayerFromNumber(game.GraphicPlaceToServerPlace(4));
            Player1Name.Text = (p1 != null) ? p1.Profile.Email : "Бот";
            Player2Name.Text = (p2 != null) ? p2.Profile.Email : "Бот";
            Player3Name.Text = (p3 != null) ? p3.Profile.Email : "Бот";
            Player4Name.Text = (p4 != null) ? p4.Profile.Email : "Бот";
            Player1AddLabel.Text = "";
            Player2AddLabel.Text = "";
            Player3AddLabel.Text = "";
            Player4AddLabel.Text = "";
            Player1BetLabel.Text = "";
            Player2BetLabel.Text = "";
            Player3BetLabel.Text = "";
            Player4BetLabel.Text = "";
            ScoreLocalLabel.Text = String.Format("C   {0} | {1}", game.LocalScore1, game.LocalScore2);
            ScoreSummLabel.Text = String.Format("S   {0} | {1}", game.TotalScore1, game.TotalScore2);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            game.QuitTable();
        }
    }
}
