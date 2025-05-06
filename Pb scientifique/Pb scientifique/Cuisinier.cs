using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pb_scientifique
{
    /// <summary>
    /// Représente un cuisinier qui propose des plats et effectue des livraisons.
    /// </summary>
    public class Cuisinier
    {
        public string Nom { get; set; }
        public string Adresse { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string Identifiant { get; set; }
        public string MotDePasse { get; set; }
        public List<Plat> Plats { get; set; } = new List<Plat>();
        public List<Livraison> Livraisons { get; set; } = new List<Livraison>();

        public Cuisinier() { }
        public Cuisinier(string nom, string adresse, string telephone, string email, string identifiant, string motDePasse)
        {
            Nom = nom;
            Adresse = adresse;
            Telephone = telephone;
            Email = email;
            Identifiant = identifiant;
            MotDePasse = motDePasse;
        }

        /// <summary>
        /// Ajoute un plat au menu du cuisinier si ce n’est pas déjà le cas.
        /// </summary>
        public void AjouterPlat(Plat plat)
        {
            if (!Plats.Any(p => p.Nom == plat.Nom))
            {
                Plats.Add(plat);
                Console.WriteLine($"Plat {plat.Nom} ajouté au menu du cuisinier {Nom}");
            }
            else
            {
                Console.WriteLine("Ce plat est déjà dans le menu de ce cuisinier.");
            }
        }

        /// <summary>
        /// Affiche les informations personnelles du cuisinier.
        /// </summary>
        public void AfficherInformations()
        {
            Console.WriteLine($"Nom: {Nom}, Adresse: {Adresse}, Téléphone: {Telephone}, Email: {Email}");
        }

        /// <summary>
        /// Affiche le nombre de livraisons effectuées par le cuisinier.
        /// </summary>
        public void AfficherLivraisons()
        {
            Console.WriteLine($"Nombre de livraisons effectuées : {Livraisons.Count}");
        }

        /// <summary>
        /// Affiche les plats triés par fréquence de préparation.
        /// </summary>
        public void AfficherPlatsParFrequence()
        {
            var platsFrequent = Plats.GroupBy(p => p.Nom).OrderByDescending(g => g.Count()).ToList();
            Console.WriteLine("Plats par fréquence:");
            foreach (var plat in platsFrequent)
            {
                Console.WriteLine($"Plat: {plat.Key}, Nombre de fois préparé: {plat.Count()}");
            }
        }

        /// <summary>
        /// Affiche le plat du jour (le premier plat de la liste).
        /// </summary>
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
    }
}
