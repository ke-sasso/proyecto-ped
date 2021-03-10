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
using System.Collections;
using System.Threading;
using System.Drawing.Drawing2D;
using System.IO;

namespace proyecto_ped
{
    public partial class principal : Form
    {
        private grafo grafo; // instanciamos la clase GRAFO
        private vertice nuevoNodo; // instanciamos la clase VERTICE
        private vertice NodoOrigen; // instanciamos la clase VERTICE
        private vertice NodoDestino; // instanciamos la clase VERTICE
        private int var_control = 0; // la utilizaremos para determinar el estado en el mapa: 0 -> sin acción, 1 -> Dibujando arco, 2 -> Nuevo vértice
  
    
        // variables para el control de ventanas modales
        private verticecrud ventanaVertice; // ventana para agregar los vértices
        private arcocrud ventanaArco; // ventana para agregar los arcos
        private editararco ventanaEditarArco; // ventana para editar los arco

        List<vertice> nodosOrdenados; // Lista de nodos ordenadas a partir del nodo origen
        bool profundidad = false, nuevoVertice = false, nuevoArco = false, anchura = false, nodoEncontrado = false, buscarNodo=false;
        private int numeronodos = 0; //Enteros para definir las diferentes opciones y el numero de nodos
        private string destino = "",origen = "";
        Queue cola = new Queue(); //para el recorrido de anchura
        private int distancia = 0;
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex > -1){
                foreach (vertice nodo in grafo.nodos){
                    if (nodo.Valor == comboBox2.SelectedItem.ToString()) {
                        grafo.nodos.Remove(nodo);
                        //Borrando arcos que posea el nodo eliminado
                        nodo.ListaAdyacencia = new List<arco>();
                        break;
                    }
                }
                foreach (vertice nodo in grafo.nodos){
                    foreach (arco arco in nodo.ListaAdyacencia){
                        if (arco.nDestino.Valor == comboBox2.SelectedItem.ToString()){
                            nodo.ListaAdyacencia.Remove(arco);
                            break;
                        }
                    }
                }
                nuevoArco = true;
                nuevoVertice = true;
                comboBox2.SelectedIndex = -1;
                mapa.Refresh();
            }else{
                label5.Text = "Seleccione un nodo";
                label5.BackColor = Color.Red;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox3.SelectedIndex > -1){
                foreach (vertice nodo in grafo.nodos){
                    foreach (arco arco in nodo.ListaAdyacencia){
                        if ("(" + nodo.Valor + "," + arco.nDestino.Valor + ") PESO: " + arco.peso == comboBox3.SelectedItem.ToString()){
                            nodo.ListaAdyacencia.Remove(arco);
                            break;
                        }
                    }
                }
                nuevoVertice = true;
                nuevoArco = true;
                comboBox3.SelectedIndex = -1;
                mapa.Refresh();
            }else{
                label5.Text = "Seleccione un arco";
                label5.BackColor = Color.Red;
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
                    comboBox1.Items.Clear();
                    comboBox1.SelectedIndex = -1;
                    comboBox2.Items.Clear();
                    comboBox2.SelectedIndex = -1;

                    foreach (vertice nodo in grafo.nodos)
                    {
                        comboBox2.Items.Add(nodo.Valor);
                        comboBox1.Items.Add(nodo.Valor);
                    }
                    nuevoVertice = false;
                }
                if (nuevoArco)
                {
                    comboBox3.Items.Clear();
                    comboBox3.SelectedIndex = -1;
                    foreach (vertice nodo in grafo.nodos)
                    {
                        foreach (arco arco in nodo.ListaAdyacencia)
                        {
                            comboBox3.Items.Add("(" + nodo.Valor + "," + arco.nDestino.Valor + ") PESO: " + arco.peso);
                        }
                    }
                    nuevoArco = false;
                }
                if (profundidad)
                {
                    //ordenando los nodos desde el que indica el usuario
                    ordenarListaNodos();
                    foreach (vertice nodo in nodosOrdenados)
                    {
                        if (!nodo.Visitado)
                            recorridoNodoProfundidad(nodo, e.Graphics);
                    }
                    profundidad = false;
                    //reestablecer los valroes
                    foreach (vertice nodo in grafo.nodos)
                        nodo.Visitado = false;

                }
                if (anchura)
                {
                    distancia = 0;
                    //ordenando los nodos desde el que indica el usuario
                    cola = new Queue();
                    ordenarListaNodos();
                    foreach (vertice nodo in nodosOrdenados)
                    {
                        if (!nodo.Visitado && !nodoEncontrado)
                            recorridoNodoAnchura(nodo, e.Graphics, destino);
                    }
                    anchura = false;
                    nodoEncontrado = false;
                    //reestablecer los valroes
                    foreach (vertice nodo in grafo.nodos)
                        nodo.Visitado = false;
                }
                if (buscarNodo)
                {
                    grafo.BuscarVertice(textBox1.Text).colorear(e.Graphics);
                    Thread.Sleep(1000);
                    grafo.BuscarVertice(textBox1.Text).DibujarVertice(e.Graphics);
                    buscarNodo = false;
                }

