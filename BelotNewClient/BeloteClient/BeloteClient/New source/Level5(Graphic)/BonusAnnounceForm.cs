﻿using System;
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
    public partial class BonusAnnounceForm : Form
    {
        private Game game;

        public BonusAnnounceForm(Game game)
        {
            this.game = game;
            InitializeComponent();
            UpdateBonusCheckList();
        }
        
        private void UpdateBonusCheckList()
        {
            for (var i = 0; i < game.Information.GameData.Bonuses.Count; i++)
            {
                BonusesCheckList.Items.Add(CoordinatesTransmitor.TextFromBonusType(game.Information.GameData.Bonuses[i].Type));
            }
        }

        private string TextCardFromCard(CardType card)
        {
            switch (card)
            {
                case CardType.C_9:
                    return "9";
                case CardType.C_10:
                    return "10";
                case CardType.C_J:
                    return "Валет";
                case CardType.C_Q:
                    return "Дама";
                case CardType.C_K:
                    return "Король";
                case CardType.C_A:
                    return "Туз";
                default:
                    return "";
            }
        }

        private void BonusesCheckList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = BonusesCheckList.SelectedIndex;
            BonusInfoPanel.Visible = (index >= 0);
            if (index < 0)
            {
                return;
            }
            BonusTypeLabel.Text = String.Format("Тип бонуса: {0}", CoordinatesTransmitor.TextFromBonusType(game.Information.GameData.Bonuses[index].Type));
            if (game.Information.GameData.Bonuses[index].Suit != CardSuit.C_NONE)
                SuitImage.Image = suitesImageList.Images[(int)game.Information.GameData.Bonuses[index].Suit - 1];
            else
                SuitImage.Image = null;
            IsTrumpCheckBox.Visible = (game.Information.GameData.Bonuses[index].Type != BonusType.BONUS_4X);
            IsTrumpCheckBox.Checked = game.Information.GameData.Bonuses[index].IsTrump;
            if (game.Information.GameData.Bonuses[index].Type != BonusType.BONUS_4X)
            {
                HighCardLabel.Text = String.Format("Старшая карта: {0}", TextCardFromCard(game.Information.GameData.Bonuses[index].HighCard));
            }
            else
            {
                HighCardLabel.Text = String.Format("Карта: {0}", TextCardFromCard(game.Information.GameData.Bonuses[index].HighCard));
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            game.Information.GameData.Bonuses.Clear();
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (var i = BonusesCheckList.Items.Count - 1; i >= 0; i--)
            {
                if (!BonusesCheckList.CheckedItems.Contains(BonusesCheckList.Items[i]))
                {
                    game.Information.GameData.Bonuses.Delete(game.Information.GameData.Bonuses[i]);
                }
            }
            Close();
        }

        private void BonusesCheckList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            /*int index = BonusesCheckList.SelectedIndex;
            bool IsChecked = BonusesCheckList.CheckedItems.Contains(BonusesCheckList.Items[index]);
            if (game.Information.GameData.Bonuses.Count > 1)
            {
                for (var i = 0; i < game.Information.GameData.Bonuses.Count; i++)
                {
                    if (i != index)
                    {
                        if (game.Information.GameData.Bonuses[index].IsIntersect(game.Information.GameData.Bonuses[i]))
                        {
                            BonusesCheckList.
                        }
                    }
                }
            }*/
        }
    }
}
