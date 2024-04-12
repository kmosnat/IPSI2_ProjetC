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

        private void AjouterNoeudsLnSn()
        {
            TreeNode noeud_ln = new TreeNode("In");
            TreeNode noeud_sn = new TreeNode("Sc");

            // Trie les clés de ImageList
            List<string> sortedKeys = In_Sc_list.Images.Keys.Cast<string>()
                             .Select(key => Path.GetFileNameWithoutExtension(key))
                             .OrderBy(key => int.Parse(key.Substring(3)))
                             .ToList();

            // Parcours des clés/Images triées
            foreach (var imageEntryKey in sortedKeys)
            {
                if (imageEntryKey.StartsWith("In_"))
                {
                    TreeNode enfant_ln = new TreeNode
                    {
                        ImageIndex = In_Sc_list.Images.Keys.IndexOf(imageEntryKey),
                        SelectedImageIndex = In_Sc_list.Images.Keys.IndexOf(imageEntryKey),
                        Text = imageEntryKey
                    };
                    noeud_ln.Nodes.Add(enfant_ln);
                }
                else if (imageEntryKey.StartsWith("Sc_"))
                {
                    TreeNode enfant_sn = new TreeNode
                    {
                        ImageIndex = In_Sc_list.Images.Keys.IndexOf(imageEntryKey),
                        SelectedImageIndex = In_Sc_list.Images.Keys.IndexOf(imageEntryKey),
                        Text = imageEntryKey
                    };
                    noeud_sn.Nodes.Add(enfant_sn);
                }
            }

            treeView_in_sc.Nodes.Add(noeud_ln);
            treeView_in_sc.Nodes.Add(noeud_sn);
        }

        private async void processImage(string Source, string Ground_Truth)
        {
            await Task.Run(() =>
            {
                // Chargement des images
                var imageDb = Image.FromFile(Source);
                var imageGt = Image.FromFile(Ground_Truth);

                Bitmap bmp = new Bitmap(imageDb);
                Bitmap bmpGt = new Bitmap(imageGt);

                ClImage Img = new ClImage();

                unsafe
                {
                    var bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                    var bmpDataGt = bmpGt.LockBits(new Rectangle(0, 0, bmpGt.Width, bmpGt.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

                    Img.processPtr(2, bmpData.Scan0, bmpDataGt.Scan0, bmpData.Stride, bmp.Height, bmp.Width);

                    bmp.UnlockBits(bmpData);
                    bmpGt.UnlockBits(bmpDataGt);
                }

                this.Invoke(new Action(() =>
                {
                    iou_label.Text = "Iou (%) : " + Img.objetLibValeurChamp(0).ToString();
                    vinet_label.Text = "Vinet (%) : " + Img.objetLibValeurChamp(1).ToString();
                    image_traitée.Image = bmp;
                }));
            });
        }

        private void ouvrirDossierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Choix du dossier contenant les images
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                string selectedPath = folderBrowserDialog1.SelectedPath;
                string sourceImagesFolder = Path.Combine(selectedPath, "Source_Images");

                // Charger toutes les images du dossier Source Images - bmp
                string[] sourceImageFiles = Directory.GetFiles(sourceImagesFolder, "*.bmp");
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

                // Construit le chemin complet de l'image dans le dossier Source Images - bmp
                string sourceImagePath = Path.Combine(folderBrowserDialog1.SelectedPath, "Source_Images", imageName + ".bmp");

                // Construit le chemin complet de l'image dans le dossier Ground truth - png
                string groundTruthImagePath = Path.Combine(folderBrowserDialog1.SelectedPath, "Ground_truth", imageName + ".bmp");

                if (!File.Exists(sourceImagePath) || !File.Exists(groundTruthImagePath))
                {
                    MessageBox.Show("Les fichiers d'image correspondants n'existent pas.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Remise à zéro des images
                image_db.Image = null;
                image_gt.Image = null;
                image_traitée.Image = null;

                //remise à zéro des labels
                iou_label.Text = "Iou (%) : ";
                vinet_label.Text = "Vinet (%) : ";

                //affiche l'image dans le picturebox
                image_db.Image = Image.FromFile(sourceImagePath);
                image_gt.Image = Image.FromFile(groundTruthImagePath);

                // Appel de la méthode processImage
                processImage(sourceImagePath, groundTruthImagePath);
            }

        }


    }
}
