using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pb_scientifique
{
    public class Cuisinier
    {
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Adresse { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string Identifiant { get; set; }
        public string MotDePasse { get; set; }
        public List<Plat> Plats { get; set; } = new List<Plat>();
        public List<Livraison> Livraisons { get; set; } = new List<Livraison>();
        public string MetroProche { get; set; }
        public Cuisinier(string nom, string prenom, string adresse, string telephone, string email, string identifiant, string motDePasse, string metroProche)
        {
            Nom = nom;
            Prenom = prenom;
            Adresse = adresse;
            Telephone = telephone;
            Email = email;
            Identifiant = identifiant;
            MotDePasse = motDePasse;
            MetroProche = metroProche;
        }

        public void AjouterPlat(Plat plat)
        {
            Plats.Add(plat);
        }

        // Afficher les informations de base du cuisinier
        public void AfficherInformations()
        {
            Console.WriteLine($"Nom: {Nom}, Adresse: {Adresse}, Téléphone: {Telephone}, Email: {Email}, Metro le plus proche : {MetroProche}");
        }

        // Afficher le nombre de livraisons effectuées
        public void AfficherLivraisons()
        {
            Console.WriteLine($"Nombre de livraisons effectuées : {Livraisons.Count}");
        }

        // Afficher les plats du cuisinier par fréquence
        public void AfficherPlatsParFrequence()
        {
            var platsFrequent = Plats.GroupBy(p => p.Nom).OrderByDescending(g => g.Count()).ToList();
            Console.WriteLine("Plats par fréquence:");
            foreach (var plat in platsFrequent)
            {
                Console.WriteLine($"Plat: {plat.Key}, Nombre de fois préparé: {plat.Count()}");
            }
        }

        // Afficher le plat du jour
        public void AfficherPlatDuJour()
        {
            var platDuJour = Plats.FirstOrDefault();
            if (platDuJour != null)
            {
                Console.WriteLine($"Plat du jour: {platDuJour.Nom}");
            }
            else
            {
                Console.WriteLine("Aucun plat du jour.");
            }
        }

        // Calculer le plus court chemin pour la livraison d'un plat
        public string CalculerCheminLivraison(string adresseClient)
        {
            // Logique simplifiée pour le calcul de chemin (à implémenter en fonction des lignes de métro ou d'autres critères)
            return $"Le chemin entre {Adresse} et {adresseClient} doit être calculé.";
        }
    }
}
