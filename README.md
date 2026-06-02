# 🎮 Liv'In Paris

[![C#](https://img.shields.io/badge/Language-C%23-blue.svg)](https://learn.microsoft.com/fr-fr/dotnet/csharp/)
[![.NET 8.0](https://img.shields.io/badge/Framework-.NET%208.0-purple.svg)](https://dotnet.microsoft.com/download)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

Liv'In Paris est une application C# .NET 8.0 développée dans le cadre du Projet Scientifique Informatique (A2) à l'ESILV. L'application centralise et optimise un service de restauration, en connectant des clients et des cuisiniers à travers le réseau de métro de Paris.
L'application intègre une architecture orientée objet complète, une base de données relationnelle MySQL performante et des structures de graphes avancées pour le calcul d'itinéraires et l'analyse de réseaux.

---

## Fonctionnalités

* **Gestion CRUD complète :** Ajout, modification et suppression sécurisée des Cuisiniers, Clients et Commandes.
* **Calcul d'itinéraire optimal :** Recherche du chemin le plus court entre les stations de métro du cuisinier et du client.
* **Coloration de Graphes :** Analyse structurelle des relations clients-cuisiniers (graphe biparti et planaire).
* **Interopérabilité :** Module d'exportation complet des bilans et des données au format XML et JSON.
* **Visualisation Graphique :** Affichage du réseau de métro avec une coloration dynamique par ligne et tracé du meilleur chemin.

---

## Architecture du projet

Le projet est structuré autour de classes métiers et d'algorithmes spécifiques pour répondre aux exigences du cahier des charges:

* **`Graphe.cs` / `Noeud.cs` / `Lien.cs` :** Classes génériques fondamentales pour modéliser et instancier le réseau sous forme de liste ou matrice d'adjacence.
* **`Bellman.cs` / `Djikista.cs` / `Floyd Warshall.cs` :** Modules d'algorithmes de recherche de chemin, avec un focus sur Bellman-Ford pour son efficacité sur les réseaux peu denses.
* **`Client.cs` / `Cuisiniers.cs` / `Commmande.cs` :** Gestion des entités de la plateforme et interactions avec la base de données.
* **`Autre.cs` :** Module créatif contenant 3 suggestions innovantes pour enrichir l'application. 


---

## Installation et Lancement

### Prérequis
* [.NET 8.0 SDK](https://dotnet.microsoft.com/download) installé sur votre machine.
* Un serveur MySQL actif.

### Configuration SQL
Avant de lancer l'application, configurez votre accès à la base de données `LivinParis` qui permet de stocker l'historique des utilisateurs, des plats et des transactions:
* Base de données : `LivinParis`
* Utilisateur : `root`
* Mot de passe : `root`

L'accès s'effectue de manière sécurisée en C# pour permettre une persistance dynamique et un auto-enrichissement de la base au fil des commandes.

### 1. Cloner le projet
```bash
git clone [https://github.com/Lucille865/Pb-scientifique.git](https://github.com/Lucille865/Pb-scientifique.git)
cd Pb-scientifique
```

### 2. Restaurer les dépendances
```bash
dotnet restore
```

### 3. Lancer l'application
Ouvrez votre terminal à la racine du projet et lancez:
```bash
dotnet run
```


---

## Algorithme du Chemin le Plus Court : Bellman-Ford

Pour permettre aux cuisiniers d'assurer la livraison de leurs plats, l'application détermine l'itinéraire le plus rapide entre les stations de métro les plus proches du cuisinier et du client. 

Le choix architectural s'est porté sur l'algorithme de Bellman-Ford pour plusieurs raisons majeures:
* **Source unique :**  Il calcule efficacement les chemins depuis une seule station de départ vers une destination.
* **Économie de mémoire :** Plus rapide et adapté pour notre besoin que Floyd-Warshall, car il ne calcule pas inutilement toutes les paires du réseau.
* **Réseau Sparse :** Le métro parisien possède un nombre limité de connexions directes par station, ce qui rend cet algorithme très performant sur ce type de graphe.
* **Évolutivité :** Permet d'intégrer facilement de futures changements de poids (temps, correspondances, trafic) sans altérer la logique centrale de l'algorithme. 



