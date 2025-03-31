using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pb_scientifique
{
    public class FloydWarshall<T>
    {
        private readonly Graphe<T> graphe;
        private Dictionary<T, Dictionary<T, double>> distances;
        private Dictionary<T, Dictionary<T, T>> predecesseurs;

        public FloydWarshall(Graphe<T> graphe)
        {
            this.graphe = graphe;
            distances = new Dictionary<T, Dictionary<T, double>>();
            predecesseurs = new Dictionary<T, Dictionary<T, T>>();
        }

        public void CalculerPlusCourtsChemins()
        {
            // Initialisation des distances et des prédécesseurs
            foreach (var i in graphe.Noeuds.Keys)
            {
                distances[i] = new Dictionary<T, double>();
                predecesseurs[i] = new Dictionary<T, T>();

                foreach (var j in graphe.Noeuds.Keys)
                {
                    if (EqualityComparer<T>.Default.Equals(i, j))
                    {
                        distances[i][j] = 0; // Distance à soi-même = 0
                    }
                    else if (graphe.ObtenirVoisins(i).Contains(j))
                    {
                        distances[i][j] = 1; // Distance entre voisins = 1
                        predecesseurs[i][j] = i; // Le prédécesseur de j est i
                    }
                    else
                    {
                        distances[i][j] = double.PositiveInfinity; // Pas de connexion
                        predecesseurs[i][j] = default;
                    }
                }
            }

            // Algorithme de Floyd-Warshall
            foreach (var k in graphe.Noeuds.Keys)
            {
                foreach (var i in graphe.Noeuds.Keys)
                {
                    foreach (var j in graphe.Noeuds.Keys)
                    {
                        if (distances[i][k] + distances[k][j] < distances[i][j])
                        {
                            distances[i][j] = distances[i][k] + distances[k][j];
                            predecesseurs[i][j] = predecesseurs[k][j];
                        }
                    }
                }
            }
        }

        public List<T> GetChemin(T depart, T arrivee)
        {
            List<T> chemin = new();
            if (predecesseurs[depart][arrivee] == null)
            {
                return chemin; // Pas de chemin possible
            }

            T courant = arrivee;
            while (!EqualityComparer<T>.Default.Equals(courant, depart))
            {
                chemin.Add(courant);
                courant = predecesseurs[depart][courant];
            }
            chemin.Add(depart);
            chemin.Reverse();
            return chemin;
        }
    }

}
