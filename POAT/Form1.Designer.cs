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
            ((System.ComponentModel.ISupportInitialize)image_db).BeginInit();
            ((System.ComponentModel.ISupportInitialize)image_gt).BeginInit();
            ((System.ComponentModel.ISupportInitialize)image_traitée).BeginInit();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // image_db
            // 
            image_db.BorderStyle = BorderStyle.FixedSingle;
            image_db.Location = new Point(370, 69);
            image_db.Name = "image_db";
            image_db.Size = new Size(402, 389);
            image_db.SizeMode = PictureBoxSizeMode.StretchImage;
            image_db.TabIndex = 7;
            image_db.TabStop = false;
            // 
            // treeView_in_sc
            // 
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
            image_gt.Location = new Point(370, 529);
            image_gt.Name = "image_gt";
            image_gt.Size = new Size(402, 389);
            image_gt.SizeMode = PictureBoxSizeMode.StretchImage;
            image_gt.TabIndex = 10;
            image_gt.TabStop = false;
            // 
            // image_traitée
            // 
            image_traitée.BorderStyle = BorderStyle.FixedSingle;
            image_traitée.Location = new Point(917, 271);
            image_traitée.Name = "image_traitée";
            image_traitée.Size = new Size(402, 389);
            image_traitée.SizeMode = PictureBoxSizeMode.StretchImage;
            image_traitée.TabIndex = 11;
            image_traitée.TabStop = false;
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(32, 32);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fichierToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1565, 40);
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
            // ProjetC
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1565, 992);
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
    }
}