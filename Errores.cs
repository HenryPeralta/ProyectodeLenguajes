using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1_SegundoSemestre_Lenguajes
{
    class Errores
    {
        private string lexema;
        private string idToken;
        private int linea;
        private int columna;

        public Errores(string lexema, string idToken, int linea, int columna)
        {
            this.lexema = lexema;
            this.idToken = idToken;
            this.linea = linea;
            this.columna = columna;
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
