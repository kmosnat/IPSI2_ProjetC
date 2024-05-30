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
using static System.Net.Mime.MediaTypeNames;
using Image = System.Drawing.Image;

namespace POAT
{
    public partial class ProjetC : Form
    {
        private int zoom = 1;                       // Init du zoom
        private Size originalSize;                  // Init taille pour pictures Box sauf comparaison

        private string sourceImagesPath = "";       // Chemain pour les images à traités/brut
        private string groundTruthsImagePath = "";  // Chemain pour les images ground truth

        private int kernelSize;                     // Init de la variable de la taille de l'element structurant
        private string structElement;               // Init de la variable de la forme de l'element structurant

        public ProjetC()
        {
            InitializeComponent();
            originalSize = new Size(image_db.Width, image_db.Height);   // Sauvegarde de la taille d'origine des pictures Box
            arrièreToolStripMenuItem.Enabled = false;                   // Desactivation du bouton zoom arriere (taille image par defaut)             
        }

        // Fonction utiliser pour bouton reset du menu et lors du changement de l'image
        private void reset()
        {
            // Remise a zero des images
            image_db.Image = null;
            image_gt.Image = null;
            image_traitee.Image = null;
            img_comparaison.Image = null;

            // Remise a zero des labels
            iou_label.Text = "Iou : ";
            vinet_label.Text = "Vinet : ";
        }

        // Fonction pour ajouter les 2 types d'images (In et Sc) dans le node Tree
        private void AjouterNoeudsLnSn()
        {
            var noeud_ln = new TreeNode("In"); // init du noeud In
            var noeud_sn = new TreeNode("Sc"); // init du noeud Sc

            // Trie et groupe les cles de ImageList par pr�fixe
            var groupedKeys = In_Sc_list.Images.Keys.Cast<string>()
                                   .Select(key => new { Key = key, FileName = Path.GetFileNameWithoutExtension(key) })
                                   .OrderBy(item => int.Parse(item.FileName.Substring(3)))
                                   .GroupBy(item => item.FileName.StartsWith("In_"))
                                   .ToList();

            // Ajoute les noeuds enfants a leur noeud parent approprie
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

        // Fonction utiliser pour faire le traitement de l'image
        private async void processImage()
        {
            // Init du formulaire de la progressForm
            using (var progressForm = new ProgressForm(this))
            {
                this.Enabled = false; // Descative le formulaire en attendant la fin du traitement
                progressForm.Show();  // Afficher le formulaire progressForm

                // Init d'une fonction dans la fonction sous forme de tache pour le traitement
                var (processedImage, groundTruthImage, iouValue, vinetValue) = await Task.Run(() =>
                {
                    Bitmap bmp = new Bitmap(image_db.Image);    // Init de l'image a traiter
                    Bitmap bmpGt = new Bitmap(image_gt.Image);  // Init de l'image ground truth
                    ClImage Img = new ClImage();                // Init d'une variable ClImage pour l'image à traiter
                    ClImage ImgGT = new ClImage();              // Init d'une variable ClImage pour l'image ground truth

                    var bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);            // blocage des pixels de l'image 
                    var bmpDataGt = bmpGt.LockBits(new Rectangle(0, 0, bmpGt.Width, bmpGt.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);    // blocage des pixels de l'image 

                    progressForm.SetProgress(98);   // Afficher notre progessbar à 98%

                    Img.objetLibDataImgPtr(2, bmpData.Scan0, bmpData.Stride, bmp.Height, bmp.Width); // Partie init : utilisation de la fonction qui envoie les donnees à notre fonction/methode en csharp
                    Img.processPtr(ImgGT.objetLibDataImgPtr(0, bmpDataGt.Scan0, bmpDataGt.Stride, bmpGt.Height, bmpGt.Width)); // Partie process : utilisation de la fonction qui envoie les donnees à notre fonction/methode en csharp

                    progressForm.SetProgress(100);  // Afficher notre progessbar à 100%

                    bmp.UnlockBits(bmpData);        // Déverrouillage des pixels de l'image
                    bmpGt.UnlockBits(bmpDataGt);    // Déverrouillage des pixels de l'image

                    double iou = Img.objetLibValeurChamp(0);    // Partie du champs attribue à l'Iou
                    double vinet = Img.objetLibValeurChamp(1);  // Partie du champs attribue à Vinet

                    return (bmp, bmpGt, iou, vinet);

                });

                // Affichage des donnes de notre traitement
                iou_label.Text = $"Iou :  {iouValue} %";
                vinet_label.Text = $"Vinet :  {vinetValue} %";
                image_traitee.Image = processedImage;
                img_comparaison.Image = groundTruthImage;

                this.Enabled = true;        // Reactive le formulaire
                progressForm.CloseForm();   // Fermeture du formulaire progressForm
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

                        // Ajoutez l'image a l'ImageList Ln_Sc
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
            // Init du formulaire Filter
            using (Filter formFilter = new Filter())
            {
                // Attente du signal avant extraction image
                if (formFilter.ShowDialog() == DialogResult.OK)
                {
                    // Extraction des parametres entrees par l'utilisateur
                    kernelSize = formFilter.getKernelSize();
                    structElement = formFilter.getStrElement();
                }
            }

        }

        // appliquer le filtrage en fonction du choix de l'utilisateur
        private void filter(String methode)
        {
            if (treeView_in_sc.SelectedNode != null)
            {
                filterDialog(); // Affichage du formulaire Filter

                if (image_db.Image != null)
                {
                    Bitmap processedImage = Task.Run(() =>
                    {
                        Bitmap bmp = new Bitmap(image_db.Image);    // init d'une image bmp
                        ClImage Img = new ClImage();                // init d'une ClImage   

                        unsafe
                        {
                            var bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb); // Blocage des pixels de l'image 
                            Img.objetLibDataImgPtr(2, bmpData.Scan0, bmpData.Stride, bmp.Height, bmp.Width); // Partie init : utilisation de la fonction qui envoie les donnees à notre fonction/methode en csharp
                            // a changer pour choisir le type d'element struct
                            Img.filterPtr(kernelSize, methode, structElement);

                            bmp.UnlockBits(bmpData); // Deblocage des pixels de l'image 
                        }

                        return bmp;
                    }).Result;

                    image_db.Image = processedImage; // Affiche l'image dans la picture Box
                }
                processImage(); // Lancemant du traitement
            }

        }


        private void moyenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            filter("moyen"); // L'utilisateur a choisit le filtre moyen
        }


