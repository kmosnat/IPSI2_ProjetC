using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POAT
{
    public partial class Filter : Form
    {
        public Filter()
        {
            InitializeComponent();

            comboBox.Items.Add("Par défaut");
            comboBox.Items.Add("Disque");
            comboBox.Items.Add("Carré");

            comboBox.SelectedIndex = 0;
        }

        // Get pour savoir ce que l'utilisateur a choisit comme taille d'element structurant, la taille doit etre impair et positive
        public int getKernelSize()
        {
            string value = kenelSizeTextBox.Text;
            int val = int.Parse(value);

            if (val % 2 == 1 && val > 0)
            {
                return val;
            }

            return -1;

        }

        // Get pour savoir ce que l'utilisateur a choisit comme forme de l'element structurant
        public string getStrElement()
        {
            int value = comboBox.SelectedIndex;
            switch (value)
            {
                case 0: { return ""; }
                case 1: { return "disk"; }
                case 2: { return "carre"; }
            }
            return "";
        }

        // Donne le signal au formulaire MainForm pour dire que l'utilisateur a fait son choix
        private void okButton_Click_1(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        // Ferme le formulaire Filter
        private void quitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
