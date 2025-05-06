using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
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
        private Autre autre;

        public Interface()
        {
            gestionClients = new GestionClients();
            gestionCuisiniers = new GestionCuisiniers();
            gestionCommandes = new GestionCommandes(gestionClients, gestionCuisiniers);
            visualiseur = new RelationVisualizer(gestionClients, gestionCuisiniers);
            autre = new Autre();
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
                Console.WriteLine("5. Autre");
                Console.WriteLine("6. Quitter");
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
                    case "5": Autre(); break;
                    case "6": continuer = false; break;
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
            Console.WriteLine("3. Modifier un client");
            Console.WriteLine("4. Supprimer un client");
            Console.WriteLine("5. Retour");
            Console.Write("\nVotre choix : ");

            var choix = Console.ReadLine();
            List<Client> clients = gestionClients.GetClients();
            List<Commande> commandes = gestionCommandes.GetCommandes();

            switch (choix)
            {
                case "1": AjouterClient(); break;
                case "2": AfficherClients(); break;
                case "3": ModifierClient(); break; 
                case "4": SupprimerClient(); break;
                case "5": AfficherMenu(); break;
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

        private void ModifierClient()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n--- MODIFICATION CLIENT ---");
            Console.ResetColor();

            Console.Write("\nEntrez l'identifiant du client à modifier : ");
            string identifiant = Console.ReadLine();

            Client client = gestionClients.GetClientParIdentifiant(identifiant);

            if (client == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Client non trouvé.");
                Console.ResetColor();
                PauseEtRetourMenu();
                return;
            }

            // Créer une copie pour modification
            Client modifications = new Client(
                client.Nom,
                client.Adresse,
                client.Telephone,
                client.Email,
                client.Identifiant,
                client.MotDePasse,
                client.TypeClient)
            {
                Prenom = client.Prenom,
                NomEntreprise = client.NomEntreprise,
                Referent = client.Referent
            };

            Console.WriteLine("\nQue souhaitez-vous modifier ? (laisser vide pour ne pas changer)");

            Console.Write($"Nom ({client.Nom}) : ");
            string input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input)) modifications.Nom = input;

            Console.Write($"Prénom ({client.Prenom ?? "N/A"}) : ");
            input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input)) modifications.Prenom = input;

            Console.Write($"Adresse ({client.Adresse}) : ");
            input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input)) modifications.Adresse = input;

            Console.Write($"Téléphone ({client.Telephone}) : ");
            input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input)) modifications.Telephone = input;

            Console.Write($"Email ({client.Email}) : ");
            input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input)) modifications.Email = input;

            Console.Write($"Mot de passe : ");
            input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input)) modifications.MotDePasse = input;

            // Modification du type client
            Console.WriteLine($"Type actuel : {client.TypeClient}");
            Console.WriteLine("Changer le type ? (O/N)");
            if (Console.ReadLine().Equals("O", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("1. Particulier");
                Console.WriteLine("2. Entreprise");
                Console.Write("Choix : ");
                modifications.TypeClient = Console.ReadLine() == "2" ? "Entreprise" : "Particulier";
            }

            // Gestion des champs entreprise si nécessaire
            if (modifications.TypeClient == "Entreprise")
            {
                Console.Write($"Nom entreprise ({client.NomEntreprise ?? "N/A"}) : ");
                input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input)) modifications.NomEntreprise = input;

                Console.Write($"Référent ({client.Referent ?? "N/A"}) : ");
                input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input)) modifications.Referent = input;
            }

            // Appliquer les modifications
            gestionClients.ModifierClient(identifiant, modifications);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nClient modifié avec succès !");
            Console.ResetColor();

            PauseEtRetourMenu();
        }

        public void SupprimerClient()
        {
            Console.Clear();
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
            Console.WriteLine("3. Modifier un cuisinier");
            Console.WriteLine("4. Supprimer des cuisiniers");
            Console.WriteLine("5. Afficher les plats par fréquence");
            Console.WriteLine("6. Afficher le plat du jour");
            Console.WriteLine("7. Afficher les clients servis");
            Console.WriteLine("8. Retour");
            Console.Write("\nVotre choix : ");

            var choix = Console.ReadLine();

            switch (choix)
            {
                case "1": AjouterCuisinier(); break;
                case "2": AfficherCuisiniers(); break;
                case "3": ModifierCuisinier(); break;
                case "4": SupprimerCuisinier(); break;
                case "5": AfficherPlatsParFrequence(); break;
                case "6": AfficherPlatDuJour(); break;
                case "7": AfficherClientsServis(); break;
                case "8": AfficherMenu(); break;
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

        private void ModifierCuisinier()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n--- MODIFICATION CUISINIER ---");
            Console.ResetColor();

            // Afficher la liste des cuisiniers pour référence
            gestionCuisiniers.AfficherCuisiniers();

            Console.Write("\nEntrez le pseudo du cuisinier à modifier : ");
            string identifiant = Console.ReadLine();

            Cuisinier cuisinier = gestionCuisiniers.GetCuisinierByIdentifiant(identifiant);

            if (cuisinier == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Cuisinier non trouvé.");
                Console.ResetColor();
                PauseEtRetourMenu();
                return;
            }

            // Créer une copie pour modification
            Cuisinier modifications = new Cuisinier(
                cuisinier.Identifiant,
                cuisinier.Nom,
                cuisinier.Adresse,
                cuisinier.Telephone,
                cuisinier.Email,
                cuisinier.MotDePasse)
            {
                Plats = cuisinier.Plats // Conserve la liste des plats existante
            };

            Console.WriteLine("\nQue souhaitez-vous modifier ? (laisser vide pour ne pas changer)");

            Console.Write($"Pseudo ({cuisinier.Identifiant}) : ");
            string input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input)) modifications.Identifiant = input;

            Console.Write($"Nom ({cuisinier.Nom}) : ");
            input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input)) modifications.Nom = input;

            Console.Write($"Adresse ({cuisinier.Adresse}) : ");
            input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input)) modifications.Adresse = input;

            Console.Write($"Téléphone ({cuisinier.Telephone}) : ");
            input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input)) modifications.Telephone = input;

            Console.Write($"Email ({cuisinier.Email}) : ");
            input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input)) modifications.Email = input;

            Console.Write($"Mot de passe : ");
            input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input)) modifications.MotDePasse = input;

            // Gestion des plats (optionnel)
            Console.WriteLine("\nGérer les plats ? (O/N)");
            if (Console.ReadLine().Equals("O", StringComparison.OrdinalIgnoreCase))
            {
                Console.Clear();
                GererPlatsCuisinier(modifications);
            }

            // Appliquer les modifications
            gestionCuisiniers.ModifierCuisinier(identifiant, modifications);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nCuisinier modifié avec succès !");
            Console.ResetColor();

            PauseEtRetourMenu();
        }

        private void GererPlatsCuisinier(Cuisinier cuisinier)
        {
            Console.WriteLine("\n--- GESTION DES PLATS ---");
            Console.WriteLine("1. Ajouter un plat");
            Console.WriteLine("2. Supprimer un plat");
            Console.WriteLine("3. Voir la liste des plats");
            Console.WriteLine("4. Terminer");
            Console.Write("Choix : ");

            string choix = Console.ReadLine();
            Console.Clear();

            switch (choix)
            {
                case "1":
                    bool ajouterPlats = true;
                    while (ajouterPlats)
                    {
                        Console.WriteLine("\n=== Ajout d'un plat ===");
                        Console.Write("Nom du plat : ");
                        string nomPlat = Console.ReadLine();

                        Console.Write("Type (Entrée/Plat/Dessert) : ");
                        string type = Console.ReadLine();

                        Console.Write("Prix par personne : ");
                        decimal prix = decimal.Parse(Console.ReadLine());

                        Console.Write("Nationalité : ");
                        string nationalite = Console.ReadLine();

                        Console.Write("Régime alimentaire : ");
                        string regime = Console.ReadLine();

                        List<string> ingredients = new List<string>();
                        Console.WriteLine("\nAjout des ingrédients (entrez 'fin' pour terminer) :");
                        while (true)
                        {
                            Console.Write("Ingrédient : ");
                            string ingredient = Console.ReadLine();
                            if (ingredient.ToLower() == "fin")
                                break;
                            ingredients.Add(ingredient);
                        }

                        DateTime dateFabrication = DateTime.Now;
                        DateTime datePeremption = dateFabrication.AddDays(30);

                        // Création du plat
                        Plat nouveauPlat = new Plat(
                            nomPlat,
                            type,
                            1,
                            DateTime.Now,
                            DateTime.Now.AddDays(2),
                            prix,
                            nationalite,
                            regime,
                            ingredients
                        );

                        cuisinier.Plats.Add(nouveauPlat);

                        // Demande si on continue
                        Console.Write("Ajouter un autre plat? (o/n) ");
                        ajouterPlats = Console.ReadLine().ToLower() == "o";
                    }
                    break;

                case "2":
                    Console.WriteLine("Plats disponibles :");
                    for (int i = 0; i < cuisinier.Plats.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {cuisinier.Plats[i].Nom}");
                    }
                    Console.Write("Numéro du plat à supprimer : ");
                    int index = int.Parse(Console.ReadLine()) - 1;
                    if (index >= 0 && index < cuisinier.Plats.Count)
                    {
                        cuisinier.Plats.RemoveAt(index);
                    }
                    break;

                case "3":
                    Console.WriteLine("Liste des plats :");
                    foreach (var plat in cuisinier.Plats)
                    {
                        Console.WriteLine($"- {plat.Nom} ({plat.Type}), Prix: {plat.PrixParPersonne}€");
                    }
                    break;
            }

            if (choix != "4")
            {
                GererPlatsCuisinier(cuisinier); // Rappel récursif
            }
        }
        private void SupprimerCuisinier()
        {
            Console.Clear();
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

        private void AfficherPlatsParFrequence()
        {
            Console.Clear();
            Console.Write("Entrez l'identifiant (pseudo) du cuisinier : ");
            string identifiant = Console.ReadLine();

            var cuisinier = gestionCuisiniers.GetCuisinierByIdentifiant(identifiant); // Méthode à implémenter dans GestionCuisiniers
            if (cuisinier != null)
            {
                cuisinier.AfficherPlatsParFrequence();  // Appel de la méthode sur l'objet cuisinier
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Cuisinier non trouvé.");
                Console.ResetColor();
            }

            PauseEtRetourMenu();
        }

        private void AfficherPlatDuJour()
        {
            Console.Clear();
            Console.Write("Entrez l'identifiant (pseudo) du cuisinier : ");
            string identifiant = Console.ReadLine();

            var cuisinier = gestionCuisiniers.GetCuisinierByIdentifiant(identifiant); 
            if (cuisinier != null)
            {
                cuisinier.AfficherPlatDuJour();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Cuisinier non trouvé.");
                Console.ResetColor();
            }

            PauseEtRetourMenu();
        }

        /// <summary>
        /// Affiche les clients servis par le cuisinier.
        /// </summary>
        public void AfficherClientsServis()
        {
            Console.Clear();
            Console.Write("Entrez l'identifiant du cuisinier : ");
            string identifiant = Console.ReadLine();

            var cuisinier = gestionCuisiniers.GetCuisinierByIdentifiant(identifiant);
            if (cuisinier != null)
            {
                var commandesCuisinier = gestionCommandes.GetCommandes()
                    .Where(c => c.Cuisinier?.Identifiant == identifiant).ToList();

                if (commandesCuisinier.Any())
                {
                    Console.WriteLine("Clients servis :");
                    foreach (var commande in commandesCuisinier)
                    {
                        Console.WriteLine($"- Client: {commande.Client.Nom}, Plat: {commande.LignesCommande.First().Plat.Nom}");
                    }
                }
                else
                {
                    Console.WriteLine("Ce cuisinier n'a pas servi de clients.");
                }
            }
            else
            {
                Console.WriteLine("Cuisinier introuvable.");
            }

            Console.WriteLine("\nAppuyez sur une touche pour revenir au menu...");
            Console.ReadKey();
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
            Console.Clear();

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
            Console.Clear();
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
                Console.WriteLine("4. Afficher le graphe des stations");
                Console.WriteLine("5. Graphe des relations");
                Console.WriteLine("6. Exportation en XML");
                Console.WriteLine("7. Revenir au menu principal");
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
                        var graphe = new AfficheGraphe<int>();
                        graphe.ChargerDepuisFichier("MetroParisNoeuds.txt", "MetroParisArcs.txt", int.Parse);
                        graphe.DessinerGraphe("metro.png");
                        break;
                    case "5":
                        visualiseur.GenererGrapheComplet(gestionCommandes.GetCommandes());
                        Process.Start("explorer.exe", "relations.png");
                        break;
                    case "6":
                        var exportateur = new Exportateur();
                        exportateur.ExporterToutesLesDonnees();
                        Process.Start("explorer.exe", "relations.png");
                        Console.WriteLine("Données exportées");
                        break;
                    case "7":
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
            Console.Clear();
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
            Console.Clear();
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
            Console.Clear();
            Console.WriteLine("\n=== MOYENNE DE COMMANDES PAR CLIENT ===");
            decimal moyenne = stats.GetMoyenneCommandesParClient();
            Console.WriteLine($"\nChaque client a effectué en moyenne {moyenne:N2} commandes");
            PauseEtRetourMenu();
        }

        private void Autre()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n--- AUTRE ---");
            Console.ResetColor();
            Console.WriteLine("1. Créer une commande");
            Console.WriteLine("2. Afficher les clients inactifs");
            Console.WriteLine("3. Vérifier les retards");
            Console.WriteLine("4. Retour");
            var choix = Console.ReadLine();
            List<Client> clients = gestionClients.GetClients();
            List<Commande> commandes = gestionCommandes.GetCommandes();
            Console.Clear();

            switch (choix)
            {
                case "1":
                    CreerCommande();
                    break;
                case "2":
                    autre.AfficherClientsInactifs(clients, commandes);
                    PauseEtRetourMenu();
                    break;
                case "3":
                    var commande = new Commande(new Client(), new Cuisinier()); // Exemple de commande
                    commande.Date = DateTime.Now.AddMinutes(-90); // Exemple: commande passée il y a 90 minutes
                    commande.Statut = "En attente"; // Statut de la commande
                    // Vérification du retard avec un délai de livraison de 2 heures
                    string resultat = autre.VerifierRetard(commande, TimeSpan.FromHours(2)); // Spécification du délai de 2h
                    Console.WriteLine(resultat);
                    PauseEtRetourMenu();
                    break;
                case "4":
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
    }
}
