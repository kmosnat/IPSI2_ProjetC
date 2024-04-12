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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void AjouterNoeudsLnSn()
        {
            TreeNode noeud_ln = new TreeNode("In");
            TreeNode noeud_sn = new TreeNode("Sn");

            for (int i = 0; i < 600; i++)
            {
                TreeNode enfant_ln = new TreeNode
                {
                    ImageIndex = i,
                    SelectedImageIndex = i,
                    Text = "In_" + (i + 1).ToString(),
                };
                noeud_ln.Nodes.Add(enfant_ln);

                int a = 300 + i;
                TreeNode enfant_sn = new TreeNode
                {
                    ImageIndex = a,
                    Text = "Sn_" + (i + 1).ToString(),
                };
                noeud_sn.Nodes.Add(enfant_sn);
            }
            treeView_in_sc.Nodes.Add(noeud_ln);
            treeView_in_sc.Nodes.Add(noeud_sn);

        }

        private void bouton_ouvrir_Click(object sender, EventArgs e)
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

                // Vérifie si les fichiers existent
                if (File.Exists(sourceImagePath) && File.Exists(groundTruthImagePath))
                {
                    // Charge les images dans les pictureBox correspondantes
                    image_db.Image = Image.FromFile(sourceImagePath);
                    image_gt.Image = Image.FromFile(groundTruthImagePath);

                    Bitmap bmp = new Bitmap(image_db.Image);
                    ClImage Img = new ClImage();

                    unsafe
                    {
                        BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                        Img.objetLibDataImgPtr(1, bmpData.Scan0, bmpData.Stride, bmp.Height, bmp.Width);
                        // 1 champ texte retour C++, le seuil auto
                        bmp.UnlockBits(bmpData);
                    }

                    //valeurSeuilAuto.Text = Img.objetLibValeurChamp(0).ToString();

                    image_traitée.Width = bmp.Width;
                    image_traitée.Height = bmp.Height;

                    // pour centrer image dans panel
                    if (image_traitée.Width < image_db.Width)
                        image_traitée.Left = (image_db.Width - image_traitée.Width) / 2;

                    if (image_traitée.Height < image_db.Height)
                        image_traitée.Top = (image_db.Height - image_traitée.Height) / 2;

                    // transférer C++ vers bmp
                    image_traitée.Image = bmp;
                }
                else
                {
                    MessageBox.Show("Les fichiers d'image correspondants n'existent pas.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }


    }
}
