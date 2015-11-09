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

        public void AddTableToListBox(Message Msg)
        {
            Dictionary<string, string> tParams = Helpers.SplitCommandString(Msg.Msg);
            Table t = new Table(game, tParams);
            TablesListBox.Items.Add(t.ID.ToString());
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            TablesListBox.Items.Clear();
            this.game.serverActions.GetAllPossibleTables();
        }

        private void TablesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            
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
