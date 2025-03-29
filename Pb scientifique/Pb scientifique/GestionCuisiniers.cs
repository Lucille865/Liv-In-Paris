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

        // Méthode pour ajouter un cuisinier
        public void AjouterCuisinier(Cuisinier cuisinier)
        {
            cuisiniers.Add(cuisinier);
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
    }
}
