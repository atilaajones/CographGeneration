using cografos.Estrutura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cografos.Classes_auxiliares
{
    public static class Representacao
    {
        public static double[,] MatrizAdjacencia(Cografo g){ 
        //linha i da matriz é referente ao vértice de Id i+1 
        //a matriz é do tipo double pois a biblioteca que calcula o espectro exige este tipo
            double[,] matriz = new double[g.NumDeVertices, g.NumDeVertices];
            for (int p = 0; p < g.NumDeVertices; p++) for (int q = 0; q < g.NumDeVertices; q++) { matriz[p, q] = 0; } //inicializa matriz com zeros
            int i = 0;
            int j = 0;
            foreach (var v in g.Vertices)
            {
                foreach (var w in g.Vertices)
                {
                    if (!(v.Id.Equals(w.Id)))
                    {
                        if (g.SaoAdjacentes(v, w)) //sao adjacentes
                        {
                            matriz[i, j] = 1; //preenche matriz
                        }
                    }
                    j++;
                }
                j = 0;
                i++;
            }
            return matriz;
        }

        public static double[,] Pesos(Cografo g)
        {
            double[,] m = Representacao.MatrizAdjacencia(g);
            for (int i = 0; i < g.NumDeVertices; i++)
                for (int j = i + 1; j < g.NumDeVertices; j++)
                    if (m[i, j] == 0)
                    {
                        m[i, j] = int.MaxValue;
                        m[j, i] = int.MaxValue;
                    }
            return m;
        }

        public static double[,] MatrizDistância(Cografo g)
        {
            int n = g.NumDeVertices;
            double[,] d1 = Representacao.Pesos(g);
            double[,] d2 = d1;
            for (int k = 0; k < n; k++)
            {
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                    {
                        d2[i, j] = Math.Min(d1[i, j], d1[i, k] + d1[k, j]);
                    }
                d1 = d2;
            }
            return d2;
        }

        #region Graph6 format
        public static string CodigoG6(Cografo g)
        {

            double[,] A = Representacao.MatrizAdjacencia(g);
            int n = g.NumDeVertices;
            string bloco6 = ""; //variável auxiliar para tratar cada código binário de 6 dígitos
            string NemASCII = ""; //armazena o número de vértices em ASCII
            string vetorAdjacenciaEmBinario = ""; //armazena código binário da matriz de adjacência
            string vetorAdjacenciaEmASCII = ""; //armazena código binário da matriz de adjacência

            //construção de NemASCII
            string nBinary = DecimalToBinary(n); //converte n para binário
            while (nBinary.Length % 6 != 0) nBinary = "0" + nBinary; //insere 0 à esquerda até obter código binário com tamanho múltiplo de 6
            for (int i = 0; i < nBinary.Length; i = i + 6) //transforma cada bloco de 6 em um ASC II
            {
                bloco6 = nBinary.Substring(i, 6);
                NemASCII = NemASCII + Convert.ToChar((Convert.ToInt32(bloco6, 2) + 63)).ToString();
            }

            //construção do vetorAdjanceciaEmASCII

            //ordem do passeio na matriz ((0,1),(0,2),(1,2),(0,3),(1,3),(2,3),(0,4),(1,4),(2,4),(3,4),(0,5),...,(n-2,n-1).
            int linha = 0;
            int coluna = 1;
            do
            {
                if (n < 3) break;
                vetorAdjacenciaEmBinario = vetorAdjacenciaEmBinario + A[linha, coluna].ToString();
                if (linha == coluna - 1)
                {
                    linha = 0;
                    coluna = coluna + 1;
                } else linha = linha + 1;
            } while (linha < n - 1 && coluna < n);
            
            while (vetorAdjacenciaEmBinario.Length % 6 != 0) vetorAdjacenciaEmBinario = vetorAdjacenciaEmBinario+"0";


            for (int i = 0; i < vetorAdjacenciaEmBinario.Length; i=i+6)
            {
                bloco6 = vetorAdjacenciaEmBinario.Substring(i, 6);
                vetorAdjacenciaEmASCII = vetorAdjacenciaEmASCII + Convert.ToChar((Convert.ToInt32(bloco6, 2) + 63)).ToString(); 
            }

            return NemASCII + vetorAdjacenciaEmASCII;
        }

        private static string DecimalToBinary(int decimalNumber)
        {
            int remainder;
            string result = string.Empty;
            while (decimalNumber > 0)
            {
                remainder = decimalNumber % 2;
                decimalNumber /= 2;
                result = remainder.ToString() + result;
            }
            while (result.Length % 6 != 0)
            {
                result = "0" + result;
            }
            return result;

        }
        #endregion
    }
}
