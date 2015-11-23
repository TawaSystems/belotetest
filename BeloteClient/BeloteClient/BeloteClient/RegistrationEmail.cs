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
    public partial class RegistrationEmail : Form
    {
        private Game game;
        public RegistrationEmail(Game Game)
        {
            this.game = Game;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (PasswordTextBox.Text != PasswordConfirmTextBox.Text)
            {
                MessageBox.Show("Пароли не совпадают!");
                return;
            }
            if ((PasswordTextBox.Text == "") || (NicknameTextBox.Text == "") || (EmailTextBox.Text == "") || (CountryComboBox.Text == ""))
            {
                MessageBox.Show("Введены не все данные!");
                return;
            }
            string Sex = Helpers.BoolToString(MaleRadio.Checked);
            game.RegistrationEmail(EmailTextBox.Text, PasswordTextBox.Text, NicknameTextBox.Text, Sex, CountryComboBox.Text);
            Close();
        }
    }
}
