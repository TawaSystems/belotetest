namespace BeloteClient
{
    partial class BonusAnnounceForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BonusAnnounceForm));
            this.BonusesCheckList = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.BonusInfoPanel = new System.Windows.Forms.Panel();
            this.IsTrumpCheckBox = new System.Windows.Forms.CheckBox();
            this.HighCardLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.BonusTypeLabel = new System.Windows.Forms.Label();
            this.SuitImage = new System.Windows.Forms.PictureBox();
            this.suitesImageList = new System.Windows.Forms.ImageList(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.BonusInfoPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SuitImage)).BeginInit();
            this.SuspendLayout();
            // 
            // BonusesCheckList
            // 
            this.BonusesCheckList.FormattingEnabled = true;
            this.BonusesCheckList.Location = new System.Drawing.Point(12, 24);
            this.BonusesCheckList.Name = "BonusesCheckList";
            this.BonusesCheckList.Size = new System.Drawing.Size(161, 169);
            this.BonusesCheckList.TabIndex = 0;
            this.BonusesCheckList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.BonusesCheckList_ItemCheck);
            this.BonusesCheckList.SelectedIndexChanged += new System.EventHandler(this.BonusesCheckList_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Возможные бонусы:";
            // 
            // BonusInfoPanel
            // 
            this.BonusInfoPanel.Controls.Add(this.IsTrumpCheckBox);
            this.BonusInfoPanel.Controls.Add(this.HighCardLabel);
            this.BonusInfoPanel.Controls.Add(this.label3);
            this.BonusInfoPanel.Controls.Add(this.BonusTypeLabel);
            this.BonusInfoPanel.Controls.Add(this.SuitImage);
            this.BonusInfoPanel.Location = new System.Drawing.Point(179, 24);
            this.BonusInfoPanel.Name = "BonusInfoPanel";
            this.BonusInfoPanel.Size = new System.Drawing.Size(160, 169);
            this.BonusInfoPanel.TabIndex = 2;
            this.BonusInfoPanel.Visible = false;
            // 
            // IsTrumpCheckBox
            // 
            this.IsTrumpCheckBox.AutoSize = true;
            this.IsTrumpCheckBox.Enabled = false;
            this.IsTrumpCheckBox.Location = new System.Drawing.Point(6, 94);
            this.IsTrumpCheckBox.Name = "IsTrumpCheckBox";
            this.IsTrumpCheckBox.Size = new System.Drawing.Size(109, 17);
            this.IsTrumpCheckBox.TabIndex = 7;
            this.IsTrumpCheckBox.Text = "Козырной бонус";
            this.IsTrumpCheckBox.UseVisualStyleBackColor = true;
            // 
            // HighCardLabel
            // 
            this.HighCardLabel.AutoSize = true;
            this.HighCardLabel.Location = new System.Drawing.Point(3, 64);
            this.HighCardLabel.Name = "HighCardLabel";
            this.HighCardLabel.Size = new System.Drawing.Size(86, 13);
            this.HighCardLabel.TabIndex = 6;
            this.HighCardLabel.Text = "Старшая карта:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Масть:";
            // 
            // BonusTypeLabel
            // 
            this.BonusTypeLabel.AutoSize = true;
            this.BonusTypeLabel.Location = new System.Drawing.Point(3, 9);
            this.BonusTypeLabel.Name = "BonusTypeLabel";
            this.BonusTypeLabel.Size = new System.Drawing.Size(67, 13);
            this.BonusTypeLabel.TabIndex = 4;
            this.BonusTypeLabel.Text = "Тип бонуса:";
            // 
            // SuitImage
            // 
            this.SuitImage.Location = new System.Drawing.Point(53, 35);
            this.SuitImage.Name = "SuitImage";
            this.SuitImage.Size = new System.Drawing.Size(16, 16);
            this.SuitImage.TabIndex = 3;
            this.SuitImage.TabStop = false;
            // 
            // suitesImageList
            // 
            this.suitesImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("suitesImageList.ImageStream")));
            this.suitesImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.suitesImageList.Images.SetKeyName(0, "трефы.png");
            this.suitesImageList.Images.SetKeyName(1, "червы.png");
            this.suitesImageList.Images.SetKeyName(2, "пики.png");
            this.suitesImageList.Images.SetKeyName(3, "бубны.png");
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 199);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(327, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Объявить выбранные бонусы";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 228);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(327, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "Не объявлять";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // BonusAnnounceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(351, 256);
            this.ControlBox = false;
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.BonusInfoPanel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BonusesCheckList);
            this.Name = "BonusAnnounceForm";
            this.Text = "Объявление бонусов...";
            this.BonusInfoPanel.ResumeLayout(false);
            this.BonusInfoPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SuitImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox BonusesCheckList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel BonusInfoPanel;
        private System.Windows.Forms.ImageList suitesImageList;
        private System.Windows.Forms.PictureBox SuitImage;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label BonusTypeLabel;
        private System.Windows.Forms.CheckBox IsTrumpCheckBox;
        private System.Windows.Forms.Label HighCardLabel;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}