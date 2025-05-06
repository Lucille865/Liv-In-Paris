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
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("╔════════════════════════ LIV'IN PARIS ═══════════════════════╗");
                Console.ResetColor();
                Console.WriteLine("1. Gérer les Clients");
                Console.WriteLine("2. Gérer les Cuisiniers");
                Console.WriteLine("3. Gérer les Commandes");
                Console.WriteLine("4. Gérer les Statistiques");
                Console.WriteLine("5. Quitter");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("╚═════════════════════════════════════════════════════════════╝");
                Console.ResetColor();
                Console.Write("\n=> Votre choix : ");


                var choix = Console.ReadLine();

                switch (choix)
                {
                    case "1": GérerClients(); break;
                    case "2": GérerCuisiniers(); break;
                    case "3": GérerCommandes(); break;
                    case "4": GérerStatistiques(); break;
                    case "5": continuer = false; break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nChoix invalide. Veuillez réessayer.\n");
                        Console.ResetColor();
                        break;
                }
            }
        }

        private void PauseEtRetourMenu()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\nAppuyez sur Entrée pour revenir au menu principal...");
            Console.ResetColor();
            Console.ReadLine();
            AfficherMenu();
        }

        private void GérerClients()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n--- GESTION DES CLIENTS ---");
            Console.ResetColor();
            Console.WriteLine("1. Ajouter un client");
            Console.WriteLine("2. Afficher tous les clients");
            Console.WriteLine("3. Supprimer un client");
            Console.WriteLine("4. Retour");
            Console.Write("\nVotre choix : ");

            var choix = Console.ReadLine();

            switch (choix)
            {
                case "1": AjouterClient(); break;
                case "2": AfficherClients(); break;
                case "3": SupprimerClient(); break;
                case "4": AfficherMenu(); break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nChoix invalide.");
                    Console.ResetColor();
                    PauseEtRetourMenu();
                    break;
            }
        }

        private void AjouterClient()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("=== Ajout d'un nouveau client ===");
            Console.ResetColor();

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
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Client ajouté avec succès.");
            Console.ResetColor();
            PauseEtRetourMenu();
        }

        private void AfficherClients()
        {
            gestionClients.AfficherClients();
            PauseEtRetourMenu();
        }

        public void SupprimerClient()
        {
            Console.Write("Entrez l'identifiant (pseudo) du client à supprimer : ");
            string identifiant = Console.ReadLine();

            string query = "DELETE FROM Clients WHERE Identifiant = @Identifiant";
            int lignes = DatabaseManager.ExecuteNonQuery(query, new MySqlParameter("@Identifiant", identifiant));

            if (lignes > 0)
                Console.WriteLine($"Client '{identifiant}' supprimé avec ses commandes.");
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Aucun client trouvé avec cet identifiant.");
                Console.ResetColor();
            }
        }

        private void GérerCuisiniers()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n--- GESTION DES CUISINIERS ---");
            Console.ResetColor();
            Console.WriteLine("1. Ajouter un cuisinier");
            Console.WriteLine("2. Afficher tous les cuisiniers");
            Console.WriteLine("3. Supprimer des cuisiniers");
            Console.WriteLine("4. Retour");
            Console.Write("\nVotre choix : ");

            var choix = Console.ReadLine();

            switch (choix)
            {
                case "1": AjouterCuisinier(); break;
                case "2": AfficherCuisiniers(); break;
                case "3": SupprimerCuisinier(); break;
                case "4": AfficherMenu(); break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nChoix invalide.");
                    Console.ResetColor();
                    PauseEtRetourMenu();
                    break;
            }
        }

        private void AjouterCuisinier()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("=== Ajout d'un nouveau cuisinier ===");
            Console.ResetColor();

            Console.Write("Nom : ");
            string nom = Console.ReadLine();
            Console.Write("Prénom : ");
            string adresse = Console.ReadLine();
            Console.Write("Téléphone : ");
            string telephone = Console.ReadLine();
            Console.Write("Email : ");
            string email = Console.ReadLine();
            Console.Write("Spécialité : ");
            string specialite = Console.ReadLine();
            Console.Write("Identifiant (pseudo) : ");
            string identifiant = Console.ReadLine();
            Console.Write("Mot de passe : ");
            string mdp = Console.ReadLine();

            var cuisinier = new Cuisinier(nom, adresse, telephone, email, identifiant, mdp);
            gestionCuisiniers.AjouterCuisinier(cuisinier);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Cuisinier ajouté avec succès.");
            Console.ResetColor();
            PauseEtRetourMenu();
        }

        private void AfficherCuisiniers()
        {
            gestionCuisiniers.AfficherCuisiniers();
            PauseEtRetourMenu();
        }

        private void SupprimerCuisinier()
        {
            Console.Write("Entrez l'identifiant (pseudo) du cuisinier à supprimer : ");
            string identifiant = Console.ReadLine();

            string query = "DELETE FROM Cuisiniers WHERE Identifiant = @Identifiant";
            int lignes = DatabaseManager.ExecuteNonQuery(query, new MySqlParameter("@Identifiant", identifiant));

            if (lignes > 0)
                Console.WriteLine($"Cuisinier '{identifiant}' supprimé avec ses commandes.");
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Aucun cuisinier trouvé avec cet identifiant.");
                Console.ResetColor();
            }

            PauseEtRetourMenu();
        }

        private void GérerCommandes()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\n--- COMMANDES ---");
            Console.ResetColor();
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
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Choix invalide.");
                    Console.ResetColor();
                    PauseEtRetourMenu();
                    break;
            }
        }

        private void CreerCommande()
        {
            Console.Write("Entrez votre identifiant de client : ");
            string clientId = Console.ReadLine();

            Client client = gestionClients.clients.FirstOrDefault(c => c.Identifiant == clientId);

            if (client == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Aucun client trouvé avec cet identifiant.");
                Console.ResetColor();
                return;
            }

            gestionCommandes.CreerCommande(client);

            string cheminStations = "MetroParisNoeuds.txt";
            string cheminLiaisons = "MetroParisArcs.txt";
            graphe.ChargerStationsDepuisFichier(cheminStations);
            graphe.ChargerLiaisonsDepuisFichier(cheminLiaisons);

            Cuisinier cuisinier = gestionCuisiniers.AssignerCuisinierRandom();
            Console.WriteLine($"Commande créée pour {client.Nom}. Cuisinier assigné : {cuisinier.Nom}");

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
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Impossible de calculer le plus court chemin à cause d'un cycle de poids négatif.");
                Console.ResetColor();
            }

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
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("\n--- STATISTIQUES ---");
                Console.ResetColor();
                Console.WriteLine("1. Chiffre d'affaires par cuisinier");
                Console.WriteLine("2. Plats les plus populaires");
                Console.WriteLine("3. Nombre moyen de commandes par client");
                Console.WriteLine("4. Graphe des relations");
                Console.WriteLine("5. Exportation en XML");
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
                        Console.WriteLine("Données exportées");
                        break;
                    case "6":
                        return;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Option invalide");
                        Console.ResetColor();
                        PauseEtRetourMenu();
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
