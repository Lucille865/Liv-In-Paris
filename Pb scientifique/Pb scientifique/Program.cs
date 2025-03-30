using Pb_scientifique;
using System;
using System.IO;
using static System.Collections.Specialized.BitVector32;

namespace Pb_scientifique
{
    public class Program
    {
        static void Main(string[] args)
        {
            var interfaceApp = new Interface();

            // Afficher le menu initial
            //interfaceApp.AfficherMenu();


            string cheminStations = "MetroParisNoeuds.txt";
            string cheminLiaisons = "MetroParisArcs.txt";

            // Création d'un graphe avec des int comme identifiants de stations
            Graphe<int> graphe = new Graphe<int>();
            //AfficheGraphe<int> image = new AfficheGraphe<int>();
            //image.ChargerDepuisFichier(cheminStations, int.Parse);

            // Chargement des stations depuis le fichier
            graphe.ChargerStationsDepuisFichier(cheminStations);
            Console.WriteLine("Liste des stations chargées :");
            


            // Chargement des liaisons entre les stations depuis le fichier
            graphe.ChargerLiaisonsDepuisFichier(cheminLiaisons);


            // Affichage des informations
            graphe.AfficherLiaisons();


            // Dessiner le graphe
            //image.DessinerGraphe("graphe.png");
            
            var image = new AfficheGraphe<string>();
            graphe.ChargerStationsDepuisFichier(cheminStations);
            //graphe.ChargerLiaisonsDepuisFichier(cheminLiaisons);
            Console.WriteLine($"Nombre de stations chargées : {graphe.Noeuds.Count}");

            // Afficher les informations des stations
            foreach (var noeud in image.Noeuds)
            {
                Console.WriteLine($"Station ID: {noeud.Id}, Latitude: {noeud.Latitude}, Longitude: {noeud.Longitude}");
            }
            image.DessinerGraphe("graphe.png");

            Noeud<int> depart = graphe.Noeuds.ContainsKey(121) ? graphe.Noeuds[121] : null;
            Noeud<int> arrivee = graphe.Noeuds.ContainsKey(124) ? graphe.Noeuds[124] : null; // Trouver le noeud d'arrivée (ID 203)
            var dijkstra = new Dijkstra<int>(graphe, depart.Id);
            var chemin = dijkstra.GetChemin(arrivee.Id);

            /*Console.WriteLine("Vérification des distances calculées par Dijkstra :");
            foreach (var kvp in dijkstra.Distances)
            {
                if (graphe.Noeuds.TryGetValue(kvp.Key, out var station))
                {
                    Console.WriteLine($"Station {station.Nom} ({station.Id}) - Distance: {kvp.Value}");
                }
            }*/

            Console.WriteLine("Chemin le plus court trouvé entre " + depart.Nom + " et " + arrivee.Nom + " :");

            foreach (var stationNom in chemin)
            {
                var stationTrouvee = graphe.Noeuds.Values.FirstOrDefault(s => s.Nom == stationNom);

                if (stationTrouvee != null)
                {
                    Console.WriteLine($"- {stationTrouvee.Nom} (Ligne {stationTrouvee.Ligne})");
                }
                else
                {
                    Console.WriteLine($"Station introuvable : {stationNom}");
                }
            }


        }
    }
}
