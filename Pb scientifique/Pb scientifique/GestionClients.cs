using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pb_scientifique
{
    public class GestionClients
    {
        private List<Client> clients = new List<Client>();

        // Méthode pour ajouter un client
        public void AjouterClient(Client client)
        {
            clients.Add(client);
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
                Console.WriteLine($"Nom: {client.Nom}, Type: {client.TypeClient}, Email: {client.Email}");
            }
        }
    }
}
