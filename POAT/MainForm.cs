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

namespace POAT
{
    public partial class ProjetC : Form
    {
        ComboBox comboBoxFilterType = new ComboBox();
        public ProjetC()
        {
            InitializeComponent();
        }

        private string sourceImagesPath = "";
        private string groundTruthsImagePath = "";

        private int kernelSize = 3;
        private string structElement = "disk";  

        private void reset()
        {
            // Remise à zéro des images
            image_db.Image = null;
            image_gt.Image = null;
            image_traitée.Image = null;
            img_comparaison.Image = null;

            //remise à zéro des labels
            iou_label.Text = "Iou : ";
            vinet_label.Text = "Vinet : ";
        }

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
            using (var progressForm = new ProgressForm(this))
            {

                this.Enabled = false;
                progressForm.Show();

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

                        progressForm.SetProgress(98);

                        Img.objetLibDataImgPtr(2, bmpData.Scan0, bmpData.Stride, bmp.Height, bmp.Width);
                        Img.processPtr(ImgGT.objetLibDataImgPtr(2, bmpDataGt.Scan0, bmpDataGt.Stride, bmpGt.Height, bmpGt.Width));

                        progressForm.SetProgress(100);

                        bmp.UnlockBits(bmpData);
                        bmpGt.UnlockBits(bmpDataGt);
                    }

                    double iou = Img.objetLibValeurChamp(0);
                    double vinet = Img.objetLibValeurChamp(1);

                    return (bmp, bmpGt, iou, vinet);
                });

                iou_label.Text = $"Iou :  {iouValue} %";
                vinet_label.Text = $"Vinet :  {vinetValue} %";
                image_traitée.Image = processedImage;
                img_comparaison.Image = groundTruthImage;

                this.Enabled = true;
                progressForm.CloseForm();
            }
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
                    MessageBox.Show("Aucune image trouvée dans le dossier Source_Images", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    return;
                }

                reset();

                //affiche l'image dans le picturebox
                image_db.Image = Image.FromFile(sourceImagePath);
                image_gt.Image = Image.FromFile(groundTruthImagePath);

                // Appel de la méthode processImage
                processImage();
            }

        }

        private void filterDialog()
        {
            // attendre la fin de la saisie dans FormFilter
            using (Filter formFilter = new Filter())
            {
                if (formFilter.ShowDialog() == DialogResult.OK)
                {
                    kernelSize = formFilter.getKernel();
                    structElement = formFilter.getStr();
                }
            }

        }


        private void moyenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView_in_sc.SelectedNode != null)
            {
                // Appel de la méthode getKernel
                filterDialog();

                if (image_db.Image != null)
                {
                    Bitmap processedImage = Task.Run(() =>
                    {
                        Bitmap bmp = new Bitmap(image_db.Image);
                        ClImage Img = new ClImage();

                        unsafe
                        {
                            var bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                            Img.objetLibDataImgPtr(2, bmpData.Scan0, bmpData.Stride, bmp.Height, bmp.Width);
                            // a changer pour choisir le type d'élément struct
                            Img.meanFilterPtr(kernelSize, structElement);

                            bmp.UnlockBits(bmpData);
                        }

                        return bmp;
                    }).Result;

                    image_db.Image = processedImage;
                }
                processImage();
            }
        }


        private void medianToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView_in_sc.SelectedNode != null)
            {
                filterDialog();

                if (image_db.Image != null)
                {
                    Bitmap processedImage = Task.Run(() =>
                    {
                        Bitmap bmp = new Bitmap(image_db.Image);
                        ClImage Img = new ClImage();

                        unsafe
                        {
                            var bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                            Img.objetLibDataImgPtr(2, bmpData.Scan0, bmpData.Stride, bmp.Height, bmp.Width);
                            // a changer pour choisir le type d'élément struct
                            Img.medianFilterPtr(kernelSize, structElement);

                            bmp.UnlockBits(bmpData);
                        }

                        return bmp;
                    }).Result;

                    image_db.Image = processedImage;
                }
                processImage();
            }
        }

        private void horaireToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView_in_sc.SelectedNode != null)
            {
                // rotation horaire
                image_db.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                image_gt.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                image_db.Refresh();
                image_gt.Refresh();

                image_traitée.Image = null;
                img_comparaison.Image = null;

                //remise à zéro des labels
                iou_label.Text = "Iou : ";
                vinet_label.Text = "Vinet : ";

                processImage();
            }

        }

        private void antihoraireToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView_in_sc.SelectedNode != null)
            {
                // rotation antihoraire
                image_db.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                image_gt.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                image_db.Refresh();
                image_gt.Refresh();

                image_traitée.Image = null;
                img_comparaison.Image = null;

                //remise à zéro des labels
                iou_label.Text = "Iou : ";
                vinet_label.Text = "Vinet : ";

                processImage();
            }
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = treeView_in_sc.SelectedNode;
            if (selectedNode != null)
            {
                string imageName = selectedNode.Text;

                string sourceImagePath = Path.Combine(sourceImagesPath, imageName + ".bmp");

                string groundTruthImagePath = Path.Combine(groundTruthsImagePath, imageName + ".bmp");

                if (!File.Exists(sourceImagePath) || !File.Exists(groundTruthImagePath))
                {
                    MessageBox.Show("Les fichiers d'image correspondants n'existent pas.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                reset();

                image_db.Image = Image.FromFile(sourceImagePath);
                image_gt.Image = Image.FromFile(groundTruthImagePath);

                processImage();
            }

        }

        private void avantToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView_in_sc.SelectedNode != null)
            {
                image_db.Width *= 2;
                image_db.Height *= 2;

                image_gt.Width *= 2;
                image_gt.Height *= 2;

                image_traitée.Width *= 2;
                image_traitée.Height *= 2;
            }
        }

        private void arrièreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView_in_sc.SelectedNode != null)
            {
                image_db.Width /= 2;
                image_db.Height /= 2;

                image_gt.Width /= 2;
                image_gt.Height /= 2;

                image_traitée.Width /= 2;
                image_traitée.Height /= 2;
            }

        }

        private void panel1_Scroll(object sender, ScrollEventArgs e)
        {
            // Récupérer la valeur du défilement vertical actuel
            int scrollValue_v = panel1.VerticalScroll.Value;

            // Déplacer les PictureBox en fonction de la valeur de défilement vertical
            image_gt.Top = -scrollValue_v;
            image_traitée.Top = -scrollValue_v;

            // Récupérer la valeur du défilement horizontal actuel
            int scrollValue_h = panel1.HorizontalScroll.Value;

            // Déplacer les PictureBox en fonction de la valeur de défilement horizontal
            image_gt.Left = -scrollValue_h;
            image_traitée.Left = -scrollValue_h;
        }
    }
}
