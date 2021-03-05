using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace proyecto_ped
{
    class arco
    {
        #region Atributos
        public vertice nDestino;
        public int peso;  //Peso de cada arista
        public float grosor_flecha;
        public Color color;
        #endregion

        #region Métodos
        //Metodo para actualizar el arco destino
        public arco(vertice destino) : this(destino, 1)
        {
            this.nDestino = destino;
        }

        //Metodo para actualizar información general del arco 
        public arco(vertice destino, int peso)
        {
            this.nDestino = destino;
            this.peso = peso;
            this.grosor_flecha = 3;
            this.color = Color.DarkRed;
        }
        #endregion
    }
}
