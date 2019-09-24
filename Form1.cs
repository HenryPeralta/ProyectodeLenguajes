using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto1_SegundoSemestre_Lenguajes
{
    public partial class Form1 : Form
    {
        List<Continente> Listacontinentes = new List<Continente>();

        ArrayList Lista = new ArrayList();
        int Contar = 0;

        static private List<Tokens> lista_de_tokens;
        static private List<Errores> lista_de_errores;

        Graficador arbol;
        public Form1()
        {
            InitializeComponent();
        }

        public RichTextBox GetRichTextBox()
        {
            RichTextBox rt = null;
            TabPage tp = tabControl1.SelectedTab;

            if (tp != null)
            {

                rt = tp.Controls[0] as RichTextBox;

            }

            return rt;

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            String texto;
            texto = GetRichTextBox().Text.ToUpper();
            AnalizadorLexico analiz = new AnalizadorLexico();
            analiz.Analizador_cadena(texto, GetRichTextBox());

            analiz.generarLista();

            analiz.generarError();

            lista_de_tokens = new List<Tokens>();
            lista_de_tokens = analiz.getListaTokens();

            lista_de_errores = new List<Errores>();
            lista_de_errores = analiz.GetErrores();

            if (lista_de_errores == null || lista_de_errores.Count == 0)
            {
                analiz.Html_Tokens();
   
            }
            else
            {
                MessageBox.Show("Hay Errores Existentes");
                analiz.Html_Errores();
            }

            graficar();
            //getMenorPoblacion();




        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TabPage tp = new TabPage("Nueva Pestaña");
            RichTextBox rt = new RichTextBox();
            rt.Dock = DockStyle.Fill;


            OpenFileDialog open = new OpenFileDialog();
            open.Title = "Abrir Archivo de Entrada";
            open.Filter = "Archivos ORG |*.ORG";
            open.InitialDirectory = @"C:Escritorio";
            open.FileName = rt.Text;

            string texto = "";
            string fila = "";

            if (open.ShowDialog() == DialogResult.OK)
            {
                string ruta1 = open.FileName;

                StreamReader streamReader = new StreamReader(ruta1, System.Text.Encoding.UTF8);
                string nombreC = Path.GetFileNameWithoutExtension(open.FileName);

                while ((fila = streamReader.ReadLine()) != null)
                {
                    texto += fila + System.Environment.NewLine;
                }
                rt.Text = texto;
                streamReader.Close();

                tp.Controls.Add(rt);
                tabControl1.TabPages.Add(tp);
            }
        }

        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "Archivo de Entrada (.ORG)|*.ORG";
            saveFileDialog1.Title = "Guardar archivo";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {

                using (Stream s = File.Open(saveFileDialog1.FileName, FileMode.CreateNew))
                using (StreamWriter sw = new StreamWriter(s))
                {
                    sw.Write(GetRichTextBox().Text);
                }

            }
        }

        private void guardarComoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            TabPage tp = new TabPage("Nueva");
            RichTextBox rt = new RichTextBox();
            rt.Dock = DockStyle.Fill;

            tp.Controls.Add(rt);
            tabControl1.TabPages.Add(tp);
        }

        private void guardarComoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        public void graficar()
        {
            /*List<Continente> Listacontinentes=new List<Continente>();
            Listacontinentes.Add(new Continente("Oceania"));
            Listacontinentes.Add(new Continente("America"));
            Listacontinentes.Add(new Continente("Asia"));

            Continente europa = new Continente("Europa");
            europa.paises.Add(new Pais("España", "xxxx","15","xxx"));
            europa.paises.Add(new Pais("Alemania", "xxxx", "50", "xxx"));
            europa.paises.Add(new Pais("Holanda", "xxxx", "90", "xxx"));
            europa.paises.Add(new Pais("Portugal", "xxxx", "75", "xxx"));
            europa.paises.Add(new Pais("Serbia", "xxxx", "36", "xxx"));

            Listacontinentes.Add(europa);




            foreach (Continente continente in Listacontinentes){
                Console.WriteLine(continente.continente + "\n");
                foreach (Pais pais in continente.paises)
                {
                    Console.WriteLine("     ->" + pais.nombre);
                }
            }*/


            Boolean primerCadena = false;
            Boolean segundaCadena = false;
            string cont = "";
            string nombre = "", poblacion = "", saturacion = "", bandera = "";
            bool fnombre = false, fpoblacion = false, fsaturacion = false, fbandera = false;

            foreach (Tokens token in lista_de_tokens)
            {
                //Console.WriteLine("---->" + token.getLexema() + " ----- " + token.getIdToken());
                if (token.getLexema() == "CONTINENTE")
                {
                    //Console.WriteLine("\n\n\n");
                    primerCadena = true;
                }

                if (token.getIdToken() == "Cadena" && primerCadena == true)
                {
                    //Console.WriteLine("------\n\n\n----");
                    cont = token.getLexema().Replace('"', ' ');
                    Listacontinentes.Add(new Continente(cont));
                    primerCadena = false;
                    //Console.WriteLine("-------->" + cont + "<---------------------\n\n\n");
                }

                if (token.getLexema() == "PAIS")
                {
                    segundaCadena = true;
                }

                if (token.getLexema() == "}"  &&  segundaCadena == true)
                {
               
                    var continenteP = from obj in Listacontinentes
                                      where obj.continente == cont
                                      select obj;

                    

                    foreach (var p in continenteP)
                    {
                        //Console.WriteLine("-------------------> Agregando " + nombre + " en: " + p.continente + " , guardados: " + p.paises.Capacity);
                        p.paises.Add(new Pais(nombre, poblacion, saturacion, bandera));
                    }


                    segundaCadena = false;
                    fnombre = false;
                    fpoblacion = false;
                    fsaturacion = false;
                    fbandera = false;

                    //Console.WriteLine("\n\n\n --------------- FIN DE PAIS --------------------\n\n\n");
                }

                if (segundaCadena && token.getLexema() == "NOMBRE")
                {
                    fnombre = true;
                }else if (segundaCadena && token.getLexema() == "POBLACION")
                {
                    fpoblacion = true;
                }
                else if (segundaCadena && token.getLexema() == "SATURACION")
                {
                    fsaturacion = true;
                }
                else if (segundaCadena && token.getLexema() == "BANDERA")
                {
                    fbandera = true;
                }


                if (fnombre && segundaCadena && token.getIdToken() == "Cadena")
                {
                    nombre = token.getLexema().Replace('"',' ');
                    //Console.WriteLine("\n++++++++>" + nombre +"<++++++++++\n");
                    fnombre = false;
                }

                if (fpoblacion && segundaCadena && token.getIdToken() == "Numero")
                {
                    poblacion = token.getLexema();
                    //Console.WriteLine("\n++++++++>" + poblacion + "<++++++++++\n");
                    fpoblacion = false;
                }

                if (fsaturacion && segundaCadena && token.getIdToken() == "Numero")
                {
                    saturacion = token.getLexema();
                    //Console.WriteLine("\n++++++++>" + saturacion + "<++++++++++\n");
                    fsaturacion = false;
                }

                if (fbandera && segundaCadena && token.getIdToken() == "Cadena")
                {
                    bandera = token.getLexema().Replace('"', ' ');
                    //Console.WriteLine("\n++++++++>" + bandera + "<++++++++++\n");
                    fbandera = false;
                }

            }

            StringBuilder grafo;
            grafo = new StringBuilder();
            grafo.Append("digraph G { ");


            grafo.Append("      start [shape=Mdiamond label=\"Grafica\"];");
            foreach (Continente continente in Listacontinentes)
            {

                int saturacioncontinente = 0;

                grafo.Append("      start->" + continente.continente + ";\n");
                foreach(Pais pais in continente.paises)
                {
                    grafo.Append("      "+ continente.continente+"->" + pais.nombre + ";\n");
                    grafo.Append("      " + pais.nombre + "[shape=record label=\"{ "+ pais.nombre +" | "+pais.saturacion + "}\"style=filled fillcolor=\" "+ getColorSaturacion(Int32.Parse(pais.saturacion)) +"  \"];\n");
                    saturacioncontinente += (Int32.Parse(pais.saturacion));
                    //Console.WriteLine("-------->" + continente.continente + "<--------------"+pais.nombre+"<------------\n\n\n");

                }
                try
                {
                    int i = 1;
                    if (continente.paises.Capacity != 0)
                    {
                        Console.WriteLine(i);
                        i = saturacioncontinente / continente.paises.Count;
                        Console.WriteLine(saturacioncontinente + " / " + continente.paises.Count + " = " + i + "----------\n\n");
                    }

                    grafo.Append("      " + continente.continente + "[shape=record label=\"{ " + continente.continente + "| "+ i +"}\"style=filled fillcolor=\" " + getColorSaturacion(i) + "  \"];\n");
                }
                catch (Exception)
                {

                    throw;
                }
                
            }



            grafo.Append("}");

            StreamWriter sw = new StreamWriter("../../dot.txt");
            sw.Write(grafo);
            sw.Close();

            string ruta = "../../dot.txt";
            string ruta2 = "../../Arbol.png";

            string cmd = "dot -Tpng " + ruta + " -o " + ruta2;

            System.Diagnostics.ProcessStartInfo miProceso = new System.Diagnostics.ProcessStartInfo("cmd", "/c " + cmd);
            System.Diagnostics.Process.Start(miProceso);

            FileStream file = new FileStream(ruta2, FileMode.Open);
            Image img = Image.FromStream(file);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.Image = img;
            file.Close();

        }

        public string getColorSaturacion(int saturacion)
        {
            if (saturacion>=0 && saturacion<= 15)
            {
                return "#ffffff";
            }
            if (saturacion >= 16 && saturacion <= 30)
            {
                return "#0080ff";
            }
            if (saturacion >= 31 && saturacion <= 45)
            {
                return "#00ff00";
            }
            if (saturacion >= 46 && saturacion <= 60)
            {
                return "#ffff00";
            }
            if (saturacion >= 61 && saturacion <= 75)
            {
                return "#ff9933";
            }
            if (saturacion >= 76 && saturacion <= 100)
            {
                return "#ff3300";
            }

            return "";
        }

        /*
        public void getMenorPoblacion()
        {
            Boolean primerCadena = false;
            string pob = "";
            int mayor = 0;
            int menor = 0;
            foreach (Tokens token in lista_de_tokens)
            {
                Console.WriteLine("---->" + token.getLexema() + " ----- " + token.getIdToken());
                if (token.getLexema() == "SATURACION")
                {
                    Console.WriteLine("\n\n\n");
                    primerCadena = true;
                }
                if (token.getIdToken() == "Numero" && primerCadena == true)
                {
                    Console.WriteLine("------\n\n\n----");
                    pob = token.getLexema();
                    Listacontinentes.Add(new Continente(pob));
                    primerCadena = false;
                    Console.WriteLine("-------->" + pob + "<---------------------\n\n\n");
                }
            }
        }
        */

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void acercaDeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Henry Gabriel Peralta Martinez \nCarnet: 201712289 \n20 Años \nEstudia ingenieria en sistemas");
        }
    }
}
