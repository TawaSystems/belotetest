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
    public partial class BetFormType4 : Form
    {
        Game game;
        public BetFormType4(Game game)
        {
            InitializeComponent();
            this.game = game;
        }

        private void RecontraButton_Click(object sender, EventArgs e)
        {
            game.MakeOrder(new Order(OrderType.ORDER_SURCOINCHE, 0, CardSuit.C_NONE));
            Close();
        }

        private void PassButton_Click(object sender, EventArgs e)
        {
            game.MakeOrder(new Order(OrderType.ORDER_PASS, 0, CardSuit.C_NONE));
            Close();
        }
    }
}
