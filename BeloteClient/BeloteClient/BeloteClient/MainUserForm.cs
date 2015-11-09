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
    public partial class MainUserForm : Form
    {
        private Game game;

        public MainUserForm(Game Game)
        {
            this.game = Game;
            InitializeComponent();
        }

        public void UpdateTables()
        {
            TablesListBox.Items.Clear();
            TablesListBox.SelectedIndex = -1;
            game.UpdatePossibleTables();
            if (game.Tables == null)
            {
                MessageBox.Show("Нет доступных столов!");
            }
            else
            {
                for (var i = 0; i < game.Tables.Count; i++)
                {
                    TablesListBox.Items.Add(game.Tables.GetTableAt(i).ID.ToString());
                }
            }
        }
        
        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            UpdateTables();
        }

        private void TablesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TablesListBox.SelectedIndex >= 0)
            {
                int ID = Int32.Parse(TablesListBox.SelectedItem.ToString());
                Table t = game.Tables[ID];
                if (t != null)
                {
                    TableChatCheckBox.Checked = t.Chat;
                    TableAICheckBox.Checked = t.AI;
                    TableModerationCheckBox.Checked = t.Moderation;
                    TableVIPCheckBox.Checked = t.VIPOnly;
                    TableMinLevelLabel.Text = t.MinimalLevel.ToString();
                    TableBetSizeLabel.Text = t.Bet.ToString();
                    TablePlayersVisibilityCheckBox.Checked = t.PlayersVisibility;
                    if (game.Players[t.TableCreator] != null)
                    {
                        Player1Label.Text = game.Players[t.TableCreator].Profile.Email;
                    }
                    else
                    {
                        Player1Label.Text = t.TableCreator.ToString();
                    }
                    if (t.Player2 != -1)
                    {
                        if (game.Players[t.Player2] != null)
                        {
                            Player2Label.Text = game.Players[t.Player2].Profile.Email;
                        }
                        else
                        if (t.Player2 < -1)
                        {
                            Player2Label.Text = "Бот";
                        }
                        else
                        {
                            Player2Label.Text = t.Player2.ToString();
                        }
                    }
                    else
                    {
                        Player2Label.Text = "Пусто (Сесть)";
                    }
                    if (t.Player3 != -1)
                    {
                        if (game.Players[t.Player3] != null)
                        {
                            Player3Label.Text = game.Players[t.Player3].Profile.Email;
                        }
                        else
                        if (t.Player3 < -1)
                        {
                            Player3Label.Text = "Бот";
                        }
                        else
                        {
                            Player3Label.Text = t.Player3.ToString();
                        }
                    }
                    else
                    {
                        Player3Label.Text = "Пусто (Сесть)";
                    }
                    if (t.Player4 != -1)
                    {
                        if (game.Players[t.Player4] != null)
                        {
                            Player4Label.Text = game.Players[t.Player4].Profile.Email;
                        }
                        else
                        if (t.Player4 < -1)
                        {
                            Player4Label.Text = "Бот";
                        }
                        else
                        {
                            Player4Label.Text = t.Player4.ToString();
                        }
                    }
                    else
                    {
                        Player4Label.Text = "Пусто (Сесть)";
                    }
                }
            }
            else
            {
                TableChatCheckBox.Checked = false;
                TableAICheckBox.Checked = false;
                TableModerationCheckBox.Checked = false;
                TableVIPCheckBox.Checked = false;
                TablePlayersVisibilityCheckBox.Checked = false;
                TableMinLevelLabel.Text = "0";
                TableBetSizeLabel.Text = "0";
                Player1Label.Text = "Пусто (Сесть)";
                Player2Label.Text = "Пусто (Сесть)";
                Player3Label.Text = "Пусто (Сесть)";
                Player4Label.Text = "Пусто (Сесть)";
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            CreatingTableForm creatingTable = new CreatingTableForm(this.game);
            creatingTable.ShowDialog();
        }

        private void Player3Label_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
        }

        private void Player2Label_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
           
        }

        private void Player4Label_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
           
        }
    }
}
