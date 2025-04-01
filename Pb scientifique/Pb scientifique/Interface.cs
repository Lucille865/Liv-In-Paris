using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pb_scientifique
{
    public class Interface
    {
        private GestionClients gestionClients;
        private GestionCuisiniers gestionCuisiniers;
        private GestionCommandes gestionCommandes;

        public Interface()
        {
            gestionClients = new GestionClients();
            gestionCuisiniers = new GestionCuisiniers();
            gestionCommandes = new GestionCommandes();
        }

        public void AfficherMenu()
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
                    break;
            }
        }


        private void GérerClients()
        {
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
            Console.WriteLine("Adresse du client : ");
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
            Console.WriteLine("Metro le plus proche de l'adresse du client : ");
            string metroclient = Console.ReadLine();

            var client = new Client(nom, adresse, telephone, email, pseudo, mdp, typeClient);
            gestionClients.AjouterClient(client);
            Console.WriteLine("Client ajouté avec succès.");
        }

        private void AfficherClients()
        {
            gestionClients.AfficherClients();
        }

        private void GérerCuisiniers()
        {
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
            Console.WriteLine("Adresse du cuisinier : ");
            string adresse = Console.ReadLine();
            Console.WriteLine("Numéro de téléphone : ");
            string telephone = Console.ReadLine();
            Console.WriteLine("Email du cuisinier : ");
            string email = Console.ReadLine();
            Console.WriteLine("Pseudo du cuisinier : ");
            string pseudo = Console.ReadLine();
            Console.WriteLine("Mot de passe du cuisinier : ");
            string mdp = Console.ReadLine();
            Console.WriteLine("Metro le plus prche du cuisinier : ");
            string metrocuisinier = Console.ReadLine();

            var cuisinier = new Cuisinier(nom, adresse, telephone, email, pseudo, mdp);
            gestionCuisiniers.AjouterCuisinier(cuisinier);
            Console.WriteLine("Cuisinier ajouté avec succès.");
        }

        private void AfficherCuisiniers()
        {
            gestionCuisiniers.AfficherCuisiniers();
        }

        private void GérerCommandes()
        {
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
            var commande = new Commande();
            gestionCommandes.AjouterCommande(commande);
            Console.WriteLine("Commande créée.");
        }

        private void AfficherCommandes()
        {
            gestionCommandes.AfficherCommandes();
        }

        private void GérerStatistiques()
        {
            // Logique pour afficher des statistiques
            Console.WriteLine("Statistiques à venir...");
            AfficherMenu();
        }
    }

}
