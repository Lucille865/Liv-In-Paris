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
        public GestionClients gestionClients;
        public GestionCuisiniers gestionCuisiniers;
        public GestionCommandes gestionCommandes;
        Graphe<int> graphe = new Graphe<int>();

        public Interface()
        {
            gestionClients = new GestionClients();
            gestionCuisiniers = new GestionCuisiniers();
            gestionCommandes = new GestionCommandes();
        }

        public void AfficherMenu()
        {
            Console.Clear();
            bool continuer = true;
            while(continuer)
            {
                Console.WriteLine("Bienvenue dans l'application Liv'in Paris !");
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
            Console.WriteLine("Nom du cuisinier : ");
            string nom = Console.ReadLine();
            Console.WriteLine("Métro le plus proche de l'adresse du cuisinier : ");
            string adresse = Console.ReadLine();
            Console.WriteLine("Numéro de téléphone : ");
            string telephone = Console.ReadLine();
            Console.WriteLine("Email du cuisinier : ");
            string email = Console.ReadLine();
            Console.WriteLine("Pseudo du cuisinier : ");
            string pseudo = Console.ReadLine();
            Console.WriteLine("Mot de passe du cuisinier : ");
            string mdp = Console.ReadLine();

            var cuisinier = new Cuisinier(nom, adresse, telephone, email, pseudo, mdp);
            gestionCuisiniers.AjouterCuisinier(cuisinier);
            Console.WriteLine("Cuisinier ajouté avec succès.");
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

        private void AfficherCommandes()
        {
            gestionCommandes.AfficherCommandes();
            PauseEtRetourMenu();
        }

        private void GérerStatistiques()
        {
            // Logique pour afficher des statistiques
            Console.WriteLine("Statistiques à venir...");
            AfficherMenu();
        }
    }

}
