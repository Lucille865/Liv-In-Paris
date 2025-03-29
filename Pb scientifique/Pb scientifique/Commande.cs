using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pb_scientifique
{
    public class Commande
    {
        public int Numero { get; set; }
        public Client Client { get; set; }
        public Cuisinier Cuisinier { get; set; }
        public List<LigneCommande> LignesCommande { get; set; } = new List<LigneCommande>();
        public decimal TotalPrix => CalculerPrix();
        public int Id { get; set; }
        public DateTime Date { get; set; }

        public Commande()
        {
            Id = new Random().Next(1000, 9999); // Génère un ID aléatoire pour chaque commande
            Date = DateTime.Now; // La date de la commande est définie comme l'heure actuelle
        }

        private decimal CalculerPrix()
        {
            decimal total = 0;
            foreach (var ligne in LignesCommande)
            {
                total += ligne.Quantite * ligne.Plat.PrixParPersonne;
            }
            return total;
        }
    }
}
