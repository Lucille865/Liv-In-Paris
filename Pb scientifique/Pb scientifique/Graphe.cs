using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Pb_scientifique
{
    public class Graphe<T>
    {
        /// <summary>
        /// Dictionnaire des stations, indexées par leur identifiant.
        /// </summary>
        public Dictionary<T, Noeud<T>> Noeuds { get; } = new();

        /// <summary>
        /// Dictionnaire des liaisons entre stations (voisins).
        /// </summary>
        private Dictionary<T, List<T>> liaisons = new();


        /// <summary>
        /// Ajoute une station au graphe.
        /// </summary>
        /// <param name="id">Identifiant de la station.</param>
        /// <param name="ligne">Nom de la ligne.</param>
        /// <param name="nom">Nom de la station.</param>
        /// <param name="longitude">Coordonnée longitude.</param>
        /// <param name="latitude">Coordonnée latitude.</param> 
        public void AjouterStation(T id, string ligne, string nom, double longitude, double latitude)
        {
            if (!Noeuds.ContainsKey(id))
            {
                Noeuds[id] = new Noeud<T>(id, ligne, nom, longitude, latitude);
                liaisons[id] = new List<T>(); // Initialiser la liste des voisins
            }
        }

        /// <summary>
        /// Ajoute une liaison (connexion) entre deux stations.
        /// </summary>
        /// <param name="id1">ID de la première station.</param>
        /// <param name="id2">ID de la deuxième station.</param>
        public void AjouterLiaison(T id1, T id2, int temps)
        {
            if (!Noeuds.ContainsKey(id1) || !Noeuds.ContainsKey(id2))
                return; // Vérification pour éviter des erreurs

            var lien = new Lien<T>(Noeuds[id1], Noeuds[id2], temps);

            if (!liaisons[id1].Contains(id2))
                liaisons[id1].Add(id2);

            if (!liaisons[id2].Contains(id1))
                liaisons[id2].Add(id1);
        }

        /// <summary>
        /// Charge les stations depuis un fichier CSV.
        /// </summary>
        /// <param name="chemin">Chemin du fichier CSV des stations.</param>
        public void ChargerStationsDepuisFichier(string cheminStations)
        {
            if (!File.Exists(cheminStations))
            {
                Console.WriteLine("Fichier de stations introuvable !");
                return;
            }

            foreach (string ligne in File.ReadLines(cheminStations).Skip(1))
            {
                var elements = ligne.Split(';');
                if (elements.Length >= 5 && int.TryParse(elements[0], out int idStation) &&
                    double.TryParse(elements[3], NumberStyles.Any, CultureInfo.InvariantCulture, out double longitude) &&
                    double.TryParse(elements[4], NumberStyles.Any, CultureInfo.InvariantCulture, out double latitude))
                {
                    AjouterStation((T)(object)idStation, elements[1], elements[2], longitude, latitude);
                }
            }
        }

        /// <summary>
        /// Charge les liaisons entre stations depuis un fichier CSV.
        /// </summary>
        /// <param name="chemin">Chemin du fichier CSV des liaisons.</param>
        public void ChargerLiaisonsDepuisFichier(string cheminFichier)
        {
            if (!File.Exists(cheminFichier))
            {
                Console.WriteLine("Fichier de liaisons introuvable !");
                return;
            }

            foreach (string ligne in File.ReadLines(cheminFichier).Skip(1))
            {
                var elements = ligne.Split(';');
                if (elements.Length >= 5 && int.TryParse(elements[0], out int idStation) &&
                    int.TryParse(elements[3], out int voisinId) &&
                    int.TryParse(elements[4], out int temps))
                {
                    AjouterLiaison((T)(object)idStation, (T)(object)voisinId, temps);
                }
            }
        }

        /// <summary>
        /// Affiche les informations de toutes les stations.
        /// </summary>
        public void AfficherStations()
        {
            foreach (var station in Noeuds.Values)
            {
                Console.WriteLine($"ID: {station.Id}, Ligne: {station.Ligne}, Nom: {station.Nom}, Coords: ({station.Longitude}, {station.Latitude})");
            }
        }

        /// <summary>
        /// Affiche les connexions entre les stations.
        /// </summary>
        public void AfficherLiaisons()
        {
            foreach (var liaison in liaisons)
            {
                Console.WriteLine($"Station {liaison.Key} ({Noeuds[liaison.Key].Nom}) connectée à : {string.Join(", ", liaison.Value.Select(id => Noeuds[id].Nom))}");
            }
        }

        /// <summary>
        /// Retourne la liste des voisins (stations connectées) d'une station.
        /// </summary>
        /// <param name="id">Identifiant de la station.</param>
        /// <returns>Liste des identifiants des voisins.</returns>
        public List<T> ObtenirVoisins(T id)
        {
            if (liaisons.ContainsKey(id))
            {
                return liaisons[id];
            }
            return new List<T>(); // Retourne une liste vide si aucun voisin trouvé
        }

        /// <summary>
        /// Recherche l'identifiant d'une station à partir de son nom.
        /// </summary>
        /// <param name="nom">Nom de la station.</param>
        /// <returns>Identifiant de la station, ou -1 si introuvable.</returns>
        public T TrouverIdParNom(string nomStation)
        {
            foreach (var noeud in Noeuds)
            {
                if (noeud.Value.Nom.Equals(nomStation, StringComparison.OrdinalIgnoreCase))
                {
                    return noeud.Key; // Retourne l'ID correspondant
                }
            }
            throw new Exception($"Station '{nomStation}' non trouvée.");
        }
    }
    
}