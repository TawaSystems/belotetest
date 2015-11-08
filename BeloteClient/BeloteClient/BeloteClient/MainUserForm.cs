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
        private int CurrentTableID;

        public MainUserForm(Game Game)
        {
            this.game = Game;
            CurrentTableID = -1;
            InitializeComponent();
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        public void UpdateTablesList()
        {
            TablesListBox.Items.Clear();
            this.game.Tables.Clear();
            this.game.ServerConnection.SendDataToServer(Messages.MESSAGE_TABLE_SELECT_ALL);
        }

        public void AddTable(int ID)
        {
            TablesListBox.Items.Add(ID);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            UpdateTablesList();
        }

        private void TablesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TablesListBox.SelectedIndex >= 0)
            {
                int ID = Int32.Parse(TablesListBox.SelectedItem.ToString());
                if (game.Tables[ID] != null)
                {
                    TableChatCheckBox.Checked = game.Tables[ID].Chat;
                    TableAICheckBox.Checked = game.Tables[ID].AI;
                    TableModerationCheckBox.Checked = game.Tables[ID].Moderation;
                    TableVIPCheckBox.Checked = game.Tables[ID].VIPOnly;
                    TableMinLevelLabel.Text = game.Tables[ID].MinimalLevel.ToString();
                    TableBetSizeLabel.Text = game.Tables[ID].Bet.ToString();
                    TablePlayersVisibilityCheckBox.Checked = game.Tables[ID].PlayersVisibility;
                    CurrentTableID = ID;
                    if (game.Players[game.Tables[ID].TableCreator] != null)
                    {
                        Player1Label.Text = game.Players[game.Tables[ID].TableCreator].Profile.Email;
                    }
                    else
                    {
                        Player1Label.Text = game.Tables[ID].TableCreator.ToString();
                    }
                    if (game.Tables[ID].Player2 != -1)
                    {
                        if (game.Players[game.Tables[ID].Player2] != null)
                        {
                            Player2Label.Text = game.Players[game.Tables[ID].Player2].Profile.Email;
                        }
                        else
                        if (game.Tables[ID].Player2 < -1)
                        {
                            Player2Label.Text = "Бот";
                        }
                        else
                        {
                            Player2Label.Text = game.Tables[ID].Player2.ToString();
                        }
                    }
                    else
                    {
                        Player2Label.Text = "Пусто (Сесть)";
                    }
                    if (game.Tables[ID].Player3 != -1)
                    {
                        if (game.Players[game.Tables[ID].Player3] != null)
                        {
                            Player3Label.Text = game.Players[game.Tables[ID].Player3].Profile.Email;
                        }
                        else
                        if (game.Tables[ID].Player3 < -1)
                        {
                            Player3Label.Text = "Бот";
                        }
                        else
                        {
                            Player3Label.Text = game.Tables[ID].Player3.ToString();
                        }
                    }
                    else
                    {
                        Player3Label.Text = "Пусто (Сесть)";
                    }
                    if (game.Tables[ID].Player4 != -1)
                    {
                        if (game.Players[game.Tables[ID].Player4] != null)
                        {
                            Player4Label.Text = game.Players[game.Tables[ID].Player4].Profile.Email;
                        }
                        else
                        if (game.Tables[ID].Player4 < -1)
                        {
                            Player4Label.Text = "Бот";
                        }
                        else
                        {
                            Player4Label.Text = game.Tables[ID].Player4.ToString();
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
                CurrentTableID = -1;
                Player1Label.Text = "Пусто (Сесть)";
                Player2Label.Text = "Пусто (Сесть)";
                Player3Label.Text = "Пусто (Сесть)";
                Player4Label.Text = "Пусто (Сесть)";
            }
        }

        private void MainUserForm_Load(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            CreatingTableForm creatingTable = new CreatingTableForm(this.game);
            creatingTable.ShowDialog();
        }

        private void Player3Label_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (CurrentTableID >= 0)
            {
                if (game.Tables[CurrentTableID].Player3 == -1)
                {
                    game.Place = 3;
                    game.CurrentTable = game.Tables[CurrentTableID];
                    game.ServerConnection.SendDataToServer(String.Format("{0}ID={1},Place={2}", Messages.MESSAGE_TABLE_PLAYERS_ADD, game.Place));
                }
            }
        }
    }
}
