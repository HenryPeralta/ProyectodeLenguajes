using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1_SegundoSemestre_Lenguajes
{
    class PalabrasReservadas
    {
        private string lexema;
        private string idToken;

        public PalabrasReservadas(string lexema, string idToken)
        {
            this.lexema = lexema;
            this.idToken = idToken;
        }

        public String getLexema()
        {
            return this.lexema;
        }
        public String getIdToken()
        {
            return this.idToken;
        }
    }
}
