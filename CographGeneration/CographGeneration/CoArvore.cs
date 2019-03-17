using cografos.Classes_auxiliares;
using cografos.GeradorCografos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace cografos.Estrutura
{
    public class CoArvore
    {

        public Vertice Raiz { get; private set; }
        public List<Vertice> Vertices { get; private set; }
        public List<Vertice> Folhas { get; private set; }
        public int NumDeVertices => this.Vertices.Count;
        public int NumDeFolhas => this.Folhas.Count;
        public List<Vertice> VerticesQueFormam2Dou3P3 { get; private set; }
        public int Nivel { get; private set; }
        public Vertice Pivô { get; private set; }
        public List<int> SeqDeGraus { get; set; }

        #region Sets
        public void SetRaiz(Vertice novaRaiz)
        {
            this.Raiz = novaRaiz;
        }

        public void SetNivelNaArvore(int nivel)
        {
            this.Nivel = nivel;
        }

        public void SetNivelNoVertice(Vertice v)
        {
            v.NivelNaArvore = v.Pai.NivelNaArvore + 1;
            if (this.Nivel < v.NivelNaArvore) this.Nivel = v.NivelNaArvore;
        }

        public void InsereVerticeNaArvore(Vertice v)
        {
            this.Vertices.Add(v);
            if (v.Tipo == 2) this.Folhas.Add(v);
        }
        #endregion
        public CoArvore(bool ComRaiz)
        {
            SeqDeGraus = null;
            Pivô = null;
            Vertices = new List<Vertice>();
            Folhas = new List<Vertice>();
            new List<Vertice>();
            if (ComRaiz)
            {
                //cria a raiz da árvore
                this.Raiz = new Vertice(); //seta o v como raiz de CoArvore
                this.Raiz.Pai = null;
                this.Raiz.Tipo = 1; //a raiz é do tipo join
                Vertices.Add(this.Raiz); //insere no conjunto de vértices
                //this.Raiz.IdArvore = 1;
                this.Raiz.NivelNaArvore = 1;
                this.Nivel = 1;
                //this.ContadorDeId = 1;
            }
            else return;
        }

        #region Inserção e edição de Vértices
        public Vertice InsereFilhoEm(Vertice pai, int tipo)
        {
            Vertice v = new Vertice();
            v.Pai = pai; // seta o pai
            pai.Filho.Add(v);
            v.Tipo = tipo; //atribui o tipo
            if (tipo == 2)
            {
                Folhas.Add(v); //se tipo folha insere no conjunto de folhas
                //v.Id = ContadorDeIdFolha;
            }
            this.Vertices.Add(v); //insere no conjunto de vértices
            //v.IdArvore = ContadorDeId; //incrementa o id e atribui no id de v
            v.NivelNaArvore = pai.NivelNaArvore + 1;
            if (this.Nivel < v.NivelNaArvore) this.Nivel = v.NivelNaArvore;
            return v;
        }

        private Vertice InsereFilhoEm(Vertice pai, int tipo, Partição distribuição)
        {
            Vertice w = InsereFilhoEm(pai, tipo);
            w.SetDistribuição(distribuição);
            return w;
        }

        public void AtribuirPartiçãoNosVerticesJáOrdenados()
        {
            this.ContagemDeFolhasNaArvore();
            List<int> lista = new List<int>(this.Vertices.Count);
            foreach (Vertice v in this.Vertices)
            {
                if (!v.EhFolha())
                {
                    foreach (Vertice w in v.Filho)
                    {
                        lista.Add(w.FolhasInduzida);
                    }
                    v.SetDistribuição(new Partição(lista));
                    lista = new List<int>();
                }
                else v.SetDistribuição(null);
            }
        }

        public void ConverteParaFolha(Vertice v)
        {
            v.Tipo = 2;
            this.Folhas.Add(v);
            //ResetaId();
        }
        #endregion


        #region Métodos envolvendo adjacências e parentesco
        public void RemoveAdjacenciaComPai(Vertice v)
        {
            if (this.Raiz != v)
            {
                Vertice antigoPai = v.Pai;
                v.Pai = null;
                antigoPai.Filho.Remove(v);
            }
        }

        public void RemoveParentesco(Vertice origem, Vertice destino)
        {
            Vertice aux = origem;
            Vertice aux2;
            do
            {
                aux2 = aux.Pai;
                this.RemoveAdjacenciaComPai(aux);
                aux = aux2;
            } while (aux.Id.CompareTo(destino.Id) != 0);
        }

        public void InsereParentesco(Vertice novoFilho, Vertice novoPai)
        {
            novoPai.Filho.Add(novoFilho);
            novoFilho.Pai = novoPai;
        }

        public Vertice AncestralComum(Vertice v, Vertice w)
        {
            if (v.Id.Equals(w.Id)) return null; //trata-se do mesmo vértice.
            Vertice ancestralv = v;
            Vertice ancestralw = w;
            Boolean encontrado = false;
            Vertice verticeRetorno = null;
            while (!encontrado) //em agluma iteração teremos encontrado=true, no pior dos casos será na raiz;
            {
                while ((!encontrado) && (ancestralv != null))
                {
                    if (ancestralv.Id.Equals(ancestralw.Id))
                    {
                        encontrado = true;
                        verticeRetorno = ancestralv;
                    }
                    else ancestralv = ancestralv.Pai; //sobe na arvore pelos pais de v
                }
                if (!encontrado)
                {
                    ancestralw = ancestralw.Pai; //nenhum ancestral em comum, sobe um nível com o pai de w...
                    ancestralv = v;  //...reseta o caminho em v
                }
            }
            return verticeRetorno;
        }

        public Boolean SaoAdjacentes(Vertice v, Vertice w)
        {
            if (AncestralComum(v, w).Tipo == 1) return true;
            else return false;
        }

        #endregion

        #region Pega e conta vértices
        public List<Vertice> FolhasDoNivel(int nivel)
        {
            return Folhas.Where(p => p.NivelNaArvore == nivel).ToList();
        }

        public List<Vertice> VerticesDoNivel(int nivel)
        {
            return Vertices.Where(p => p.NivelNaArvore == nivel).ToList();
        }

        public Vertice getVertice(Guid id)
        { //retorna um vértice pelo seu Id
            foreach (Vertice v in Vertices)
            {
                if (v.Id == id) return v;
            }
            return null;
        }

        public void ContagemDeFolhasNaArvore()
        {
            this.Raiz.FolhasInduzida = this.ContagemDeFolhas(this.Raiz);
        }
        private int ContagemDeFolhas(Vertice v)
        {
            if (v.Tipo != 2)
            {
                v.FolhasInduzida = v.Filho.Sum(p => this.ContagemDeFolhas(p));
            }
            else v.FolhasInduzida = 1;
            return v.FolhasInduzida;
        }
        #endregion

        #region Geração Cografos
        private Vertice EncontraPivô(Vertice w)
        {

            if (!w.EhFolha())
            {
                int numDeFilhos = w.Filho.Count();
                for (int i = numDeFilhos - 1; i >= 0; i--)
                {
                    Vertice v = EncontraPivô(w.Filho[i]);
                    if (v != null) return v;
                }
                if (!w.EstáEsgotado()) return w;
                else return null;
            }
            else return null;
        }

        public Vertice EncontraPivô()
        {
            this.Pivô = EncontraPivô(this.Raiz);
            return this.Pivô;
        }

        public void ConstruirFilhosAPartirDeDistribuição(Vertice v, Partição parti)
        {
            //assumindo que os vértices já possuem a respectivas partições associadas 
            this.ApagarSubárvoreInduzidaPor(v);
            v.SetDistribuição(parti);
            for (int i = 0; i < parti.s.Count(); i++)
            {
                if (parti.s[i] > 1)
                {
                    Vertice w = this.InsereFilhoEm(v, 1 - v.Tipo);
                    for (int j = 0; j < parti.s[i]; j++) this.InsereFilhoEm(w, 2);
                    w.SetDistribuição(new Partição(parti.s[i]));
                }
                else this.InsereFilhoEm(v, 2);
            }
            ContagemDeFolhas(v);
            if (v.FolhasInduzida != v.PartiçãoInduzida.n)
            {
                throw new ArgumentException("erro na distribuição induzida er01");
            }
            //this.ResetaId();
        }

        public bool CoárvoreSeguinteConexa()
        {
            this.AtribuirPartiçãoNosVerticesJáOrdenados();
            Vertice v = this.EncontraPivô();
            if (v == null) return false;
            List<Vertice> irmãosMaiores = this.IrmãosMaioresOuIguaisA(v);
            ConstruirFilhosAPartirDeDistribuição(v, v.PartiçãoInduzida.GeraSeguinte());
           // v.PartiçãoInduzida = v.PartiçãoInduzida.GeraSeguinte();
            //irmaos do pivô
            for (int i = 0; i < irmãosMaiores.Count; i++)
            {
                if (irmãosMaiores[i].FolhasInduzida == v.FolhasInduzida)
                    ConstruirFilhosAPartirDeDistribuição(irmãosMaiores[i], v.PartiçãoInduzida);
                else
                    ConstruirFilhosAPartirDeDistribuição(irmãosMaiores[i], new Partição(irmãosMaiores[i].FolhasInduzida));
            }
            // níveis acima do pivo
            Vertice antecessor = v;
            while (antecessor != this.Raiz)
            {
                antecessor = antecessor.Pai;
                irmãosMaiores = this.IrmãosMaioresOuIguaisA(antecessor);
                for (int i = 1; i < irmãosMaiores.Count; i++) //inicio do irmão imediatamente após 'sucessor'
                {
                    if (irmãosMaiores[i].FolhasInduzida == antecessor.FolhasInduzida)
                        Backup.AlocarEstruturaDeEm(antecessor, irmãosMaiores[i], this);
                    else
                        ConstruirFilhosAPartirDeDistribuição(irmãosMaiores[i], new Partição(irmãosMaiores[i].FolhasInduzida));
                }
            }
            return true; 
        }



        public List<Vertice> IrmãosMaioresOuIguaisA(Vertice v)
        {
            if (v.Pai == null) return new List<Vertice>() { v };
            List<Vertice> IrmandadeMaior = new List<Vertice>(v.Pai.Filho.Count);
            int inicial = v.Pai.Filho.IndexOf(v);
            int final = v.Pai.Filho.Count() - 1;
            return v.Pai.Filho.GetRange(inicial, final - inicial + 1);
        }

        public void InverterLabels_10()
        {
            foreach (Vertice v in this.Vertices)
            {
                if (!v.EhFolha()) v.Tipo = 1 - v.Tipo;
            }
        }

        public bool CoárvoreSeguinte()
        {
            if (this.Raiz.Tipo == 1)
            {
                this.InverterLabels_10();
                return true;
            }
            else
            {
                this.InverterLabels_10();
                if (this.CoárvoreSeguinteConexa())
                    return true;
                else return false;
            }
        }

        public void ApagarSubárvoreInduzidaPor(Vertice v)
        {
            //ATENÇÃO: o vértice v é mantido
            if (!v.EhFolha())
            {
                for (int i = 0; i < v.Filho.Count; i++)
                {
                    ApagarSubárvoreInduzidaPor(v.Filho[i]);
                    Vertices.Remove(v.Filho[i]);
                    v.Filho[i] = null;
                }
                v.Filho.Clear();
            }
            else
            {
                Vertices.Remove(v);
                Folhas.Remove(v);
                v = null;
            }
            // this.ResetaId();
        }


        #endregion
        public void AplicarIdParaImpressão()
        {
            int contador = 2;
            this.Raiz.IdPrint = 1;
            foreach (Vertice v in this.Vertices)
            {
                if (v != this.Raiz) v.IdPrint = contador;
                contador++;
            }
        }
    }
}