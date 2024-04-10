namespace POAT
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ouvrirImage = new OpenFileDialog();
            bSeuillageAuto = new Button();
            buttonOuvrir = new Button();
            valeurSeuilAuto = new TextBox();
            imageSeuillee = new PictureBox();
            imageDepart = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)imageSeuillee).BeginInit();
            ((System.ComponentModel.ISupportInitialize)imageDepart).BeginInit();
            SuspendLayout();
            // 
            // ouvrirImage
            // 
            ouvrirImage.FileName = "openFileDialog1";
            ouvrirImage.Filter = "bmp | *.bmp*";
            ouvrirImage.Multiselect = true;
            // 
            // bSeuillageAuto
            // 
            bSeuillageAuto.Location = new Point(957, 477);
            bSeuillageAuto.Name = "bSeuillageAuto";
            bSeuillageAuto.Size = new Size(150, 46);
            bSeuillageAuto.TabIndex = 2;
            bSeuillageAuto.Text = "Go";
            bSeuillageAuto.UseVisualStyleBackColor = true;
            bSeuillageAuto.Click += bSeuillageAuto_Click;
            // 
            // buttonOuvrir
            // 
            buttonOuvrir.Location = new Point(398, 952);
            buttonOuvrir.Name = "buttonOuvrir";
            buttonOuvrir.Size = new Size(150, 46);
            buttonOuvrir.TabIndex = 3;
            buttonOuvrir.Text = "OpenFile";
            buttonOuvrir.UseVisualStyleBackColor = true;
            buttonOuvrir.Click += buttonOuvrir_Click_1;
            // 
            // valeurSeuilAuto
            // 
            valeurSeuilAuto.Location = new Point(1487, 956);
            valeurSeuilAuto.Name = "valeurSeuilAuto";
            valeurSeuilAuto.Size = new Size(200, 39);
            valeurSeuilAuto.TabIndex = 4;
            // 
            // imageSeuillee
            // 
            imageSeuillee.Location = new Point(1203, 136);
            imageSeuillee.Name = "imageSeuillee";
            imageSeuillee.Size = new Size(723, 699);
            imageSeuillee.SizeMode = PictureBoxSizeMode.StretchImage;
            imageSeuillee.TabIndex = 6;
            imageSeuillee.TabStop = false;
            // 
            // imageDepart
            // 
            imageDepart.Location = new Point(98, 136);
            imageDepart.Name = "imageDepart";
            imageDepart.Size = new Size(698, 699);
            imageDepart.TabIndex = 7;
            imageDepart.TabStop = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1981, 1047);
            Controls.Add(imageDepart);
            Controls.Add(imageSeuillee);
            Controls.Add(valeurSeuilAuto);
            Controls.Add(buttonOuvrir);
            Controls.Add(bSeuillageAuto);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)imageSeuillee).EndInit();
            ((System.ComponentModel.ISupportInitialize)imageDepart).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private OpenFileDialog ouvrirImage;
        private Button bSeuillageAuto;
        private Button buttonOuvrir;
        private TextBox valeurSeuilAuto;
        private PictureBox imageSeuillee;
        private PictureBox imageDepart;
    }
}