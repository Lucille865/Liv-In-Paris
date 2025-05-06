using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;

namespace Pb_scientifique
{
    public class Dijkstra<T>
    {
        private Dictionary<T, double> distances;
        private Dictionary<T, T> predecesseurs;
        private HashSet<T> visites;
        private PriorityQueue<T, double> filePriorite;
        private Graphe<T> graphe;
        public Dictionary<T, double> Distances => distances;

        /// <summary>
        /// Initialise une nouvelle instance de l'algorithme de Dijkstra et exécute le calcul à partir du nœud de départ.
        /// </summary>
        /// <param name="graphe">Le graphe sur lequel exécuter l'algorithme.</param>
        /// <param name="depart">Le nœud de départ.</param>
        public Dijkstra(Graphe<T> graphe, T depart)
        {
            this.graphe = graphe;
            distances = new Dictionary<T, double>();
            predecesseurs = new Dictionary<T, T>();
            visites = new HashSet<T>();
            filePriorite = new PriorityQueue<T, double>();

            // Initialisation : toutes les distances sont infinies sauf le point de départ
            foreach (var noeud in graphe.Noeuds.Values)
            {
                distances[noeud.Id] = double.PositiveInfinity;
            }
            distances[depart] = 0;
            filePriorite.Enqueue(depart, 0);

            while (filePriorite.Count > 0)
            {
                T courant = filePriorite.Dequeue();
                if (visites.Contains(courant)) continue;
                visites.Add(courant);

                var stationCourante = graphe.Noeuds[courant];

                foreach (T voisinId in graphe.ObtenirVoisins(courant))
                {
                    if (!distances.ContainsKey(voisinId))
                    {
                        Console.WriteLine($"⚠️ Ignoré : le voisin {voisinId} de {courant} n'existe pas dans distances !");
                        continue; // Passe au voisin suivant
                    }
                    var voisin = graphe.Noeuds[voisinId];
                    double poids = 1; // Distance normale entre deux stations

                    // Si on change de ligne, on ajoute un coût supplémentaire
                    if (stationCourante.Ligne != voisin.Ligne)
                    {
                        poids += 3; // Pénalité pour changement de ligne
                    }

                    double nouvelleDistance = distances[courant] + poids;

                    if (nouvelleDistance < distances[voisinId])
                    {
                        distances[voisinId] = nouvelleDistance;
                        predecesseurs[voisinId] = courant;
                        filePriorite.Enqueue(voisinId, nouvelleDistance);
                    }
                }
            }
        }

        /// <summary>
        /// Reconstruit le chemin le plus court entre le nœud de départ et un nœud d'arrivée donné.
        /// </summary>
        /// <param name="arrivee">Le nœud de destination.</param>
        /// <returns>Une liste des noms des stations formant le chemin le plus court.</returns>
        public List<string> GetChemin(T arrivee)
        {
            List<string> chemin = new List<string>();

            if (!predecesseurs.ContainsKey(arrivee))
            {
                Console.WriteLine("Aucun chemin trouvé !");
                return chemin;
            }

            T courant = arrivee;
            while (predecesseurs.ContainsKey(courant))
            {
                chemin.Insert(0, graphe.Noeuds[courant].Nom);
                courant = predecesseurs[courant];
            }
            chemin.Insert(0, graphe.Noeuds[courant].Nom);

            return chemin;
        }


    }

}
