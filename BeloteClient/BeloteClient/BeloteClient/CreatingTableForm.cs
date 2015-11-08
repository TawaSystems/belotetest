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
            game.CurrentTable = new Table(game, -1, game.Player.Profile.Id, (int)BetUpDown.Value, TablePlayersVisibilityCheckBox.Checked,
                TableChatCheckBox.Checked, (int)LevelUpDown.Value, true, TableVIPCheckBox.Checked, TableModerationCheckBox.Checked, TableAICheckBox.Checked);
            game.ServerConnection.SendDataToServer(String.Format("{0}Bet={1},PlayersVisibility={2},Chat={3},MinimalLevel={4},TableVisibility={5},VIPOnly={6},Moderation={7},AI={8}",
                Messages.MESSAGE_TABLE_MODIFY_CREATE, game.CurrentTable.Bet, game.CurrentTable.PlayersVisibility, game.CurrentTable.Chat,
                game.CurrentTable.MinimalLevel, game.CurrentTable.TableVisibility, game.CurrentTable.VIPOnly, game.CurrentTable.Moderation,
                game.CurrentTable.AI));
            Close();
        }
    }
}
