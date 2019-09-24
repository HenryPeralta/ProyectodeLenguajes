using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto1_SegundoSemestre_Lenguajes
{
    class AnalizadorLexico
    {
        ArrayList tokens;
        ArrayList tipos;

        ArrayList Lista_Operaciones;
        static private List<Tokens> listaTokens;
        private String retorno;
        public int estado_token;

        static private List<PalabrasReservadas> reserva;
        static private List<Cadenas> cad;

        static private List<Errores> listaErrores;
        private String retorno1;

        public AnalizadorLexico()
        {
            listaTokens = new List<Tokens>();
            tokens = new ArrayList();
            tipos = new ArrayList();

            Lista_Operaciones = new ArrayList();

            listaErrores = new List<Errores>();

            reserva = new List<PalabrasReservadas>();
            cad = new List<Cadenas>();
        }

        public void addToken(String lexema, String idToken, int linea, int columna, int indice)
        {
            Tokens nuevo = new Tokens(lexema, idToken, linea, columna, indice);
            listaTokens.Add(nuevo);
        }

        public void addError(String lexema, String idToken, int linea, int columna)
        {
            Errores errtok = new Errores(lexema, idToken, linea, columna);
            listaErrores.Add(errtok);
        }

        public void addReservada(string lexema, string idToken)
        {
            PalabrasReservadas res = new PalabrasReservadas(lexema, idToken);
            reserva.Add(res);
        }

        public void addCadena(string lexema, string idToken)
        {
            Cadenas s = new Cadenas(lexema, idToken);
            cad.Add(s);
        }

        public void Pintar(RichTextBox CajaPintar, int PosicionInicial, int PosicionFinal, String Tipo)
        {

            CajaPintar.SelectionStart = PosicionInicial;
            CajaPintar.SelectionLength = PosicionFinal;
            switch (Tipo)
            {
                case "Palabra Reservada":
                    CajaPintar.SelectionColor = Color.Blue;
                    break;

                case "Cadena":
                    CajaPintar.SelectionColor = Color.Yellow;
                    break;

                case "Numero":
                    CajaPintar.SelectionColor = Color.Green;
                    break;

                case "Llave":
                    CajaPintar.SelectionColor = Color.Red;
                    break;

                case "Punto y Coma":
                    CajaPintar.SelectionColor = Color.Orange;
                    break;

                default://servira para cualquiera que no sea reconocido como lexema o errores a pintar
                    CajaPintar.SelectionColor = Color.Black;
                    break;
            }
        }

        public void Analizador_cadena(String entrada, RichTextBox CajaPintar)
        {
            int estado = 0;
            int columna = 0;
            int fila = 1;
            string lexema = "";
            Char c;
            Char c1;
            entrada = entrada + " ";
            for (int i = 0; i < entrada.Length; i++)
            {
                c = entrada[i];
                columna++;
                switch (estado)
                {
                    case 0:
                        if (Char.IsLetter(c))
                        {
                            estado = 1;
                            lexema += c;
                        }
                        else if (Char.IsDigit(c))
                        {
                            estado = 2;
                            lexema += c;
                        }
                        else if (c == '"')
                        {
                            estado = 3;
                            i--;
                            columna--;
                        }
                        else if (c == '\n')
                        {
                            columna = 0;
                            fila++;
                            estado = 0;
                        }
                        else if (c == '\t')
                        {
                            columna = 0;
                            fila++;
                            estado = 0;
                        }
                        else if (c == '{')
                        {
                            estado = 6;
                            lexema += c;
                        }
                        else if (c == '}')
                        {
                            estado = 7;
                            lexema += c;
                        }
                        else if (c == ' ')
                        {
                            estado = 0;
                        }
                        else if (c == ':')
                        {
                            lexema += c;
                            addToken(lexema, "Dos Puntos", fila, columna, i - lexema.Length);
                            lexema = "";
                        }
                        else if (c == ';')
                        {
                            estado = 8;
                            lexema += c;
                        }
                        else if (c == '%')
                        {
                            lexema += c;
                            addToken(lexema, "Porcentaje", fila, columna, i - lexema.Length);
                            lexema = "";
                        }
                        else
                        {
                            estado = -99;
                            i--;
                            columna--;
                        }
                        break;
                    case 1:
                        if (Char.IsLetterOrDigit(c))
                        {
                            lexema += c;
                            estado = 1;
                        }
                        else if (c == '\n')
                        {
                            columna = 0;
                            fila++;
                            estado = 0;
                        }
                        else
                        {
                            Boolean encontrado = false;
                            encontrado = Macht_enReser(lexema);
                            if (encontrado)
                            {
                                if (lexema == "GRAFICA" || lexema == "NOMBRE" || lexema == "CONTINENTE" || lexema == "PAIS" || lexema == "POBLACION" || lexema == "SATURACION" || lexema == "BANDERA")
                                {
                                    addToken(lexema, "Palabra Reservada", fila, columna-1, i - lexema.Length);
                                    addReservada(lexema, "Palabra Reservada");
                                    Pintar(CajaPintar, i - lexema.Length, lexema.Length, "Palabra Reservada");
                                }
                                else
                                {

                                    lexema += c;
                                    addError(lexema, "Elemento Lexico Desconocido", fila, columna-1);
                                    estado = 0;
                                    lexema = "";

                                }
                            }
                            else
                            {
                                if (lexema == "GRAFICA" || lexema == "NOMBRE" || lexema == "CONTINENTE" || lexema == "PAIS" || lexema == "POBLACION" || lexema == "SATURACION" || lexema == "BANDERA")
                                {

                                    addToken(lexema, "Palabra Reservada", fila, columna-1, i - lexema.Length);
                                    addReservada(lexema, "Palabra Reservada");
                                    Pintar(CajaPintar, i - lexema.Length, lexema.Length, "Palabra Reservada");
                                }
                                else
                                {

                                    lexema += c;
                                    addError(lexema, "Elemento Lexico Desconocido", fila, columna-1);
                                    estado = 0;
                                    lexema = "";

                                }
                            }

                            lexema = "";
                            i--;
                            columna--;
                            estado = 0;
                        }
                        break;
                    case 2:
                        if (Char.IsDigit(c))
                        {
                            lexema += c;
                            estado = 2;
                        }
                        else
                        {
                            //token = new Token(3, "Numero", lexema, fila, columna);
                            //tokens.add(token);
                            addToken(lexema, "Numero", fila, columna-1, i - lexema.Length);
                            Pintar(CajaPintar, i - lexema.Length, lexema.Length, "Numero");
                            lexema = "";
                            i--;
                            columna--;
                            estado = 0;
                        }
                        break;

                    case 3:
                        if (c == '"')
                        {
                            lexema += c;
                            estado = 4;
                        }
                        break;
                    case 4:
                        if (c != '"')
                        {
                            lexema += c;
                            estado = 4;
                        }
                        else
                        {
                            estado = 5;
                            i--;
                            columna--;
                        }
                        break;
                    case 5:
                        if (c == '"')
                        {
                            lexema += c;

                            addToken(lexema, "Cadena", fila, columna-1, i - lexema.Length);
                            addCadena(lexema, "Cadena");
                            Pintar(CajaPintar, i - lexema.Length, lexema.Length, "Cadena");
                            estado = 0;
                            lexema = "";
                        }
                        break;

                    case 6:
                        if (c == '{')
                        {
                            lexema += c;
                        }
                        else
                        {
                            addToken(lexema, "Llave", fila, columna-1, i - lexema.Length);
                            Pintar(CajaPintar, i - lexema.Length, lexema.Length, "Llave");
                            lexema = "";
                            i--;
                            columna--;
                            estado = 0;
                        }
                        break;

                    case 7:
                        if (c == '}')
                        {
                            lexema += c;
                        }
                        else
                        {
                            addToken(lexema, "Llave", fila, columna-1, i - lexema.Length);
                            Pintar(CajaPintar, i - lexema.Length, lexema.Length, "Llave");
                            lexema = "";
                            i--;
                            columna--;
                            estado = 0;
                        }
                        break;

                    case 8:
                        if (c == ';')
                        {
                            lexema += c;
                        }
                        else
                        {
                            addToken(lexema, "Punto y Coma", fila, columna-2, i - lexema.Length);
                            Pintar(CajaPintar, i - lexema.Length, lexema.Length, "Punto y Coma");
                            lexema = "";
                            i--;
                            columna--;
                            estado = 0;
                        }
                        break;

                    case -99:
                        lexema += c;
                        addError(lexema, "Elemento Lexico Desconocido", fila, columna);
                        estado = 0;
                        lexema = "";
                        break;
                }
            }

        }

        public Boolean Macht_enReser(String sente)
        {
            Boolean enco = false;
            for (int i = 0; i < tokens.Count; ++i)
            {
                if (sente.ToString() == tokens[i].ToString())
                {
                    enco = true;
                    estado_token = i;
                    return enco;
                }
                else { enco = false; }

            }
            return enco;
        }

        public void Html_Tokens()
        {
            //MessageBox.Show("Creando archivo html", "entra");

            String Contenido_html;
            Contenido_html = "<html>" +
                "<head>" +
                "<meta charset = " + "'utf-8'" + " />" +
                "<title> Reporte de Tokens</title>" +
                "<meta name = " + "'viewport'" + " content = " + "'initial-scale=1.0; maximum-scale=1.0; width=device-width;'" + ">" +
                "<Style type = " + "'text/css'" + ">" +
                "@import url(https://fonts.googleapis.com/css?family=Roboto:400,500,700,300,100);;" +
                "body{" +
                "background: rgba(204, 204, 204, 1);" +
            "background: -moz-linear-gradient(top, rgba(204, 204, 204, 1) 0%, rgba(255, 255, 255, 1) 100%);" +
            "background: -webkit-gradient(left top, left bottom, color-stop(0%, rgba(204, 204, 204, 1)), color - stop(100%, rgba(255, 255, 255, 1)));" +
            "background: -webkit-linear-gradient(top, rgba(204, 204, 204, 1) 0%, rgba(255, 255, 255, 1) 100%);" +
            "background: -o-linear-gradient(top, rgba(204, 204, 204, 1) 0%, rgba(255, 255, 255, 1) 100%);" +
            "background: -ms-linear-gradient(top, rgba(204, 204, 204, 1) 0%, rgba(255, 255, 255, 1) 100%);" +
            "background: linear-gradient(to bottom, rgba(204, 204, 204, 1) 0%, rgba(255, 255, 255, 1) 100%);" +
            "filter: progid: DXImageTransform.Microsoft.gradient(startColorstr = '#cccccc', endColorstr = '#ffffff', GradientType = 0);" +
                "font-family: " + "'Roboto'" + ", helvetica, arial, sans-serif;" +
                "font-size: 16px;" +
                "font-weight: 400;" +
                "text-rendering: optimizeLegibility;" +
                "}" +

                "div.table-title{" +
            "display: block;" +
            "margin: auto;" +
                "max-width: 600px;" +
            "padding: 5px;" +
            "width: 100 %;" +
            "}" +

".table-title h3{" +
            "color: #fff;" +
   "font-size: 30px;" +
                "font-weight: 400;" +
                "font-style:normal;" +
                "font-family: " + "'Roboto'" + ", helvetica, arial, sans-serif;" +
                "text-shadow: 1px 1px black;" +
                "text-transform:uppercase;" +
                "}" +
/* Table Styles **/

".table-fill{" +
            "background: white;" +
                "border-radius:3px;" +
                "border-color: black;" +
                "border-collapse: collapse;" +
            "height: 320px;" +
            "margin: auto;" +
                "max-width: 600px;" +
            "padding: 5px;" +
            "width: 100 %;" +
                "box-shadow: 30px 30px 30px 30px rgba(1, 0.1, 0.1, 0.1);" +
            "animation: float 5s infinite;" +
            "}" +

            "th{" +
            "color:#D5DDE5;;" +
  "background:#1b1e24;" +
  "border-bottom:4px solid #9ea7af;" +
  "border-right: 1px solid #343a45;" +
  "font-size:23px;" +
                "font-weight: 100;" +
            "padding: 24px;" +
                "text-align:left;" +
                "text-shadow: 0 1px 1px rgba(0, 0, 0, 0.1);" +
                "vertical-align:middle;" +
            "}" +

            "th:last-child {" +
                "border-top - right-radius:3px;" +
                "border-right:none;" +
            "}" +

            "tr{" +
                "border-top: 1px solid #C1C3D1;" +
  "border-bottom -: 1px solid #C1C3D1;" +
  "color:#666B85;" +
  "font-size:16px;" +
                "font-weight:normal;" +
                "text-shadow: 0 1px 1px rgba(256, 256, 256, 0.1);" +
            "}" +

        "tr: hover td{" +
            "background:#4E5066;" +
  "color:#FFFFFF;" +
  "border-top: 1px solid #22262e;" +
"}" +

        "tr: first-child{" +
                "border-top:none;" +
            "}" +

        "tr: last-child{" +
                "border-bottom:none;" +
            "}" +

        "tr: nth-child(odd) td{" +
            "background:#EBEBEB;" +
"}" +

        "tr: nth-child(odd):hover td{" +
            "background:#4E5066;" +
"}" +

        "tr: last-child td: first-child{" +
                "border-bottom-left-radius:3px;" +
            "}" +

        "tr: last-child td: last-child{" +
                "border-bottom-right-radius:3px;" +
            "}" +

            "td{" +
            "background:#FFFFFF;" +
  "padding: 20px;" +
                "text-align:left;" +
                "vertical-align:middle;" +
                "font-weight:300;" +
                "font-size:18px;" +
                "text-shadow: -1px -1px 1px rgba(0, 0, 0, 0.1);" +
                "border-right: 1px solid #C1C3D1;" +
"}" +

        "td: last-child{" +
                "border-right: 0px;" +
            "}" +

            "th.text-left {" +
                "text-align: left;" +
            "}" +

            "th.text-center {" +
                "text-align: center;" +
            "}" +

            "th.text-right {" +
                "text-align: right;" +
            "}" +

            "td.text-left {" +
                "text-align: left;" +
            "}" +

            "td.text-center {" +
                "text-align: center;" +
            "}" +

            "td.text-right {" +
                "text-align: right;" +
            "}" +
".encabezado h3{" +
                "font-family: arial;" +
            "color: #fff;" +
    "position: relative;" +
            "left: 5 %;" +
            "top: 5 %;" +

                "text-shadow: 0 1px 1px black;" +
            "}" +

    "</Style>" +
"</head>" +

"<body>" +

    "<div class=" + "'encabezado'" + ">" +
        "<h3>Universidad de San Carlos de Guatemala</h3>" +
        "<h3>Facultad de Ingenieria</h3>" +
        "<h3>Lenguajes formales</h3>" +

    "</div>" +
    "<div class=" + "'table-title'" + ">" +
        "<h3>Tokens Registrados</h3>" +
    "</div>" +
    "<table class=" + "'table-fill'" + ">" +
        "<thead>" +
            "<tr>" +
                "<th class=" + "'text-left'" + ">#</th>" +
                "<th class=" + "'text-left'" + ">Lexema</th>" +
                "<th class=" + "'text-left'" + ">Fila</th>" +
                "<th class=" + "'text-left'" + ">Columna</th>" +
                "<th class=" + "'text-left'" + ">Token</th>" +

                "</tr>" +

        "</thead>" +

        "<tbody class=" + "'table-hover'" + ">";

            String Cad_tokens = "";
            String tempo_tokens;

            for (int i = 0; i < listaTokens.Count; i++)
            {
                Tokens sen_pos = listaTokens.ElementAt(i);
                //MessageBox.Show("Token: " + sen_pos.getToken() + "\nLexema: " + sen_pos.getlexema() + "\nlinea: " + sen_pos.getLinea() + "\nColumna: " + sen_pos.getColumna(), i.ToString());

                tempo_tokens = "";
                tempo_tokens = "<tr>" +
                "<td><strong>" + (i + 1).ToString() +
                "</strong></td>" +

                "<td>" + sen_pos.getLexema() +
                "</td>" +

                "<td>" + sen_pos.getLinea() +
                "</td>" +

                "<td>" + sen_pos.getColumna() +
                "</td>" +

                "<td>" + sen_pos.getIdToken() +
                "</td>" +

                "</tr>";
                Cad_tokens = Cad_tokens + tempo_tokens;

            }

            Contenido_html = Contenido_html + Cad_tokens +
            "</table>" +
            "</body>" +
            "</html>";

            //MessageBox.Show(Contenido_html, "Contenido_html");

            /*creando archivo html*/
            File.WriteAllText("Reporte de Tokens.html", Contenido_html);
            System.Diagnostics.Process.Start("Reporte de Tokens.html");

            string ruta = "C:\\Prueba_Para_El_Proyecto";
            if (Directory.Exists(ruta) == false)
            {
                Directory.CreateDirectory(ruta);
            }
            File.WriteAllText("C:\\Prueba_Para_El_Proyecto\\Reporte.html", Contenido_html);

        }

        public void Html_Errores()
        {
            String Contenido_html;
            Contenido_html = "<html>" +
                "<head>" +
                "<meta charset = " + "'utf-8'" + " />" +
                "<title> Reporte de Tokens</title>" +
                "<meta name = " + "'viewport'" + " content = " + "'initial-scale=1.0; maximum-scale=1.0; width=device-width;'" + ">" +
                "<Style type = " + "'text/css'" + ">" +
                "@import url(https://fonts.googleapis.com/css?family=Roboto:400,500,700,300,100);;" +
                "body{" +
                "background: rgba(204, 204, 204, 1);" +
            "background: -moz-linear-gradient(top, rgba(204, 204, 204, 1) 0%, rgba(255, 255, 255, 1) 100%);" +
            "background: -webkit-gradient(left top, left bottom, color-stop(0%, rgba(204, 204, 204, 1)), color - stop(100%, rgba(255, 255, 255, 1)));" +
            "background: -webkit-linear-gradient(top, rgba(204, 204, 204, 1) 0%, rgba(255, 255, 255, 1) 100%);" +
            "background: -o-linear-gradient(top, rgba(204, 204, 204, 1) 0%, rgba(255, 255, 255, 1) 100%);" +
            "background: -ms-linear-gradient(top, rgba(204, 204, 204, 1) 0%, rgba(255, 255, 255, 1) 100%);" +
            "background: linear-gradient(to bottom, rgba(204, 204, 204, 1) 0%, rgba(255, 255, 255, 1) 100%);" +
            "filter: progid: DXImageTransform.Microsoft.gradient(startColorstr = '#cccccc', endColorstr = '#ffffff', GradientType = 0);" +
                "font-family: " + "'Roboto'" + ", helvetica, arial, sans-serif;" +
                "font-size: 16px;" +
                "font-weight: 400;" +
                "text-rendering: optimizeLegibility;" +
                "}" +

                "div.table-title{" +
            "display: block;" +
            "margin: auto;" +
                "max-width: 600px;" +
            "padding: 5px;" +
            "width: 100 %;" +
            "}" +

".table-title h3{" +
            "color: #fff;" +
   "font-size: 30px;" +
                "font-weight: 400;" +
                "font-style:normal;" +
                "font-family: " + "'Roboto'" + ", helvetica, arial, sans-serif;" +
                "text-shadow: 1px 1px black;" +
                "text-transform:uppercase;" +
                "}" +
/* Table Styles **/

".table-fill{" +
            "background: white;" +
                "border-radius:3px;" +
                "border-color: black;" +
                "border-collapse: collapse;" +
            "height: 320px;" +
            "margin: auto;" +
                "max-width: 600px;" +
            "padding: 5px;" +
            "width: 100 %;" +
                "box-shadow: 30px 30px 30px 30px rgba(1, 0.1, 0.1, 0.1);" +
            "animation: float 5s infinite;" +
            "}" +

            "th{" +
            "color:#D5DDE5;;" +
  "background:#1b1e24;" +
  "border-bottom:4px solid #9ea7af;" +
  "border-right: 1px solid #343a45;" +
  "font-size:23px;" +
                "font-weight: 100;" +
            "padding: 24px;" +
                "text-align:left;" +
                "text-shadow: 0 1px 1px rgba(0, 0, 0, 0.1);" +
                "vertical-align:middle;" +
            "}" +

            "th:last-child {" +
                "border-top - right-radius:3px;" +
                "border-right:none;" +
            "}" +

            "tr{" +
                "border-top: 1px solid #C1C3D1;" +
  "border-bottom -: 1px solid #C1C3D1;" +
  "color:#666B85;" +
  "font-size:16px;" +
                "font-weight:normal;" +
                "text-shadow: 0 1px 1px rgba(256, 256, 256, 0.1);" +
            "}" +

        "tr: hover td{" +
            "background:#4E5066;" +
  "color:#FFFFFF;" +
  "border-top: 1px solid #22262e;" +
"}" +

        "tr: first-child{" +
                "border-top:none;" +
            "}" +

        "tr: last-child{" +
                "border-bottom:none;" +
            "}" +

        "tr: nth-child(odd) td{" +
            "background:#EBEBEB;" +
"}" +

        "tr: nth-child(odd):hover td{" +
            "background:#4E5066;" +
"}" +

        "tr: last-child td: first-child{" +
                "border-bottom-left-radius:3px;" +
            "}" +

        "tr: last-child td: last-child{" +
                "border-bottom-right-radius:3px;" +
            "}" +

            "td{" +
            "background:#FFFFFF;" +
  "padding: 20px;" +
                "text-align:left;" +
                "vertical-align:middle;" +
                "font-weight:300;" +
                "font-size:18px;" +
                "text-shadow: -1px -1px 1px rgba(0, 0, 0, 0.1);" +
                "border-right: 1px solid #C1C3D1;" +
"}" +

        "td: last-child{" +
                "border-right: 0px;" +
            "}" +

            "th.text-left {" +
                "text-align: left;" +
            "}" +

            "th.text-center {" +
                "text-align: center;" +
            "}" +

            "th.text-right {" +
                "text-align: right;" +
            "}" +

            "td.text-left {" +
                "text-align: left;" +
            "}" +

            "td.text-center {" +
                "text-align: center;" +
            "}" +

            "td.text-right {" +
                "text-align: right;" +
            "}" +
".encabezado h3{" +
                "font-family: arial;" +
            "color: #fff;" +
    "position: relative;" +
            "left: 5 %;" +
            "top: 5 %;" +

                "text-shadow: 0 1px 1px black;" +
            "}" +

    "</Style>" +
"</head>" +

"<body>" +

    "<div class=" + "'encabezado'" + ">" +
        "<h3>Universidad de San Carlos de Guatemala</h3>" +
        "<h3>Facultad de Ingenieria</h3>" +
        "<h3>Lenguajes Formales</h3>" +

    "</div>" +
    "<div class=" + "'table-title'" + ">" +
        "<h3>Tabla de Errores</h3>" +
    "</div>" +
    "<table class=" + "'table-fill'" + ">" +
        "<thead>" +
            "<tr>" +
                "<th class=" + "'text-left'" + ">No</th>" +
                "<th class=" + "'text-left'" + ">Error</th>" +
                "<th class=" + "'text-left'" + ">Descripcion</th>" +
                "<th class=" + "'text-left'" + ">Fila</th>" +
                "<th class=" + "'text-left'" + ">Columna</th>" +

                "</tr>" +

        "</thead>" +

        "<tbody class=" + "'table-hover'" + ">";

            String Cad_tokens = "";
            String tempo_tokens;

            for (int i = 0; i < listaErrores.Count; i++)
            {
                Errores sen_pos = listaErrores.ElementAt(i);

                tempo_tokens = "";
                tempo_tokens = "<tr>" +
                "<td><strong>" + (i + 1).ToString() +
                "</strong></td>" +

                "<td>" + sen_pos.getLexema() +
                "</td>" +

                "<td>" + sen_pos.getIdToken() +
                "</td>" +

                "<td>" + sen_pos.getLinea() +
                "</td>" +

                "<td>" + sen_pos.getColumna() +
                "</td>" +

                "</tr>";
                Cad_tokens = Cad_tokens + tempo_tokens;

            }

            Contenido_html = Contenido_html + Cad_tokens +
            "</table>" +
            "</body>" +
            "</html>";

            File.WriteAllText("Reporte de Errores.html", Contenido_html);
            System.Diagnostics.Process.Start("Reporte de Errores.html");

            string ruta = "C:\\Prueba_Para_El_Proyecto";
            if (Directory.Exists(ruta) == false)
            {
                Directory.CreateDirectory(ruta);
            }
            File.WriteAllText("C:\\Prueba_Para_El_Proyecto\\ReporteErrores.html", Contenido_html);

        }

        public void generarLista()
        {
            for (int i = 0; i < listaTokens.Count; i++)
            {
                Tokens actual = listaTokens.ElementAt(i);
                retorno += "Lexema: " + actual.getLexema() + ", IdToken: " + actual.getIdToken() + ", Linea: " + actual.getLinea() + "" + Environment.NewLine;
            }
        }
        public String getRetorno()
        {
            return this.retorno;
        }

        public List<Tokens> getListaTokens()
        {
            return listaTokens;
        }

        public void generarError()
        {
            for (int i = 0; i < listaErrores.Count; i++)
            {
                Errores actual1 = listaErrores.ElementAt(i);
                retorno1 += "[Lexema:" + actual1.getLexema() + ",IdToken: " + actual1.getIdToken() + ",Linea: " + actual1.getLinea() + "]" + Environment.NewLine;
            }
        }

        public String getRetorno1()
        {
            return this.retorno1;
        }

        public List<Errores> GetErrores()
        {
            return listaErrores;
        }
    }
}
