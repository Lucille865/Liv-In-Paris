using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pb_scientifique
{
    public class Plat
    {
        public string Nom { get; set; }
        public string Type { get; set; } // Entrée, Plat, Dessert
        public DateTime DateFabrication { get; set; }
        public DateTime DatePeremption { get; set; }
        public decimal PrixParPersonne { get; set; }
        public string Nationalite { get; set; }
        public string RegimeAlimentaire { get; set; }
        public List<string> Ingredients { get; set; } = new List<string>();
        public int Plat_Id { get; set; }
        //public string Photo { get; set; } // Chemin vers l'image

        public Plat(string nom, string type, DateTime dateFabrication, DateTime datePeremption, decimal prixParPersonne, string nationaliteCuisine, string regimeAlimentaire, List<string> ingredientsPrincipaux/*, string photo*/)
        {
            Nom = nom;
            Type = type;
            DateFabrication = dateFabrication;
            DatePeremption = datePeremption;
            PrixParPersonne = prixParPersonne;
            Nationalite = nationaliteCuisine;
            RegimeAlimentaire = regimeAlimentaire;
            Ingredients = ingredientsPrincipaux;
            //Photo = photo;
        }
    }
}

