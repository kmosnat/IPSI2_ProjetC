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

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void buttonOuvrir_Click_1(object sender, EventArgs e)
        {
            if (ouvrirImage.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Bitmap bmp;
                    Image img = Image.FromFile(ouvrirImage.FileName);
                    bmp = new Bitmap(img);

                    image_db.Width = bmp.Width;
                    image_db.Height = bmp.Height;
                    // pour centrer image dans panel
                    if (image_db.Width < image_db.Width)
                        image_db.Left = (image_db.Width - image_db.Width) / 2;

                    if (image_db.Height < image_db.Height)
                        image_db.Top = (image_db.Height - image_db.Height) / 2;

                    image_db.Image = bmp;

                    imageSeuillee.Hide();
                    //valeurSeuilAuto.Hide();
                }
                catch
                {
                    MessageBox.Show("erreur !");
                }
            }
        }

        private void bSeuillageAuto_Click(object sender, EventArgs e)
        {
            // traitement donc transférer data bmp vers C++

            imageSeuillee.Show();
            //valeurSeuilAuto.Show();

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

            imageSeuillee.Width = bmp.Width;
            imageSeuillee.Height = bmp.Height;

            // pour centrer image dans panel
            if (imageSeuillee.Width < image_db.Width)
                imageSeuillee.Left = (image_db.Width - imageSeuillee.Width) / 2;

            if (imageSeuillee.Height < image_db.Height)
                imageSeuillee.Top = (image_db.Height - imageSeuillee.Height) / 2;

            // transférer C++ vers bmp
            imageSeuillee.Image = bmp;

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
                string groundTruthImagePath = Path.Combine(folderBrowserDialog1.SelectedPath, "Ground_truth", imageName + ".png");

                // Vérifie si les fichiers existent
                if (File.Exists(sourceImagePath) && File.Exists(groundTruthImagePath))
                {
                    // Charge les images dans les pictureBox correspondantes
                    image_db.Image = Image.FromFile(sourceImagePath);
                    image_gt.Image = Image.FromFile(groundTruthImagePath);
                }
                else
                {
                    MessageBox.Show("Les fichiers d'image correspondants n'existent pas.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

     
    }
}
