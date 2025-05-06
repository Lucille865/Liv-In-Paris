using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pb_scientifique
{
    public class Livraison
    {
        public string AdresseClient { get; set; }
        public DateTime DateLivraison { get; set; }
        public Plat PlatLivraison { get; set; }

        public Livraison() { }
        public Livraison(string adresseClient, DateTime dateLivraison, Plat platLivraison)
        {
            AdresseClient = adresseClient;
            DateLivraison = dateLivraison;
            PlatLivraison = platLivraison;
        }
        public static string CalculerChemin(string adresseDepart, string adresseArrivee)
        {
            return "Chemin en métro simplifié entre " + adresseDepart + " et " + adresseArrivee;
        }
    }
}
