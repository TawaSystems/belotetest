namespace BeloteClient
{
    partial class BetFormType4
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
            this.RecontraButton = new System.Windows.Forms.Button();
            this.PassButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // RecontraButton
            // 
            this.RecontraButton.Location = new System.Drawing.Point(12, 30);
            this.RecontraButton.Name = "RecontraButton";
            this.RecontraButton.Size = new System.Drawing.Size(75, 23);
            this.RecontraButton.TabIndex = 0;
            this.RecontraButton.Text = "Реконтра!";
            this.RecontraButton.UseVisualStyleBackColor = true;
            this.RecontraButton.Click += new System.EventHandler(this.RecontraButton_Click);
            // 
            // PassButton
            // 
            this.PassButton.Location = new System.Drawing.Point(102, 30);
            this.PassButton.Name = "PassButton";
            this.PassButton.Size = new System.Drawing.Size(75, 23);
            this.PassButton.TabIndex = 1;
            this.PassButton.Text = "Пас";
            this.PassButton.UseVisualStyleBackColor = true;
            this.PassButton.Click += new System.EventHandler(this.PassButton_Click);
            // 
            // BetFormType4
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(195, 79);
            this.ControlBox = false;
            this.Controls.Add(this.PassButton);
            this.Controls.Add(this.RecontraButton);
            this.Name = "BetFormType4";
            this.Text = "Сделать заказ...";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button RecontraButton;
        private System.Windows.Forms.Button PassButton;
    }
}