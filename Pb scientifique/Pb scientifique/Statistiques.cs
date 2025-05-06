using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pb_scientifique
{
    /// <summary>
    /// Fournit des statistiques globales sur les commandes, clients et cuisiniers.
    /// </summary>
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

        /// <summary>
        /// Retourne le chiffre d'affaires total généré par chaque cuisinier.
        /// </summary>
        /// <returns>Dictionnaire contenant le nom du cuisinier et son chiffre d'affaires.</returns>
        public Dictionary<string, decimal> GetChiffreAffaireParCuisinier()
        {
            Dictionary<string, decimal> resultats = new Dictionary<string, decimal>();
            List<Commande> commandes = _gestionCommandes.GetCommandes();

            foreach (Commande commande in commandes)
            {
                if (commande.Cuisinier != null && commande.LignesCommande != null)
                {
                    string nomCuisinier = commande.Cuisinier.Nom;
                    decimal montant = 0;

                    foreach (LigneCommande ligne in commande.LignesCommande)
                    {
                        if (ligne.Plat != null)
                        {
                            montant += ligne.Plat.PrixParPersonne * ligne.Quantite;
                        }
                    }

                    if (resultats.ContainsKey(nomCuisinier))
                    {
                        resultats[nomCuisinier] += montant;
                    }
                    else
                    {
                        resultats[nomCuisinier] = montant;
                    }
                }
            }

            return resultats;
        }

        /// <summary>
        /// Retourne les plats les plus populaires, triés par quantité commandée.
        /// </summary>
        /// <param name="top">Nombre de plats à retourner.</param>
        /// <returns>Dictionnaire contenant le nom du plat et le nombre total de commandes.</returns>
        public Dictionary<string, int> GetPlatsPopulaires(int top = 5)
        {
            Dictionary<string, int> platsCount = new Dictionary<string, int>();
            List<Commande> commandes = _gestionCommandes.GetCommandes();

            foreach (Commande commande in commandes)
            {
                foreach (LigneCommande ligne in commande.LignesCommande)
                {
                    if (ligne.Plat != null)
                    {
                        string nomPlat = ligne.Plat.Nom;
                        int quantite = ligne.Quantite;

                        if (platsCount.ContainsKey(nomPlat))
                        {
                            platsCount[nomPlat] += quantite;
                        }
                        else
                        {
                            platsCount[nomPlat] = quantite;
                        }
                    }
                }
            }
            // Tri par quantité décroissante et on garde les "top" premiers
            Dictionary<string, int> platsPopulaires = new Dictionary<string, int>();
            foreach (var pair in platsCount)
            {
                platsPopulaires.Add(pair.Key, pair.Value);
            }

            List<KeyValuePair<string, int>> listeTriee = new List<KeyValuePair<string, int>>(platsPopulaires);
            listeTriee.Sort((x, y) => y.Value.CompareTo(x.Value));

            Dictionary<string, int> topPlats = new Dictionary<string, int>();
            int compteur = 0;
            foreach (var pair in listeTriee)
            {
                if (compteur >= top)
                    break;

                topPlats[pair.Key] = pair.Value;
                compteur++;
            }

            return topPlats;
        }

        /// <summary>
        /// Calcule la moyenne du nombre de commandes par client.
        /// </summary>
        /// <returns>Nombre moyen de commandes par client.</returns>
        public decimal GetMoyenneCommandesParClient()
        {
            List<Commande> commandes = _gestionCommandes.GetCommandes();
            List<Client> clients = _gestionClients.GetClients();

            if (clients.Count == 0) return 0;

            decimal moyenne = (decimal)commandes.Count / clients.Count;
            return Math.Round(moyenne, 2);
        }

        /// <summary>
        /// Calcule la moyenne du chiffre d'affaires par client.
        /// </summary>
        /// <returns>Chiffre d'affaires moyen par client.</returns>
        public decimal GetMoyenneChiffreAffaireParClient()
        {
            Dictionary<string, decimal> caParCuisinier = GetChiffreAffaireParCuisinier();
            decimal totalCA = 0;

            foreach (var montant in caParCuisinier.Values)
            {
                totalCA += montant;
            }

            List<Client> clients = _gestionClients.GetClients();
            if (clients.Count == 0) return 0;

            return Math.Round(totalCA / clients.Count, 2);
        }
    }
}
