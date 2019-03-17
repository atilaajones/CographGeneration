using System;
using System.Collections.Generic;
using cografos.GeradorCografos;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace cografos.Estrutura
{
    public class Vertice
    {
        public Vertice Pai { get; set; }
        public List<Vertice> Filho { get; set; }
        public int Tipo { get; set; }
        public Guid Id { get; set; } //Id na coárvore
        public int IdPrint { get; set; }
        public HashSet<Vertice> Vizinhanca { get; set; }
        public int NivelNaArvore { get; set; }
        public int NumCromatico { get; set; }   //numero cromático do subgrafo induzido
        public int FolhasInduzida { get; set; }
        public Partição PartiçãoInduzida { get; set; }

        public int Grau { get; set; }
        public Vertice()
        {
            Filho = new List<Vertice>();
            Pai = null;
            Vizinhanca = null;
            Id = Guid.NewGuid();
            //for (int i = 0; i < 5; i++) this.filho.Add(null);
        }

        public Vertice(int folhasInduzidas)
        {
            Pai = null;
            Id = Guid.NewGuid();
            PartiçãoInduzida = new Partição(folhasInduzidas);
        }

        public Boolean EhFolha() //verifica se o vértice é uma folha
        {
            if (this.Tipo == 2) return true;
            else return false;
        }

        public Vertice FilhoTipo(int tipo)
        {
            foreach (var v in this.Filho)
            {
                if (v.Tipo == tipo) return v;
            }
            return null;
        }

        public int TemQuantosFilhosTipo(int tipo)
        {
            int contador = 0;
            foreach (var v in this.Filho)
            {
                if (v.Tipo == tipo) contador++;
            }
            return contador;
        }

        public bool EhAdjacente(Vertice v)
        {
            return this.Vizinhanca.Contains(v);
        }

        public void SetTipo(int tipo)
        {
            this.Tipo = tipo;
        }

        #region Distribuição Induzida

        public bool EstáEsgotado()
        {
            if (this.EhFolha())
                return true;
            else
                return this.PartiçãoInduzida.MaiorElemento();
        }


        public void SetDistribuição(Partição distribuição)
        {
            this.PartiçãoInduzida = distribuição;
        }
        #endregion


    }
}
