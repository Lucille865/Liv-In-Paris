using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Pb_scientifique
{
    public class Autre
    {
        private GestionClients gestionClients;
        private GestionCuisiniers gestionCuisiniers;

        public Autre(GestionClients clients, GestionCuisiniers cuisiniers)
        {
            gestionClients = clients;
            gestionCuisiniers = cuisiniers;
        }
        public string VerifierRetard(Commande commande, TimeSpan? dureeLivraisonMax = null)
        {
            // Si la commande n'a pas encore de statut définitif, on peut la considérer comme "En attente"
            if (commande.Statut == "En attente")
            {
                // Si aucune durée de livraison n'est spécifiée, on peut par défaut considérer une heure.
                TimeSpan dureeLivraison = dureeLivraisonMax ?? TimeSpan.FromHours(1);

                var delaiMax = commande.Date.Add(dureeLivraison); // Calcul du moment limite
                var tempsRestant = delaiMax - DateTime.Now; // Temps restant avant retard

                if (tempsRestant > TimeSpan.Zero)
                {
                    return $"La commande n'est pas en retard. Il reste encore {tempsRestant.Hours} heure(s) et {tempsRestant.Minutes} minute(s) avant le délai de livraison.";
                }
                else
                {
                    // Si la commande est en retard
                    var tempsRetard = DateTime.Now - delaiMax;
                    return $"La commande est en retard de {tempsRetard.Hours} heure(s) et {tempsRetard.Minutes} minute(s).";
                }
            }
            else
            {
                return $"La commande a déjà un statut final ({commande.Statut}) et ne peut pas être en retard.";
            }
        }
        public void AfficherClientsInactifs(List<Client> clients, List<Commande> commandes)
        {
            var clientsActifs = commandes.Select(c => c.Client.Identifiant).Distinct();
            var inactifs = clients.Where(c => !clientsActifs.Contains(c.Identifiant)).ToList();

            if (inactifs.Count == 0)
            {
                Console.WriteLine("Tous les clients ont passé au moins une commande.");
                return;
            }

            Console.WriteLine("Clients inactifs :");
            foreach (var client in inactifs)
            {
                Console.WriteLine($"{client.Nom} {client.Prenom ?? ""} - {client.Email}");
            }
        }

        public void AfficherTousLesUtilisateurs()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("=== LISTE DES UTILISATEURS (Clients + Cuisiniers) ===");
            Console.ResetColor();

            Console.WriteLine("\n--- Clients ---");
            foreach (var client in gestionClients.GetClients())
            {
                Console.WriteLine($"{client.Identifiant} | {client.Nom} | {client.Email}");
            }

            Console.WriteLine("\n--- Cuisiniers ---");
            foreach (var cuis in gestionCuisiniers.GetTousCuisiniers())
            {
                Console.WriteLine($"{cuis.Identifiant} | {cuis.Nom} | {cuis.Email}");
            }

        }

    }
}
