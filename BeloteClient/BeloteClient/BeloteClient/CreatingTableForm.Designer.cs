namespace BeloteClient
{
    partial class CreatingTableForm
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
            this.TableAICheckBox = new System.Windows.Forms.CheckBox();
            this.TableModerationCheckBox = new System.Windows.Forms.CheckBox();
            this.TableVIPCheckBox = new System.Windows.Forms.CheckBox();
            this.TableChatCheckBox = new System.Windows.Forms.CheckBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.LevelUpDown = new System.Windows.Forms.NumericUpDown();
            this.BetUpDown = new System.Windows.Forms.NumericUpDown();
            this.button1 = new System.Windows.Forms.Button();
            this.TablePlayersVisibilityCheckBox = new System.Windows.Forms.CheckBox();
            this.TableTableVisibilityCheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.LevelUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BetUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // TableAICheckBox
            // 
            this.TableAICheckBox.AutoSize = true;
            this.TableAICheckBox.Location = new System.Drawing.Point(12, 81);
            this.TableAICheckBox.Name = "TableAICheckBox";
            this.TableAICheckBox.Size = new System.Drawing.Size(211, 17);
            this.TableAICheckBox.TabIndex = 21;
            this.TableAICheckBox.Text = "Возможность замены игроков на AI";
            this.TableAICheckBox.UseVisualStyleBackColor = true;
            // 
            // TableModerationCheckBox
            // 
            this.TableModerationCheckBox.AutoSize = true;
            this.TableModerationCheckBox.Location = new System.Drawing.Point(12, 58);
            this.TableModerationCheckBox.Name = "TableModerationCheckBox";
            this.TableModerationCheckBox.Size = new System.Drawing.Size(146, 17);
            this.TableModerationCheckBox.TabIndex = 20;
            this.TableModerationCheckBox.Text = "Наличие премодерации";
            this.TableModerationCheckBox.UseVisualStyleBackColor = true;
            // 
            // TableVIPCheckBox
            // 
            this.TableVIPCheckBox.AutoSize = true;
            this.TableVIPCheckBox.Location = new System.Drawing.Point(12, 35);
            this.TableVIPCheckBox.Name = "TableVIPCheckBox";
            this.TableVIPCheckBox.Size = new System.Drawing.Size(129, 17);
            this.TableVIPCheckBox.TabIndex = 19;
            this.TableVIPCheckBox.Text = "Стол только для VIP";
            this.TableVIPCheckBox.UseVisualStyleBackColor = true;
            // 
            // TableChatCheckBox
            // 
            this.TableChatCheckBox.AutoSize = true;
            this.TableChatCheckBox.Location = new System.Drawing.Point(12, 12);
            this.TableChatCheckBox.Name = "TableChatCheckBox";
            this.TableChatCheckBox.Size = new System.Drawing.Size(141, 17);
            this.TableChatCheckBox.TabIndex = 18;
            this.TableChatCheckBox.Text = "Наличие чата на столе";
            this.TableChatCheckBox.UseVisualStyleBackColor = true;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(249, 59);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(78, 13);
            this.label14.TabIndex = 17;
            this.label14.Text = "Мин. уровень:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(249, 13);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(87, 13);
            this.label13.TabIndex = 16;
            this.label13.Text = "Размер ставки:";
            // 
            // LevelUpDown
            // 
            this.LevelUpDown.Location = new System.Drawing.Point(252, 81);
            this.LevelUpDown.Maximum = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.LevelUpDown.Name = "LevelUpDown";
            this.LevelUpDown.Size = new System.Drawing.Size(120, 20);
            this.LevelUpDown.TabIndex = 23;
            // 
            // BetUpDown
            // 
            this.BetUpDown.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.BetUpDown.Location = new System.Drawing.Point(252, 36);
            this.BetUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.BetUpDown.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.BetUpDown.Name = "BetUpDown";
            this.BetUpDown.Size = new System.Drawing.Size(120, 20);
            this.BetUpDown.TabIndex = 24;
            this.BetUpDown.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(252, 117);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(120, 23);
            this.button1.TabIndex = 25;
            this.button1.Text = "Создать стол";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // TablePlayersVisibilityCheckBox
            // 
            this.TablePlayersVisibilityCheckBox.AutoSize = true;
            this.TablePlayersVisibilityCheckBox.Location = new System.Drawing.Point(12, 104);
            this.TablePlayersVisibilityCheckBox.Name = "TablePlayersVisibilityCheckBox";
            this.TablePlayersVisibilityCheckBox.Size = new System.Drawing.Size(126, 17);
            this.TablePlayersVisibilityCheckBox.TabIndex = 26;
            this.TablePlayersVisibilityCheckBox.Text = "Видимость игроков";
            this.TablePlayersVisibilityCheckBox.UseVisualStyleBackColor = true;
            // 
            // TableTableVisibilityCheckBox
            // 
            this.TableTableVisibilityCheckBox.AutoSize = true;
            this.TableTableVisibilityCheckBox.Checked = true;
            this.TableTableVisibilityCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.TableTableVisibilityCheckBox.Location = new System.Drawing.Point(12, 127);
            this.TableTableVisibilityCheckBox.Name = "TableTableVisibilityCheckBox";
            this.TableTableVisibilityCheckBox.Size = new System.Drawing.Size(114, 17);
            this.TableTableVisibilityCheckBox.TabIndex = 27;
            this.TableTableVisibilityCheckBox.Text = "Видимость стола";
            this.TableTableVisibilityCheckBox.UseVisualStyleBackColor = true;
            // 
            // CreatingTableForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(405, 161);
            this.Controls.Add(this.TableTableVisibilityCheckBox);
            this.Controls.Add(this.TablePlayersVisibilityCheckBox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.BetUpDown);
            this.Controls.Add(this.LevelUpDown);
            this.Controls.Add(this.TableAICheckBox);
            this.Controls.Add(this.TableModerationCheckBox);
            this.Controls.Add(this.TableVIPCheckBox);
            this.Controls.Add(this.TableChatCheckBox);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Name = "CreatingTableForm";
            this.Text = "CreatingTableForm";
            ((System.ComponentModel.ISupportInitialize)(this.LevelUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BetUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox TableAICheckBox;
        private System.Windows.Forms.CheckBox TableModerationCheckBox;
        private System.Windows.Forms.CheckBox TableVIPCheckBox;
        private System.Windows.Forms.CheckBox TableChatCheckBox;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.NumericUpDown LevelUpDown;
        private System.Windows.Forms.NumericUpDown BetUpDown;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox TablePlayersVisibilityCheckBox;
        private System.Windows.Forms.CheckBox TableTableVisibilityCheckBox;
    }
}