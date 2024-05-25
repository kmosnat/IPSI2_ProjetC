namespace POAT
{
    partial class ProjetC
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
            image_db = new PictureBox();
            treeView_in_sc = new TreeView();
            In_Sc_list = new ImageList(components);
            folderBrowserDialog1 = new FolderBrowserDialog();
            image_gt = new PictureBox();
            image_traitee = new PictureBox();
            menuStrip1 = new MenuStrip();
            fichierToolStripMenuItem = new ToolStripMenuItem();
            ouvrirDossierToolStripMenuItem = new ToolStripMenuItem();
            filtreToolStripMenuItem = new ToolStripMenuItem();
            moyenToolStripMenuItem = new ToolStripMenuItem();
            medianToolStripMenuItem = new ToolStripMenuItem();
            rotationToolStripMenuItem = new ToolStripMenuItem();
            horaireToolStripMenuItem = new ToolStripMenuItem();
            antihoraireToolStripMenuItem = new ToolStripMenuItem();
            zoomToolStripMenuItem = new ToolStripMenuItem();
            avantToolStripMenuItem = new ToolStripMenuItem();
            arrièreToolStripMenuItem = new ToolStripMenuItem();
            resetToolStripMenuItem = new ToolStripMenuItem();
            iou_label = new Label();
            vinet_label = new Label();
            img_comparaison = new PictureBox();
            label_db = new Label();
            label_gt = new Label();
            label_imageTraitée = new Label();
            label1 = new Label();
            panel1 = new Panel();
            panel2 = new Panel();
            panel3 = new Panel();
            panel4 = new Panel();
            ((System.ComponentModel.ISupportInitialize)image_db).BeginInit();
            ((System.ComponentModel.ISupportInitialize)image_gt).BeginInit();
            ((System.ComponentModel.ISupportInitialize)image_traitee).BeginInit();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)img_comparaison).BeginInit();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            panel4.SuspendLayout();
            SuspendLayout();
            // 
            // image_db
            // 
            image_db.BorderStyle = BorderStyle.FixedSingle;
            image_db.Location = new Point(3, 3);
            image_db.Name = "image_db";
            image_db.Size = new Size(378, 359);
            image_db.SizeMode = PictureBoxSizeMode.StretchImage;
            image_db.TabIndex = 7;
            image_db.TabStop = false;
            // 
            // treeView_in_sc
            // 
            treeView_in_sc.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
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
            In_Sc_list.ImageSize = new Size(30, 30);
            In_Sc_list.TransparentColor = Color.Transparent;
            // 
            // image_gt
            // 
            image_gt.BorderStyle = BorderStyle.FixedSingle;
            image_gt.Location = new Point(3, 3);
            image_gt.Name = "image_gt";
            image_gt.Size = new Size(378, 359);
            image_gt.SizeMode = PictureBoxSizeMode.StretchImage;
            image_gt.TabIndex = 10;
            image_gt.TabStop = false;
            // 
            // image_traitee
            // 
            image_traitee.BorderStyle = BorderStyle.FixedSingle;
            image_traitee.Location = new Point(3, 0);
            image_traitee.Name = "image_traitee";
            image_traitee.Size = new Size(378, 359);
            image_traitee.SizeMode = PictureBoxSizeMode.StretchImage;
            image_traitee.TabIndex = 11;
            image_traitee.TabStop = false;
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(32, 32);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fichierToolStripMenuItem, filtreToolStripMenuItem, rotationToolStripMenuItem, zoomToolStripMenuItem, resetToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(6, 3, 0, 3);
            menuStrip1.Size = new Size(1424, 42);
            menuStrip1.TabIndex = 12;
            menuStrip1.Text = "menuStrip1";
            // 
            // fichierToolStripMenuItem
            // 
            fichierToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { ouvrirDossierToolStripMenuItem });
            fichierToolStripMenuItem.Name = "fichierToolStripMenuItem";
            fichierToolStripMenuItem.Size = new Size(104, 36);
            fichierToolStripMenuItem.Text = "Fichier";
            // 
            // ouvrirDossierToolStripMenuItem
            // 
            ouvrirDossierToolStripMenuItem.Name = "ouvrirDossierToolStripMenuItem";
            ouvrirDossierToolStripMenuItem.Size = new Size(330, 44);
            ouvrirDossierToolStripMenuItem.Text = "Ouvrir un dossier";
            ouvrirDossierToolStripMenuItem.Click += ouvrirDossierToolStripMenuItem_Click;
            // 
            // filtreToolStripMenuItem
            // 
            filtreToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { moyenToolStripMenuItem, medianToolStripMenuItem });
            filtreToolStripMenuItem.Name = "filtreToolStripMenuItem";
            filtreToolStripMenuItem.Size = new Size(97, 36);
            filtreToolStripMenuItem.Text = "Filtres";
            // 
            // moyenToolStripMenuItem
            // 
            moyenToolStripMenuItem.Name = "moyenToolStripMenuItem";
            moyenToolStripMenuItem.Size = new Size(228, 44);
            moyenToolStripMenuItem.Text = "Moyen";
            moyenToolStripMenuItem.Click += moyenToolStripMenuItem_Click;
            // 
            // medianToolStripMenuItem
            // 
            medianToolStripMenuItem.Name = "medianToolStripMenuItem";
            medianToolStripMenuItem.Size = new Size(228, 44);
            medianToolStripMenuItem.Text = "Median";
            medianToolStripMenuItem.Click += medianToolStripMenuItem_Click;
            // 
            // rotationToolStripMenuItem
            // 
            rotationToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { horaireToolStripMenuItem, antihoraireToolStripMenuItem });
            rotationToolStripMenuItem.Name = "rotationToolStripMenuItem";
            rotationToolStripMenuItem.Size = new Size(123, 36);
            rotationToolStripMenuItem.Text = "Rotation";
            // 
            // horaireToolStripMenuItem
            // 
            horaireToolStripMenuItem.Name = "horaireToolStripMenuItem";
            horaireToolStripMenuItem.Size = new Size(278, 44);
            horaireToolStripMenuItem.Text = "Horaire";
            horaireToolStripMenuItem.Click += horaireToolStripMenuItem_Click;
            // 
            // antihoraireToolStripMenuItem
            // 
            antihoraireToolStripMenuItem.Name = "antihoraireToolStripMenuItem";
            antihoraireToolStripMenuItem.Size = new Size(278, 44);
            antihoraireToolStripMenuItem.Text = "Anti-Horaire";
            antihoraireToolStripMenuItem.Click += antihoraireToolStripMenuItem_Click;
            // 
            // zoomToolStripMenuItem
            // 
            zoomToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { avantToolStripMenuItem, arrièreToolStripMenuItem });
            zoomToolStripMenuItem.Name = "zoomToolStripMenuItem";
            zoomToolStripMenuItem.Size = new Size(97, 36);
            zoomToolStripMenuItem.Text = "Zoom";
            // 
            // avantToolStripMenuItem
            // 
            avantToolStripMenuItem.Name = "avantToolStripMenuItem";
            avantToolStripMenuItem.Size = new Size(218, 44);
            avantToolStripMenuItem.Text = "Avant";
            avantToolStripMenuItem.Click += avantToolStripMenuItem_Click;
            // 
            // arrièreToolStripMenuItem
            // 
            arrièreToolStripMenuItem.Name = "arrièreToolStripMenuItem";
            arrièreToolStripMenuItem.Size = new Size(218, 44);
            arrièreToolStripMenuItem.Text = "Arrière";
            arrièreToolStripMenuItem.Click += arrièreToolStripMenuItem_Click;
            // 
            // resetToolStripMenuItem
            // 
            resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            resetToolStripMenuItem.Size = new Size(91, 36);
            resetToolStripMenuItem.Text = "Reset";
            resetToolStripMenuItem.Click += resetToolStripMenuItem_Click;
            // 
            // iou_label
            // 
            iou_label.AutoSize = true;
            iou_label.Location = new Point(948, 872);
            iou_label.Name = "iou_label";
            iou_label.Size = new Size(60, 32);
            iou_label.TabIndex = 15;
            iou_label.Text = "Iou :";
            // 
            // vinet_label
            // 
            vinet_label.AutoSize = true;
            vinet_label.Location = new Point(1154, 872);
            vinet_label.Name = "vinet_label";
            vinet_label.Size = new Size(82, 32);
            vinet_label.TabIndex = 16;
            vinet_label.Text = "Vinet :";
            // 
            // img_comparaison
            // 
            img_comparaison.BorderStyle = BorderStyle.FixedSingle;
            img_comparaison.Location = new Point(3, 3);
            img_comparaison.Name = "img_comparaison";
            img_comparaison.Size = new Size(282, 257);
            img_comparaison.SizeMode = PictureBoxSizeMode.StretchImage;
            img_comparaison.TabIndex = 17;
            img_comparaison.TabStop = false;
            // 
            // label_db
            // 
            label_db.AutoSize = true;
            label_db.Location = new Point(370, 65);
            label_db.Name = "label_db";
            label_db.Size = new Size(172, 32);
            label_db.TabIndex = 18;
            label_db.Text = "Image Source :";
            // 
            // label_gt
            // 
            label_gt.AutoSize = true;
            label_gt.Location = new Point(370, 503);
            label_gt.Name = "label_gt";
            label_gt.Size = new Size(241, 32);
            label_gt.TabIndex = 19;
            label_gt.Text = "Image Ground Truth :";
            // 
            // label_imageTraitée
            // 
            label_imageTraitée.AutoSize = true;
            label_imageTraitée.Location = new Point(928, 426);
            label_imageTraitée.Name = "label_imageTraitée";
            label_imageTraitée.Size = new Size(170, 32);
            label_imageTraitée.TabIndex = 20;
            label_imageTraitée.Text = "Image Traitée :";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(981, 82);
            label1.Name = "label1";
            label1.Size = new Size(166, 32);
            label1.TabIndex = 21;
            label1.Text = "Comparaison :";
            // 
            // panel1
            // 
            panel1.AutoScroll = true;
            panel1.Controls.Add(image_db);
            panel1.Location = new Point(370, 101);
            panel1.Margin = new Padding(4, 4, 4, 4);
            panel1.Name = "panel1";
            panel1.Size = new Size(403, 389);
            panel1.TabIndex = 22;
            panel1.Scroll += panel1_Scroll;
            // 
            // panel2
            // 
            panel2.Controls.Add(img_comparaison);
            panel2.Location = new Point(948, 131);
            panel2.Margin = new Padding(4, 4, 4, 4);
            panel2.Name = "panel2";
            panel2.Size = new Size(291, 260);
            panel2.TabIndex = 23;
            // 
            // panel3
            // 
            panel3.Controls.Add(image_gt);
            panel3.Location = new Point(370, 539);
            panel3.Margin = new Padding(4, 4, 4, 4);
            panel3.Name = "panel3";
            panel3.Size = new Size(403, 389);
            panel3.TabIndex = 24;
            // 
            // panel4
            // 
            panel4.Controls.Add(image_traitee);
            panel4.Location = new Point(928, 462);
            panel4.Margin = new Padding(4, 4, 4, 4);
            panel4.Name = "panel4";
            panel4.Size = new Size(403, 389);
            panel4.TabIndex = 25;
            // 
            // ProjetC
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1424, 956);
            Controls.Add(panel4);
            Controls.Add(panel3);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(label1);
            Controls.Add(label_imageTraitée);
            Controls.Add(label_gt);
            Controls.Add(label_db);
            Controls.Add(vinet_label);
            Controls.Add(iou_label);
            Controls.Add(treeView_in_sc);
            Controls.Add(menuStrip1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MainMenuStrip = menuStrip1;
            MaximizeBox = false;
            Name = "ProjetC";
            Text = "Projet C++";
            ((System.ComponentModel.ISupportInitialize)image_db).EndInit();
            ((System.ComponentModel.ISupportInitialize)image_gt).EndInit();
            ((System.ComponentModel.ISupportInitialize)image_traitee).EndInit();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)img_comparaison).EndInit();
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel4.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button bSeuillageAuto;
        private PictureBox imageSeuillee;
        private PictureBox image_db;
        private TreeView treeView_in_sc;
        private ImageList In_Sc_list;
        private FolderBrowserDialog folderBrowserDialog1;
        private PictureBox image_gt;
        private PictureBox image_traitee;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fichierToolStripMenuItem;
        private ToolStripMenuItem ouvrirDossierToolStripMenuItem;
        private Label iou_label;
        private Label vinet_label;
        private PictureBox img_comparaison;
        private ToolStripMenuItem filtreToolStripMenuItem;
        private ToolStripMenuItem rotationToolStripMenuItem;
        private ToolStripMenuItem horaireToolStripMenuItem;
        private ToolStripMenuItem moyenToolStripMenuItem;
        private ToolStripMenuItem medianToolStripMenuItem;
        private ToolStripMenuItem antihoraireToolStripMenuItem;
        private Label label_db;
        private Label label_gt;
        private Label label_imageTraitée;
        private Label label1;
        private ToolStripMenuItem resetToolStripMenuItem;
        private ToolStripMenuItem zoomToolStripMenuItem;
        private ToolStripMenuItem avantToolStripMenuItem;
        private ToolStripMenuItem arrièreToolStripMenuItem;
        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private Panel panel4;
    }
}