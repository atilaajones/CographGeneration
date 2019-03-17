using cografos.Classes_auxiliares;
using cografos.Estrutura;
using System;
using System.Collections.Generic;
using System.Linq;

namespace cografos.Estrutura
{
    public class Cografo
    {
        public List<Vertice> Vertices { get; set; }
        public int NumDeVertices { get; set; }
        public CoArvore CoArvore { get; set; }

        public Cografo(CoArvore coArvore)
        {
            this.CoArvore = coArvore;
            this.Vertices = CoArvore.Folhas;
            this.NumDeVertices = CoArvore.NumDeFolhas;
            this.ConstroiVizinhanca();
        }
                
        private void ConstroiVizinhanca()
        {
            foreach (var v in this.Vertices)
            {
                v.Vizinhanca = new HashSet<Vertice>(); //inicializa vizinhanca
                foreach (var w in this.Vertices)
                {
                    if (!(v.Id.Equals(w.Id)))
                    {
                        if (this.CoArvore.SaoAdjacentes(v, w)) v.Vizinhanca.Add(w);
                    }
                }
            }
        }
            

        public bool SaoAdjacentes(Vertice v, Vertice w)
        {
            return v.Vizinhanca.Contains(w);
        }

        public void AplicarIdParaImpressão()
        {
            int contador = 1;
            foreach (Vertice v in this.Vertices)
            {
                v.IdPrint = contador;
                contador++;
            }
        }

        

    }

}
