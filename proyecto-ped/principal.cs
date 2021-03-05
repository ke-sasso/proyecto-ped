using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Threading;
using System.Drawing.Drawing2D;

namespace proyecto_ped
{
    public partial class principal : Form
    {
        private grafo grafo; // instanciamos la clase CGrafo
        private vertice nuevoNodo; // instanciamos la clase CVertice
        private vertice NodoOrigen; // instanciamos la clase CVertice
        private vertice NodoDestino; // instanciamos la clase CVertice
        private int var_control = 0; // la utilizaremos para determinar el estado en la pizarra:
        // 0 -> sin acción, 1 -> Dibujando arco, 2 -> Nuevo vértice
        // variables para el control de ventanas modales
        //private Recorrido ventanaRecorrido; // ventana para seleccionar el nodo inicial del recorrido
        private Label[] arreglo, arreglo2; //Arreglos de Label se usan para la simulacion de la cola, pila y vector
        private verticecrud ventanaVertice; // ventana para agregar los vértices
        private arcocrud ventanaArco; // ventana para agregar los arcos
        List<vertice> nodosRuta; // Lista de nodos utilizada para almacenar la ruta
        List<vertice> nodosOrdenados; // Lista de nodos ordenadas a partir del nodo origen
        bool buscarRuta = false, nuevoVertice = false, nuevoArco = false;
        private int numeronodos = 0, opc; //Enteros para definir las diferentes opciones y el numero de nodos
        double peso = 0.0;
        bool profundidad = false, anchura = false, nodoEncontrado = false, buscarNodo = false;

        private void mapa_MouseLeave(object sender, EventArgs e)
        {
            mapa.Refresh();
        }

        private void mapa_MouseMove(object sender, MouseEventArgs e)
        {
            switch (var_control)
            {
                case 2: //Creando nuevo nodo
                    if (nuevoNodo != null)
                    {
                        int posX = e.Location.X;
                        int posY = e.Location.Y;
                        if (posX < nuevoNodo.Dimensiones.Width / 2)
                            posX = nuevoNodo.Dimensiones.Width / 2;
                        else if (posX > mapa.Size.Width - nuevoNodo.Dimensiones.Width / 2)
                            posX = mapa.Size.Width - nuevoNodo.Dimensiones.Width / 2;
                        if (posY < nuevoNodo.Dimensiones.Height / 2)
                            posY = nuevoNodo.Dimensiones.Height / 2;
                        else if (posY > mapa.Size.Height - nuevoNodo.Dimensiones.Width / 2)
                            posY = mapa.Size.Height - nuevoNodo.Dimensiones.Width / 2;
                        nuevoNodo.Posicion = new Point(posX, posY);
                        mapa.Refresh();
                        nuevoNodo.DibujarVertice(mapa.CreateGraphics());
                    }
                    break;

                case 1: // Dibujar arco
                    AdjustableArrowCap bigArrow = new AdjustableArrowCap(4, 4, true);
                    bigArrow.BaseCap = System.Drawing.Drawing2D.LineCap.Triangle;
                    mapa.Refresh();
                    mapa.CreateGraphics().DrawLine(new Pen(Brushes.Black, 2) { CustomEndCap = bigArrow },
                         NodoOrigen.Posicion, e.Location);
                    break;
            }
        }

