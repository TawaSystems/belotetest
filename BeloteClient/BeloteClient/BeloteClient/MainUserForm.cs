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
    public partial class MainUserForm : Form
    {
        private Game game;
        public MainUserForm(Game Game)
        {
            this.game = Game;
            InitializeComponent();
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        public void UpdateTablesList()
        {
            TablesListBox.Items.Clear();
            this.game.Tables.Clear();
            this.game.ServerConnection.SendDataToServer("TSA");
        }

        public void AddTable(int ID)
        {
            TablesListBox.Items.Add(ID);
        }
    }
}
