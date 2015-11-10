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
        public GameForm(Game game)
        {
            this.game = game;
            InitializeComponent();
        }

        public void UpdateGraphics()
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            game.QuitTable();
        }
    }
}
