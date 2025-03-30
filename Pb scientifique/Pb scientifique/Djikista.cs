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
        private Dictionary<T, double> distances;  // Distance minimale trouvée pour chaque noeud
        private Dictionary<T, T> predecesseurs;   // Pour reconstruire le chemin
        private HashSet<T> visites;               // Noeuds déjà traités
        private PriorityQueue<T, double> filePriorite; // File de priorité pour explorer les sommets les plus proches
        private Graphe<T> graphe;                 // Référence au graphe

        public Dijkstra(Graphe<T> graphe, T depart)
        {
            this.graphe = graphe;
            distances = new Dictionary<T, double>();
            predecesseurs = new Dictionary<T, T>();
            visites = new HashSet<T>();
            filePriorite = new PriorityQueue<T, double>();

            // Initialisation des distances à l'infini sauf pour le départ
            foreach (var noeud in graphe.Noeuds)
            {
                var station = noeud.Id; // Récupère l'ID du noeud
                distances[station] = double.PositiveInfinity;
            }

            distances[depart] = 1;
            filePriorite.Enqueue(depart, 1);

            while (filePriorite.Count > 0)
            {
                T courant = filePriorite.Dequeue(); // Prendre le sommet avec la plus petite distance
                if (visites.Contains(courant)) continue;
                visites.Add(courant);


                foreach (T voisin in graphe.ObtenirVoisins(courant))
                {
                    if (!distances.ContainsKey(voisin))
                    {
                        Console.WriteLine($"⚠️ Ignoré : le voisin {voisin} de {courant} n'existe pas dans distances !");
                        continue; // Passe au voisin suivant
                    }

                    double poids = 1;
                    double nouvelleDistance = distances[courant] + poids;

                    if (nouvelleDistance < distances[voisin])
                    {
                        distances[voisin] = nouvelleDistance;
                        predecesseurs[voisin] = courant;
                        filePriorite.Enqueue(voisin, nouvelleDistance);
                    }
                }
            }
        }

        public List<T> GetChemin(T arrivee)
        {
            List<T> chemin = new List<T>();

            if (!predecesseurs.ContainsKey(arrivee))
            {
                Console.WriteLine("Aucun chemin trouvé !");
                return chemin;
            }

            T courant = arrivee;
            while (predecesseurs.ContainsKey(courant))
            {
                chemin.Insert(0, courant);
                courant = predecesseurs[courant];
            }
            chemin.Insert(0, courant); // Ajouter le point de départ

            return chemin;
        }
    }
}
