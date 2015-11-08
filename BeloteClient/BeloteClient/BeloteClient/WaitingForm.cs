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
