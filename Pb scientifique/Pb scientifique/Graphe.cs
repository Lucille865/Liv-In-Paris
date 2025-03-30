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
        public Dictionary<T, Noeud<T>> Noeuds { get; }  // Associe l'ID d'une station à son objet Station
        public Dictionary<T, List<T>> liaisons; // Liste des connexions entre stations
        //public List<Noeud<T>> Noeuds { get; set; } = new List<Noeud<T>>();
        public List<Lien<T>> Liens { get; set; } = new List<Lien<T>>();

        public Graphe()
        {
            Noeuds = new Dictionary<T, Noeud<T>>();
            liaisons = new Dictionary<T, List<T>>();
        }

        public void AjouterStation(T id, string ligne, string nom, double longitude, double latitude)
        {
            if (!Noeuds.ContainsKey(id))
            {
                Noeuds[id] = new Noeud<T>(id, ligne, nom, longitude, latitude);

                // Ajouter le noeud correspondant
                //Noeuds.Add(new Noeud<T>(id, nom, longitude, latitude));
            }
        }

        public void AjouterLiaison(T id1, T id2)
        {
            if (!liaisons.ContainsKey(id1))
            {
                liaisons[id1] = new List<T>();
            }
            if (!liaisons.ContainsKey(id2))
            {
                liaisons[id2] = new List<T>();
            }

            if (!liaisons[id1].Contains(id2))
            {
                liaisons[id1].Add(id2);
            }
            if (!liaisons[id2].Contains(id1))
            {
                liaisons[id2].Add(id1);
            }
        }

        public void AfficherStations()
        {
            foreach (var station in Noeuds.Values)
            {
                Console.WriteLine($"ID: {station.Id}, Nom: {station.Nom}, Coords: ({station.Longitude}, {station.Latitude})");
            }
        }

        public void AfficherLiaisons()
        {
            foreach (var liaison in liaisons)
            {
                Console.WriteLine($"Station {liaison.Key} est connectée à : {string.Join(", ", liaison.Value)}");
            }
        }

        public void ChargerStationsDepuisFichier(string cheminStations)
        {
            if (!File.Exists(cheminStations))
            {
                Console.WriteLine("Fichier de stations introuvable !");
                return;
            }

            string[] lignes = File.ReadAllLines(cheminStations);
            foreach (string ligne in lignes.Skip(1)) // Ignore l'en-tête
            {
                var elements = ligne.Split(';');
                if (elements.Length >= 7)  // Vérifie le format attendu
                {
                    // Convertir ID station (idStation est un entier)
                    if (int.TryParse(elements[0], out int idStation))
                    {
                        string ligneMetro = elements[1];
                        string nomStation = elements[2];

                        // Séparation des coordonnées (longitude et latitude)
                        if (double.TryParse(elements[3], NumberStyles.Any, CultureInfo.InvariantCulture, out double longitude) &&
                            double.TryParse(elements[4], NumberStyles.Any, CultureInfo.InvariantCulture, out double latitude))
                        {
                            // Ajouter la station au graphe
                            AjouterStation((T)(object)idStation, ligneMetro, nomStation, longitude, latitude);
                        }
                        else
                        {
                            Console.WriteLine($"Erreur de conversion pour la station {nomStation}: {elements[3]} / {elements[4]}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Erreur de format ID pour la ligne: {ligne}");
                    }
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

            string[] lignes = File.ReadAllLines(cheminFichier);
            foreach (string ligne in lignes.Skip(1)) // Ignore l'en-tête
            {
                var elements = ligne.Split(';');

                if (elements.Length >= 6) // Vérifie qu'il y a bien 6 éléments dans la ligne
                {
                    if (int.TryParse(elements[0], out int idStation1) && int.TryParse(elements[3], out int idStation2))
                    {
                        // Conversion explicite d'idStation1 et idStation2 de int vers T
                        T station1 = (T)Convert.ChangeType(idStation1, typeof(T));
                        T station2 = (T)Convert.ChangeType(idStation2, typeof(T));

                        // Vérifier si les champs Précédent et Suivant sont vides ou non
                        if (!string.IsNullOrEmpty(elements[2]) && int.TryParse(elements[2], out int precedentId))
                        {
                            T precedentStation = (T)Convert.ChangeType(precedentId, typeof(T));
                            AjouterLiaison(precedentStation, station1);
                        }
                        if (!string.IsNullOrEmpty(elements[4]) && int.TryParse(elements[4], out int suivantId))
                        {
                            T suivantStation = (T)Convert.ChangeType(suivantId, typeof(T));
                            AjouterLiaison(station1, suivantStation);
                        }

                        // Ajouter la liaison entre Station1 et Station2
                        AjouterLiaison(station1, station2);
                    }
                    else
                    {
                        Console.WriteLine($"Erreur de format pour la ligne: {ligne}");
                    }
                }
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


    }
}