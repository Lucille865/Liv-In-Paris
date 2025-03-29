using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pb_scientifique
{
    public class GestionCommandes
    {
        private List<Commande> commandes = new List<Commande>();

        // Méthode pour ajouter une commande
        public void AjouterCommande(Commande commande)
        {
            commandes.Add(commande);
            Console.WriteLine("Commande ajoutée.");
        }

        // Méthode pour afficher toutes les commandes
        public void AfficherCommandes()
        {
            if (commandes.Count == 0)
            {
                Console.WriteLine("Aucune commande à afficher.");
                return;
            }

            foreach (var commande in commandes)
            {
                Console.WriteLine($"Commande ID: {commande.Id}, Date: {commande.Date}");
            }
        }
    }
}
