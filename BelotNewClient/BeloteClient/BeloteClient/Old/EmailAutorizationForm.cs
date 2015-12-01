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
    public partial class EmailAutorizationForm : Form
    {
        private Game game;
        public EmailAutorizationForm(Game Game)
        {
            game = Game;
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.game.AutorizationEmail(textBox1.Text, textBox2.Text);
            Close();
        }
    }
}
