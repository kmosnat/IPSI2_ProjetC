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
        private int zoom = 1;
        private Size originalSize;

        private string sourceImagesPath = "";
        private string groundTruthsImagePath = "";

        private int kernelSize = 3;
        private string structElement = "disk";

        public ProjetC()
        {
            InitializeComponent();
            originalSize = new Size(image_db.Width, image_db.Height);
            arrièreToolStripMenuItem.Enabled = false;
        }

        private void reset()
        {
            // Remise � z�ro des images
            image_db.Image = null;
            image_gt.Image = null;
            image_traitee.Image = null;
            img_comparaison.Image = null;

            //remise � z�ro des labels
            iou_label.Text = "Iou : ";
            vinet_label.Text = "Vinet : ";
        }

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
                image_traitee.Image = processedImage;
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

                        // Ajoutez l'image � l'ImageList Ln_Sc
                        In_Sc_list.Images.Add(fileName, Image.FromFile(filePath));
                    }
                    AjouterNoeudsLnSn();
                }
                else
                {
                    MessageBox.Show("Aucune image trouv�e dans le dossier Source_Images", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    return;
                }

                reset();

                //affiche l'image dans le picturebox
                image_db.Image = Image.FromFile(sourceImagePath);
                image_gt.Image = Image.FromFile(groundTruthImagePath);

                processImage();
            }

        }

        private void filterDialog()
        {
            using (Filter formFilter = new Filter())
            {
                if (formFilter.ShowDialog() == DialogResult.OK)
                {
                    kernelSize = formFilter.getKernelSize();
                    structElement = formFilter.getStrElement();
                }
            }

        }

        private void filter(String methode)
        {
            if (treeView_in_sc.SelectedNode != null)
            {
                // Appel du Form Filter
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
                            // a changer pour choisir le type d'�l�ment struct
                            Img.filterPtr(kernelSize, methode, structElement);

                            bmp.UnlockBits(bmpData);
                        }

                        return bmp;
                    }).Result;

                    image_db.Image = processedImage;
                }
                processImage();
            }

        }


        private void moyenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            filter("moyen");
        }


        private void medianToolStripMenuItem_Click(object sender, EventArgs e)
        {
            filter("median");
        }

        private void refreshImages()
        {
            image_db.Refresh();
            image_gt.Refresh();

            image_traitee.Image = null;
            img_comparaison.Image = null;

            //remise � z�ro des labels
            iou_label.Text = "Iou : ";
            vinet_label.Text = "Vinet : ";

            processImage();
        }

        private void horaireToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView_in_sc.SelectedNode != null)
            {
                // rotation horaire
                image_db.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                image_gt.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                
                refreshImages();
            }

        }

        private void antihoraireToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView_in_sc.SelectedNode != null)
            {
                // rotation antihoraire
                image_db.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                image_gt.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);

                refreshImages();
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

                //reset zoom
                zoom = 1;
                ZoomImage(1);

                reset();

                image_db.Image = Image.FromFile(sourceImagePath);
                image_gt.Image = Image.FromFile(groundTruthImagePath);

                processImage();
            }

        }

        private void avantToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZoomImage(2);
        }

        private void arrièreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZoomImage(0.5);
        }

        private void ZoomImage(double scaleFactor)
        {
            if (treeView_in_sc.SelectedNode != null)
            {
               
                zoom = (int)(zoom * scaleFactor);
                zoom = Math.Max(1, Math.Min(zoom, 6));

                AdjustImageSize(image_db, zoom);
                AdjustImageSize(image_gt, zoom);
                AdjustImageSize(image_traitee, zoom);

                avantToolStripMenuItem.Enabled = zoom < 6;
                arrièreToolStripMenuItem.Enabled = zoom > 1;

                AdjustImagePosition();
            }
        }

        private void AdjustImageSize(PictureBox pictureBox, int zoomLevel)
        {
            pictureBox.Size = new Size(originalSize.Width * zoomLevel, originalSize.Height * zoomLevel);
        }

        private void panel1_Scroll(object sender, ScrollEventArgs e)
        {
            AdjustImagePosition();
        }

        private void AdjustImagePosition()
        {
            int scrollValue_v = panel1.VerticalScroll.Value;
            int scrollValue_h = panel1.HorizontalScroll.Value;

            SetImagePosition(image_db, scrollValue_h, scrollValue_v);
            SetImagePosition(image_gt, scrollValue_h, scrollValue_v);
            SetImagePosition(image_traitee, scrollValue_h, scrollValue_v);
        }

        private void SetImagePosition(PictureBox image, int scrollValue_h, int scrollValue_v)
        {
            image.Top = -scrollValue_v;
            image.Left = -scrollValue_h;
        }

    }
}
