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
            CreatorPanel.Visible = (game.Place == 1);
            UpdateLabels();
        }

        public void UpdateLabels()
        {
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

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
