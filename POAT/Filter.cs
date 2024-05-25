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

        public int getKernel()
        {
            string value = kenelSizeTextBox.Text;
            int val = int.Parse(value);

            if (val % 2 == 1 && val > 0)
            {
                return val;
            }

            return -1;

        }

        public string getStr()
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


        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
