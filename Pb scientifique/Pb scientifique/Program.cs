using Pb_scientifique;
using System;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;

namespace Pb_scientifique
{
    public class Program
    {
        static void Main(string[] args)
        {
            var interfaceApp = new Interface();

            // Afficher le menu initial
            //interfaceApp.AfficherMenu();

            string fichierExcel = "MetroParis";
            string fichierCsv = "MetroParis.csv";

            var excelApp = new Excel.Application();
            var workbook = excelApp.Workbooks.Open(fichierExcel);

            // Enregistrer la première feuille en CSV
            var worksheet = (Excel.Worksheet)workbook.Sheets[1];
            worksheet.SaveAs(fichierCsv, Excel.XlFileFormat.xlCSV);

            // Fermer le fichier Excel sans enregistrer
            workbook.Close(false);
            excelApp.Quit();

            // Libérer les ressources COM
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
        

        string cheminFichier = "MetroParis.csv";
            Graphe<int> graphe = new Graphe<int>();
            AfficheGraphe<int> image = new AfficheGraphe<int>();
            image.ChargerDepuisFichier(cheminFichier, int.Parse);
            LireRelationsDepuisFichier(cheminFichier, graphe);

            // Affichage des informations
            graphe.AfficherListeAdjacence();
            Console.WriteLine("Le graphe est-il connexe ? " + (graphe.EstConnexe() ? "Oui" : "Non"));
            Console.WriteLine("Le graphe contient-il des cycles ? " + (graphe.ContientCycle() ? "Oui" : "Non"));

            // Dessiner le graphe
            image.DessinerGraphe("graphe.png");
        }

        static void LireRelationsDepuisFichier(string cheminFichier, Graphe<int> graphe)
        {
            if (!File.Exists(cheminFichier))
            {
                Console.WriteLine("Fichier introuvable !");
                return;
            }

            string[] lignes = File.ReadAllLines(cheminFichier);

            foreach (string ligne in lignes)
            {
                string[] elements = ligne.Split(' ');
                if (elements.Length == 2 && int.TryParse(elements[0], out int num1) && int.TryParse(elements[1], out int num2))
                {
                    graphe.AjouterLien(num1, num2);
                }
            }
        }
    }
}
