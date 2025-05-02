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


        private void GérerClients()
        {
            Console.Clear();
            Console.WriteLine("=== CLIENTS ===");
            Console.WriteLine("1. Ajouter un client");
            Console.WriteLine("2. Afficher tous les clients");
            Console.WriteLine("3. Retour");
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
                    AfficherMenu();
                    break;
                default:
                    Console.WriteLine("Choix invalide.");
                    break;
            }
        }

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
            string pseudo = Console.ReadLine();
            Console.WriteLine("Mot de passe du client : ");
            string mdp = Console.ReadLine();
            Console.WriteLine("Type de client (Particulier / Entreprise) : ");
            string typeClient = Console.ReadLine();


            var client = new Client(nom, adresse, telephone, email, pseudo, mdp, typeClient);
            if (typeClient == "Particulier")
            {
                Console.WriteLine($"Prénom: ");
                string prenom = Console.ReadLine();
            }
            else if (typeClient == "Entreprise")
            {
                Console.WriteLine($"Nom Entreprise: ");
                string nomEntreprise = Console.ReadLine();
                Console.WriteLine("Référent: ");
                string referent = Console.ReadLine();
            }

            gestionClients.AjouterClient(client);
            Console.WriteLine("Client ajouté avec succès.");
            PauseEtRetourMenu();
        }

        private void AfficherClients()
        {
            gestionClients.AfficherClients();
            PauseEtRetourMenu();
        }

        private void GérerCuisiniers()
        {
            Console.Clear();
            Console.WriteLine("=== CUISINIERS ===");
            Console.WriteLine("1. Ajouter un cuisinier");
            Console.WriteLine("2. Afficher tous les cuisiniers");
            Console.WriteLine("3. Retour");
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
                    AfficherMenu();
                    break;
                default:
                    Console.WriteLine("Choix invalide.");
                    break;
            }
        }

        private void AjouterCuisinier()
        {
            Console.Clear();
            Console.WriteLine("=== Ajout d'un nouveau cuisinier ===");

            Cuisinier cuisinier = gestionCuisiniers.CreerCuisinierPlat();
            gestionCuisiniers.AjouterCuisinier(cuisinier);
            Console.WriteLine($"Cuisinier ajouté : {cuisinier.Nom} avec succès !");
            PauseEtRetourMenu();
        }

        private void AfficherCuisiniers()
        {
            gestionCuisiniers.AfficherCuisiniers();
            PauseEtRetourMenu();
        }

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

            /*string cheminStations = "MetroParisNoeuds.txt";
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
            }*/

            PauseEtRetourMenu();
        }

        private void AfficherCommandes()
        {
            gestionCommandes.AfficherCommandes();
            PauseEtRetourMenu();
        }

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
                Console.WriteLine("5. Revenir au menu principal");
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
                        return;
                    default:
                        Console.WriteLine("Option invalide");
                        break;
                }
            }
        }

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

        private void AfficherMoyenneCommandes(Statistiques stats)
        {
            Console.WriteLine("\n=== MOYENNE DE COMMANDES PAR CLIENT ===");
            decimal moyenne = stats.GetMoyenneCommandesParClient();
            Console.WriteLine($"\nChaque client a effectué en moyenne {moyenne:N2} commandes");
            PauseEtRetourMenu();
        }
    }

}
