using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pb_scientifique
{
    public class GestionCuisiniers
    {
<<<<<<< Updated upstream
        public void AjouterCuisinier(Cuisinier cuisinier)
        {
            string query = @"INSERT INTO Cuisiniers 
                            (Nom, Prenom, Adresse, Telephone, Email, Identifiant, 
                             MotDePasse, StationMetroProche) 
                            VALUES 
                            (@Nom, @Prenom, @Adresse, @Telephone, @Email, @Identifiant, 
                             @Mdp, @Metro)";

            DatabaseManager.ExecuteNonQuery(query,
                new MySqlParameter("@Nom", cuisinier.Nom),
                new MySqlParameter("@Prenom", cuisinier.Prenom),
                new MySqlParameter("@Adresse", cuisinier.Adresse),
                new MySqlParameter("@Telephone", cuisinier.Telephone),
                new MySqlParameter("@Email", cuisinier.Email),
                new MySqlParameter("@Identifiant", cuisinier.Identifiant),
                new MySqlParameter("@Mdp", cuisinier.MotDePasse),
                new MySqlParameter("@Metro", cuisinier.MetroProche)
            );
        }

        public Cuisinier GetCuisinierParIdentifiant(string identifiant)
=======
        public List<Cuisinier> cuisiniers = new List<Cuisinier>();
        private const string filePath = "Cuisiniers.txt";
        Graphe<int> graphe = new Graphe<int>();

        public GestionCuisiniers()
        {
            cuisiniers = new List<Cuisinier>();
            cuisiniers = ChargerCuisiniers();
        }

        // Méthode pour ajouter un cuisinier
        public void AjouterCuisinier()
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

            List<Plat> plats = new List<Plat>();
            bool ajouterPlats = true;

            while (ajouterPlats)
            {
                Console.WriteLine("\nAjouter un plat à ce cuisinier (o/n) ?");
                if (Console.ReadLine().ToLower() == "o")
                {
                    Plat plat = CreerPlat();
                    plats.Add(plat);
                }
                else
                {
                    ajouterPlats = false;
                }
            }

            var cuisinier = new Cuisinier(nom, adresse, telephone, email, pseudo, mdp, plats);
            cuisiniers.Add(cuisinier);
            SauvegarderCuisiniers();
            Console.WriteLine($"Cuisinier ajouté : {cuisinier.Nom}");
        }

        private Plat CreerPlat()
<<<<<<< Updated upstream
=======
        {
            Console.WriteLine("Nom du plat : ");
            string nom = Console.ReadLine();
            Console.WriteLine("Type (Entrée/Plat/Dessert) : ");
            string type = Console.ReadLine();
            Console.WriteLine("Prix par personne : ");
            decimal prix = decimal.Parse(Console.ReadLine());
            Console.WriteLine("Nationalité : ");
            string nationalite = Console.ReadLine();
            Console.WriteLine("Régime alimentaire : ");
            string regime = Console.ReadLine();

            // Création d'une liste d'ingrédients
            List<string> ingredients = new List<string>();
            Console.WriteLine("Ajouter des ingrédients (un par ligne, vide pour terminer) :");
            string ingredient;
            while (!string.IsNullOrWhiteSpace(ingredient = Console.ReadLine()))
            {
                ingredients.Add(ingredient);
            }

            return new Plat(nom, type, 1, DateTime.Now, DateTime.Now.AddDays(2), prix, nationalite, regime, ingredients);
        }

        // Méthode pour afficher tous les cuisiniers
        public void AfficherCuisiniers()
>>>>>>> Stashed changes
        {
            Console.WriteLine("Nom du plat : ");
            string nom = Console.ReadLine();
            Console.WriteLine("Type (Entrée/Plat/Dessert) : ");
            string type = Console.ReadLine();
            Console.WriteLine("Prix par personne : ");
            decimal prix = decimal.Parse(Console.ReadLine());
            Console.WriteLine("Nationalité : ");
            string nationalite = Console.ReadLine();
            Console.WriteLine("Régime alimentaire : ");
            string regime = Console.ReadLine();

            // Création d'une liste d'ingrédients
            List<string> ingredients = new List<string>();
            Console.WriteLine("Ajouter des ingrédients (un par ligne, vide pour terminer) :");
            string ingredient;
            while (!string.IsNullOrWhiteSpace(ingredient = Console.ReadLine()))
            {
                ingredients.Add(ingredient);
            }

            return new Plat(nom, type, 1, DateTime.Now, DateTime.Now.AddDays(2), prix, nationalite, regime, ingredients);
        }

        // Méthode pour afficher tous les cuisiniers
        public void AfficherCuisiniers()
>>>>>>> Stashed changes
        {
            using (var conn = DatabaseManager.GetConnection())
            {
                conn.Open();
                string query = "SELECT * FROM Cuisiniers WHERE Identifiant = @Identifiant";

                using (var cmd = new MySqlCommand(query, conn))
                {
<<<<<<< Updated upstream
                    cmd.Parameters.AddWithValue("@Identifiant", identifiant);
=======
                    writer.WriteLine($"{cuisinier.Nom};{cuisinier.Adresse};{cuisinier.Telephone};{cuisinier.Email};{cuisinier.Identifiant};{cuisinier.MotDePasse}");

                    foreach (var plat in cuisinier.Plats)
                    {
                        writer.WriteLine($"PLAT;{plat.Nom};{plat.Type};{plat.PrixParPersonne};{plat.Nationalite};{plat.RegimeAlimentaire};{string.Join(",", plat.Ingredients)}");
                    }
                }
            }
        }
        private List<Cuisinier> ChargerCuisiniers()
        {
            List<Cuisinier> listeCuisiniers = new List<Cuisinier>();
            Cuisinier cuisinierCourant = null;
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes

                    using (var reader = cmd.ExecuteReader())
                    {
<<<<<<< Updated upstream
<<<<<<< Updated upstream
                        if (reader.Read())
                        {
                            return new Cuisinier(
                                reader["Nom"].ToString(),
                                reader["Prenom"].ToString(),
                                reader["Adresse"].ToString(),
                                reader["Telephone"].ToString(),
                                reader["Email"].ToString(),
                                reader["Identifiant"].ToString(),
                                reader["MotDePasse"].ToString(),
                                reader["StationMetroProche"].ToString()
                            );
=======
                        if (ligne.StartsWith("PLAT;") && cuisinierCourant != null)
                        {
=======
                        if (ligne.StartsWith("PLAT;") && cuisinierCourant != null)
                        {
>>>>>>> Stashed changes
                            var data = ligne.Split(';');
                            var ingredients = data.Length > 6 ? data[6].Split(',').ToList() : new List<string>();

                            var plat = new Plat(
                                data[1], data[2], 1, DateTime.Now, DateTime.Now.AddDays(2),
                                decimal.Parse(data[3]), data[4], data[5], ingredients);

                            cuisinierCourant.Plats.Add(plat);
                        }
                        else
                        {
                            // C'est une ligne de cuisinier
                            var data = ligne.Split(';');
                            if (data.Length >= 6)
                            {
                                cuisinierCourant = new Cuisinier(
                                    data[0], data[1], data[2], data[3], data[4], data[5]);
                                listeCuisiniers.Add(cuisinierCourant);
                            }
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
                        }
                    }
                }
            }
            return new Cuisinier("","", "", "", "", "", "", "");
        }

        public List<Cuisinier> GetTousCuisiniers()
        {
            var cuisiniers = new List<Cuisinier>();
            using (var conn = DatabaseManager.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand("SELECT * FROM Cuisiniers", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cuisiniers.Add(new Cuisinier
                        (
                            
                            reader["Nom"].ToString(),
                            reader["Prenom"].ToString(),
                            reader["Adresse"].ToString(),
                            reader["Telephone"].ToString(),
                            reader["Email"].ToString(),
                            reader["Identifiant"].ToString(),
                            reader["MotDePasse"].ToString(),
                            reader["StationMetroProche"].ToString()
                        ));
                    }
                }
            }
            return cuisiniers;
        }
<<<<<<< Updated upstream
     
=======

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
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
    }
}
