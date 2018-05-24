using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using AnalisadorLexico;
using AnalisadorSintatico;

namespace CompiladorCSmall {
    class Compilador {

        public static ModuloIO io;
        public static Lexico analisadorLexico;
        public static Sintatico analisadorSintatico;

        static int Main(string[] args) {
            /*
            if (args.Length == 0) {
                Console.WriteLine("Digite o nome do arquivo de entrada!");
                return 1;
            }*/

            //var path = System.IO.Path.Combine(Environment.CurrentDirectory, args[0]);
            var path = "Teste.c";
            if (System.IO.File.Exists(path)) {
                io = new ModuloIO(path);
                analisadorLexico = new Lexico();
                analisadorLexico.Executa(io);
                io.SalvaTokens(analisadorLexico.ListaTokens);
                foreach (Token tk in analisadorLexico.ListaTokens)
                    Console.WriteLine(tk.ToString());

                if (analisadorLexico.Status) {
                    analisadorSintatico = new Sintatico(analisadorLexico.ListaTokens);
                    analisadorSintatico.Programa();
                    Debuga();
                }
                
                return 0;
            } else {
                Console.WriteLine("Arquivo não existe!");
                return 1;
            }
        }

        static void Debuga() {
            
            try {
                XmlDocument doc = new XmlDocument();
                StringWriter sw = new StringWriter();
                doc.LoadXml(analisadorSintatico.OutputDebug);
                doc.Save(sw);
                using (StreamWriter writer = new StreamWriter("Debug.xml")) {
                    writer.Write(sw.ToString());
                }
            } catch (Exception) {
                Console.WriteLine("Não foi possivel montar o XML");
            }
            
        }
    }
}
