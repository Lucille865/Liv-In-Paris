using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pb_scientifique
{
    public class LigneCommande
    {
        public Plat Plat { get; set; }
        public int Quantite { get; set; }
        public DateTime DateLivraison { get; set; }
        public string AdresseLivraison { get; set; }

        // Constructeur par défaut
        public LigneCommande() { }

        // Constructeur avec paramètres
        public LigneCommande(Plat plat, int quantite, DateTime dateLivraison, string adresseLivraison)
        {
            Plat = plat;
            Quantite = quantite;
            DateLivraison = dateLivraison;
            AdresseLivraison = adresseLivraison;
        }
    }
}
