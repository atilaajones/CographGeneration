using cografos.Estrutura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cografos.Classes_auxiliares
{
    public static class Backup
    {
        public static void AlocarEstruturaDeEm(Vertice origem, Vertice destino, CoArvore arvore)
        {
            arvore.ApagarSubárvoreInduzidaPor(destino);
            DuplicarEstruturaApartirDe(origem, destino, arvore);
            if (origem.PartiçãoInduzida != null) arvore.AtribuirPartiçãoNosVerticesJáOrdenados();
            else if (origem.FolhasInduzida != 0) arvore.ContagemDeFolhasNaArvore();
        }

        public static void DuplicarEstruturaApartirDe(Vertice original, Vertice copia, CoArvore arvore)
        {
            //Vertice novaRaiz = arvore.InsereFilhoEm(raiz.Pai, raiz.Tipo);
            if (original.Tipo != 2)
            {
                foreach (Vertice v in original.Filho)
                {
                    DuplicarEstruturaApartirDe(v, arvore.InsereFilhoEm(copia, v.Tipo), arvore);
                }
            }
        }

    }
}
