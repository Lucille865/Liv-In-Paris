using Pb_scientifique;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;        
using System.Linq;      
using System.Globalization;


namespace Pb_scientifique
{
    public class AfficheGraphe<T> where T : notnull
    {
        public List<Noeud<T>> Noeuds { get; set; } = new List<Noeud<T>>();
        public List<Lien<T>> Liens { get; set; } = new List<Lien<T>>();

        private Dictionary<T, string> stationToLine = new Dictionary<T, string>();

        private Dictionary<string, Color> couleurParLigne = new Dictionary<string, Color>()
{
    { "1", Color.Gold },
    { "2", Color.BlueViolet },
    { "3", Color.Olive },
    { "4", Color.Magenta },
    { "5", Color.Orange },
    { "6", Color.LightGreen },
    { "7", Color.Pink },
    { "8", Color.SlateBlue },
    { "9", Color.Yellow },
    { "10", Color.Tan },
    { "11", Color.Brown },
    { "12", Color.ForestGreen },
    { "13", Color.Teal },
    { "14", Color.DarkViolet }
};


        public void ChargerDepuisFichier(string cheminNoeuds, string cheminLiens, Func<string, T> parseFunc)
        {
            // Charger les noeuds (comme avant)
            ChargerNoeuds(cheminNoeuds, parseFunc);

            // Charger les liens
            ChargerLiens(cheminLiens, parseFunc);
        }

        private void ChargerNoeuds(string cheminFichier, Func<string, T> parseFunc)
        {
            Noeuds.Clear();
            stationToLine.Clear(); // ← ajoute ça

            using (StreamReader sr = new StreamReader(cheminFichier))
            {
                string ligne;
                while ((ligne = sr.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(ligne) || ligne.StartsWith("ID Station"))
                        continue;

                    string[] parties = ligne.Split(';');
                    if (parties.Length >= 5)
                    {
                        try
                        {
                            T id = parseFunc(parties[0].Trim());
                            string ligneMetro = parties[1].Trim();
                            string nom = parties[2].Trim();
                            double longitude = double.Parse(parties[3].Trim(), CultureInfo.InvariantCulture);
                            double latitude = double.Parse(parties[4].Trim(), CultureInfo.InvariantCulture);

                            Noeuds.Add(new Noeud<T>(id, ligneMetro, nom, longitude, latitude));
                            stationToLine[id] = ligneMetro; // ← stocke la ligne ici
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Erreur ligne noeud: {ligne}\n{ex.Message}");
                        }
                    }
                }
            }
        }

        private void ChargerLiens(string cheminFichier, Func<string, T> parseFunc)
        {
            Liens.Clear();
            try
            {
                using (StreamReader sr = new StreamReader(cheminFichier))
                {
                    string ligne;
                    while ((ligne = sr.ReadLine()) != null)
                    {
                        if (string.IsNullOrWhiteSpace(ligne) || ligne.StartsWith("%") || ligne.StartsWith("Station Id"))
                            continue;

                        string[] parties = ligne.Split(';');
                        if (parties.Length >= 4)
                        {
                            try
                            {
                                T sourceId = parseFunc(parties[0].Trim());
                                var source = Noeuds.FirstOrDefault(n => n.Id.Equals(sourceId));
                                if (source == null)
                                    continue;

                                // Lien vers "Suivant"
                                if (!string.IsNullOrWhiteSpace(parties[3]))
                                {
                                    T suivantId = parseFunc(parties[3].Trim());
                                    var suivant = Noeuds.FirstOrDefault(n => n.Id.Equals(suivantId));
                                    int temps = int.Parse(parties[4].Trim());

                                    if (suivant != null && source.Ligne == suivant.Ligne)
                                    {
                                        if (!Liens.Any(l => (l.Noeud1.Id.Equals(source.Id) && l.Noeud2.Id.Equals(suivant.Id)) ||
                                                            (l.Noeud1.Id.Equals(suivant.Id) && l.Noeud2.Id.Equals(source.Id))))
                                        {
                                            Liens.Add(new Lien<T>(source, suivant, temps));
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Erreur ligne lien: {ligne}\n{ex.Message}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lecture fichier liens: {ex.Message}");
            }
        }


        public void DessinerGraphe(string cheminFichier)
        {
            if (Noeuds.Count == 0)
            {
                Console.WriteLine("Aucun noeud à afficher!");
                return;
            }

            int largeur = 1200;
            int hauteur = 800;
            int marge = 50;

            using (Bitmap bmp = new Bitmap(largeur, hauteur))
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                // Calcul des bornes
                double minLong = Noeuds.Min(n => n.Longitude);
                double maxLong = Noeuds.Max(n => n.Longitude);
                double minLat = Noeuds.Min(n => n.Latitude);
                double maxLat = Noeuds.Max(n => n.Latitude);

                // Fonction de conversion des coordonnées
                PointF ToPixel(double longitude, double latitude)
                {
                    float x = marge + (float)((longitude - minLong) * (largeur - 2 * marge) / (maxLong - minLong));
                    float y = hauteur - marge - (float)((latitude - minLat) * (hauteur - 2 * marge) / (maxLat - minLat));
                    return new PointF(x, y);
                }

                // Dictionnaire pour accéder rapidement aux noeuds
                var noeudDict = Noeuds.ToDictionary(n => n.Id);

                // Dessin des liens en premier (en arrière-plan)
                using (Pen penLien = new Pen(Color.Gray, 2))
                {
                    foreach (var lien in Liens)
                    {
                        var ptSource = ToPixel(lien.Noeud1.Longitude, lien.Noeud1.Latitude);
                        var ptCible = ToPixel(lien.Noeud2.Longitude, lien.Noeud2.Latitude);

                        if (stationToLine.TryGetValue((T)lien.Noeud1.Id, out var ligne))
                        {
                            if (couleurParLigne.TryGetValue(ligne, out var couleur))
                            {
                                using (Pen pen = new Pen(couleur, 2))
                                {
                                    g.DrawLine(pen, ptSource, ptCible);
                                }
                            }
                            else
                            {
                                using (Pen pen = new Pen(Color.Gray, 2))
                                {
                                    g.DrawLine(pen, ptSource, ptCible);
                                }
                            }
                        }
                    }

                }

                // Dessin des noeuds par-dessus les liens
                using (Font font = new Font("Arial", 8))
                using (Brush brush = Brushes.Black)
                {
                    foreach (var noeud in Noeuds)
                    {
                        var pt = ToPixel(noeud.Longitude, noeud.Latitude);

                        // Cercle du noeud
                        g.FillEllipse(Brushes.LightBlue, pt.X - 5, pt.Y - 5, 10, 10);
                        g.DrawEllipse(Pens.Black, pt.X - 5, pt.Y - 5, 10, 10);

                        // Texte
                        g.DrawString(noeud.Nom, font, brush, pt.X + 8, pt.Y - 5);

                    }
                }

                // Sauvegarde
                try
                {
                    bmp.Save(cheminFichier, System.Drawing.Imaging.ImageFormat.Png);
                    Console.WriteLine($"Graphe généré: {Noeuds.Count} noeuds, {Liens.Count} liens");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur sauvegarde: {ex.Message}");
                }
            }
        }
    }
}



