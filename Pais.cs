using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1_SegundoSemestre_Lenguajes
{
    class Pais
    {
        public string nombre, poblacion, saturacion, bandera;
        
        public Pais()
        {
            nombre = "";
            poblacion = "";
            saturacion = "";
            bandera = "";
        }

        public Pais(string nombre, string poblacion, string saturacion, string bandera)
        {
            this.nombre = nombre;
            this.poblacion = poblacion;
            this.saturacion = saturacion;
            this.bandera = bandera;
        }
    }
}
