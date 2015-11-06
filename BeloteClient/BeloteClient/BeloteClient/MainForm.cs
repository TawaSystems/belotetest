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
    public partial class MainGuestForm : Form
    {
        private Game game;
        public MainGuestForm(Game Game)
        {
            this.game = Game;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void сПомощьюEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RegistrationEmail regEmailForm = new RegistrationEmail(game);
            regEmailForm.ShowDialog();
        }

        private void войтиСПомощьюEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EmailAutorizationForm form = new EmailAutorizationForm(game);
            form.ShowDialog();
        }

        private void MainGuestForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
    }
}
