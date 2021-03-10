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
    public partial class verticecrud : Form
    {
        public bool control; // variable de control
        public string dato; // el dato que almacenará el vértice
        public verticecrud()
        {
            InitializeComponent();
            control = false;
            dato = " ";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dato = textBox1.Text.Trim();
            if ((dato == "") || (dato == " "))
            {
                MessageBox.Show("Debes ingresar un valor", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
            }
            else if (dato.Length > 3)
            {
                var result = MessageBox.Show("Se tomaran en cuenta solamente los tres primeros carateres. Desea continuar?", "Advertencia", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    dato = dato.Substring(0, 3);
                    control = true;
                    Hide();
                }
            }
            else
            {
                control = true;
                Hide();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            control = false;
            Hide();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(null, null);
            }
        }

        private void verticecrud_Shown(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox1.Focus();
        }
    }
}