                if (ventanaEditarArco.control)
                {
                    int distancia = ventanaEditarArco.dato;
                    if (comboBox3.SelectedIndex > -1)
                    {
                        foreach (vertice nodo in grafo.nodos) {
                            foreach (arco arco in nodo.ListaAdyacencia) {
                                if ("(" + nodo.Valor + "," + arco.nDestino.Valor + ") PESO: " + arco.peso == comboBox3.SelectedItem.ToString()) {
                                    arco.peso = distancia;
                                    break;
                                }
                            }
                        }
                        nuevoVertice = true;
                        nuevoArco = true;
                        comboBox3.SelectedIndex = -1;
                        mapa.Refresh();
                        ventanaEditarArco.control = false;
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void recorridoNodoAnchura(vertice vertice, Graphics g, string destino)
        {
            vertice.Visitado = true;
            cola.Enqueue(vertice);
            vertice.colorear(g);
            Thread.Sleep(1000);
            vertice.DibujarVertice(g);
            if (vertice.Valor == destino) {
                nodoEncontrado = true;
                return;
            }
            while (cola.Count > 0){
                vertice aux = (vertice)cola.Dequeue();
                foreach (arco adya in aux.ListaAdyacencia) {
                    if (!adya.nDestino.Visitado) {
                        if (!nodoEncontrado) {
                            adya.nDestino.Visitado = true;
                            adya.nDestino.colorear(g);
                            Thread.Sleep(1000);
                            adya.nDestino.DibujarVertice(g);
                            if (destino != "")
                                distancia += adya.peso;
                            cola.Enqueue(adya.nDestino);
                            if (adya.nDestino.Valor == destino)
                            {
                                nodoEncontrado = true;
                                return;
                            }
                        }
                    }
                }
            }
        }
        public void ordenarListaNodos()
        {
            nodosOrdenados = new List<vertice>();
            bool est = false;
            foreach (vertice nodo in grafo.nodos)
            {
                if (nodo.Valor == origen)
                {
                    nodosOrdenados.Add(nodo);
                    est = true;
                }
                else if (est)
                    nodosOrdenados.Add(nodo);
            }
            foreach (vertice nodo in grafo.nodos)
            {
                if (nodo.Valor == origen)
                {
                    est = false;
                    break;
                }
                else if (est)
                    nodosOrdenados.Add(nodo);
            }
        }
        private void recorridoNodoProfundidad(vertice vertice, Graphics g)
        {
            vertice.Visitado = true;
            vertice.colorear(g);
            Thread.Sleep(1000);
            vertice.DibujarVertice(g);
            foreach (arco adya in vertice.ListaAdyacencia){
                if (!adya.nDestino.Visitado) recorridoNodoProfundidad(adya.nDestino, g);
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

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            Graphics gr = this.CreateGraphics();
            // Tamaño de lo que queremos copiar
            Size fSize = this.Size;
            // Creamos el bitmap con el área que vamos a capturar
            // En este caso, con el tamaño del formulario actual
            Bitmap bm = new Bitmap(fSize.Width, fSize.Height, gr);
            // Un objeto Graphics a partir del bitmap
            Graphics gr2 = Graphics.FromImage(bm);
            // Copiar el área de la pantalla que ocupa el formulario
            gr2.CopyFromScreen(this.Location.X, this.Location.Y, 0, 0, fSize);

            // Asignamos la imagen al PictureBox
            this.pictureBox1.Image = bm;
          
            //bm.Save("C:\\Users\\kevin.sasso\\Desktop\\POO\\proyecto-ped\\proyecto-ped\\imagenes\\mapa-esa.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.pictureBox1.Image.Save("mapa-esa.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
           // this.pictureBox1.Image.Save("C:\\Users\\kevin.sasso\\Desktop\\POO\\proyecto-ped\\proyecto-ped\\imagenes\\mapa-esa.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (comboBox3.SelectedIndex > -1)
            {
                ventanaEditarArco.Visible = false;
                ventanaEditarArco.control = false;
                ventanaEditarArco.ShowDialog();
            }
            else
            {
                label5.Text = "Seleccione un arco";
                label5.BackColor = Color.Red;
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() != "")
            {
                if (grafo.BuscarVertice(textBox1.Text) != null)
                {
                    label5.Text = "Si se encuentra el vértice " + textBox1.Text;
                    label5.BackColor = Color.Green;
                    buscarNodo = true;
                    mapa.Refresh();
                }
                else
                {
                    label5.Text = "No se encuentra el vértice " + textBox1.Text;
                    label5.BackColor = Color.Red;
                }
            }
        }

        private void nUEVOVERTICEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            nuevoNodo = new vertice();
            var_control = 2; // recordemos que es usado para indicar el estado en la pizarra: 0 ->
                            // sin accion, 1 -> Dibujando arco, 2 -> Nuevo vértice  
        }

   


        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1){
                profundidad = true;
                origen = comboBox1.SelectedItem.ToString();
                mapa.Refresh();
                comboBox1.SelectedIndex = -1;
            }else{
                label5.Text = "Seleccione un nodo de partida";
                label5.BackColor = Color.Red;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1){
                origen = comboBox1.SelectedItem.ToString();
                anchura = true;
                mapa.Refresh();
                comboBox1.SelectedIndex = -1;
            } else{
                label5.Text = "Seleccione un nodo de partida";
                label5.BackColor = Color.Red;
            }
        }

        public principal()
        {
            InitializeComponent();
            grafo = new grafo();
            nuevoNodo = null;
            var_control = 0;
            ventanaVertice = new verticecrud();
            ventanaArco = new arcocrud();
            ventanaEditarArco = new editararco();
            nodosOrdenados = new List<vertice>();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
        }

        private void principal_Load(object sender, EventArgs e)
        {

        }
    }
}
