using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pb_scientifique
{
    public class GestionClients
    {
        public List<Client> clients = new List<Client>();
        private const string filePath = "Clients.txt";

        public GestionClients()
        {
            clients = new List<Client>(); // Assurer que la liste est initialisée
            clients = ChargerClients();
        }

        public List<Client> GetClients()
        {
            return clients; // Retourne la liste interne des clients
        }

        // Méthode pour ajouter un client
        public void AjouterClient(Client client)
        {
            clients.Add(client);
            SauvegarderClients();
            Console.WriteLine($"Client ajouté : {client.Nom}");
        }

        // Méthode pour afficher tous les clients
        public void AfficherClients()
        {
            if (clients.Count == 0)
            {
                Console.WriteLine("Aucun client à afficher.");
                return;
            }

            foreach (var client in clients)
            {
                Console.WriteLine($"Nom: {client.Nom}, Adresse: {client.Adresse}, Type: {client.TypeClient}, Email: {client.Email}");
            }
        }
        private void SauvegarderClients()
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("Nom\tAdresse\tTelephone\tEmail\tPseudo\tMotDePasse\tTypeClient"); // En-tête

                foreach (var client in clients)
                {
                    writer.WriteLine($"{client.Nom}\t{client.Adresse}\t{client.Telephone}\t{client.Email}\t{client.Identifiant}\t{client.MotDePasse}\t{client.TypeClient}");
                }
            }
        }
        private List<Client> ChargerClients()
        {
            List<Client> listeClients = new List<Client>();

            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    bool firstLine = true;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (firstLine) // Ignore l'en-tête
                        {
                            firstLine = false;
                            continue;
                        }
                        string[] data = line.Split('\t'); // Séparateur tabulation
                        if (data.Length == 7) // Vérification
                        {
                            var client = new Client(data[0], data[1], data[2], data[3], data[4], data[5], data[6]);
                            listeClients.Add(client);

                        }
                    }
                }

            }
            return listeClients;
        }
    }
}
