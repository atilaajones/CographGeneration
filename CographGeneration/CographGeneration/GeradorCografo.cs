using cografos.Estrutura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cografos.GeradorCografos
{
    public static class GeradorCografo
    {
        public static CoArvore CoarvoreInicial(int n)
        {
            CoArvore t = new CoArvore(true);
            Vertice raiz = t.Raiz;
            Partição d = new Partição(n);
            t.AtribuirPartiçãoNosVerticesJáOrdenados();
            t.ConstruirFilhosAPartirDeDistribuição(t.Raiz, d);
            return t;
        }

        public static int ContagemConexos(int n)
        {
            CoArvore t = CoarvoreInicial(n);
            int i = 0;
            do
            {
                i++;
                t.EncontraPivô();
                //Imprimir.CoarvoreEmArquivoDot(t, "coarvore_teste" + i, true);
            } while (t.CoárvoreSeguinteConexa());
            return i;
        }
    }
}
