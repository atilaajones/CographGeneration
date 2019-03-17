using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace cografos.GeradorCografos
{
    public class Partição
    {

        public List<int> s { get; private set; }
        public int n { get; private set; }
        public Partição(int n)
        {
            if (n < 2) throw new ArgumentException("distribuição do 1: não existe err02");
            s = new List<int>();
            this.n = n;
            for (int i = 0; i < n; i++) s.Add(1); //inicializa
        }

        public Partição(List<int> lista)
        {
            s = lista;
            n = lista.Sum();
        }

        private void _Inicializa()
        {
            for (int i = 0; i < n; i++) s.Add(1);
        }

        public Partição GeraSeguinte()
        {
            if ((s[0] != s.Sum() / 2))
            {
                int k = s.Count() - 1;
                if (s[k] - s[k - 1] <= 1)
                {
                    s[k - 1] = s[k - 1] + s[k];
                    s.RemoveAt(k);//desconsiderando
                }
                else
                {
                    s[k - 1]++;
                    s[k]--;
                    int q = s[k] / s[k - 1];
                    int r = s[k] % s[k - 1];
                    if (q > 1)
                    {
                        s[k] = s[k - 1];
                        for (int i = k + 1; i <= k + q - 2; i++) s.Add(s[k - 1]);
                        s.Add(s[k - 1] + r);
                    }
                }
            }
            else
            {
                if (s.Sum() != 3) s.Clear();
                else
                {
                    if (s[1] == 2) s.Clear();
                    else
                    {
                        s[1] = 2;
                        s.RemoveAt(2);
                    };
                };
            };
            return this;
        }

        public bool MaiorElemento()
        {
            if (this.s[0] == n / 2)
            {
                if (n != 3) return true;
                else return (s[1] == 2);
            }
            else return false;
        }

        public bool EstáVazia()
        {
            return this.s.Any();
        }

    }
}
