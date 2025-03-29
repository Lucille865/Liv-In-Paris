using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pb_scientifique
{
    public class Noeud<T>
    {
        public T Valeur { get; }
        public List<Noeud<T>> Voisins { get; }

        public Noeud(T valeur)
        {
            Valeur = valeur;
            Voisins = new List<Noeud<T>>();
        }

        public void AjouterVoisin(Noeud<T> voisin)
        {
            if (!Voisins.Contains(voisin))
            {
                Voisins.Add(voisin);
                voisin.Voisins.Add(this); // Graphe non orienté
            }
        }

        public string toString()
        {
            return Valeur.ToString();
        }
    }
}
