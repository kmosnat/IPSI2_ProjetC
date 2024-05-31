namespace POAT
{
    partial class Filter
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
            label1 = new Label();
            okButton = new Button();
            quitButton = new Button();
            label2 = new Label();
            kenelSizeTextBox = new TextBox();
            comboBox = new ComboBox();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(16, 31);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(283, 32);
            label1.TabIndex = 0;
            label1.Text = "Entrez la taille du noyau :";
            // 
            // okButton
            // 
            okButton.Location = new Point(604, 91);
            okButton.Margin = new Padding(4);
            okButton.Name = "okButton";
            okButton.Size = new Size(146, 44);
            okButton.TabIndex = 1;
            okButton.Text = "OK";
            okButton.UseVisualStyleBackColor = true;
            okButton.Click += okButton_Click_1;
            // 
            // quitButton
            // 
            quitButton.Location = new Point(604, 217);
            quitButton.Margin = new Padding(4);
            quitButton.Name = "quitButton";
            quitButton.Size = new Size(146, 44);
            quitButton.TabIndex = 2;
            quitButton.Text = "Quitter";
            quitButton.UseVisualStyleBackColor = true;
            quitButton.Click += quitButton_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(16, 293);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(364, 32);
            label2.TabIndex = 3;
            label2.Text = "Choissisez l'élément structurant :";
            // 
            // kenelSizeTextBox
            // 
            kenelSizeTextBox.Location = new Point(47, 91);
            kenelSizeTextBox.Margin = new Padding(4);
            kenelSizeTextBox.Name = "kenelSizeTextBox";
            kenelSizeTextBox.Size = new Size(194, 39);
            kenelSizeTextBox.TabIndex = 4;
            kenelSizeTextBox.Text = "3";
            // 
            // comboBox
            // 
            comboBox.DisplayMember = "Disque";
            comboBox.FormattingEnabled = true;
            comboBox.Location = new Point(47, 376);
            comboBox.Margin = new Padding(4);
            comboBox.Name = "comboBox";
            comboBox.Size = new Size(235, 40);
            comboBox.TabIndex = 5;
            // 
            // Filter
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(803, 450);
            ControlBox = false;
            Controls.Add(comboBox);
            Controls.Add(kenelSizeTextBox);
            Controls.Add(label2);
            Controls.Add(quitButton);
            Controls.Add(okButton);
            Controls.Add(label1);
            Margin = new Padding(4);
            Name = "Filter";
            Text = "Filtre";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Button okButton;
        private Button quitButton;
        private Label label2;
        private TextBox kenelSizeTextBox;
        private ComboBox comboBox;
    }
}