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
    public partial class WaitingForm : Form
    {
        private Game game;
        public WaitingForm(Game Game)
        {
            this.game = Game;
            InitializeComponent();
            CreatorPanel.Visible = (Game.Information.Place == 1);
        }

        public void UpdateLabels()
        {
            button6.Enabled = (game.Information.CurrentTable.Player2 == -1);
            button5.Enabled = (game.Information.CurrentTable.Player3 == -1);
            button4.Enabled = (game.Information.CurrentTable.Player4 == -1);
            button3.Enabled = (game.Information.CurrentTable.Player2 < -1);
            button2.Enabled = (game.Information.CurrentTable.Player3 < -1);
            button1.Enabled = (game.Information.CurrentTable.Player4 < -1);
            if (game.Information.Players[game.Information.CurrentTable.TableCreator] != null)
            {
                Player1Label.Text = game.Information.Players[game.Information.CurrentTable.TableCreator].Profile.Email;
            }
            else
            {
                Player1Label.Text = game.Information.CurrentTable.TableCreator.ToString();
            }

            if (game.Information.CurrentTable.Player2 != -1)
            {
                if (game.Information.Players[game.Information.CurrentTable.Player2] != null)
                {
                    Player2Label.Text = game.Information.Players[game.Information.CurrentTable.Player2].Profile.Email;
                }
                else
                if (game.Information.CurrentTable.Player2 < -1)
                {
                    Player2Label.Text = "Бот";
                }
                else
                {
                    Player2Label.Text = game.Information.CurrentTable.Player2.ToString();
                }
            }
            else
            {
                Player2Label.Text = "Пусто";
            }

            if (game.Information.CurrentTable.Player3 != -1)
            {
                if (game.Information.Players[game.Information.CurrentTable.Player3] != null)
                {
                    Player3Label.Text = game.Information.Players[game.Information.CurrentTable.Player3].Profile.Email;
                }
                else
                if (game.Information.CurrentTable.Player3 < -1)
                {
                    Player3Label.Text = "Бот";
                }
                else
                {
                    Player3Label.Text = game.Information.CurrentTable.Player3.ToString();
                }
            }
            else
            {
                Player3Label.Text = "Пусто";
            }

            if (game.Information.CurrentTable.Player4 != -1)
            {
                if (game.Information.Players[game.Information.CurrentTable.Player4] != null)
                {
                    Player4Label.Text = game.Information.Players[game.Information.CurrentTable.Player4].Profile.Email;
                }
                else
                if (game.Information.CurrentTable.Player4 < -1)
                {
                    Player4Label.Text = "Бот";
                }
                else
                {
                    Player4Label.Text = game.Information.CurrentTable.Player4.ToString();
                }
            }
            else
            {
                Player4Label.Text = "Пусто";
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            game.ExitFromWaitingTable();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            game.AddBot(2);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            game.AddBot(3);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            game.AddBot(4);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            game.DeleteBot(2);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            game.DeleteBot(3);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            game.DeleteBot(4);
        }

        private void WaitingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            game.Information.OnUpdateWaitingTable = null;
        }
    }
}
