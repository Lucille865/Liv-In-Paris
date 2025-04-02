using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Pb_scientifique
{
    public class GestionCommandes
    {

        public void AjouterCommande(Commande commande)
        {
            string query = @"INSERT INTO Commandes 
                           (ClientId, CuisinierId, Statut, AdresseLivraison, StationLivraison) 
                           VALUES 
                           (@ClientId, @CuisinierId, @Statut, @Adresse, @Station)";

            DatabaseManager.ExecuteNonQuery(query,
                new MySqlParameter("@ClientId", commande.Client),
                new MySqlParameter("@CuisinierId", commande.Cuisinier),
                new MySqlParameter("@Statut", commande.Statut),
                new MySqlParameter("@Adresse", commande.AdresseLivraison),
                new MySqlParameter("@Station", commande.StationLivraison)
            );
        }

        public List<Commande> GetToutesCommandes()
        {
            var commandes = new List<Commande>();
            using (var conn = DatabaseManager.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand("SELECT * FROM Commandes", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        commandes.Add(new Commande
                        {
                            Commande_Id = Convert.ToInt32(reader["Commande_Id"]),
                            ClientId = Convert.ToInt32(reader["ClientId"]),
                            CuisinierId = Convert.ToInt32(reader["CuisinierId"]),
                            Statut = reader["Statut"].ToString(),
                            AdresseLivraison = reader["AdresseLivraison"].ToString(),
                            StationLivraison = reader["StationLivraison"].ToString(),
                            DateCommande = Convert.ToDateTime(reader["DateCommande"])
                        });
                    }
                }
            }
            return commandes;
        }


    }
}
