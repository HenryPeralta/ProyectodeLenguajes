using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1_SegundoSemestre_Lenguajes
{
    class Continente
    {
        public string continente;
        public List<Pais> paises;

        public Continente(string continente)
        {
            this.continente = continente;
            paises = new List<Pais>();
        }
    }
}
