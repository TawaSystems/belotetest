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
    public partial class CreatingTableForm : Form
    {
        private Game game;

        public CreatingTableForm(Game Game)
        {
            this.game = Game;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            game.CreateTable((int)BetUpDown.Value, TablePlayersVisibilityCheckBox.Checked,
                TableChatCheckBox.Checked, (int)LevelUpDown.Value, TableTableVisibilityCheckBox.Checked, TableVIPCheckBox.Checked,
                TableModerationCheckBox.Checked, TableAICheckBox.Checked);
            Close();
        }
    }
}
