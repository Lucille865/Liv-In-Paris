using Pb_scientifique;
using System;
using System.Collections.Generic;
using System.Drawing;

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
                    if (ligne.StartsWith("%"))
                        continue;

                    if (!debutData)
                    {
                        debutData = true;
                        continue;
                    }

                    string[] parties = ligne.Split(' ');
                    if (parties.Length >= 2)
                    {
                        T id1 = parseFunc(parties[0]);
                        T id2 = parseFunc(parties[1]);

                        if (!noeudDict.ContainsKey(id1))
                        {
                            var noeud = new Noeud<T>(id1);
                            noeudDict[id1] = noeud;
                            Noeuds.Add(noeud);
                        }

                        if (!noeudDict.ContainsKey(id2))
                        {
                            var noeud = new Noeud<T>(id2);
                            noeudDict[id2] = noeud;
                            Noeuds.Add(noeud);
                        }

                        var noeud1 = noeudDict[id1];
                        var noeud2 = noeudDict[id2];

                        noeud1.AjouterVoisin(noeud2);
                        noeud2.AjouterVoisin(noeud1);

                        Liens.Add(new Lien<T>(noeud1, noeud2));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erreur lors de la lecture du fichier : " + ex.Message);
        }
    }

    public void DessinerGraphe(string cheminFichier)
    {
        int largeur = 800;
        int hauteur = 600;
        Bitmap bmp = new Bitmap(largeur, hauteur);
        Graphics g = Graphics.FromImage(bmp);
        g.Clear(Color.White);

        // Définir une police et des pinceaux
        Font font = new Font("Arial", 12);
        Brush brush = Brushes.Black;
        Pen pen = new Pen(Color.Black, 2);

        // Placer les nœuds en cercle
        int rayonCercle = 200;
        Point centre = new Point(largeur / 2, hauteur / 2);
        Dictionary<Noeud<T>, Point> positions = new Dictionary<Noeud<T>, Point>();

        int totalNoeuds = Noeuds.Count;
        for (int i = 0; i < totalNoeuds; i++)
        {
            double angle = i * (2 * Math.PI / totalNoeuds);
            int x = centre.X + (int)(rayonCercle * Math.Cos(angle));
            int y = centre.Y + (int)(rayonCercle * Math.Sin(angle));
            positions[Noeuds[i]] = new Point(x, y);
        }

        // Dessiner les liens
        foreach (var lien in Liens)
        {
            Point p1 = positions[lien.Noeud1];
            Point p2 = positions[lien.Noeud2];
            g.DrawLine(pen, p1, p2);
        }

        // Dessiner les nœuds avec leurs numéros/noms
        foreach (var noeud in Noeuds)
        {
            Point pos = positions[noeud];
            g.FillEllipse(Brushes.LightBlue, pos.X - 20, pos.Y - 20, 40, 40);
            g.DrawEllipse(Pens.Black, pos.X - 20, pos.Y - 20, 40, 40);

            // Afficher le numéro/le nom au centre du nœud
            string texte = noeud.Valeur.ToString();
            SizeF textSize = g.MeasureString(texte, font);
            g.DrawString(texte, font, brush, pos.X - textSize.Width / 2, pos.Y - textSize.Height / 2);
        }

        // Sauvegarder l'image
        bmp.Save(cheminFichier);
        Console.WriteLine($"Image sauvegardée à {cheminFichier}");
    }
}



