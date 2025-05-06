using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pb_scientifique
{
    public class GestionCommandes
    {
        public List<Commande> commandes = new List<Commande>();
        private const string filePath = "Commandes.txt";
        private readonly GestionClients gestionClients;
        private readonly GestionCuisiniers gestionCuisiniers;
        Graphe<int> graphe = new Graphe<int>();

        /// <summary>
        /// Initialise le gestionnaire de commandes avec les gestionnaires de clients et cuisiniers.
        /// Charge également les commandes depuis un fichier au démarrage.
        /// </summary>
        public GestionCommandes(GestionClients gestionClients, GestionCuisiniers gestionCuisiniers)
        {
            this.gestionClients = gestionClients;
            this.gestionCuisiniers = gestionCuisiniers;
            commandes = ChargerCommandes();
        }

        /// <summary>
        /// Récupère la liste des commandes en cours.
        /// </summary>
        public List<Commande> GetCommandes()
        {
            return commandes; // Retourne la liste interne des commandes
        }

        /// <summary>
        /// Permet de créer une commande pour un client, ajoutant des plats et leur gestion.
        /// </summary>
        public void CreerCommande(Client client)
        {
            Commande nouvelleCommande = new Commande(client, null);
            decimal prixTotal = 0;
            bool premierPlat = true;
            Cuisinier cuisinierPrincipal = null;

            while(true)
            {
                Console.Clear();
                Console.WriteLine("=== AJOUT D'UN PLAT ===");
                // Afficher tous les plats disponibles avec leurs cuisiniers
                var platsDisponibles = GetPlatsDisponibles();

                if (platsDisponibles.Count == 0)
                {
                    Console.WriteLine("Aucun plat disponible pour le moment.");
                    return;
                }

                Console.WriteLine("\nPlats disponibles :");
                for (int i = 0; i < platsDisponibles.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {platsDisponibles[i].Nom} ({platsDisponibles[i].PrixParPersonne} euros) - {platsDisponibles[i].Type}");
                }

                Console.Write("\nChoisissez un plat avec son numéro (numéro 0 pour terminer) : ");
                if (int.TryParse(Console.ReadLine(), out int choixPlat) && choixPlat > 0 && choixPlat <= platsDisponibles.Count)
                {
                    if (choixPlat == 0) break;

                    Plat platChoisi = platsDisponibles[choixPlat - 1];

                    // Demander la quantité
                    Console.Write("Quantité : ");
                    int quantite = Convert.ToInt32(Console.ReadLine());
                    if (quantite <= 0)
                    {
                        Console.WriteLine("Quantité invalide, valeur par défaut (1) utilisée.");
                        quantite = 1;
                    }

                    decimal prixParLigne = platChoisi.PrixParPersonne * quantite;
                    Console.WriteLine($"\nPrix à payer pour ce plat : {prixParLigne} euros");

                    // Choix de l'adresse de livraison
                    Console.WriteLine("\nOptions de livraison :");
                    Console.WriteLine($"1. Livraison à l'adresse enregistrée ({client.Adresse})");
                    Console.WriteLine("2. Livraison à une autre adresse");
                    Console.Write("Votre choix : ");

                    string adresseLivraison = client.Adresse;
                    if (Console.ReadLine() == "2")
                    {
                        Console.Write("Entrez l'adresse complète de livraison : ");
                        adresseLivraison = Console.ReadLine();
                    }

                    // Choix de la date de livraison
                    Console.WriteLine("\nDate de livraison :");
                    Console.WriteLine("1. Livraison maintenant (dans 2 heures)");
                    Console.WriteLine("2. Livraison à une date/heure spécifique");
                    Console.Write("Votre choix : ");

                    DateTime dateLivraison = DateTime.Now.AddHours(2);
                    if (Console.ReadLine() == "2")
                    {
                        Console.Write("Date souhaitée (JJ/MM/AAAA HH:MM) : ");
                        if (DateTime.TryParse(Console.ReadLine(), out DateTime dateSaisie))
                        {
                            dateLivraison = dateSaisie;
                        }
                        else
                        {
                            Console.WriteLine("Format invalide, livraison dans 2 heures par défaut.");
                        }
                    }

                    // Assignation cuisinier (on garde le premier assigné pour toute la commande)
                    if (premierPlat)
                    {
                        cuisinierPrincipal = gestionCuisiniers.TrouverCuisinierPourPlat(platChoisi.Nom);
                        if (cuisinierPrincipal == null)
                        {
                            Console.WriteLine("Aucun cuisinier disponible.");
                            return;
                        }
                        nouvelleCommande.Cuisinier = cuisinierPrincipal;
                        premierPlat = false;
                    }

                    // Ajout de la ligne
                    var ligne = new LigneCommande
                    {
                        Plat = platChoisi,
                        Quantite = quantite,
                        AdresseLivraison = adresseLivraison,
                        DateLivraison = dateLivraison
                    };

                    nouvelleCommande.LignesCommande.Add(ligne);
                    prixTotal += prixParLigne;

                    Console.WriteLine($"\nPlat ajouté : {quantite}x {platChoisi.Nom} pour {adresseLivraison}");
                    Console.WriteLine($"Total actuel : {prixTotal} euros");
                    Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                    Console.ReadKey();
                }

                // Validation finale
                if (nouvelleCommande.LignesCommande.Any())
                {
                    Console.Clear();
                    Console.WriteLine("=== RÉCAPITULATIF FINAL ===");
                    Console.WriteLine($"Client : {client.Nom}");
                    Console.WriteLine($"Cuisinier : {nouvelleCommande.Cuisinier.Nom}");

                    foreach (var ligne in nouvelleCommande.LignesCommande)
                    {
                        Console.WriteLine($"\n- {ligne.Quantite}x {ligne.Plat.Nom}");
                        Console.WriteLine($"  Adresse : {ligne.AdresseLivraison}");
                        Console.WriteLine($"  Livraison : {ligne.DateLivraison:dd/MM/yyyy HH:mm}");
                        Console.WriteLine($"  Prix : {ligne.Plat.PrixParPersonne * ligne.Quantite}€");
                    }

                    Console.WriteLine($"\nTOTAL À PAYER : {prixTotal} euros");
                    Console.Write("\nConfirmer la commande ? (o/n) ");

                    if (Console.ReadLine().ToLower() == "o")
                    {
                        AjouterCommande(nouvelleCommande);
                        Console.WriteLine("\n✅ Commande validée !");

                        // 1. Afficher le récapitulatif final
                        Console.WriteLine("\n=== ITINÉRAIRE DE LIVRAISON ===");
                        Console.WriteLine($"Cuisinier : {nouvelleCommande.Cuisinier.Nom}");
                        Console.WriteLine($"Adresse cuisinier : {nouvelleCommande.Cuisinier.Adresse}");

                        // 2. Calculer et afficher les chemins pour chaque destination unique
                        var destinationsUniques = nouvelleCommande.LignesCommande
                            .Select(l => l.AdresseLivraison)
                            .Distinct();

                        foreach (var adresse in destinationsUniques)
                        {
                            Console.WriteLine($"\n--- Livraison à : {adresse} ---");
                            AfficherPlusCourtChemin(nouvelleCommande.Cuisinier.Adresse, adresse);
                        }
                        return; // Retour immédiat au menu après affichage
                    }
                    else
                    {
                        Console.WriteLine("Commande annulée.");
                    }
                }
                else
                {
                    Console.WriteLine("Aucun plat ajouté.");
                }
            }
            
        }

        /// <summary>
        /// Affiche le trajet le plus court entre le cuisinier et une destination donnée.
        /// Utilise le graphe de métro pour calculer et afficher le chemin.
        /// </summary>
        /// <param name="depart">L'adresse de départ (cuisinier).</param>
        /// <param name="arrivee">L'adresse d'arrivée (destination).</param>
        private void AfficherPlusCourtChemin(string depart, string arrivee)
        {
            try
            {
                // 1. Initialisation du graphe
                graphe.ChargerStationsDepuisFichier("MetroParisNoeuds.txt");
                graphe.ChargerLiaisonsDepuisFichier("MetroParisArcs.txt");

                // 2. Trouver les IDs des stations
                int idDepart = graphe.TrouverIdParNom(depart);
                int idArrivee = graphe.TrouverIdParNom(arrivee);

                if (idDepart == -1 || idArrivee == -1)
                {
                    Console.WriteLine("Erreur : Stations non trouvées");
                    return;
                }

                // 3. Calcul du chemin
                var bellmanFord = new BellmanFord<int>(graphe);
                bool succes = bellmanFord.CalculerPlusCourtChemin(idDepart);

                if (succes)
                {
                    var chemin = bellmanFord.GetChemin(idArrivee);
                    Console.WriteLine($"Trajet de {depart} à {arrivee} :");

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
                    Console.WriteLine("Aucun chemin trouvé");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur de calcul d'itinéraire : {ex.Message}");
            }
        }

        /// <summary>
        /// Récupère tous les plats disponibles dans les cuisiniers enregistrés.
        /// </summary>
        /// <returns>Liste des plats disponibles.</returns
        private List<Plat> GetPlatsDisponibles()
        {
            List<Plat> platsDisponibles = new List<Plat>();

            // Parcours de chaque cuisinier
            foreach (var cuisinier in gestionCuisiniers.cuisiniers)
            {
                // Ajoute les plats du cuisinier à la liste des plats disponibles
                foreach (var plat in cuisinier.Plats)
                {
                    platsDisponibles.Add(plat);
                }
            }

            return platsDisponibles;
        }

        /// <summary>
        /// Ajoute une nouvelle commande à la liste et la sauvegarde dans le fichier.
        /// </summary>
        /// <param name="commande">La commande à ajouter.</param>
        public void AjouterCommande(Commande commande)
        {
            commandes.Add(commande);
            SauvegarderCommandes();
            Console.WriteLine("Commande ajoutée.");
        }

        /// <summary>
        /// Affiche la commande
        /// </summary>
        public void AfficherCommandes()
        {
            if (commandes.Count == 0)
            {
                Console.WriteLine("Aucune commande à afficher.");
                return;
            }

            foreach (var commande in commandes)
            {
                string clientNom = commande.Client != null ? commande.Client.Nom : "Inconnu";
                string cuisinierNom = commande.Cuisinier != null ? commande.Cuisinier.Nom : "Inconnu";

                Console.WriteLine($"Commande ID: {commande.Id}, Client: {clientNom}, Cuisinier: {cuisinierNom}, Total: {commande.TotalPrix} euros, Date: {commande.Date}");
            }
        }

        /// <summary>
        /// Sauvegarde la liste des commandes dans un fichier.
        /// </summary>
        private void SauvegarderCommandes()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath, false)) // false pour écraser le fichier
                {
                    foreach (var commande in commandes)
                    {
                        // Écrire l'en-tête de la commande
                        writer.WriteLine($"COMMANDE;{commande.Id};{commande.Client.Nom};{commande.Cuisinier.Nom};{commande.Date:yyyy-MM-dd HH:mm}");

                        // Écrire chaque ligne de commande
                        foreach (var ligne in commande.LignesCommande)
                        {
                            writer.WriteLine($"LIGNE;{ligne.Plat.Nom};{ligne.Quantite};{ligne.DateLivraison:yyyy-MM-dd HH:mm};{ligne.AdresseLivraison}");
                        }

                        writer.WriteLine("END_COMMANDE"); // Marqueur de fin
                    }
                }
                Console.WriteLine("Sauvegarde des commandes réussie."); // Message de debug
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la sauvegarde : {ex.Message}");
            }
        }

        // <summary>
        /// Charge la liste des commandes depuis le fichier de sauvegarde.
        /// </summary>
        /// <returns>Liste des commandes chargées.</returns>
        public List<Commande> ChargerCommandes()
        {
            List<Commande> loadedCommandes = new List<Commande>();

            if (!File.Exists(filePath))
            {
                return loadedCommandes;
            }

            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    Commande currentCommande = null;
                    string line;

                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.StartsWith("COMMANDE;"))
                        {
                            string[] data = line.Split(';');
                            Client client = gestionClients.clients.FirstOrDefault(c => c.Nom == data[2]);
                            Cuisinier cuisinier = gestionCuisiniers.cuisiniers.FirstOrDefault(c => c.Nom == data[3]);

                            if (client != null && cuisinier != null)
                            {
                                currentCommande = new Commande(client, cuisinier)
                                {
                                    Id = int.Parse(data[1]),
                                    Date = DateTime.Parse(data[4])
                                };
                            }
                        }
                        else if (line.StartsWith("LIGNE;") && currentCommande != null)
                        {
                            string[] data = line.Split(';');
                            Plat plat =GetPlatsDisponibles().FirstOrDefault(p => p.Nom == data[1]);

                            if (plat != null)
                            {
                                currentCommande.LignesCommande.Add(new LigneCommande
                                {
                                    Plat = plat,
                                    Quantite = int.Parse(data[2]),
                                    DateLivraison = DateTime.Parse(data[3]),
                                    AdresseLivraison = data[4]
                                });
                            }
                        }
                        else if (line == "END_COMMANDE" && currentCommande != null)
                        {
                            loadedCommandes.Add(currentCommande);
                            currentCommande = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du chargement : {ex.Message}");
            }

            return loadedCommandes;
        }
    }
}