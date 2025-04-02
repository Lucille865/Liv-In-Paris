using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pb_scientifique
{
    public class GestionClients
    {
        public void AjouterClient(Client client)
        {
            string query = @"
            INSERT INTO Clients 
            (Nom, Prenom, Adresse, Telephone, Email, Identifiant, 
             MotDePasse, TypeClient, NomEntreprise, Referent, StationMetroProche) 
            VALUES 
            (@Nom, @Prenom, @Adresse, @Telephone, @Email, @Identifiant, 
             @MotDePasse, @TypeClient, @NomEntreprise, @Referent, @Metro)";

            DatabaseManager.ExecuteNonQuery(query,
                new MySqlParameter("@Nom", client.Nom),
                new MySqlParameter("@Prenom", client.Prenom ?? (object)DBNull.Value),
                new MySqlParameter("@Adresse", client.Adresse),
                new MySqlParameter("@Telephone", client.Telephone),
                new MySqlParameter("@Email", client.Email),
                new MySqlParameter("@Identifiant", client.Identifiant),
                new MySqlParameter("@MotDePasse", client.MotDePasse),
                new MySqlParameter("@TypeClient", client.TypeClient),
                new MySqlParameter("@NomEntreprise", client.NomEntreprise ?? (object)DBNull.Value),
                new MySqlParameter("@Referent", client.Referent ?? (object)DBNull.Value),
                new MySqlParameter("@Metro", client.MetroProche)
            );
        }
        public List<Client> GetTousClients()
        {
            var clients = new List<Client>();
            using (var conn = DatabaseManager.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand("SELECT * FROM Clients", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        clients.Add(new Client
                        (
                            reader["Nom"].ToString(),
                            reader["Prenom"] is DBNull ? null : reader["Prenom"].ToString(),
                            reader["Adresse"].ToString(),
                            reader["Telephone"].ToString(),
                            reader["Email"].ToString(),
                            reader["Identifiant"].ToString(),
                            reader["TypeClient"].ToString(),
                            reader["NomEntreprise"] is DBNull ? null : reader["NomEntreprise"].ToString(),
                            reader["Referent"] is DBNull ? null : reader["Referent"].ToString(),
                            reader["StationMetroProche"].ToString()
                        ));
                    }
                }
            }
            return clients;
        }

        public Client GetClientParIdentifiant(string identifiant)
        {
            using (var conn = DatabaseManager.GetConnection())
            {
                conn.Open();
                string query = "SELECT * FROM Clients WHERE Identifiant = @Identifiant";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Identifiant", identifiant);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Client
                            (
                                reader["Nom"].ToString(),
                                reader["Prenom"]?.ToString(),
                                reader["Adresse"].ToString(),
                                reader["Telephone"].ToString(),
                                reader["Email"].ToString(),
                                reader["Identifiant"].ToString(),
                                reader["TypeClient"].ToString(),
                                reader["NomEntreprise"] is DBNull ? null : reader["NomEntreprise"].ToString(),
                                reader["Referent"] is DBNull ? null : reader["Referent"].ToString(),
                                reader["StationMetroProche"].ToString()
                            );
                        }
                    }
                }
            }
            return null;
        }
        
    }
}
