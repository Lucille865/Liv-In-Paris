using Pb_scientifique;
using System;
using System.IO;

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


            Noeud<int> depart = graphe.Noeuds.FirstOrDefault(n => n.Id.Equals(101));  // Trouver le noeud de départ (ID 101)
            Noeud<int> arrivee = graphe.Noeuds.FirstOrDefault(n => n.Id.Equals(203));  // Trouver le noeud d'arrivée (ID 203)
            /*var dijkstra = new Dijkstra<int>(graphe, depart);
            var chemin = dijkstra.GetChemin(arrivee);

            foreach (var station in chemin)
            {
                Console.WriteLine(station.toString());
            }*/

            
        }
    }
}
