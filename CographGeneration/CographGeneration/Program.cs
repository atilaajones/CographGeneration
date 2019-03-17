using cografos.Classes_auxiliares;
using cografos.Estrutura;
using cografos.GeradorCografos;
using System;
using System.IO;

namespace Cograph_Generator
{
    class Program
    {
        private static void Main(string[] args)
        {
            System.IO.Directory.CreateDirectory("c:/retorno_app_cografos/");
            CoArvore t;
            Console.WriteLine("---------- Cograph Generator (format .g6)----------");
            Console.WriteLine("This program is a implementation of algorithm from paper:");
            Console.WriteLine("Cograph generation with linear delay (doi.org/10.1016/j.tcs.2017.12.037)");
            Console.WriteLine("Atila A.Jones, Fábio Protti and Renata R.Del-Vecchio");
            Console.WriteLine("--------------------------");
            Console.WriteLine("Visit goo.gl/9cRzPX to download connected cographs until 19 vertices. If you wanna generate all cographs with n > 19 vertices, use this program");
            Console.WriteLine("");
            Console.WriteLine("--------------------------");
            Console.WriteLine("");
            Console.WriteLine("Choose the number of vertices you want to generate the cographs and press enter");
            int n = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("You choice " + n + " vertices");
            StreamWriter arquivo;
            arquivo = new StreamWriter("c:/retorno_app_cografos/" + "cographConnected_" + n + "_vertices.g6");
            Console.WriteLine("The file .g6 will be create in c:/retorno_app_cografos/cographConnected_" + n + "_vertices.g6");
            Console.WriteLine("What do you want?");
            Console.WriteLine("0: generate only connected cographs");
            Console.WriteLine("1: generate all cographs");
            int escolha = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("");
            t = GeradorCografo.CoarvoreInicial(n);
            int contador = 0;
            if (escolha == 0)
            {
                Console.WriteLine("You chose to generate only connected cographs");
                Console.WriteLine("Generation started. Please wait...");
                using (arquivo)
                {
                    do
                    {
                        arquivo.WriteLine(Representacao.CodigoG6(new Cografo(t)));
                        //pode editar para usar outra Representação além do formato .g6
                        t.EncontraPivô();
                        contador++;
                    } while (t.CoárvoreSeguinteConexa());
                    Console.WriteLine("");
                    Console.WriteLine("");
                    Console.WriteLine("");
                    Console.WriteLine("Generation finished: There are " + contador + " connecteds cographs with " + n + " vertices");
                }
            }
            if (escolha == 1)
            {
                Console.WriteLine("You chose to generate all cographs");
                Console.WriteLine("Generation started. Please wait...");
                using (arquivo)
                {
                    do
                    {
                        arquivo.WriteLine(Representacao.CodigoG6(new Cografo(t)));
                        //pode editar para usar outra Representação além do formato .g6
                        t.EncontraPivô();
                        contador++;
                    } while (t.CoárvoreSeguinte());
                    Console.WriteLine("");
                    Console.WriteLine("");
                    Console.WriteLine("");
                    Console.WriteLine("Generation finished: There are " + contador + " cographs with " + n + " vertices");
                }
            }
            Console.WriteLine("Press any key to close.");
            Console.ReadKey();
        }
    }
}
