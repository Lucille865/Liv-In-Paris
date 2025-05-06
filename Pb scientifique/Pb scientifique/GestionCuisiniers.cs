using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Pb_scientifique
{
    /// <summary>
    /// Fournit des méthodes pour accéder aux données des plats, cuisiniers et ingrédients depuis la base de données LivInParis.
    /// </summary>
    public class GestionCuisiniers
    {
        public List<Cuisinier> cuisiniers = new List<Cuisinier>();
        Graphe<int> graphe = new Graphe<int>();

        public GestionCuisiniers()
        {
            cuisiniers = GetTousCuisiniers(); // Chargement depuis la BDD
        }

        /// <summary>
        /// Ajoute un nouveau cuisinier à la base de données ainsi que ses plats.
        /// </summary>
        /// <param name="cuisinier">Le cuisinier à ajouter.</param>
        public void AjouterCuisinier(Cuisinier cuisinier)
        {
            string query = @"INSERT INTO Cuisiniers 
                    (Nom, Adresse, Telephone, Email, Identifiant, 
                     MotDePasse) 
                    VALUES 
                    (@Nom, @Adresse, @Telephone, @Email, @Identifiant, 
                     @Mdp);
                     SELECT LAST_INSERT_ID();";  // Pour récupérer l'ID généré

            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Nom", cuisinier.Nom);
                        cmd.Parameters.AddWithValue("@Adresse", cuisinier.Adresse);
                        cmd.Parameters.AddWithValue("@Telephone", cuisinier.Telephone);
                        cmd.Parameters.AddWithValue("@Email", cuisinier.Email);
                        cmd.Parameters.AddWithValue("@Identifiant", cuisinier.Identifiant);
                        cmd.Parameters.AddWithValue("@Mdp", cuisinier.MotDePasse);

                        int cuisinierId = Convert.ToInt32(cmd.ExecuteScalar());  // Récupérer l'ID du cuisinier inséré

                        foreach (var plat in cuisinier.Plats)
                        {
                            AjouterPlat(cuisinierId, plat);  // Passer l'ID du cuisinier dans la méthode AjouterPlat
                        }
                    }
                }

                Console.WriteLine($"Cuisinier {cuisinier.Identifiant} ajouté avec succès");
                cuisiniers = GetTousCuisiniers();  // Recharger la liste des cuisiniers
            }
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                Console.WriteLine($"ERREUR: L'identifiant '{cuisinier.Identifiant}' existe déjà.");

            }
            AfficherCuisiniers();
        }

        /// <summary>
        /// Ajoute un plat associé à un cuisinier existant en base de données.
        /// </summary>
        /// <param name="cuisinierId">L'identifiant du cuisinier en base.</param>
        /// <param name="plat">Le plat à ajouter.</param>
        public void AjouterPlat(int cuisinierId, Plat plat)
        {
            string platQuery = @"INSERT INTO Plats 
                (Nom, Type, Portions, DateFabrication, DatePeremption,
                 PrixParPersonne, Nationalite, RegimeAlimentaire, CuisinierId) 
                VALUES 
                (@Nom, @Type, @Portions, @DateFab, @DatePeremp,
                 @Prix, @Nationalite, @Regime, @CuisinierId);
                SELECT LAST_INSERT_ID();";

            int platId = Convert.ToInt32(DatabaseManager.ExecuteScalar(platQuery,
                new MySqlParameter("@Nom", plat.Nom),
                new MySqlParameter("@Type", plat.Type),
                new MySqlParameter("@Portions", plat.Portions),
                new MySqlParameter("@DateFab", plat.DateFabrication),
                new MySqlParameter("@DatePeremp", plat.DatePeremption),
                new MySqlParameter("@Prix", plat.PrixParPersonne),
                new MySqlParameter("@Nationalite", plat.Nationalite),
                new MySqlParameter("@Regime", plat.RegimeAlimentaire),
                new MySqlParameter("@CuisinierId", cuisinierId)  // Utilisation de l'ID du cuisinier
            ));

            foreach (var ingredientNom in plat.Ingredients)
            {
                string checkQuery = "SELECT Id FROM Ingredients WHERE Nom = @Nom";
                var ingredientId = DatabaseManager.ExecuteScalar(checkQuery,
                    new MySqlParameter("@Nom", ingredientNom));

                if (ingredientId == null)
                {
                    string insertQuery = "INSERT INTO Ingredients (Nom) VALUES (@Nom); SELECT LAST_INSERT_ID();";
                    ingredientId = DatabaseManager.ExecuteScalar(insertQuery,
                        new MySqlParameter("@Nom", ingredientNom));
                }

                string linkQuery = "INSERT IGNORE INTO Plat_Ingredients (PlatId, IngredientId) VALUES (@PlatId, @IngredientId)";
                DatabaseManager.ExecuteNonQuery(linkQuery,
                    new MySqlParameter("@PlatId", platId),
                    new MySqlParameter("@IngredientId", ingredientId));
            }
        }


        /// <summary>
        /// Récupère la liste de tous les plats enregistrés en base.
        /// </summary>
        /// <returns>Liste des plats.</returns>
        public List<Plat> GetTousPlats()
        {
            var plats = new List<Plat>();
            using (var conn = DatabaseManager.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand("SELECT * FROM Plats", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var ingredientsJson = reader["Ingredients"].ToString();
                        var ingredientsList = JsonSerializer.Deserialize<List<string>>(ingredientsJson);

                        plats.Add(new Plat(
                            reader["Nom"].ToString(),
                            reader["Type"].ToString(),
                            Convert.ToInt32(reader["Portions"]),
                            Convert.ToDateTime(reader["DateFabrication"]),
                            Convert.ToDateTime(reader["DatePeremption"]),
                            Convert.ToDecimal(reader["PrixParPersonne"]),
                            reader["Nationalite"].ToString(),
                            reader["RegimeAlimentaire"].ToString(),
                            ingredientsList
                        ));
                    }
                }
            }

            return plats;
        }

        /// <summary>
        /// Crée un nouveau cuisinier et lui associe des plats saisis par l'utilisateur.
        /// </summary>
        /// <returns>Le cuisinier créé.</returns>
        public Cuisinier CreerCuisinierPlat()
        {
            // Infos de base
            Console.Write("Nom : ");
            string nom = Console.ReadLine();

            Console.Write("Métro le plus proche : ");
            string adresse = Console.ReadLine();

            Console.Write("Téléphone : ");
            string telephone = Console.ReadLine();

            Console.Write("Email : ");
            string email = Console.ReadLine();

            Console.Write("Identifiant : ");
            string identifiant = Console.ReadLine();

            Console.Write("Mot de passe : ");
            string mdp = Console.ReadLine();

            // Création du cuisinier
            var nouveauCuisinier = new Cuisinier(nom, adresse, telephone, email, identifiant, mdp);

            // Ajout des plats
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

                nouveauCuisinier.Plats.Add(nouveauPlat);

                // Demande si on continue
                Console.Write("Ajouter un autre plat? (o/n) ");
                ajouterPlats = Console.ReadLine().ToLower() == "o";
            }

            // Sauvegarde en BDD
            AjouterCuisinier(nouveauCuisinier);

            return nouveauCuisinier;
        }

        /// <summary>
        /// Récupère tous les cuisiniers avec leurs plats depuis la base de données.
        /// </summary>
        /// <returns>Liste des cuisiniers.</returns>
        public List<Cuisinier> GetTousCuisiniers()
        {
            var cuisiniers = new List<Cuisinier>();
            string query = @"SELECT c.*,p.Plat_Id as Plat_Id, p.Nom as PlatNom, p.Type as PlatType, 
                                p.PrixParPersonne as PlatPrix, p.Nationalite as PlatNationalite,
                                p.RegimeAlimentaire as PlatRegime
                                FROM Cuisiniers c
                                LEFT JOIN Plats p ON p.CuisinierId = Cuisinier_Id";
            using (var conn = DatabaseManager.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    Cuisinier currentCuisinier = null;
                    while (reader.Read())
                    {
                        string identifiant = reader["Identifiant"].ToString();

                        if (currentCuisinier == null || currentCuisinier.Identifiant != identifiant)
                        {
                            currentCuisinier = new Cuisinier(
                                reader["Nom"].ToString(),
                                reader["Adresse"].ToString(),
                                reader["Telephone"].ToString(),
                                reader["Email"].ToString(),
                                identifiant,
                                reader["MotDePasse"].ToString()
                            );
                            cuisiniers.Add(currentCuisinier);
                        }

                        // Ajouter le plat s'il existe
                        if (!string.IsNullOrEmpty(reader["PlatNom"]?.ToString()))
                        {
                            int platId = Convert.ToInt32(reader["Plat_Id"]);
                            var ingredients = GetIngredientsForPlat(platId);

                            var plat = new Plat(
                                reader["PlatNom"].ToString(),
                                reader["PlatType"].ToString(),
                                1, // Quantité par défaut
                                DateTime.Now,
                                DateTime.Now.AddDays(2),
                                Convert.ToDecimal(reader["PlatPrix"]),
                                reader["PlatNationalite"].ToString(),
                                reader["PlatRegime"].ToString(),
                                ingredients // cette fois tu passes bien la liste d'ingrédients
                            );

                            currentCuisinier.Plats.Add(plat);
                        }

                    }
                }
            }
            return cuisiniers;
        }

        /// <summary>
        /// Récupère les ingrédients associés à un plat donné.
        /// </summary>
        /// <param name="platId">Identifiant du plat.</param>
        /// <returns>Liste des ingrédients.</returns>
        private List<string> GetIngredientsForPlat(int platId)
        {
            var ingredients = new List<string>();

            string query = @"SELECT i.Nom 
                     FROM Ingredients i
                     JOIN Plat_Ingredients pi ON i.Id = pi.IngredientId
                     WHERE pi.PlatId = @PlatId";

            using (var conn = DatabaseManager.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@PlatId", platId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ingredients.Add(reader["Nom"].ToString());
                        }
                    }
                }
            }

            return ingredients;
        }


        /// <summary>
        /// Affiche la liste des cuisiniers et leurs plats dans la console.
        /// </summary>
        public void AfficherCuisiniers()
        {
            if (cuisiniers.Count == 0)
            {
                Console.WriteLine("Aucun cuisinier à afficher.");
                return;
            }

            Console.WriteLine("Liste des cuisiniers:");
            Console.WriteLine(new string('-', 50));

            foreach (var cuisinier in cuisiniers)
            {
                Console.WriteLine($"Nom: {cuisinier.Nom}, Adresse: {cuisinier.Adresse}, Email: {cuisinier.Email}");
                if (cuisinier.Plats.Count > 0)
                {
                    Console.WriteLine("Plats proposés:");
                    foreach (var p in cuisinier.Plats)
                    {
                        Console.WriteLine($"- {p.Nom} ({p.PrixParPersonne}€)");
                        Console.WriteLine($"  Type: {p.Type} | Nationalité: {p.Nationalite}");
                        Console.WriteLine($"  Ingrédients: {string.Join(", ", p.Ingredients)}");
                    }
                }
                else
                {
                    Console.WriteLine("Aucun plat proposé par ce cuisinier.");
                }
                Console.WriteLine(new string('-', 40));
            }
        }

        /// <summary>
        /// Sélectionne un cuisinier aléatoirement parmi la liste.
        /// </summary>
        /// <returns>Cuisinier choisi aléatoirement.</returns>
        public Cuisinier AssignerCuisinierRandom()
        {
            Random random = new Random();
            int indexRandom = random.Next(cuisiniers.Count); // Choisir un index aléatoire
            return cuisiniers[indexRandom]; // Retourner le cuisinier à cet index
        }

        /// <summary>
        /// Ajoute un plat à un cuisinier donné par son nom.
        /// </summary>
        /// <param name="nomCuisinier">Nom du cuisinier.</param>
        /// <param name="plat">Plat à ajouter.</param>
        public void AjouterPlatACuisinier(string nomCuisinier, Plat plat)
        {
            var cuisinier = cuisiniers.FirstOrDefault(c => c.Nom == nomCuisinier);
            if (cuisinier != null)
            {
                cuisinier.AjouterPlat(plat);
            }
            else
            {
                Console.WriteLine("Cuisinier non trouvé.");
            }
        }

        /// <summary>
        /// Recherche un cuisinier pouvant fournir un plat par son nom.
        /// </summary>
        /// <param name="nomPlat">Nom du plat recherché.</param>
        /// <returns>Un cuisinier proposant ce plat, ou null.</returns>
        public Cuisinier TrouverCuisinierPourPlat(string nomPlat)
        {
            // Version optimisée avec gestion des cas null
            var candidats = cuisiniers
                .Where(c => c?.Plats != null)
                .Where(c => c.Plats.Any(p =>
                    p != null &&
                    p.Nom.Equals(nomPlat, StringComparison.OrdinalIgnoreCase)))
                .ToList();

            if (candidats.Count == 0)
            {
                Console.WriteLine($"\n⚠ Aucun cuisinier disponible pour le plat '{nomPlat}'");
                return null;
            }

            // Sélection aléatoire plus équitable
            int index = new Random().Next(0, candidats.Count);
            return candidats[index];
        }

        
    }
}