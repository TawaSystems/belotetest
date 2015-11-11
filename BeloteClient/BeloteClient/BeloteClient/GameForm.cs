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

        private void DrawPlayersCards()
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
            for (var i = game.AllCards.Count; i < 8; i++)
            {
                ClearPlayerCard(i);
            }
        }

        public void UpdateGraphics()
        {
            DrawPlayersCards();
            Player1Name.Text = "Игрок №" + game.GraphicPlaceToServerPlace(1).ToString();
            Player2Name.Text = "Игрок №" + game.GraphicPlaceToServerPlace(2).ToString();
            Player3Name.Text = "Игрок №" + game.GraphicPlaceToServerPlace(3).ToString();
            Player4Name.Text = "Игрок №" + game.GraphicPlaceToServerPlace(4).ToString();
            Player1AddLabel.Text = game.Place.ToString();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            game.QuitTable();
        }
    }
}
