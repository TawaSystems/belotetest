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
    public partial class BetFromType123 : Form
    {
        BetType bType;
        Game game;
        public BetFromType123(Game game)
        {
            InitializeComponent();
            this.game = game;
        }

        public void ShowForm(int MinBetSize, BetType betType)
        {
            OrderSizeUpDown.Minimum = MinBetSize;
            OrderSizeUpDown.Value = MinBetSize;
            CapotCheck.Enabled = (MinBetSize >= 250);
            bType = betType;
            switch (betType)
            {
                case BetType.T_BET:
                    {
                        ContraButton.Visible = false;
                        break;
                    }
                case BetType.BET_CAPOT:
                    {
                        CapotCheck.Checked = true;
                        CapotCheck.Enabled = false;
                        break;
                    }
            }
            ShowDialog();
        }
        private CardSuit GetOrderSuit()
        {
            if (WithoutTrumpCheck.Checked)
                return CardSuit.C_NONE;
            if (HeartsRadio.Checked)
                return CardSuit.C_HEARTS;
            if (ClubsRadio.Checked)
                return CardSuit.C_CLUBS;
            if (SpadesRadio.Checked)
                return CardSuit.C_SPADES;
            if (DiamondsRadio.Checked)
                return CardSuit.С_DIAMONDS;
            return CardSuit.C_NONE;
        }

        private void OrderSizeUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (bType != BetType.BET_CAPOT)
                CapotCheck.Enabled = (OrderSizeUpDown.Value >= 250);
        }

        private void WithoutTrumpCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (WithoutTrumpCheck.Checked)
            {
                HeartsRadio.Checked = false;
                ClubsRadio.Checked = false;
                DiamondsRadio.Checked = false;
                SpadesRadio.Checked = false;
            }
            else
            {
                DiamondsRadio.Checked = true;
            }
        }

        private void BetButton_Click(object sender, EventArgs e)
        {
            OrderType oType = OrderType.ORDER_BET;
            CardSuit oSuit = GetOrderSuit();
            int oSize = (int)OrderSizeUpDown.Value; 
            if (CapotCheck.Checked)
            {
                oType = OrderType.ORDER_CAPOT;
            }
            Order order = new Order(oType, oSize, oSuit);
            game.MakeOrder(order);
            Close();
        }

        private void ContraButton_Click(object sender, EventArgs e)
        {
            game.MakeOrder(new Order(OrderType.ORDER_COINCHE, 0, CardSuit.C_NONE));
            Close();
        }

        private void PassButton_Click(object sender, EventArgs e)
        {
            game.MakeOrder(new Order(OrderType.ORDER_PASS, 0, CardSuit.C_NONE));
            Close();
        }
    }
}
