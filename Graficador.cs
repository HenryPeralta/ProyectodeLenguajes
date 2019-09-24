using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Proyecto1_SegundoSemestre_Lenguajes
{
    class Graficador
    {
        public string graficar(String texto)
        {
            StringBuilder grafo;
            grafo = new StringBuilder();
            grafo.Append("digraph G { ");
            grafo.Append(texto);
            grafo.Append("}");

            StreamWriter sw = new StreamWriter("C:\\Ejemplo_graphviz\\dot.txt");
            sw.Write(grafo);
            sw.Close();

            string ruta = "C:\\Ejemplo_graphviz\\dot.txt";
            string ruta2 = "C:\\Ejemplo_graphviz\\Arbol.png";

            string cmd = "dot -Tpng " + ruta + " -o " + ruta2;

            System.Diagnostics.ProcessStartInfo miProceso = new System.Diagnostics.ProcessStartInfo("cmd", "/c " + cmd);
            System.Diagnostics.Process.Start(miProceso);

            return ruta2;
        }
    }
}