        private void mapa_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left) // Si se ha presionado el botón
            // izquierdo del mouse
            {
                if ((NodoOrigen = grafo.DetectarPunto(e.Location)) != null)
                {
                    var_control = 1; // recordemos que es usado para indicar el estado en la pizarra:
                    // 0 -> sin accion, 1 -> Dibujando arco, 2 -> Nuevo vértice
                }
                if (nuevoNodo != null && NodoOrigen == null)
                {
                    ventanaVertice.Visible = false;
                    ventanaVertice.control = false;
                    ventanaVertice.ShowDialog();
                    numeronodos = grafo.nodos.Count;//cuenta cuantos nodos hay en el grafo  
                    if (ventanaVertice.control)
                    {
                        if (grafo.BuscarVertice(ventanaVertice.dato) == null)
                        {
                            grafo.AgregarVertice(nuevoNodo);
                            nuevoNodo.Valor = ventanaVertice.dato;
                        }
                        else
                        {
                            label5.Text = "El Nodo " + ventanaVertice.dato + " ya existe en el grafo";
                            label5.BackColor = Color.Red;
                        }
                    }
                    nuevoNodo = null;
                    nuevoVertice = true;
                    var_control = 0;
                    mapa.Refresh();
                }
                if (e.Button == System.Windows.Forms.MouseButtons.Right) // Si se ha presionado el botón
                // derecho del mouse
                {
                    if (var_control == 0)
                    {
                        if ((NodoOrigen = grafo.DetectarPunto(e.Location)) != null)
                        {
                            // nuevoVerticeToolStripMenuItem.Text = "Nodo " + NodoOrigen.Valor;
                        }
                        else
                            mapa.ContextMenuStrip = this.contextMenuStrip1;
                    }
                }
            }
            if (e.Button == System.Windows.Forms.MouseButtons.Right) // Si se ha presionado el botón
            // derecho del mouse
            {
                if (var_control == 0)
                {
                    if ((NodoOrigen = grafo.DetectarPunto(e.Location)) != null)
                    {
                        //nuevoVerticeToolStripMenuItem.Text = "Nodo " + NodoOrigen.Valor;
                    }
                    else
                        mapa.ContextMenuStrip = this.contextMenuStrip1;
                }
            }
        }

        private void mapa_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                grafo.DibujarGrafo(e.Graphics);
                if (nuevoVertice)
                {
                    /*comboBox1.Items.Clear();
                    comboBox1.SelectedIndex = -1;
                    comboBox5.Items.Clear();
                    comboBox5.SelectedIndex = -1;
                    */
                    foreach (vertice nodo in grafo.nodos)
                    {
                        /*comboBox1.Items.Add(nodo.Valor);
                        comboBox5.Items.Add(nodo.Valor);*/
                    }
                    nuevoVertice = false;
                }
                if (nuevoArco)
                {
                    /*comboBox2.Items.Clear();
                    comboBox2.SelectedIndex = -1;*/
                    foreach (vertice nodo in grafo.nodos)
                    {
                        foreach (arco arco in nodo.ListaAdyacencia)
                        {
                            //comboBox2.Items.Add("(" + nodo.Valor + "," + arco.nDestino.Valor + ") peso: " + arco.peso);
                        }
                    }
                    nuevoArco = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void mapa_MouseUp(object sender, MouseEventArgs e)
        {
            switch (var_control)
            {
                case 1: // Dibujando arco
                    if ((NodoDestino = grafo.DetectarPunto(e.Location)) != null && NodoOrigen !=
                   NodoDestino)
                    {
                        ventanaArco.Visible = false;
                        ventanaArco.control = false;
                        ventanaArco.ShowDialog();
                        if (ventanaArco.control)
                        {
                            if (grafo.AgregarArco(NodoOrigen, NodoDestino, ventanaArco.dato)) //Se procede a crear la arista
                            {
                                int distancia = ventanaArco.dato;
                                NodoOrigen.ListaAdyacencia.Find(v => v.nDestino == NodoDestino).peso =
                               distancia;
                            }
                            nuevoArco = true;
                        }
                    }
                    var_control = 0;
                    NodoOrigen = null;
                    NodoDestino = null;
                    mapa.Refresh();
                    break;
            }
        }

        private void nUEVOVERTICEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            nuevoNodo = new vertice();
            var_control = 2; // recordemos que es usado para indicar el estado en la pizarra: 0 ->
            // sin accion, 1 -> Dibujando arco, 2 -> Nuevo vértice  
        }

        Queue cola = new Queue(); //para el recorrido de anchura
        private string destino = "", origen = "";

        public principal()
        {
            InitializeComponent();
            grafo = new grafo();
            nuevoNodo = null;
            var_control = 0;
            ventanaVertice = new verticecrud();
            ventanaArco = new arcocrud();
            nodosOrdenados = new List<vertice>();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
        }

        private void principal_Load(object sender, EventArgs e)
        {

        }
    }
}
