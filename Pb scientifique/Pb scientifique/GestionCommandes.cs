using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pb_scientifique
{
    public class GestionCommandes
    {
        private List<Commande> commandes = new List<Commande>();
        private const string filePath = "Commandes.txt";

        public GestionCommandes()
        {
            commandes = ChargerCommandes();
        }

        // Méthode pour ajouter une commande
        public void AjouterCommande(Commande commande)
        {
            commandes.Add(commande);
            SauvegarderCommandes();
            Console.WriteLine("Commande ajoutée.");
        }

        // Méthode pour afficher toutes les commandes
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

                Console.WriteLine($"Commande ID: {commande.Id}, Client: {clientNom}, Cuisinier: {cuisinierNom}, Total: {commande.TotalPrix}€, Date: {commande.Date}");
            }
        }

        private void SauvegarderCommandes()
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var commande in commandes)
                {
                    string clientNom = commande.Client != null ? commande.Client.Nom : "Inconnu";
                    string cuisinierNom = commande.Cuisinier != null ? commande.Cuisinier.Nom : "Inconnu";

                    writer.WriteLine($"{commande.Id};{clientNom};{cuisinierNom};{commande.TotalPrix};{commande.Date}");
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
                    string ligne;
                    while ((ligne = reader.ReadLine()) != null)
                    {
                        var data = ligne.Split(';');
                        if (data.Length == 5)
                        {
                            Commande commande = new Commande
                            {
                                Id = int.Parse(data[0]),
                                Client = !string.IsNullOrEmpty(data[1]) ? new Client(data[1], "", "", "", "", "", "") : new Client("Inconnu", "", "", "", "", "", ""),
                                Cuisinier = !string.IsNullOrEmpty(data[2]) ? new Cuisinier(data[2], "", "", "", "", "") : new Cuisinier("Inconnu", "", "", "", "", ""),
                                Date = DateTime.Parse(data[4])
                            };
                            listeCommandes.Add(commande);
                        }
                    }
                }
            }

            return listeCommandes;
        }
    }
}
