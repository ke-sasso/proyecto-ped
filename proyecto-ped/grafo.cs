using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing; // Librería agregada, para poder dibujar
using System.Drawing.Drawing2D;

namespace proyecto_ped
{
    class grafo
    {
        public List<vertice> nodos; // Lista de nodos del grafo
        public grafo() // Constructor
        {
            nodos = new List<vertice>();
        }
        //=====================Operaciones Básicas=================================
        //Construye un nodo a partir de su valor y lo agrega a la lista de nodos
        public vertice AgregarVertice(string valor)
        {
            vertice nodo = new vertice(valor);
            nodos.Add(nodo);
            return nodo;
        }
        //Agrega un nodo a la lista de nodos del grafo
        public void AgregarVertice(vertice nuevonodo)
        {
            nodos.Add(nuevonodo);
        }
        //Busca un nodo en la lista de nodos del grafo
        public vertice BuscarVertice(string valor)
        {
            return nodos.Find(v => v.Valor == valor);
        }
        //Crea una arista a partir de los valores de los nodos de origen y de destino
        public bool AgregarArco(string origen, string nDestino, int peso = 1)
        {
            vertice vOrigen, vnDestino;
            //Si alguno de los nodos no existe, se activa una excepción
            if ((vOrigen = nodos.Find(v => v.Valor == origen)) == null)
                throw new Exception("El nodo " + origen + " no existe dentro del grafo");
            if ((vnDestino = nodos.Find(v => v.Valor == nDestino)) == null)
                throw new Exception("El nodo " + nDestino + " no existe dentro del grafo");
            return AgregarArco(vOrigen, vnDestino);
        }
        // Crea la arista a partir de los nodos de origen y de destino
        public bool AgregarArco(vertice origen, vertice nDestino, int peso = 1)
        {
            if (origen.ListaAdyacencia.Find(v => v.nDestino == nDestino) == null)
            {
                origen.ListaAdyacencia.Add(new arco(nDestino, peso));
                return true;
            }
            return false;
        }
        // Método para dibujar el grafo
        public void DibujarGrafo(Graphics g)
        {
            // Dibujando los arcos
            foreach (vertice nodo in nodos)
                nodo.DibujarArco(g);

            // Dibujando los vértices
            foreach (vertice nodo in nodos)
                nodo.DibujarVertice(g);
        }
        public vertice DetectarPunto(Point posicionMouse)
        {
            foreach (vertice nodoActual in nodos)
                if (nodoActual.DetectarPunto(posicionMouse)) return nodoActual;
            return null;
        }
        // Método para regresar al estado original
        public void ReestablecerGrafo(Graphics g)
        {
            foreach (vertice nodo in nodos)
            {
                nodo.Color = Color.White;
                nodo.FontColor = Color.Black;
                foreach (arco arco in nodo.ListaAdyacencia)
                {
                    arco.grosor_flecha = 1;
                    arco.color = Color.Black;
                }
            }
            DibujarGrafo(g);
        }

        public void ColoArista(string o, string d)
        {
            foreach (vertice nodo in nodos)
            {
                foreach (arco a in nodo.ListaAdyacencia)
                {
                    if (nodo.ListaAdyacencia != null && nodo.Valor == o && a.nDestino.Valor == d)
                    {
                        a.color = Color.Red;
                        a.grosor_flecha = 4;
                    }
                }
            }
        }

        public void Colorear(vertice nodo)
        {
            nodo.Color = Color.AliceBlue;
            nodo.FontColor = Color.Black;
        }

        public vertice nododistanciaminima()
        {
            int min = int.MaxValue;
            vertice temp = null;
            foreach (vertice origen in nodos)
            {
                if (origen.Visitado)
                {
                    foreach (vertice destino in nodos)
                    {
                        if (!destino.Visitado)
                        {
                            foreach (arco a in origen.ListaAdyacencia)
                            {
                                if (a.nDestino == destino && min > a.peso)
                                {
                                    min = a.peso;
                                    temp = destino;
                                }
                            }
                        }
                    }
                }
            }
            return temp;
        }

        //Funcion para re-dibujar los arcos que llegan a un nodo
        public void DibujarEntrantes(vertice nDestino)
        {
            foreach (vertice nodo in nodos)
            {
                foreach (arco a in nodo.ListaAdyacencia)
                {
                    if (nodo.ListaAdyacencia != null && nodo != nDestino)
                    {
                        if (a.nDestino == nDestino)
                        {
                            a.color = Color.Black;
                            a.grosor_flecha = 2;
                            break;
                        }
                    }
                }
            }
        }

        //funcion que desmarca como visitados todos los nodos del grafo
        public void Desmarcar()
        {
            foreach (vertice n in nodos)
            {
                n.Visitado = false;
                n.Padre = null;
                n.distancianodo = int.MaxValue;
                n.pesoasignado = false;
            }
        }


    }

}
