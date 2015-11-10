namespace BeloteClient
{
    partial class WaitingForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.CreatorPanel = new System.Windows.Forms.Panel();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.Player1Label = new System.Windows.Forms.Label();
            this.Player2Label = new System.Windows.Forms.Label();
            this.Player3Label = new System.Windows.Forms.Label();
            this.Player4Label = new System.Windows.Forms.Label();
            this.button7 = new System.Windows.Forms.Button();
            this.CreatorPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // CreatorPanel
            // 
            this.CreatorPanel.Controls.Add(this.button4);
            this.CreatorPanel.Controls.Add(this.button5);
            this.CreatorPanel.Controls.Add(this.button6);
            this.CreatorPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.CreatorPanel.Location = new System.Drawing.Point(0, 192);
            this.CreatorPanel.Name = "CreatorPanel";
            this.CreatorPanel.Size = new System.Drawing.Size(303, 102);
            this.CreatorPanel.TabIndex = 0;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(12, 68);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(135, 22);
            this.button4.TabIndex = 5;
            this.button4.Text = "Посадить бота №4";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(12, 40);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(135, 22);
            this.button5.TabIndex = 4;
            this.button5.Text = "Посадить бота №3";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(12, 12);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(135, 22);
            this.button6.TabIndex = 3;
            this.button6.Text = "Посадить бота №2";
            this.button6.UseVisualStyleBackColor = true;
            // 
            // Player1Label
            // 
            this.Player1Label.AutoSize = true;
            this.Player1Label.Location = new System.Drawing.Point(128, 20);
            this.Player1Label.Name = "Player1Label";
            this.Player1Label.Size = new System.Drawing.Size(35, 13);
            this.Player1Label.TabIndex = 1;
            this.Player1Label.Text = "label1";
            // 
            // Player2Label
            // 
            this.Player2Label.AutoSize = true;
            this.Player2Label.Location = new System.Drawing.Point(189, 74);
            this.Player2Label.Name = "Player2Label";
            this.Player2Label.Size = new System.Drawing.Size(35, 13);
            this.Player2Label.TabIndex = 2;
            this.Player2Label.Text = "label2";
            // 
            // Player3Label
            // 
            this.Player3Label.AutoSize = true;
            this.Player3Label.Location = new System.Drawing.Point(128, 136);
            this.Player3Label.Name = "Player3Label";
            this.Player3Label.Size = new System.Drawing.Size(35, 13);
            this.Player3Label.TabIndex = 3;
            this.Player3Label.Text = "label3";
            // 
            // Player4Label
            // 
            this.Player4Label.AutoSize = true;
            this.Player4Label.Location = new System.Drawing.Point(63, 74);
            this.Player4Label.Name = "Player4Label";
            this.Player4Label.Size = new System.Drawing.Size(35, 13);
            this.Player4Label.TabIndex = 4;
            this.Player4Label.Text = "label4";
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(192, 163);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(96, 23);
            this.button7.TabIndex = 5;
            this.button7.Text = "Выйти со стола";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // WaitingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(303, 294);
            this.ControlBox = false;
            this.Controls.Add(this.button7);
            this.Controls.Add(this.Player4Label);
            this.Controls.Add(this.Player3Label);
            this.Controls.Add(this.Player2Label);
            this.Controls.Add(this.Player1Label);
            this.Controls.Add(this.CreatorPanel);
            this.Name = "WaitingForm";
            this.Text = "Ожидание игроков...";
            this.CreatorPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel CreatorPanel;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Label Player1Label;
        private System.Windows.Forms.Label Player2Label;
        private System.Windows.Forms.Label Player3Label;
        private System.Windows.Forms.Label Player4Label;
        private System.Windows.Forms.Button button7;
    }
}