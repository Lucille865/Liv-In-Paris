using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pb_scientifique
{
    public class Noeud<T>
    {
        public T Id { get; set; }
        public string Ligne { get; set; }
        public string Nom { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public List<Noeud<T>> Voisins { get; set; } = new List<Noeud<T>>();

        public Noeud(T id, string ligne, string nom, double longitude, double latitude)
        {
            Id = id;
            Ligne = ligne;
            Nom = nom;
            Longitude = longitude;
            Latitude = latitude;
        }

        public void AjouterVoisin(Noeud<T> voisin)
        {
            if (!Voisins.Contains(voisin))
                Voisins.Add(voisin);
        }

        public string toString()
        {
            return Nom;
        }
    }
}