        private void medianToolStripMenuItem_Click(object sender, EventArgs e)
        {
            filter("median"); // L'utilisateur a choisit le filtre median
        }

        // Actualiser les pictures Box
        private void refreshImages()
        {
            // Actualise l'image
            image_db.Refresh(); 
            image_gt.Refresh(); 

            // Image dans pictureBox init
            image_traitee.Image = null;
            img_comparaison.Image = null;

            // Remise � z�ro des labels
            iou_label.Text = "Iou : ";
            vinet_label.Text = "Vinet : ";

            // Lancement du traitement par la fonction
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

        // Fonction pour revennir a l'image d'origine
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

        // Fonction pour faire un zoom avant
        private void avantToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZoomImage(2);
        }

        // Fonction pour faire un zoom arriere
        private void arrièreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZoomImage(0.5);
        }

        // Fonction pour zoomer l'image
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

        // Fonction pour ajuster la taille de l'image
        private void AdjustImageSize(PictureBox pictureBox, int zoomLevel)
        {
            pictureBox.Size = new Size(originalSize.Width * zoomLevel, originalSize.Height * zoomLevel);
        }

        // Fonction pour ajuster la position de l'image
        private void panel1_Scroll(object sender, ScrollEventArgs e)
        {
            AdjustImagePosition();
        }

        // Fonction pour ajuster la position de l'image
        private void AdjustImagePosition()
        {
            int scrollValue_v = panel1.VerticalScroll.Value;
            int scrollValue_h = panel1.HorizontalScroll.Value;

            SetImagePosition(image_db, scrollValue_h, scrollValue_v);
            SetImagePosition(image_gt, scrollValue_h, scrollValue_v);
            SetImagePosition(image_traitee, scrollValue_h, scrollValue_v);
        }

        // Fonction pour positionner l'image
        private void SetImagePosition(PictureBox image, int scrollValue_h, int scrollValue_v)
        {
            image.Top = -scrollValue_v;
            image.Left = -scrollValue_h;
        }

    }
}
