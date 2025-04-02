using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Pb_scientifique
{
    public class Interface
    {
        private GestionClients gestionClients;
        private GestionCuisiniers gestionCuisiniers;
        private GestionPlats gestionPlats;
        private GestionCommandes gestionCommandes;

        public Interface()
        {
            gestionClients = new GestionClients();
            gestionCuisiniers = new GestionCuisiniers();
            gestionPlats = new GestionPlats();
            gestionCommandes = new GestionCommandes();
        }

        public void AfficherMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== LIV'IN PARIS ===");
                Console.WriteLine("1. Gestion Clients");
                Console.WriteLine("2. Gestion Cuisiniers");
                Console.WriteLine("3. Gestion Plats");
                Console.WriteLine("4. Gestion Commandes");
                Console.WriteLine("5. Quitter");
                Console.Write("Choix : ");

                switch (Console.ReadLine())
                {
                    case "1": MenuClients(); break;
                    case "2": MenuCuisiniers(); break;
                    case "3": MenuPlats(); break;
                    case "4": MenuCommandes(); break;
                    case "5": Environment.Exit(0); break;
                }
            }
        }

        #region Clients
        private void MenuClients()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== CLIENTS ===");
                Console.WriteLine("1. Ajouter");
                Console.WriteLine("2. Lister");
                Console.WriteLine("3. Rechercher");
                Console.WriteLine("4. Retour");
                Console.Write("Choix : ");

                switch (Console.ReadLine())
                {
                    case "1": AjouterClient(); break;
                    case "2": ListerClients(); break;
                    case "3": RechercherClient(); break;
                    case "4": return;
                }
            }
        }

        private void AjouterClient()
        {
            Console.Write("Nom : ");
            string? nom = Console.ReadLine();

            Console.Write("Prénom : ");
            string? prenom = Console.ReadLine();

            Console.Write("Adresse : ");
            string? adresse = Console.ReadLine();

            Console.Write("Téléphone : ");
            string? telephone = Console.ReadLine();

            Console.Write("Email : ");
            string? email = Console.ReadLine();

            Console.Write("Identifiant : ");
            string? identifiant = Console.ReadLine();

            Console.Write("Mot de passe : ");
            string? mdp = Console.ReadLine();

            Console.Write("Type (Particulier/Entreprise) : ");
            string? type = Console.ReadLine();

            string? entreprise = null, referent = null;
            if (type == "Entreprise")
            {
                Console.Write("Nom entreprise : ");
                entreprise = Console.ReadLine();
                Console.Write("Référent : ");
                referent = Console.ReadLine();
            }

            Console.Write("Station métro proche : ");
            string? metro = Console.ReadLine();

            if (nom == null || prenom == null || adresse == null || telephone == null || email == null || identifiant == null || mdp == null || type == null || metro == null)
            {
                Console.WriteLine("Erreur : Tous les champs sont obligatoires.");
                return;
            }

            var client = new Client(nom, adresse, telephone, email, identifiant, mdp, type,entreprise, referent, metro)
            {
                NomEntreprise = entreprise,
                Referent = referent
            };

            gestionClients.AjouterClient(client);
            Console.WriteLine("Client ajouté !");
            Console.ReadKey();
        }

        private void ListerClients()
        {
            foreach (var c in gestionClients.GetTousClients())
            {
                Console.WriteLine($"{c.Identifiant} | {c.Nom} {c.Prenom} | {c.Email} | {c.MetroProche}");
            }
            Console.ReadKey();
        }

        private void RechercherClient()
        {
            Console.Write("Identifiant : ");
            var client = gestionClients.GetClientParIdentifiant(Console.ReadLine());
            if (client != null)
            {
                Console.WriteLine($"Nom: {client.Nom}\nAdresse: {client.Adresse}\nMétro: {client.MetroProche}");
            }
            else Console.WriteLine("Non trouvé");
            Console.ReadKey();
        }
        #endregion

        #region Cuisiniers
        private void MenuCuisiniers()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== CUISINIERS ===");
                Console.WriteLine("1. Ajouter");
                Console.WriteLine("2. Lister");
                Console.WriteLine("3. Retour");
                Console.Write("Choix : ");

                switch (Console.ReadLine())
                {
                    case "1": AjouterCuisinier(); break;
                    case "2": ListerCuisiniers(); break;
                    case "3": return;
                }
            }
        }

        private void AjouterCuisinier()
        {
            Console.Write("Nom : ");
            string nom = Console.ReadLine();

            Console.Write("Prénom : ");
            string prenom = Console.ReadLine();

            Console.Write("Adresse : ");
            string adresse = Console.ReadLine();

            Console.Write("Téléphone : ");
            string telephone = Console.ReadLine();

            Console.Write("Email : ");
            string email = Console.ReadLine();

            Console.Write("Identifiant : ");
            string identifiant = Console.ReadLine();

            Console.Write("Mot de passe : ");
            string mdp = Console.ReadLine();

            Console.Write("Station métro proche : ");
            string metro = Console.ReadLine();

            var cuisinier = new Cuisinier
            (nom, prenom, adresse, telephone, email, identifiant, mdp, metro);

            gestionCuisiniers.AjouterCuisinier(cuisinier);
            Console.WriteLine("Cuisinier ajouté !");
            Console.ReadKey();
        }

        private void ListerCuisiniers()
        {
            foreach (var c in gestionCuisiniers.GetTousCuisiniers())
            {
                Console.WriteLine($"{c.Identifiant} | {c.Nom} {c.Prenom} | {c.Email} | {c.MetroProche}");
            }
            Console.ReadKey();
        }
        #endregion

        #region Plats
        private void MenuPlats()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== PLATS ===");
                Console.WriteLine("1. Ajouter");
                Console.WriteLine("2. Lister");
                Console.WriteLine("3. Retour");
                Console.Write("Choix : ");

                switch (Console.ReadLine())
                {
                    case "1": AjouterPlat(); break;
                    case "2": ListerPlats(); break;
                    case "3": return;
                }
            }
        }

        private void AjouterPlat()
        {
            Console.Write("ID Cuisinier : ");
            int idCuisinier = int.Parse(Console.ReadLine());

            Console.Write("Nom du plat : ");
            string nom = Console.ReadLine();

            Console.Write("Type (Entrée/Plat/Dessert) : ");
            string type = Console.ReadLine();

            Console.Write("Régime alimentaire : ");
            string regime = Console.ReadLine();

            Console.Write("Nationalité : ");
            string nationalite = Console.ReadLine();

            Console.Write("Prix par personne : ");
            decimal prix = decimal.Parse(Console.ReadLine());

            Console.Write("Date péremption (YYYY-MM-DD) : ");
            DateTime peremption = DateTime.Parse(Console.ReadLine());

            var plat = new Plat
            (nom, type, 1, DateTime.Now, peremption, prix, nationalite, regime, new List<string>());
            gestionPlats.AjouterPlat(plat);
            Console.WriteLine("Plat ajouté !");
            Console.ReadKey();
        }

        private void ListerPlats()
        {
            foreach (var p in gestionPlats.GetTousPlats())
            {
                Console.WriteLine($"{p.Plat_Id} | {p.Nom} | {p.Type} | {p.PrixParPersonne}€");
            }
            Console.ReadKey();
        }
        #endregion

        #region Commandes
        private void MenuCommandes()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== COMMANDES ===");
                Console.WriteLine("1. Créer");
                Console.WriteLine("2. Lister");
                Console.WriteLine("3. Retour");
                Console.Write("Choix : ");

                /*switch (Console.ReadLine())
                {
                    case "1": CreerCommande(); break;
                    case "2": ListerCommandes(); break;
                    case "3": return;
                }*/
            }
        }

        /*private void CreerCommande()
        {
            Console.Write("ID Client : ");
            int idClient = int.Parse(Console.ReadLine());

            Console.Write("ID Cuisinier : ");
            int idCuisinier = int.Parse(Console.ReadLine());

            Console.Write("Adresse livraison : ");
            string adresse = Console.ReadLine();

            Console.Write("Station métro livraison : ");
            string station = Console.ReadLine();

            var commande = new Commande
            {
                ClientId = idClient,
                CuisinierId = idCuisinier,
                AdresseLivraison = adresse,
                StationLivraison = station,
                Statut = "En préparation"
            };

            gestionCommandes.AjouterCommande(commande);
            Console.WriteLine("Commande créée !");
            Console.ReadKey();
        }

        private void ListerCommandes()
        {
            foreach (var c in gestionCommandes.GetToutesCommandes())
            {
                Console.WriteLine($"#{c.Commande_Id} | Client: {c.ClientId} | Statut: {c.Statut}");
            }
            Console.ReadKey();
        }*/
        #endregion

       

        /*private void CreerCommande()
        {
            Console.Write("Entrez votre identifiant de client : ");
            string clientId = Console.ReadLine();

            // Rechercher le client correspondant dans la liste des clients
            Client client = gestionClients.clients.FirstOrDefault(c => c.Identifiant == clientId);

            if (client == null)
            {
                Console.WriteLine("Erreur : Aucun client trouvé avec cet identifiant.");
                return;
            }

            string cheminStations = "MetroParisNoeuds.txt";
            string cheminLiaisons = "MetroParisArcs.txt";
            graphe.ChargerStationsDepuisFichier(cheminStations);
            graphe.ChargerLiaisonsDepuisFichier(cheminLiaisons);
            // Trouver un cuisinier disponible et proche
            Cuisinier cuisinier = gestionCuisiniers.AssignerCuisinierRandom(); // Assigner un cuisinier au hasard
            Console.WriteLine($"Commande créée pour {client.Nom}. Cuisinier assigné : {cuisinier.Nom}");

            // Calcul du chemin le plus court entre le cuisinier et le client
            var bellmanFord = new BellmanFord<int>(graphe);
            int idStationclient = graphe.TrouverIdParNom(client.Adresse);
            int idStationcuisinier = graphe.TrouverIdParNom(cuisinier.Adresse);
            bool succes = bellmanFord.CalculerPlusCourtChemin(idStationcuisinier);

            if (succes)
            {
                var chemin = bellmanFord.GetChemin(idStationclient);
                Console.WriteLine("Chemin le plus court entre " + cuisinier.Nom + " et " + client.Nom + " :");

                foreach (var stationId in chemin)
                {
                    if (graphe.Noeuds.TryGetValue(stationId, out var station))
                    {
                        Console.WriteLine($"- {station.Nom} (Ligne {station.Ligne})");
                    }
                }
            }
            else
            {
                Console.WriteLine("Impossible de calculer le plus court chemin à cause d'un cycle de poids négatif.");
            }

            PauseEtRetourMenu();
        }*/

        

        private void GérerStatistiques()
        {
            // Logique pour afficher des statistiques
            Console.WriteLine("Statistiques à venir...");
            AfficherMenu();
        }
    }

}
