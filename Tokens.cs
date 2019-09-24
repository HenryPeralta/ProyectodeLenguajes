using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1_SegundoSemestre_Lenguajes
{
    class Tokens
    {
        private string lexema;
        private string idToken;
        private int linea;
        private int columna;
        private int indice;

        public Tokens(string lexema, string idToken, int linea, int columna, int indice)
        {
            this.lexema = lexema;
            this.idToken = idToken;
            this.linea = linea;
            this.columna = columna;
            this.indice = indice;
        }
        public int getIndice()
        {
            return this.indice;
        }
        public String getLexema()
        {
            return this.lexema;
        }
        public String getIdToken()
        {
            return this.idToken;
        }
        public int getLinea()
        {
            return this.linea;
        }
        public int getColumna()
        {
            return this.columna;
        }
    }
}
