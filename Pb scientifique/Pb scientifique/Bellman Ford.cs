using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pb_scientifique
{
    public class BellmanFord<T>
    {
        private readonly Graphe<T> graphe;
        private Dictionary<T, double> distances;
        private Dictionary<T, T> predecesseurs;

        public BellmanFord(Graphe<T> graphe)
        {
            this.graphe = graphe;
            distances = new Dictionary<T, double>();
            predecesseurs = new Dictionary<T, T>();
        }

        public bool CalculerPlusCourtChemin(T depart)
        {
            // Initialisation : toutes les distances à l'infini sauf le départ
            foreach (var noeud in graphe.Noeuds.Keys)
            {
                distances[noeud] = double.PositiveInfinity;
                predecesseurs[noeud] = default;
            }
            distances[depart] = 0;

            // Relâchement des arêtes (nombre de stations - 1 fois)
            for (int i = 0; i < graphe.Noeuds.Count - 1; i++)
            {
                foreach (var (id, station) in graphe.Noeuds)
                {
                    foreach (var voisinId in graphe.ObtenirVoisins(id))
                    {
                        double poids = 1; // Poids par défaut (modifie si nécessaire)
                        if (distances[id] + poids < distances[voisinId])
                        {
                            distances[voisinId] = distances[id] + poids;
                            predecesseurs[voisinId] = id;
                        }
                    }
                }
            }

            // Détection des cycles de poids négatif (si applicable)
            foreach (var (id, station) in graphe.Noeuds)
            {
                foreach (var voisinId in graphe.ObtenirVoisins(id))
                {
                    double poids = 1;
                    if (distances[id] + poids < distances[voisinId])
                    {
                        Console.WriteLine("⚠️ Cycle de poids négatif détecté !");
                        return false;
                    }
                }
            }

            return true;
        }

        public List<T> GetChemin(T arrivee)
        {
            List<T> chemin = new();
            T courant = arrivee;

            while (!EqualityComparer<T>.Default.Equals(courant, default) && predecesseurs.ContainsKey(courant))
            {
                chemin.Add(courant);
                courant = predecesseurs[courant];
            }

            chemin.Reverse();
            return chemin;
        }

        public Dictionary<T, double> Distances => distances;
    }

}
