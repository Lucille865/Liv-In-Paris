using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pb_scientifique
{
    public class Lien<T>
    {
        public Noeud<T> Noeud1 { get; }
        public Noeud<T> Noeud2 { get; }
        public int Temps { get; set; }

        public Lien(Noeud<T> n1, Noeud<T> n2, int temps)
        {
            Noeud1 = n1;
            Noeud2 = n2;
            Temps = temps;

        }
        public string toString()
        {
            return "Relation " + this;
        }

    }
}
