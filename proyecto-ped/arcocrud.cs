using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace proyecto_ped
{
    public partial class arcocrud : Form
    {
        public bool control; // variable de control
        public int dato; // el dato que almacenará el vértice
        public arcocrud()
        {
            InitializeComponent();
            control = false;
            dato = 0;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                dato = Convert.ToInt16(textBox1.Text.Trim());

                if (dato < 0)
                {
                    MessageBox.Show("Debes ingresar un valor positivo", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation);
                }
                else
                {
                    control = true;
                    Hide();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Debes ingresar un valor numerico");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            control = false;
            Hide();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void arcocrud_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(null, null);
            }
        }
    }
}
