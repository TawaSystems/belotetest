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
        public RegistrationEmail()
        {
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
            }
            string Sex = (MaleRadio.Checked ? "1" : "0");
            Program.Client.Registration(NicknameTextBox.Text, PasswordTextBox.Text, EmailTextBox.Text, CountryComboBox.Text, Sex);
        }
    }
}
