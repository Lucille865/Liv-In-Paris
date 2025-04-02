using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pb_scientifique
{
    public class GestionCuisiniers
    {
        public void AjouterCuisinier(Cuisinier cuisinier)
        {
            string query = @"INSERT INTO Cuisiniers 
                            (Nom, Prenom, Adresse, Telephone, Email, Identifiant, 
                             MotDePasse, StationMetroProche) 
                            VALUES 
                            (@Nom, @Prenom, @Adresse, @Telephone, @Email, @Identifiant, 
                             @Mdp, @Metro)";

            DatabaseManager.ExecuteNonQuery(query,
                new MySqlParameter("@Nom", cuisinier.Nom),
                new MySqlParameter("@Prenom", cuisinier.Prenom),
                new MySqlParameter("@Adresse", cuisinier.Adresse),
                new MySqlParameter("@Telephone", cuisinier.Telephone),
                new MySqlParameter("@Email", cuisinier.Email),
                new MySqlParameter("@Identifiant", cuisinier.Identifiant),
                new MySqlParameter("@Mdp", cuisinier.MotDePasse),
                new MySqlParameter("@Metro", cuisinier.MetroProche)
            );
        }

        public Cuisinier GetCuisinierParIdentifiant(string identifiant)
        {
            using (var conn = DatabaseManager.GetConnection())
            {
                conn.Open();
                string query = "SELECT * FROM Cuisiniers WHERE Identifiant = @Identifiant";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Identifiant", identifiant);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Cuisinier(
                                reader["Nom"].ToString(),
                                reader["Prenom"].ToString(),
                                reader["Adresse"].ToString(),
                                reader["Telephone"].ToString(),
                                reader["Email"].ToString(),
                                reader["Identifiant"].ToString(),
                                reader["MotDePasse"].ToString(),
                                reader["StationMetroProche"].ToString()
                            );
                        }
                    }
                }
            }
            return new Cuisinier("","", "", "", "", "", "", "");
        }

        public List<Cuisinier> GetTousCuisiniers()
        {
            var cuisiniers = new List<Cuisinier>();
            using (var conn = DatabaseManager.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand("SELECT * FROM Cuisiniers", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cuisiniers.Add(new Cuisinier
                        (
                            
                            reader["Nom"].ToString(),
                            reader["Prenom"].ToString(),
                            reader["Adresse"].ToString(),
                            reader["Telephone"].ToString(),
                            reader["Email"].ToString(),
                            reader["Identifiant"].ToString(),
                            reader["MotDePasse"].ToString(),
                            reader["StationMetroProche"].ToString()
                        ));
                    }
                }
            }
            return cuisiniers;
        }
     
    }
}
