using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Pb_scientifique
{
    public class GestionCommandes
    {

<<<<<<< Updated upstream
=======
        public GestionClients gestionClients;
        public GestionCuisiniers gestionCuisiniers;

        private const string filePath = "Commandes.txt";

        public GestionCommandes()
        {
            commandes = ChargerCommandes();
        }

        // Méthode pour ajouter une commande
>>>>>>> Stashed changes
        public void AjouterCommande(Commande commande)
        {
            string query = @"INSERT INTO Commandes 
                           (ClientId, CuisinierId, Statut, AdresseLivraison, StationLivraison) 
                           VALUES 
                           (@ClientId, @CuisinierId, @Statut, @Adresse, @Station)";

            DatabaseManager.ExecuteNonQuery(query,
                new MySqlParameter("@ClientId", commande.Client),
                new MySqlParameter("@CuisinierId", commande.Cuisinier),
                new MySqlParameter("@Statut", commande.Statut),
                new MySqlParameter("@Adresse", commande.AdresseLivraison),
                new MySqlParameter("@Station", commande.StationLivraison)
            );
        }

        public List<Commande> GetToutesCommandes()
        {
            var commandes = new List<Commande>();
            using (var conn = DatabaseManager.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand("SELECT * FROM Commandes", conn))
                using (var reader = cmd.ExecuteReader())
                {
<<<<<<< Updated upstream
                    while (reader.Read())
                    {
                        commandes.Add(new Commande
                        {
                            Commande_Id = Convert.ToInt32(reader["Commande_Id"]),
                            ClientId = Convert.ToInt32(reader["ClientId"]),
                            CuisinierId = Convert.ToInt32(reader["CuisinierId"]),
                            Statut = reader["Statut"].ToString(),
                            AdresseLivraison = reader["AdresseLivraison"].ToString(),
                            StationLivraison = reader["StationLivraison"].ToString(),
                            DateCommande = Convert.ToDateTime(reader["DateCommande"])
                        });
=======
                    string clientNom = commande.Client != null ? commande.Client.Nom : "Inconnu";
                    string cuisinierNom = commande.Cuisinier != null ? commande.Cuisinier.Nom : "Inconnu";

                    writer.WriteLine($"{commande.Id};{clientNom};{cuisinierNom};{commande.TotalPrix};{commande.Date}");

                    foreach (var ligne in commande.LignesCommande)
                    {
                        writer.WriteLine($"LIGNE;{ligne.Plat.Nom};{ligne.Quantite};{ligne.DateLivraison};{ligne.AdresseLivraison}");
                    }

                    writer.WriteLine("FIN_COMMANDE");
                }
            }
        }

        private List<Commande> ChargerCommandes()
        {
            List<Commande> listeCommandes = new List<Commande>();

            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    Commande commandeEnCours = null;
                    string ligne;

                    while ((ligne = reader.ReadLine()) != null)
                    {
                        var data = ligne.Split(';');

                        if (data[0] == "COMMANDE")
                        {
                            var client = gestionClients.clients.FirstOrDefault(c => c.Nom == data[2]);
                            var cuisinier = gestionCuisiniers.cuisiniers.FirstOrDefault(c => c.Nom == data[3]);

                            if (client != null && cuisinier != null)
                            {
                                commandeEnCours = new Commande(client, cuisinier)
                                {
                                    Id = int.Parse(data[1]),
                                    Date = DateTime.Parse(data[4])
                                };
                            }
                        }
                        else if (data[0] == "LIGNE" && commandeEnCours != null)
                        {
                            // Trouver le plat correspondant
                            var plat = gestionCuisiniers.cuisiniers
                                .SelectMany(c => c.Plats)
                                .FirstOrDefault(p => p.Nom == data[1]);

                            if (plat != null)
                            {
                                var ligneCommande = new LigneCommande
                                {
                                    Plat = plat,
                                    Quantite = int.Parse(data[2]),
                                    DateLivraison = DateTime.Parse(data[3]),
                                    AdresseLivraison = data[4]
                                };
                                commandeEnCours.LignesCommande.Add(ligneCommande);
                            }
                        }
                        else if (data[0] == "FIN_COMMANDE" && commandeEnCours != null)
                        {
                            listeCommandes.Add(commandeEnCours);
                            commandeEnCours = null;
                        }
>>>>>>> Stashed changes
                    }
                }
            }
            return commandes;
        }

<<<<<<< Updated upstream

=======
        public void CreerCommande(Client client)
        {
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
                Console.WriteLine($"{i + 1}. {platsDisponibles[i].Nom} ({platsDisponibles[i].PrixParPersonne}€) - {platsDisponibles[i].Type}");
            }

            Console.Write("\nChoisissez un plat (numéro) : ");
            if (int.TryParse(Console.ReadLine(), out int choixPlat) && choixPlat > 0 && choixPlat <= platsDisponibles.Count)
            {
                Plat platChoisi = platsDisponibles[choixPlat - 1];

                // Demander la quantité
                Console.Write("Quantité : ");
                if (!int.TryParse(Console.ReadLine(), out int quantite) || quantite <= 0)
                {
                    Console.WriteLine("Quantité invalide, valeur par défaut (1) utilisée.");
                    quantite = 1;
                }

                // Trouver les cuisiniers qui proposent ce plat
                var cuisiniersDisponibles = gestionCuisiniers.cuisiniers
                    .Where(c => c.Plats.Any(p => p.Nom == platChoisi.Nom))
                    .ToList();

                if (cuisiniersDisponibles.Count == 0)
                {
                    Console.WriteLine("Aucun cuisinier disponible pour ce plat.");
                    return;
                }

                // Assigner un cuisinier aléatoire
                Random random = new Random();
                Cuisinier cuisinier = cuisiniersDisponibles[random.Next(cuisiniersDisponibles.Count)];

                // Créer la commande
                Commande nouvelleCommande = new Commande(client, cuisinier);

                // Créer la ligne de commande avec toutes les informations
                var ligneCommande = new LigneCommande
                {
                    Plat = platChoisi,
                    Quantite = quantite,
                    DateLivraison = DateTime.Now.AddHours(2), // Livraison dans 2 heures par défaut
                    AdresseLivraison = client.Adresse
                };

                nouvelleCommande.LignesCommande.Add(ligneCommande);

                AjouterCommande(nouvelleCommande);

                Console.WriteLine($"\nCommande créée :");
                Console.WriteLine($"- Plat: {platChoisi.Nom}");
                Console.WriteLine($"- Quantité: {quantite}");
                Console.WriteLine($"- Cuisinier: {cuisinier.Nom}");
                Console.WriteLine($"- Date de livraison estimée: {ligneCommande.DateLivraison}");
            }
            else
            {
                Console.WriteLine("Choix invalide.");
            }
        }

        private List<Plat> GetPlatsDisponibles()
        {
            return gestionCuisiniers.cuisiniers.SelectMany(c => c.Plats).ToList();
        }
>>>>>>> Stashed changes
    }
}
