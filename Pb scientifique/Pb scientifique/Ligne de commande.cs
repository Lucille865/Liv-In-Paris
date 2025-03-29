using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pb_scientifique
{
    public class LigneCommande
    {
        public Plat Plat { get; set; }
        public int Quantite { get; set; }
        public DateTime DateLivraison { get; set; }
        public string AdresseLivraison { get; set; }
    }
}
