using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pb_scientifique
{
    internal class GestionPlats
    {
        public void AjouterPlat(Plat plat)
        {
            string query = @"INSERT INTO Plats 
                        (CuisinierId, Nom, Type, Regime, Nationalite, 
                         Ingredients, PrixParPersonne, DatePeremption) 
                        VALUES 
                        (@CuisinierId, @Nom, @Type, @Regime, @Nationalite, 
                         @Ingredients, @Prix, @DatePeremption)";

            DatabaseManager.ExecuteNonQuery(query,
                //new MySqlParameter("@CuisinierId", plat.CuisinierId),
                new MySqlParameter("@Nom", plat.Nom),
                new MySqlParameter("@Type", plat.Type),
                new MySqlParameter("@Regime", plat.RegimeAlimentaire),
                new MySqlParameter("@Nationalite", plat.Nationalite),
                new MySqlParameter("@Ingredients", plat.Ingredients),
                new MySqlParameter("@Prix", plat.PrixParPersonne),
                new MySqlParameter("@DatePeremption", plat.DatePeremption)
            );
        }

        public List<Plat> GetTousPlats()
        {
            var plats = new List<Plat>();
            using (var conn = DatabaseManager.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand("SELECT * FROM Plats", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        plats.Add(new Plat(
                            reader["Nom"].ToString(),
                            reader["Type"].ToString(),
                            Convert.ToDateTime(reader["DateFabrication"]),
                            Convert.ToDateTime(reader["DatePeremption"]),
                            Convert.ToDecimal(reader["PrixParPersonne"]),
                            reader["Nationalite"].ToString(),
                            reader["Regime"].ToString(),
                            reader["Ingredients"].ToString().Split(',').ToList()
                        ));
                    }
                }
            }
            return plats;
        }
    }
}
