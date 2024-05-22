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

            comboBox1.Items.Add("Par défaut");
            comboBox1.Items.Add("Disque");
            comboBox1.Items.Add("Carré");

            comboBox1.SelectedIndex = 0;
        }

        private int getKernel()
        {
            string value = textBox1.Text;
            int val = int.Parse(value);

            if (val % 2 == 1 && val > 0)
            {
                return val;
            }

            return -1;

        }

        private string getStr()
        {
            int value = comboBox1.SelectedIndex;
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
            
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
