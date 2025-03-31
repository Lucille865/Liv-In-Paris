using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pb_scientifique
{
    public class GestionCuisiniers
    {
        private List<Cuisinier> cuisiniers = new List<Cuisinier>();
        private const string filePath = "Cuisiniers.txt";

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
            Console.WriteLine($"Cuisinier ajouté : {cuisinier.Nom}");
        }

        // Méthode pour afficher tous les cuisiniers
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

        private void SauvegarderCuisiniers()
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var cuisinier in cuisiniers)
                {
                    writer.WriteLine($"{cuisinier.Nom};{cuisinier.Adresse};{cuisinier.Telephone};{cuisinier.Email};{cuisinier.Identifiant};{cuisinier.MotDePasse}");
                }
            }
        }
        private List<Cuisinier> ChargerCuisiniers()
        {
            List<Cuisinier> listeCuisiniers = new List<Cuisinier>();

            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string ligne;
                    while ((ligne = reader.ReadLine()) != null)
                    {
                        var data = ligne.Split(';');
                        if (data.Length == 6)
                        {
                            Cuisinier cuisinier = new Cuisinier(data[0], data[1], data[2], data[3], data[4], data[5]);
                            listeCuisiniers.Add(cuisinier);
                        }
                    }
                }
            }

            return listeCuisiniers;
        }
    }
}
