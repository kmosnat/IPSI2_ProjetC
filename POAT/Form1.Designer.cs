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
            components = new System.ComponentModel.Container();
            ouvrirImage = new OpenFileDialog();
            image_db = new PictureBox();
            treeView_in_sc = new TreeView();
            In_Sc_list = new ImageList(components);
            bouton_ouvrir = new Button();
            folderBrowserDialog1 = new FolderBrowserDialog();
            image_gt = new PictureBox();
            image_traitée = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)image_db).BeginInit();
            ((System.ComponentModel.ISupportInitialize)image_gt).BeginInit();
            ((System.ComponentModel.ISupportInitialize)image_traitée).BeginInit();
            SuspendLayout();
            // 
            // ouvrirImage
            // 
            ouvrirImage.FileName = "openFileDialog1";
            ouvrirImage.Filter = "bmp | *.bmp*";
            ouvrirImage.Multiselect = true;
            // 
            // image_db
            // 
            image_db.Location = new Point(370, 69);
            image_db.Name = "image_db";
            image_db.Size = new Size(402, 389);
            image_db.TabIndex = 7;
            image_db.TabStop = false;
            // 
            // treeView_in_sc
            // 
            treeView_in_sc.ImageIndex = 0;
            treeView_in_sc.ImageList = In_Sc_list;
            treeView_in_sc.Location = new Point(16, 69);
            treeView_in_sc.Margin = new Padding(4, 4, 4, 4);
            treeView_in_sc.Name = "treeView_in_sc";
            treeView_in_sc.SelectedImageIndex = 0;
            treeView_in_sc.Size = new Size(235, 858);
            treeView_in_sc.TabIndex = 8;
            treeView_in_sc.AfterSelect += treeView_in_sc_AfterSelect;
            // 
            // In_Sc_list
            // 
            In_Sc_list.ColorDepth = ColorDepth.Depth8Bit;
            In_Sc_list.ImageSize = new Size(16, 16);
            In_Sc_list.TransparentColor = Color.Transparent;
            // 
            // bouton_ouvrir
            // 
            bouton_ouvrir.Location = new Point(57, 18);
            bouton_ouvrir.Margin = new Padding(4, 4, 4, 4);
            bouton_ouvrir.Name = "bouton_ouvrir";
            bouton_ouvrir.Size = new Size(146, 44);
            bouton_ouvrir.TabIndex = 9;
            bouton_ouvrir.Text = "Ouvrir";
            bouton_ouvrir.UseVisualStyleBackColor = true;
            bouton_ouvrir.Click += bouton_ouvrir_Click;
            // 
            // image_gt
            // 
            image_gt.Location = new Point(370, 529);
            image_gt.Name = "image_gt";
            image_gt.Size = new Size(402, 389);
            image_gt.TabIndex = 10;
            image_gt.TabStop = false;
            // 
            // image_traitée
            // 
            image_traitée.Location = new Point(917, 271);
            image_traitée.Name = "image_traitée";
            image_traitée.Size = new Size(402, 389);
            image_traitée.TabIndex = 11;
            image_traitée.TabStop = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1565, 992);
            Controls.Add(image_traitée);
            Controls.Add(image_gt);
            Controls.Add(bouton_ouvrir);
            Controls.Add(treeView_in_sc);
            Controls.Add(image_db);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)image_db).EndInit();
            ((System.ComponentModel.ISupportInitialize)image_gt).EndInit();
            ((System.ComponentModel.ISupportInitialize)image_traitée).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private OpenFileDialog ouvrirImage;
        private Button bSeuillageAuto;
        private PictureBox imageSeuillee;
        private PictureBox image_db;
        private TreeView treeView_in_sc;
        private Button bouton_ouvrir;
        private ImageList In_Sc_list;
        private FolderBrowserDialog folderBrowserDialog1;
        private PictureBox image_gt;
        private PictureBox image_traitée;
    }
}