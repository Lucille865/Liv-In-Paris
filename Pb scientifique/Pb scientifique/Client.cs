using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pb_scientifique
{
    public class Client : Utilisateur
    {
        public string Nom { get; set; }
        public string Adresse { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string Identifiant { get; set; }
        public string MotDePasse { get; set; }
        public string TypeClient { get; set; } // "Particulier" ou "Entreprise"

        public string Prenom { get; set; } // Seulement pour les clients particuliers
        public string NomEntreprise { get; set; } // Seulement pour les entreprises
        public string Referent { get; set; } // Seulement pour les entreprises

        // Constructeur
        public Client(string nom, string adresse, string telephone, string email, string identifiant, string motDePasse, string typeClient)
        {
            Nom = nom;
            Adresse = adresse;
            Telephone = telephone;
            Email = email;
            Identifiant = identifiant;
            MotDePasse = motDePasse;
            TypeClient = typeClient;
        }

        // Méthode pour afficher les informations du client
        public void AfficherInformations()
        {
            Console.WriteLine($"Nom: {Nom}, Adresse: {Adresse}, Téléphone: {Telephone}, Email: {Email}");

            // Vérifie si c'est un particulier ou une entreprise
            if (TypeClient == "Particulier")
            {
                Console.WriteLine($"Prénom: {Prenom}");
            }
            else if (TypeClient == "Entreprise")
            {
                Console.WriteLine($"Nom Entreprise: {NomEntreprise}, Référent: {Referent}");
            }
        }
    }
}
