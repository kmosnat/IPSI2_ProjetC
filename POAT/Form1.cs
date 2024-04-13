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

            // Trie et groupe les cl�s de ImageList par pr�fixe
            var groupedKeys = In_Sc_list.Images.Keys.Cast<string>()
                                   .Select(key => new { Key = key, FileName = Path.GetFileNameWithoutExtension(key) })
                                   .OrderBy(item => int.Parse(item.FileName.Substring(3)))
                                   .GroupBy(item => item.FileName.StartsWith("In_"))
                                   .ToList();

            // Ajoute les n�uds enfants � leur noeud parent appropri�
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


        private async void processImage(string Source, string Ground_Truth)
        {
            // Utilisation de Task.Run pour ex�cuter le traitement d'image de mani�re asynchrone et retourner les valeurs n�cessaires
            (Bitmap processedImage, Bitmap groundTruthImage, double iouValue, double vinetValue) = await Task.Run(() =>
            {
                Bitmap bmp = new Bitmap(Image.FromFile(Source));
                Bitmap bmpGt = new Bitmap(Image.FromFile(Ground_Truth));
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

                // R�cup�ration des valeurs de performance � partir de l'objet Img
                double iou = Img.objetLibValeurChamp(0);
                double vinet = Img.objetLibValeurChamp(1);

                return (bmp, bmpGt, iou, vinet);
            });

            // Mise � jour de l'interface utilisateur avec les valeurs retourn�es
            iou_label.Text = $"Iou (%) : {iouValue}";
            vinet_label.Text = $"Vinet (%) : {vinetValue}";
            image_trait�e.Image = processedImage;
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

                        // Ajoutez l'image � l'ImageList Ln_Sc
                        In_Sc_list.Images.Add(fileName, Image.FromFile(filePath));
                    }
                    AjouterNoeudsLnSn();
                }
                else
                {
                    MessageBox.Show("Aucune image trouv�e dans le dossier Source Images - bmp.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void treeView_in_sc_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // V�rifie si un n�ud est s�lectionn�
            if (e.Node != null)
            {
                // R�cup�re le nom de l'image � partir du texte du n�ud
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

                // Remise � z�ro des images
                image_db.Image = null;
                image_gt.Image = null;
                image_trait�e.Image = null;
                comparaison.Image = null;

                //remise � z�ro des labels
                iou_label.Text = "Iou (%) : ";
                vinet_label.Text = "Vinet (%) : ";

                //affiche l'image dans le picturebox
                image_db.Image = Image.FromFile(sourceImagePath);
                image_gt.Image = Image.FromFile(groundTruthImagePath);

                // Appel de la m�thode processImage
                processImage(sourceImagePath, groundTruthImagePath);
            }

        }


    }
}
