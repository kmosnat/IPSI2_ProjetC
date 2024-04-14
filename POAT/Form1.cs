using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;

using System.Runtime.InteropServices;
using libImage;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace POAT
{
    public partial class ProjetC : Form
    {
        public ProjetC()
        {
            InitializeComponent();
        }

        private string sourceImagesPath;
        private string groundTruthsImagePath;

        private void AjouterNoeudsLnSn()
        {
            var noeud_ln = new TreeNode("In");
            var noeud_sn = new TreeNode("Sc");

            // Trie et groupe les clés de ImageList par préfixe
            var groupedKeys = In_Sc_list.Images.Keys.Cast<string>()
                                   .Select(key => new { Key = key, FileName = Path.GetFileNameWithoutExtension(key) })
                                   .OrderBy(item => int.Parse(item.FileName.Substring(3)))
                                   .GroupBy(item => item.FileName.StartsWith("In_"))
                                   .ToList();

            // Ajoute les nœuds enfants à leur noeud parent approprié
            foreach (var group in groupedKeys)
            {
                var parent = group.Key ? noeud_ln : noeud_sn;
                foreach (var item in group)
                {
                    int index = In_Sc_list.Images.Keys.IndexOf(item.Key);
                    var child = new TreeNode(item.FileName) { ImageIndex = index, SelectedImageIndex = index };
                    parent.Nodes.Add(child);
                }
            }

            treeView_in_sc.Nodes.AddRange(new[] { noeud_ln, noeud_sn });
        }


        private async void processImage()
        {
            // Utilisation de Task.Run pour exécuter le traitement d'image de manière asynchrone et retourner les valeurs nécessaires
            (Bitmap processedImage, Bitmap groundTruthImage, double iouValue, double vinetValue) = await Task.Run(() =>
            {
                Bitmap bmp = new Bitmap(image_db.Image);
                Bitmap bmpGt = new Bitmap(image_gt.Image);
                ClImage Img = new ClImage();
                ClImage ImgGT = new ClImage();

                unsafe
                {
                    var bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                    var bmpDataGt = bmpGt.LockBits(new Rectangle(0, 0, bmpGt.Width, bmpGt.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                    Img.objetLibDataImgPtr(2, bmpData.Scan0, bmpData.Stride, bmp.Height, bmp.Width);
                    Img.processPtr(ImgGT.objetLibDataImgPtr(2, bmpDataGt.Scan0, bmpDataGt.Stride, bmpGt.Height, bmpGt.Width));

                    bmp.UnlockBits(bmpData);
                    bmpGt.UnlockBits(bmpDataGt);
                }

                // Récupération des valeurs de performance à partir de l'objet Img
                double iou = Img.objetLibValeurChamp(0);
                double vinet = Img.objetLibValeurChamp(1);

                return (bmp, bmpGt, iou, vinet);
            });

            // Mise à jour de l'interface utilisateur avec les valeurs retournées
            iou_label.Text = $"Iou (%) : {iouValue}";
            vinet_label.Text = $"Vinet (%) : {vinetValue}";
            image_traitée.Image = processedImage;
            comparaison.Image = groundTruthImage;
        }

        private void ouvrirDossierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Choix du dossier contenant les images
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                string selectedPath = folderBrowserDialog1.SelectedPath;
                sourceImagesPath = Path.Combine(selectedPath, "Source_Images");
                groundTruthsImagePath = Path.Combine(selectedPath, "Ground_truth");

                // Charger toutes les images du dossier Source Images - bmp
                string[] sourceImageFiles = Directory.GetFiles(sourceImagesPath, "*.bmp");
                if (sourceImageFiles.Length > 0)
                {
                    foreach (string filePath in sourceImageFiles)
                    {
                        // Obtenez juste le nom du fichier sans le chemin
                        string fileName = Path.GetFileName(filePath);

                        // Ajoutez l'image à l'ImageList Ln_Sc
                        In_Sc_list.Images.Add(fileName, Image.FromFile(filePath));
                    }
                    AjouterNoeudsLnSn();
                }
                else
                {
                    MessageBox.Show("Aucune image trouvée dans le dossier Source Images - bmp.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void treeView_in_sc_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // Vérifie si un nœud est sélectionné
            if (e.Node != null)
            {
                // Récupère le nom de l'image à partir du texte du nœud
                string imageName = e.Node.Text;

                // Construit le chemin complet de l'image dans le dossier Source Images
                string sourceImagePath = Path.Combine(sourceImagesPath, imageName + ".bmp");

                // Construit le chemin complet de l'image dans le dossier Ground truth
                string groundTruthImagePath = Path.Combine(groundTruthsImagePath, imageName + ".bmp");

                if (!File.Exists(sourceImagePath) || !File.Exists(groundTruthImagePath))
                {
                    MessageBox.Show("Les fichiers d'image correspondants n'existent pas.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Remise à zéro des images
                image_db.Image = null;
                image_gt.Image = null;
                image_traitée.Image = null;
                comparaison.Image = null;

                //remise à zéro des labels
                iou_label.Text = "Iou (%) : ";
                vinet_label.Text = "Vinet (%) : ";

                //affiche l'image dans le picturebox
                image_db.Image = Image.FromFile(sourceImagePath);
                image_gt.Image = Image.FromFile(groundTruthImagePath);

                // Appel de la méthode processImage
                processImage();
            }

        }

        private int? GetKernel()
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox("Entrez la taille du noyau (impair) : ", "Taille du noyau", "3");
            int kernelSize;

            if (int.TryParse(input, out kernelSize) && kernelSize % 2 == 1 && kernelSize > 0)
            {
                return kernelSize;
            }
            else
            {
                MessageBox.Show("La taille du noyau doit être un nombre impair positif.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }


        private void moyenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int? kernelSize = GetKernel();
            if (kernelSize.HasValue && image_db.Image != null)
            {
                Bitmap processedImage = Task.Run(() =>
                {
                    Bitmap bmp = new Bitmap(image_db.Image);
                    ClImage Img = new ClImage();

                    unsafe
                    {
                        var bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                        Img.objetLibDataImgPtr(2, bmpData.Scan0, bmpData.Stride, bmp.Height, bmp.Width);
                        Img.meanFilterPtr(kernelSize.Value);

                        bmp.UnlockBits(bmpData);
                    }

                    return bmp;
                }).Result;

                image_db.Image = processedImage;
            }
            processImage();
        }


        private void medianToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int? kernelSize = GetKernel();
            if (kernelSize.HasValue && image_db.Image != null)
            {
                Bitmap processedImage = Task.Run(() =>
                {
                    Bitmap bmp = new Bitmap(image_db.Image);
                    ClImage Img = new ClImage();

                    unsafe
                    {
                        var bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                        Img.objetLibDataImgPtr(2, bmpData.Scan0, bmpData.Stride, bmp.Height, bmp.Width);
                        Img.medianFilterPtr(kernelSize.Value);

                        bmp.UnlockBits(bmpData);
                    }

                    return bmp;
                }).Result;

                image_db.Image = processedImage;
            }
            processImage();
        }

        private void horaireToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // rotation horaire
            image_db.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            image_gt.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            image_db.Refresh();
            image_gt.Refresh();

            processImage();

        }

        private void antihoraireToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // rotation antihoraire
            image_db.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
            image_gt.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
            image_db.Refresh();
            image_gt.Refresh();

            processImage();
        }
    }
}
