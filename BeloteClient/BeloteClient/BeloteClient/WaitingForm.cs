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
    public partial class WaitingForm : Form
    {
        private Game game;
        public WaitingForm(Game Game)
        {
            this.game = Game;
            InitializeComponent();
            CreatorPanel.Visible = (Game.Place == 1);
        }

        public void UpdateLabels()
        {
            button6.Enabled = (game.CurrentTable.Player2 == -1);
            button5.Enabled = (game.CurrentTable.Player3 == -1);
            button4.Enabled = (game.CurrentTable.Player4 == -1);
            button3.Enabled = (game.CurrentTable.Player2 < -1);
            button2.Enabled = (game.CurrentTable.Player3 < -1);
            button1.Enabled = (game.CurrentTable.Player4 < -1);
            if (game.Players[game.CurrentTable.TableCreator] != null)
            {
                Player1Label.Text = game.Players[game.CurrentTable.TableCreator].Profile.Email;
            }
            else
            {
                Player1Label.Text = game.CurrentTable.TableCreator.ToString();
            }

            if (game.CurrentTable.Player2 != -1)
            {
                if (game.Players[game.CurrentTable.Player2] != null)
                {
                    Player2Label.Text = game.Players[game.CurrentTable.Player2].Profile.Email;
                }
                else
                if (game.CurrentTable.Player2 < -1)
                {
                    Player2Label.Text = "Бот";
                }
                else
                {
                    Player2Label.Text = game.CurrentTable.Player2.ToString();
                }
            }
            else
            {
                Player2Label.Text = "Пусто";
            }

            if (game.CurrentTable.Player3 != -1)
            {
                if (game.Players[game.CurrentTable.Player3] != null)
                {
                    Player3Label.Text = game.Players[game.CurrentTable.Player3].Profile.Email;
                }
                else
                if (game.CurrentTable.Player3 < -1)
                {
                    Player3Label.Text = "Бот";
                }
                else
                {
                    Player3Label.Text = game.CurrentTable.Player3.ToString();
                }
            }
            else
            {
                Player3Label.Text = "Пусто";
            }

            if (game.CurrentTable.Player4 != -1)
            {
                if (game.Players[game.CurrentTable.Player4] != null)
                {
                    Player4Label.Text = game.Players[game.CurrentTable.Player4].Profile.Email;
                }
                else
                if (game.CurrentTable.Player4 < -1)
                {
                    Player4Label.Text = "Бот";
                }
                else
                {
                    Player4Label.Text = game.CurrentTable.Player4.ToString();
                }
            }
            else
            {
                Player4Label.Text = "Пусто";
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            game.ExitFromTable(true);
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
    }
}
