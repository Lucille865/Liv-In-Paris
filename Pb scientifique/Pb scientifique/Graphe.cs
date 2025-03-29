using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pb_scientifique
{
    public class Graphe<T>
{
    private Dictionary<T, List<T>> listeAdjacence;

    public Graphe()
    {
        listeAdjacence = new Dictionary<T, List<T>>();
    }

    public void AjouterLien(T val1, T val2)
    {
        if (!listeAdjacence.ContainsKey(val1))
            listeAdjacence[val1] = new List<T>();
        if (!listeAdjacence.ContainsKey(val2))
            listeAdjacence[val2] = new List<T>();

        listeAdjacence[val1].Add(val2);
        listeAdjacence[val2].Add(val1);
    }

    public void AfficherListeAdjacence()
    {
        foreach (var noeud in listeAdjacence)
        {
            Console.Write(noeud.Key + " -> ");
            Console.WriteLine(string.Join(", ", noeud.Value));
        }
    }

    private void ExplorerEnProfondeur(T noeud, HashSet<T> visites, List<T> ordreVisite)
    {
        visites.Add(noeud);
        ordreVisite.Add(noeud);

        foreach (var voisin in listeAdjacence[noeud])
        {
            if (!visites.Contains(voisin))
                ExplorerEnProfondeur(voisin, visites, ordreVisite);
        }
    }

    public bool EstConnexe()
    {
        if (listeAdjacence.Count == 0) return false;

        HashSet<T> visites = new HashSet<T>();
        T premierNoeud = listeAdjacence.Keys.First();

        ExplorerEnProfondeur(premierNoeud, visites, new List<T>());

        return visites.Count == listeAdjacence.Count;
    }

    public bool ContientCycle()
    {
        HashSet<T> visites = new HashSet<T>();
        foreach (var noeud in listeAdjacence.Keys)
        {
            if (!visites.Contains(noeud))
            {
                if (ExplorerCycle(noeud, default(T), visites)) return true;
            }
        }
        return false;
    }

    private bool ExplorerCycle(T noeud, T parent, HashSet<T> visites)
    {
        visites.Add(noeud);
        foreach (var voisin in listeAdjacence[noeud])
        {
            if (!visites.Contains(voisin))
            {
                if (ExplorerCycle(voisin, noeud, visites)) return true;
            }
            else if (!voisin.Equals(parent))
            {
                return true;
            }
        }
        return false;
    }
}
}
