using Pb_scientifique;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;

public class AfficheGraphe<T> where T : notnull
{
    public List<Noeud<T>> Noeuds { get; set; } = new List<Noeud<T>>();
    public List<Lien<T>> Liens { get; set; } = new List<Lien<T>>();

    public void ChargerDepuisFichier(string cheminFichier, Func<string, T> parseFunc)
    {
        try
        {
            using (StreamReader sr = new StreamReader(cheminFichier))
            {
                string ligne;
                bool debutData = false;
                var noeudDict = new Dictionary<T, Noeud<T>>();

                while ((ligne = sr.ReadLine()) != null)
                {
                    // Affichage des lignes lues pour débogage
                    Console.WriteLine($"Ligne lue: {ligne}");

                    if (ligne.StartsWith("%"))
                        continue;

                    if (!debutData)
                    {
                        debutData = true;
                        continue;
                    }

                    string[] parties = ligne.Split(';');
                    Console.WriteLine("Ligne lue : " + ligne);
                    if (parties.Length >= 5)  // Ajusté pour 5 éléments dans ton format
                    {
                        try
                        {
                            T id1 = parseFunc(parties[0]);
                            string nomStation = parties[1];
                            double longitude = double.Parse(parties[3]);
                            double latitude = double.Parse(parties[4]);

                            // Ajout d'un message de débogage pour chaque station
                            Console.WriteLine($"Station lue: {nomStation} - ID: {id1}, Latitude: {latitude}, Longitude: {longitude}");

                            if (!noeudDict.ContainsKey(id1))
                            {
                                var noeud = new Noeud<T>(id1, ligne, nomStation, longitude, latitude);
                                noeud.Latitude = latitude;
                                noeud.Longitude = longitude;
                                noeudDict[id1] = noeud;
                                Noeuds.Add(noeud);  // Ajouter à la liste Noeuds
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Erreur lors de l'ajout de la station : {ex.Message}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erreur lors de la lecture du fichier : " + ex.Message);
        }
        Console.WriteLine($"Nombre de stations chargées : {Noeuds.Count}");
        foreach (var noeud in Noeuds)
        {
            Console.WriteLine($"Station ID: {noeud.Id}, Latitude: {noeud.Latitude}, Longitude: {noeud.Longitude}");
        }

    }


    public void DessinerGraphe(string cheminFichier)
    {
        int largeur = 800, hauteur = 600;
        Bitmap bmp = new Bitmap(largeur, hauteur);
        Graphics g = Graphics.FromImage(bmp);
        g.Clear(Color.White);

        // Déterminer les min/max pour normaliser les coordonnées
        double minLong = double.MaxValue, maxLong = double.MinValue;
        double minLat = double.MaxValue, maxLat = double.MinValue;

        foreach (var n in Noeuds)
        {
            minLong = Math.Min(minLong, n.Longitude);
            maxLong = Math.Max(maxLong, n.Longitude);
            minLat = Math.Min(minLat, n.Latitude);
            maxLat = Math.Max(maxLat, n.Latitude);
        }

        double echelleX = (largeur - 50) / (maxLong - minLong);
        double echelleY = (hauteur - 50) / (maxLat - minLat);

        Font font = new Font("Arial", 8);
        Brush brush = Brushes.Black;

        // Dessiner les stations
        foreach (var n in Noeuds)
        {
            int x = (int)((n.Longitude - minLong) * echelleX) + 25;
            int y = hauteur - (int)((n.Latitude - minLat) * echelleY) - 25;

            g.FillEllipse(Brushes.LightBlue, x - 3, y - 3, 6, 6);
            g.DrawEllipse(Pens.Black, x - 3, y - 3, 6, 6);
            g.DrawString(n.Id.ToString(), font, brush, x + 5, y + 5);
        }

        bmp.Save(cheminFichier);
        Console.WriteLine($"Graphe sauvegardé à {cheminFichier}");
    }
}



