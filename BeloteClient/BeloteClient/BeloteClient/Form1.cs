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
        public MainGuestForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void сПомощьюEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RegistrationEmail regEmailForm = new RegistrationEmail();
            regEmailForm.ShowDialog();
        }
    }
}
