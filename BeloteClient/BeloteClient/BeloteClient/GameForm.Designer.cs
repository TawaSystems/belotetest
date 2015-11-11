namespace BeloteClient
{
    partial class GameForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameForm));
            this.suitesImageList = new System.Windows.Forms.ImageList(this.components);
            this.CloseButton = new System.Windows.Forms.PictureBox();
            this.Hearts = new System.Windows.Forms.ImageList(this.components);
            this.Spades = new System.Windows.Forms.ImageList(this.components);
            this.Diamonds = new System.Windows.Forms.ImageList(this.components);
            this.Clubs = new System.Windows.Forms.ImageList(this.components);
            this.PlayerCard1PB = new System.Windows.Forms.PictureBox();
            this.PlayerCard2PB = new System.Windows.Forms.PictureBox();
            this.PlayerCard3PB = new System.Windows.Forms.PictureBox();
            this.PlayerCard4PB = new System.Windows.Forms.PictureBox();
            this.PlayerCard5PB = new System.Windows.Forms.PictureBox();
            this.PlayerCard6PB = new System.Windows.Forms.PictureBox();
            this.PlayerCard7PB = new System.Windows.Forms.PictureBox();
            this.PlayerCard8PB = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.CloseButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PlayerCard1PB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PlayerCard2PB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PlayerCard3PB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PlayerCard4PB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PlayerCard5PB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PlayerCard6PB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PlayerCard7PB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PlayerCard8PB)).BeginInit();
            this.SuspendLayout();
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
            // CloseButton
            // 
            this.CloseButton.Image = ((System.Drawing.Image)(resources.GetObject("CloseButton.Image")));
            this.CloseButton.Location = new System.Drawing.Point(0, 0);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(30, 30);
            this.CloseButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.CloseButton.TabIndex = 0;
            this.CloseButton.TabStop = false;
            this.CloseButton.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // Hearts
            // 
            this.Hearts.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("Hearts.ImageStream")));
            this.Hearts.TransparentColor = System.Drawing.Color.Transparent;
            this.Hearts.Images.SetKeyName(0, "H0.jpg");
            this.Hearts.Images.SetKeyName(1, "H1.jpg");
            this.Hearts.Images.SetKeyName(2, "H2.JPG");
            this.Hearts.Images.SetKeyName(3, "H3.JPG");
            this.Hearts.Images.SetKeyName(4, "H4.JPG");
            this.Hearts.Images.SetKeyName(5, "H5.JPG");
            this.Hearts.Images.SetKeyName(6, "H6.JPG");
            this.Hearts.Images.SetKeyName(7, "H7.JPG");
            // 
            // Spades
            // 
            this.Spades.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("Spades.ImageStream")));
            this.Spades.TransparentColor = System.Drawing.Color.Transparent;
            this.Spades.Images.SetKeyName(0, "S0.jpg");
            this.Spades.Images.SetKeyName(1, "S1.jpg");
            this.Spades.Images.SetKeyName(2, "S2.JPG");
            this.Spades.Images.SetKeyName(3, "S3.JPG");
            this.Spades.Images.SetKeyName(4, "S4.JPG");
            this.Spades.Images.SetKeyName(5, "S5.JPG");
            this.Spades.Images.SetKeyName(6, "S6.JPG");
            this.Spades.Images.SetKeyName(7, "S7.JPG");
            // 
            // Diamonds
            // 
            this.Diamonds.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("Diamonds.ImageStream")));
            this.Diamonds.TransparentColor = System.Drawing.Color.Transparent;
            this.Diamonds.Images.SetKeyName(0, "D0.jpg");
            this.Diamonds.Images.SetKeyName(1, "D1.jpg");
            this.Diamonds.Images.SetKeyName(2, "D2.JPG");
            this.Diamonds.Images.SetKeyName(3, "D3.JPG");
            this.Diamonds.Images.SetKeyName(4, "D4.JPG");
            this.Diamonds.Images.SetKeyName(5, "D5.JPG");
            this.Diamonds.Images.SetKeyName(6, "D6.JPG");
            this.Diamonds.Images.SetKeyName(7, "D7.JPG");
            // 
            // Clubs
            // 
            this.Clubs.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("Clubs.ImageStream")));
            this.Clubs.TransparentColor = System.Drawing.Color.Transparent;
            this.Clubs.Images.SetKeyName(0, "C0.jpg");
            this.Clubs.Images.SetKeyName(1, "C1.jpg");
            this.Clubs.Images.SetKeyName(2, "C2.JPG");
            this.Clubs.Images.SetKeyName(3, "C3.JPG");
            this.Clubs.Images.SetKeyName(4, "C4.JPG");
            this.Clubs.Images.SetKeyName(5, "C5.JPG");
            this.Clubs.Images.SetKeyName(6, "C6.JPG");
            this.Clubs.Images.SetKeyName(7, "C7.JPG");
            // 
            // PlayerCard1PB
            // 
            this.PlayerCard1PB.BackColor = System.Drawing.Color.Transparent;
            this.PlayerCard1PB.Location = new System.Drawing.Point(60, 420);
            this.PlayerCard1PB.Name = "PlayerCard1PB";
            this.PlayerCard1PB.Size = new System.Drawing.Size(60, 83);
            this.PlayerCard1PB.TabIndex = 2;
            this.PlayerCard1PB.TabStop = false;
            // 
            // PlayerCard2PB
            // 
            this.PlayerCard2PB.BackColor = System.Drawing.Color.Transparent;
            this.PlayerCard2PB.Location = new System.Drawing.Point(150, 420);
            this.PlayerCard2PB.Name = "PlayerCard2PB";
            this.PlayerCard2PB.Size = new System.Drawing.Size(60, 83);
            this.PlayerCard2PB.TabIndex = 3;
            this.PlayerCard2PB.TabStop = false;
            // 
            // PlayerCard3PB
            // 
            this.PlayerCard3PB.BackColor = System.Drawing.Color.Transparent;
            this.PlayerCard3PB.Location = new System.Drawing.Point(240, 420);
            this.PlayerCard3PB.Name = "PlayerCard3PB";
            this.PlayerCard3PB.Size = new System.Drawing.Size(60, 83);
            this.PlayerCard3PB.TabIndex = 4;
            this.PlayerCard3PB.TabStop = false;
            // 
            // PlayerCard4PB
            // 
            this.PlayerCard4PB.BackColor = System.Drawing.Color.Transparent;
            this.PlayerCard4PB.Location = new System.Drawing.Point(330, 420);
            this.PlayerCard4PB.Name = "PlayerCard4PB";
            this.PlayerCard4PB.Size = new System.Drawing.Size(60, 83);
            this.PlayerCard4PB.TabIndex = 5;
            this.PlayerCard4PB.TabStop = false;
            // 
            // PlayerCard5PB
            // 
            this.PlayerCard5PB.BackColor = System.Drawing.Color.Transparent;
            this.PlayerCard5PB.Location = new System.Drawing.Point(420, 420);
            this.PlayerCard5PB.Name = "PlayerCard5PB";
            this.PlayerCard5PB.Size = new System.Drawing.Size(60, 83);
            this.PlayerCard5PB.TabIndex = 6;
            this.PlayerCard5PB.TabStop = false;
            // 
            // PlayerCard6PB
            // 
            this.PlayerCard6PB.BackColor = System.Drawing.Color.Transparent;
            this.PlayerCard6PB.Location = new System.Drawing.Point(510, 420);
            this.PlayerCard6PB.Name = "PlayerCard6PB";
            this.PlayerCard6PB.Size = new System.Drawing.Size(60, 83);
            this.PlayerCard6PB.TabIndex = 7;
            this.PlayerCard6PB.TabStop = false;
            // 
            // PlayerCard7PB
            // 
            this.PlayerCard7PB.BackColor = System.Drawing.Color.Transparent;
            this.PlayerCard7PB.Location = new System.Drawing.Point(600, 420);
            this.PlayerCard7PB.Name = "PlayerCard7PB";
            this.PlayerCard7PB.Size = new System.Drawing.Size(60, 83);
            this.PlayerCard7PB.TabIndex = 8;
            this.PlayerCard7PB.TabStop = false;
            // 
            // PlayerCard8PB
            // 
            this.PlayerCard8PB.BackColor = System.Drawing.Color.Transparent;
            this.PlayerCard8PB.Location = new System.Drawing.Point(690, 420);
            this.PlayerCard8PB.Name = "PlayerCard8PB";
            this.PlayerCard8PB.Size = new System.Drawing.Size(60, 83);
            this.PlayerCard8PB.TabIndex = 9;
            this.PlayerCard8PB.TabStop = false;
            // 
            // GameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(944, 531);
            this.ControlBox = false;
            this.Controls.Add(this.PlayerCard8PB);
            this.Controls.Add(this.PlayerCard7PB);
            this.Controls.Add(this.PlayerCard6PB);
            this.Controls.Add(this.PlayerCard5PB);
            this.Controls.Add(this.PlayerCard4PB);
            this.Controls.Add(this.PlayerCard3PB);
            this.Controls.Add(this.PlayerCard2PB);
            this.Controls.Add(this.PlayerCard1PB);
            this.Controls.Add(this.CloseButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximumSize = new System.Drawing.Size(960, 570);
            this.MinimumSize = new System.Drawing.Size(960, 570);
            this.Name = "GameForm";
            this.Text = "GameForm";
            ((System.ComponentModel.ISupportInitialize)(this.CloseButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PlayerCard1PB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PlayerCard2PB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PlayerCard3PB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PlayerCard4PB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PlayerCard5PB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PlayerCard6PB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PlayerCard7PB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PlayerCard8PB)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ImageList suitesImageList;
        private System.Windows.Forms.PictureBox CloseButton;
        private System.Windows.Forms.ImageList Hearts;
        private System.Windows.Forms.ImageList Spades;
        private System.Windows.Forms.ImageList Diamonds;
        private System.Windows.Forms.ImageList Clubs;
        private System.Windows.Forms.PictureBox PlayerCard1PB;
        private System.Windows.Forms.PictureBox PlayerCard2PB;
        private System.Windows.Forms.PictureBox PlayerCard3PB;
        private System.Windows.Forms.PictureBox PlayerCard4PB;
        private System.Windows.Forms.PictureBox PlayerCard5PB;
        private System.Windows.Forms.PictureBox PlayerCard6PB;
        private System.Windows.Forms.PictureBox PlayerCard7PB;
        private System.Windows.Forms.PictureBox PlayerCard8PB;
    }
}