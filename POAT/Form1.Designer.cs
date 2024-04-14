﻿namespace POAT
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
            image_traitée = new PictureBox();
            menuStrip1 = new MenuStrip();
            fichierToolStripMenuItem = new ToolStripMenuItem();
            ouvrirDossierToolStripMenuItem = new ToolStripMenuItem();
            filtreToolStripMenuItem = new ToolStripMenuItem();
            moyenToolStripMenuItem = new ToolStripMenuItem();
            medianToolStripMenuItem = new ToolStripMenuItem();
            rotationToolStripMenuItem = new ToolStripMenuItem();
            horaireToolStripMenuItem = new ToolStripMenuItem();
            antihoraireToolStripMenuItem = new ToolStripMenuItem();
            iou_label = new Label();
            vinet_label = new Label();
            comparaison = new PictureBox();
            label_db = new Label();
            label_gt = new Label();
            label_imageTraitée = new Label();
            label1 = new Label();
            resetToolStripMenuItem = new ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)image_db).BeginInit();
            ((System.ComponentModel.ISupportInitialize)image_gt).BeginInit();
            ((System.ComponentModel.ISupportInitialize)image_traitée).BeginInit();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)comparaison).BeginInit();
            SuspendLayout();
            // 
            // image_db
            // 
            image_db.BorderStyle = BorderStyle.FixedSingle;
            image_db.Location = new Point(370, 100);
            image_db.Name = "image_db";
            image_db.Size = new Size(402, 389);
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
            treeView_in_sc.Margin = new Padding(4);
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
            image_gt.Location = new Point(370, 538);
            image_gt.Name = "image_gt";
            image_gt.Size = new Size(402, 389);
            image_gt.SizeMode = PictureBoxSizeMode.StretchImage;
            image_gt.TabIndex = 10;
            image_gt.TabStop = false;
            // 
            // image_traitée
            // 
            image_traitée.BorderStyle = BorderStyle.FixedSingle;
            image_traitée.Location = new Point(928, 461);
            image_traitée.Name = "image_traitée";
            image_traitée.Size = new Size(402, 389);
            image_traitée.SizeMode = PictureBoxSizeMode.StretchImage;
            image_traitée.TabIndex = 11;
            image_traitée.TabStop = false;
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(32, 32);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fichierToolStripMenuItem, filtreToolStripMenuItem, rotationToolStripMenuItem, resetToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
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
            ouvrirDossierToolStripMenuItem.Size = new Size(298, 44);
            ouvrirDossierToolStripMenuItem.Text = "Ouvrir Dossier";
            ouvrirDossierToolStripMenuItem.Click += ouvrirDossierToolStripMenuItem_Click;
            // 
            // filtreToolStripMenuItem
            // 
            filtreToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { moyenToolStripMenuItem, medianToolStripMenuItem });
            filtreToolStripMenuItem.Name = "filtreToolStripMenuItem";
            filtreToolStripMenuItem.Size = new Size(97, 38);
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
            rotationToolStripMenuItem.Size = new Size(123, 38);
            rotationToolStripMenuItem.Text = "Rotation";
            // 
            // horaireToolStripMenuItem
            // 
            horaireToolStripMenuItem.Name = "horaireToolStripMenuItem";
            horaireToolStripMenuItem.Size = new Size(359, 44);
            horaireToolStripMenuItem.Text = "Horaire";
            horaireToolStripMenuItem.Click += horaireToolStripMenuItem_Click;
            // 
            // antihoraireToolStripMenuItem
            // 
            antihoraireToolStripMenuItem.Name = "antihoraireToolStripMenuItem";
            antihoraireToolStripMenuItem.Size = new Size(359, 44);
            antihoraireToolStripMenuItem.Text = "Anti-Horaire";
            antihoraireToolStripMenuItem.Click += antihoraireToolStripMenuItem_Click;
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
            // comparaison
            // 
            comparaison.BorderStyle = BorderStyle.FixedSingle;
            comparaison.Location = new Point(981, 117);
            comparaison.Name = "comparaison";
            comparaison.Size = new Size(282, 257);
            comparaison.SizeMode = PictureBoxSizeMode.StretchImage;
            comparaison.TabIndex = 17;
            comparaison.TabStop = false;
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
            // resetToolStripMenuItem
            // 
            resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            resetToolStripMenuItem.Size = new Size(91, 38);
            resetToolStripMenuItem.Text = "Reset";
            resetToolStripMenuItem.Click += resetToolStripMenuItem_Click;
            // 
            // ProjetC
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1424, 956);
            Controls.Add(label1);
            Controls.Add(label_imageTraitée);
            Controls.Add(label_gt);
            Controls.Add(label_db);
            Controls.Add(comparaison);
            Controls.Add(vinet_label);
            Controls.Add(iou_label);
            Controls.Add(image_traitée);
            Controls.Add(image_gt);
            Controls.Add(treeView_in_sc);
            Controls.Add(image_db);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "ProjetC";
            Text = "Projet C++";
            ((System.ComponentModel.ISupportInitialize)image_db).EndInit();
            ((System.ComponentModel.ISupportInitialize)image_gt).EndInit();
            ((System.ComponentModel.ISupportInitialize)image_traitée).EndInit();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)comparaison).EndInit();
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
        private PictureBox image_traitée;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fichierToolStripMenuItem;
        private ToolStripMenuItem ouvrirDossierToolStripMenuItem;
        private Label iou_label;
        private Label vinet_label;
        private PictureBox comparaison;
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
    }
}