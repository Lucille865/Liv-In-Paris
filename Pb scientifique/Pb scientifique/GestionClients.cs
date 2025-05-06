using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Transactions;

namespace Pb_scientifique
{
    /// <summary>
    /// Gère les opérations liées aux clients de la base de données LivInParis.
    /// </summary>
    public class GestionClients
    {

        public List<Client> clients;

        /// <summary>
        /// Initialise une nouvelle instance de <see cref="GestionClients"/> et charge les clients depuis la base de données.
        /// </summary>
        public GestionClients()
        {
            clients = new List<Client>();
            ChargerClientsDepuisBDD();
        }

        /// <summary>
        /// Retourne la liste des clients actuellement en mémoire.
        /// </summary>
        public List<Client> GetClients()
        {
            return clients;
        }

        /// <summary>
        /// Ajoute un client à la base de données, avec gestion conditionnelle des champs entreprise.
        /// </summary>
        /// /// <param name="client">Client à ajouter.</param>
        public void AjouterClient(Client client)
        {
 
            string query = @"
            INSERT INTO Clients 
            (Nom, Prenom, Adresse, Telephone, Email, Identifiant, MotDePasse, TypeClient, NomEntreprise, Referent) 
            VALUES 
            (@Nom, @Prenom, @Adresse, @Telephone, @Email, @Identifiant, @MotDePasse, @TypeClient, @NomEntreprise, @Referent)";

            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@Nom", client.Nom),
                new MySqlParameter("@Prenom", client.Prenom ?? (object)DBNull.Value),
                new MySqlParameter("@Adresse", client.Adresse),
                new MySqlParameter("@Telephone", client.Telephone),
                new MySqlParameter("@Email", client.Email),
                new MySqlParameter("@Identifiant", client.Identifiant),
                new MySqlParameter("@MotDePasse", client.MotDePasse),
                new MySqlParameter("@TypeClient", client.TypeClient)
            };

            // Si le client est de type "Entreprise", ajouter les informations supplémentaires
            if (client.TypeClient == "Entreprise")
            {
                parameters.Add(new MySqlParameter("@NomEntreprise", client.NomEntreprise ?? (object)DBNull.Value));
                parameters.Add(new MySqlParameter("@Referent", client.Referent ?? (object)DBNull.Value));
            }
            else
            {
                // Pour un client particulier, on passe NULL pour ces deux champs
                parameters.Add(new MySqlParameter("@NomEntreprise", DBNull.Value));
                parameters.Add(new MySqlParameter("@Referent", DBNull.Value));
            }

            // Exécution de la requête avec tous les paramètres
            DatabaseManager.ExecuteNonQuery(query, parameters.ToArray());
            ChargerClientsDepuisBDD(); // Recharge immédiatement après ajout


        }



        /// <summary>
        /// Affiche la liste des clients dans la console.
        /// </summary>
        public void AfficherClients()
        {
            ChargerClientsDepuisBDD(); // Recharge à chaque affichage

            if (clients.Count == 0)
            {
                Console.WriteLine("Aucun client dans la base de données.");
                return;
            }

            Console.WriteLine("Liste des clients:");
            Console.WriteLine(new string('-', 60));

            foreach (var client in clients)
            {
                Console.WriteLine($"{client.Identifiant} | {client.Nom} | {client.Prenom ?? "N/A"} | {client.Email} | {client.TypeClient}");
            }
        }

        /// <summary>
        /// Charge tous les clients depuis la base de données vers la liste interne.
        /// </summary>
        private void ChargerClientsDepuisBDD()
        {
            clients.Clear();
            string query = "SELECT * FROM Clients";

            using(var conn = DatabaseManager.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var client = new Client(
                            reader["Nom"].ToString(),
                            reader["Adresse"].ToString(),
                            reader["Telephone"].ToString(),
                            reader["Email"].ToString(),
                            reader["Identifiant"].ToString(),
                            reader["MotDePasse"].ToString(),
                            reader["TypeClient"].ToString()
                        )
                        {
                            Prenom = reader["Prenom"] is DBNull ? null : reader["Prenom"].ToString(),
                            NomEntreprise = reader["NomEntreprise"] is DBNull ? null : reader["NomEntreprise"].ToString(),
                            Referent = reader["Referent"] is DBNull ? null : reader["Referent"].ToString()
                        };

                        clients.Add(client);
                    }
                }
            }

            Console.WriteLine($"{clients.Count} clients chargés depuis la BDD."); // Log de débogage
        }
    }
}
