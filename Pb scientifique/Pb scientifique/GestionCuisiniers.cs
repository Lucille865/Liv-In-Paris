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
        public List<Cuisinier> cuisiniers = new List<Cuisinier>();
        private const string filePath = "Cuisiniers.txt";
        Graphe<int> graphe = new Graphe<int>();

        public GestionCuisiniers()
        {
            cuisiniers = new List<Cuisinier>();
            cuisiniers = ChargerCuisiniers();
        }

        // Méthode pour ajouter un cuisinier
        public void AjouterCuisinier(Cuisinier cuisinier)
        {
            cuisiniers.Add(cuisinier);
            SauvegarderCuisiniers();
        }
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

            return nouveauCuisinier; // Ligne cruciale ajoutée
        }

        public void AfficherCuisiniers()
        {
            if (cuisiniers.Count == 0)
            {
                Console.WriteLine("Aucun cuisinier à afficher.");
                return;
            }

            foreach (var cuisinier in cuisiniers)
            {
                Console.WriteLine($"Nom: {cuisinier.Nom}, Adresse: {cuisinier.Adresse}, Email: {cuisinier.Email}");
            }
        }

        // SauvegarderCuisiniers pour inclure les plats
        private void SauvegarderCuisiniers()
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var cuisinier in cuisiniers)
                {
                    // Écriture des infos de base
                    writer.WriteLine($"{cuisinier.Nom};{cuisinier.Adresse};{cuisinier.Telephone};{cuisinier.Email};{cuisinier.Identifiant};{cuisinier.MotDePasse}");

                    // Écriture des plats
                    foreach (var plat in cuisinier.Plats)
                    {
                        writer.WriteLine($"PLAT;{plat.Nom};{plat.Type};{plat.PrixParPersonne};{plat.Nationalite};{plat.RegimeAlimentaire};{string.Join(",", plat.Ingredients)}");
                    }
                }
            }
        }

        // Modifions la méthode ChargerCuisiniers pour charger les plats
        private List<Cuisinier> ChargerCuisiniers()
        {
            List<Cuisinier> listeCuisiniers = new List<Cuisinier>();
            Cuisinier cuisinierCourant = null;

            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string ligne;
                    while ((ligne = reader.ReadLine()) != null)
                    {
                        if (ligne.StartsWith("PLAT;") && cuisinierCourant != null)
                        {
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
                        }
                    }
                }
            }

            return listeCuisiniers;
        }

        public Cuisinier AssignerCuisinierRandom()
        {
            Random random = new Random();
            int indexRandom = random.Next(cuisiniers.Count); // Choisir un index aléatoire
            return cuisiniers[indexRandom]; // Retourner le cuisinier à cet index
        }

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
