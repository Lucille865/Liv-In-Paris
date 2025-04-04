using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pb_scientifique
{
    public class Statistiques
    {
        private readonly GestionCommandes _gestionCommandes;
        private readonly GestionClients _gestionClients;
        private readonly GestionCuisiniers _gestionCuisiniers;

        public Statistiques(GestionCommandes gestionCommandes,
                          GestionClients gestionClients,
                          GestionCuisiniers gestionCuisiniers)
        {
            _gestionCommandes = gestionCommandes;
            _gestionClients = gestionClients;
            _gestionCuisiniers = gestionCuisiniers;
        }

        public Dictionary<string, decimal> GetChiffreAffaireParCuisinier()
        {
            var commandes = _gestionCommandes.GetCommandes();

            // Alternative plus robuste avec vérification des null
            return commandes
                .Where(c => c.Cuisinier != null && c.LignesCommande != null)
                .GroupBy(c => c.Cuisinier.Nom)
                .Select(g => new {
                    Nom = g.Key,
                    CA = g.Sum(c => c.LignesCommande
                        .Where(l => l.Plat != null)
                        .Sum(l => l.Plat.PrixParPersonne * l.Quantite)) // Utilisation de Quantité
                })
                .OrderByDescending(x => x.CA)
                .ToDictionary(x => x.Nom, x => x.CA);
        }

        public Dictionary<string, int> GetPlatsPopulaires(int top = 5)
        {
            var commandes = _gestionCommandes.GetCommandes();

            return commandes
                .SelectMany(c => c.LignesCommande)
                .Where(l => l.Plat != null)
                .GroupBy(l => l.Plat.Nom)
                .OrderByDescending(g => g.Sum(l => l.Quantite))
                .Take(top)
                .ToDictionary(g => g.Key, g => g.Sum(l => l.Quantite));
        }

        public decimal GetMoyenneCommandesParClient()
        {
            var commandes = _gestionCommandes.GetCommandes();
            var clients = _gestionClients.GetClients(); // À implémenter si absent

            if (!clients.Any()) return 0;

            return Math.Round((decimal)commandes.Count / clients.Count, 2);
        }
    }
}
