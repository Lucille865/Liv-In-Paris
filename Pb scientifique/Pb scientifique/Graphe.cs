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
        public Dictionary<T, Noeud<T>> Noeuds { get; } = new();
        private Dictionary<T, List<T>> liaisons = new();

        public void AjouterStation(T id, string ligne, string nom, double longitude, double latitude)
        {
            if (!Noeuds.ContainsKey(id))
            {
                Noeuds[id] = new Noeud<T>(id, ligne, nom, longitude, latitude);
                liaisons[id] = new List<T>(); // Initialiser la liste des voisins
            }
        }

        public void AjouterLiaison(T id1, T id2)
        {
            if (!Noeuds.ContainsKey(id1) || !Noeuds.ContainsKey(id2))
                return; // Vérification pour éviter des erreurs

            if (!liaisons[id1].Contains(id2))
                liaisons[id1].Add(id2);

            if (!liaisons[id2].Contains(id1))
                liaisons[id2].Add(id1);
        }

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
                    int.TryParse(elements[3], out int voisinId))
                {
                    AjouterLiaison((T)(object)idStation, (T)(object)voisinId);
                }
            }
        }

        public void AfficherStations()
        {
            foreach (var station in Noeuds.Values)
            {
                Console.WriteLine($"ID: {station.Id}, Ligne: {station.Ligne}, Nom: {station.Nom}, Coords: ({station.Longitude}, {station.Latitude})");
            }
        }

        public void AfficherLiaisons()
        {
            foreach (var liaison in liaisons)
            {
                Console.WriteLine($"Station {liaison.Key} ({Noeuds[liaison.Key].Nom}) connectée à : {string.Join(", ", liaison.Value.Select(id => Noeuds[id].Nom))}");
            }
        }
        public List<T> ObtenirVoisins(T id)
        {
            if (liaisons.ContainsKey(id))
            {
                return liaisons[id];
            }
            return new List<T>(); // Retourne une liste vide si aucun voisin trouvé
        }

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