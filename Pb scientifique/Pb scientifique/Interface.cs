using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Pb_scientifique
{

    /// <summary>
    /// Classe représentant l'interface principale de l'application Liv'in Paris.
    /// Gère l'interaction avec l'utilisateur pour la gestion des clients, cuisiniers, commandes et statistiques.
    /// </summary>
    public class Interface
    {
        public GestionClients gestionClients;
        public GestionCuisiniers gestionCuisiniers;
        public GestionCommandes gestionCommandes;
        Graphe<int> graphe = new Graphe<int>();
        private RelationVisualizer visualiseur;

        public Interface()
        {
            gestionClients = new GestionClients();
            gestionCuisiniers = new GestionCuisiniers();
            gestionCommandes = new GestionCommandes(gestionClients, gestionCuisiniers);
            visualiseur = new RelationVisualizer(gestionClients, gestionCuisiniers);
        }

        // <summary>
        /// Affiche le menu principal et permet à l'utilisateur de choisir une action à effectuer.
        /// </summary>
        public void AfficherMenu()
        {
            Console.Clear();
            bool continuer = true;
            while (continuer)
            {
                Console.WriteLine("=== Bienvenue dans l'application Liv'in Paris ! ===");
                Console.WriteLine("Que souhaitez-vous faire ?");
                Console.WriteLine("1. Gérer les Clients");
                Console.WriteLine("2. Gérer les Cuisiniers");
                Console.WriteLine("3. Gérer les Commandes");
                Console.WriteLine("4. Gérer les Statistiques");
                Console.WriteLine("5. Quitter");
                var choix = Console.ReadLine();

                switch (choix)
                {
                    case "1":
                        GérerClients();
                        break;
                    case "2":
                        GérerCuisiniers();
                        break;
                    case "3":
                        GérerCommandes();
                        break;
                    case "4":
                        GérerStatistiques();
                        break;
                    case "5":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Choix invalide.");
                        PauseEtRetourMenu();
                        break;
                }
            }

        }
        private void PauseEtRetourMenu()
        {
            Console.WriteLine("\nQue souhaitez-vous faire ?");
            Console.WriteLine("1. Revenir au menu principal");
            Console.WriteLine("2. Quitter l'application");
            var choix = Console.ReadLine();

            switch (choix)
            {
                case "1":
                    Console.Clear(); // Efface l'écran avant de revenir au menu
                    AfficherMenu();
                    break;
                case "2":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Choix invalide. Retour au menu principal...");
                    Console.Clear();
                    AfficherMenu();
                    break;
            }
        }

        /// <summary>
        /// Affiche le menu de gestion des clients.
        /// Permet d'ajouter, afficher ou supprimer des clients.
        /// </summary>
        private void GérerClients()
        {
            Console.Clear();
            Console.WriteLine("=== CLIENTS ===");
            Console.WriteLine("1. Ajouter un client");
            Console.WriteLine("2. Afficher tous les clients");
            Console.WriteLine("3. Supprimer un client");
            Console.WriteLine("4. Retour");
            var choix = Console.ReadLine();

            switch (choix)
            {
                case "1":
                    AjouterClient();
                    break;
                case "2":
                    AfficherClients();
                    break;
                case "3":
                    SupprimerClient();
                    break;
                case "4":
                    AfficherMenu();
                    break;
                default:
                    Console.WriteLine("Choix invalide.");
                    break;
            }
        }

        /// <summary>
        /// Permet d'ajouter un nouveau client à la base de données.
        /// </summary>
        private void AjouterClient()
        {
            Console.Clear();
            Console.WriteLine("=== Ajout d'un nouveau client ===");

            Console.WriteLine("Nom du client : ");
            string nom = Console.ReadLine();
            Console.WriteLine("Metro le plus proche de l'adresse du client : ");
            string adresse = Console.ReadLine();
            Console.WriteLine("Numéro de téléphone : ");
            string telephone = Console.ReadLine();
            Console.WriteLine("Email du client : ");
            string email = Console.ReadLine();
            Console.WriteLine("Pseudo du client : ");
            string identifiant = Console.ReadLine();
            Console.WriteLine("Mot de passe du client : ");
            string mdp = Console.ReadLine();
            Console.WriteLine("Type de client (Particulier / Entreprise) : ");
            string typeClient = Console.ReadLine();
            string prenom = null;
            string nomEntreprise = null;
            string referent = null;
            if (typeClient == "Particulier")
            {
                Console.Write("Prénom : ");
                prenom = Console.ReadLine();
            }
            else if (typeClient == "Entreprise")
            {
                Console.Write("Nom entreprise : ");
                nomEntreprise = Console.ReadLine();

                Console.Write("Référent : ");
                referent = Console.ReadLine();
            }

            var client = new Client(nom, adresse, telephone, email, identifiant, mdp, typeClient)
            {
                Prenom = prenom,
                NomEntreprise = nomEntreprise,
                Referent = referent
            };

            gestionClients.AjouterClient(client);
            gestionClients.clients = gestionClients.GetClients();
            Console.WriteLine("Client ajouté avec succès.");
            PauseEtRetourMenu();
        }

        /// <summary>
        /// Affiche la liste de tous les clients existants.
        /// </summary>
        private void AfficherClients()
        {
            gestionClients.AfficherClients();
            PauseEtRetourMenu();
        }

        // <summary>
        /// Permet de supprimer un client en fonction de son identifiant (pseudo).
        /// </summary>
        public void SupprimerClient()
        {
            Console.Write("Entrez l'identifiant (pseudo) du client à supprimer : ");
            string identifiant = Console.ReadLine();

            string query = "DELETE FROM Clients WHERE Identifiant = @Identifiant";

            int lignes = DatabaseManager.ExecuteNonQuery(query,
                new MySqlParameter("@Identifiant", identifiant));

            if (lignes > 0)
                Console.WriteLine($"Client '{identifiant}' supprimé avec ses commandes.");
            else
                Console.WriteLine("Aucun client trouvé avec cet identifiant.");
        }

        /// <summary>
        /// Affiche le menu de gestion des cuisiniers.
        /// Permet d'ajouter, afficher ou supprimer des cuisiniers.
        /// </summary>
        private void GérerCuisiniers()
        {
            Console.Clear();
            Console.WriteLine("=== CUISINIERS ===");
            Console.WriteLine("1. Ajouter un cuisinier");
            Console.WriteLine("2. Afficher tous les cuisiniers");
            Console.WriteLine("3. Supprimer des cuisiniers");
            Console.WriteLine("4. Retour");
            var choix = Console.ReadLine();

            switch (choix)
            {
                case "1":
                    AjouterCuisinier();
                    break;
                case "2":
                    AfficherCuisiniers();
                    break;
                case "3":
                    SupprimerCuisinier();
                    break;
                case "4":
                    AfficherMenu();
                    break;
                default:
                    Console.WriteLine("Choix invalide.");
                    break;
            }
        }
        
        /// <summary>
        /// Permet d'ajouter un nouveau cuisinier à la base de données.
        /// </summary>
        private void AjouterCuisinier()
        {
            Console.Clear();
            Console.WriteLine("=== Ajout d'un nouveau cuisinier ===");

            Cuisinier cuisinier = gestionCuisiniers.CreerCuisinierPlat();
            Console.WriteLine($"Cuisinier ajouté : {cuisinier.Nom} avec succès !");
            PauseEtRetourMenu();
        }

        /// <summary>
        /// Affiche la liste de tous les cuisiniers existants.
        /// </summary>
        private void AfficherCuisiniers()
        {
            gestionCuisiniers.AfficherCuisiniers();
            PauseEtRetourMenu();
        }

        /// <summary>
        /// Supprime un cuisinier de la base de données en fonction de son identifiant.
        /// </summary>
        public void SupprimerCuisinier()
        {
            Console.Write("Entrez l'identifiant (pseudo) du cuisinier à supprimer : ");
            string identifiant = Console.ReadLine();

            string query = "DELETE FROM Cuisiniers WHERE Identifiant = @Identifiant";

            int lignes = DatabaseManager.ExecuteNonQuery(query,
                new MySqlParameter("@Identifiant", identifiant));

            if (lignes > 0)
                Console.WriteLine($"Cuisinier '{identifiant}' supprimé avec ses plats et commandes associées.");
            else
                Console.WriteLine("Aucun cuisinier trouvé avec cet identifiant.");
        }

        /// <summary>
        /// Affiche le menu de gestion des commandes.
        /// Permet de créer ou afficher des commandes.
        /// </summary>
        private void GérerCommandes()
        {
            Console.Clear();
            Console.WriteLine("=== COMMANDES ===");
            Console.WriteLine("1. Créer une commande");
            Console.WriteLine("2. Afficher toutes les commandes");
            Console.WriteLine("3. Retour");
            var choix = Console.ReadLine();

            switch (choix)
            {
                case "1":
                    CreerCommande();
                    break;
                case "2":
                    AfficherCommandes();
                    break;
                case "3":
                    AfficherMenu();
                    break;
                default:
                    Console.WriteLine("Choix invalide.");
                    break;
            }
        }

        /// <summary>
        /// Permet de créer une nouvelle commande pour un client.
        /// </summary>
        private void CreerCommande()
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

            gestionCommandes.CreerCommande(client);

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
        }

        /// <summary>
        /// Affiche la liste de toutes les commandes existantes.
        /// </summary>
        private void AfficherCommandes()
        {
            gestionCommandes.AfficherCommandes();
            PauseEtRetourMenu();
        }

        /// <summary>
        /// Affiche le menu des statistiques et permet d'afficher les résultats statistiques
        /// concernant les clients, cuisiniers et commandes.
        /// </summary>
        private void GérerStatistiques()
        {
            Console.Clear();
            var statistiques = new Statistiques(gestionCommandes, gestionClients, gestionCuisiniers);

            while (true)
            {
                Console.WriteLine("=== STATISTIQUES ===");
                Console.WriteLine("1. Chiffre d'affaires par cuisinier");
                Console.WriteLine("2. Plats les plus populaires");
                Console.WriteLine("3. Nombre moyen de commandes par client");
                Console.WriteLine("4. Graphe des relations");
                Console.WriteLine("5. Expotation en XML");
                Console.WriteLine("6. Revenir au menu principal");
                Console.Write("\nVotre choix : ");

                switch (Console.ReadLine())
                {
                    case "1":
                        AfficherChiffreAffaires(statistiques);
                        break;
                    case "2":
                        AfficherPlatsPopulaires(statistiques);
                        break;
                    case "3":
                        AfficherMoyenneCommandes(statistiques);
                        break;
                    case "4":
                        visualiseur.GenererGrapheComplet(gestionCommandes.GetCommandes());
                        Process.Start("explorer.exe", "relations.png");
                        break;
                    case "5":
                        var exportateur = new Exportateur();
                        exportateur.ExporterToutesLesDonnees();
                        Process.Start("explorer.exe", "relations.png");
                        Console.WriteLine("données exportées");
                        break;
                    case "6":
                        return;
                    default:
                        Console.WriteLine("Option invalide");
                        break;
                }
            }
        }

        /// <summary>
        /// Affiche les chiffres d'affaires par cuisinier.
        /// </summary>
        private void AfficherChiffreAffaires(Statistiques stats)
        {
            Console.WriteLine("\n=== CHIFFRE D'AFFAIRES PAR CUISINIER ===");
            var caParCuisinier = stats.GetChiffreAffaireParCuisinier();

            if (!caParCuisinier.Any())
            {
                Console.WriteLine("Aucune donnée disponible");
                return;
            }

            Console.WriteLine("{0,-20} {1,10}", "Cuisinier", "CA (€)");
            Console.WriteLine(new string('-', 32));

            foreach (var item in caParCuisinier)
            {
                Console.WriteLine("{0,-20} {1,10:N2}", item.Key, item.Value);
            }
            PauseEtRetourMenu();
        }

        /// <summary>
        /// Affiche les plats les plus populaires.
        /// </summary>
        private void AfficherPlatsPopulaires(Statistiques stats)
        {
            Console.WriteLine("\n=== PLATS LES PLUS POPULAIRES ===");
            var platsPopulaires = stats.GetPlatsPopulaires(5);

            if (!platsPopulaires.Any())
            {
                Console.WriteLine("Aucune donnée disponible");
                return;
            }

            Console.WriteLine("{0,-20} {1,10}", "Plat", "Commandes");
            Console.WriteLine(new string('-', 32));

            foreach (var plat in platsPopulaires)
            {
                Console.WriteLine("{0,-20} {1,10}", plat.Key, plat.Value);
            }
            PauseEtRetourMenu();
        }

        /// <summary>
        /// Affiche la moyenne de commandes effectuées par client.
        /// </summary>
        private void AfficherMoyenneCommandes(Statistiques stats)
        {
            Console.WriteLine("\n=== MOYENNE DE COMMANDES PAR CLIENT ===");
            decimal moyenne = stats.GetMoyenneCommandesParClient();
            Console.WriteLine($"\nChaque client a effectué en moyenne {moyenne:N2} commandes");
            PauseEtRetourMenu();
        }
    }

}
