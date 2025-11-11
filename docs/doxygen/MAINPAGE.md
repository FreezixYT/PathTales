# BlogApi - Documentation technique complète

\tableofcontents

---

## Présentation du projet

**BlogApi** est une API REST développée en **ASP.NET Core 8.0** pour la gestion complète d'articles de blog. Ce projet implémente les bonnes pratiques de développement d'API modernes avec une architecture en couches claire et maintenable.

### Informations générales

| Propriété | Valeur |
|-----------|--------|
| **Nom** | BlogApi |
| **Version** | 1.0.0 |
| **Framework** | .NET 8.0 (ASP.NET Core) |
| **Base de données** | MariaDB 10.11 |
| **ORM** | Entity Framework Core 9.0 |
| **Auteur** | [Votre nom - À personnaliser] |
| **Date** | [Date - À personnaliser] |

---

## Objectifs du projet

Ce projet a été développé dans le cadre du cours **APROG (Programmation Avancée)** et démontre les compétences suivantes :

1. **Architecture en couches** : Séparation claire des responsabilités (Controllers, Models, DTOs, Repositories, Persistence)
2. **Patterns de conception** : Repository Pattern, DTO Pattern
3. **Entity Framework Core** : ORM pour l'accès aux données avec migrations
4. **API RESTful** : Respect des conventions REST (GET, POST, PUT, DELETE)
5. **Validation des données** : Data Annotations pour garantir l'intégrité
6. **Documentation API** : Swagger/OpenAPI pour la documentation interactive
7. **Conteneurisation** : Docker Compose pour MariaDB et phpMyAdmin

---

## Architecture

Le projet suit une architecture en couches qui sépare les préoccupations :

```
┌─────────────────────────────────────────┐
│         Controllers (API Layer)         │  ← Points d'entrée HTTP
│  - PostsController                      │
│  - VersionController                    │
└─────────────────┬───────────────────────┘
                  │
┌─────────────────▼───────────────────────┐
│              DTOs Layer                 │  ← Objets de transfert
│  - PostDto                              │
│  - PostCreateDto                        │
│  - PostUpdateDto                        │
└─────────────────┬───────────────────────┘
                  │
┌─────────────────▼───────────────────────┐
│         Repository Layer                │  ← Logique d'accès aux données
│  - IPostRepository (Interface)          │
│  - PostRepository (Implémentation)      │
└─────────────────┬───────────────────────┘
                  │
┌─────────────────▼───────────────────────┐
│          Persistence Layer              │  ← Configuration EF Core
│  - BlogDbContext                        │
│  - Migrations                           │
└─────────────────┬───────────────────────┘
                  │
┌─────────────────▼───────────────────────┐
│            Models Layer                 │  ← Entités du domaine
│  - Post                                 │
└─────────────────────────────────────────┘
```

### Diagrammes UML

Pour une meilleure compréhension de l'architecture, voici les diagrammes techniques :

#### 1. Diagramme de classes

\image html class-diagram.png "Diagramme de classes complet"
\image latex class-diagram.eps "Diagramme de classes complet" width=\textwidth

Ce diagramme montre toutes les classes de l'application et leurs relations.

#### 2. Diagramme d'architecture (Composants)

\image html architecture-diagram.png "Architecture du système"
\image latex architecture-diagram.eps "Architecture du système" width=\textwidth

Ce diagramme illustre l'architecture globale du système avec Docker Compose.

#### 3. Diagramme de paquetages

\image html package-diagram.png "Organisation en paquetages"
\image latex package-diagram.eps "Organisation en paquetages" width=\textwidth

Ce diagramme présente l'organisation en couches et les dépendances entre namespaces.

---

## Fonctionnalités principales

### Opérations CRUD complètes

L'API expose les endpoints suivants :

| Méthode | Endpoint | Description |
|---------|----------|-------------|
| **GET** | `/api/posts` | Liste tous les articles |
| **GET** | `/api/posts/{id}` | Récupère un article spécifique |
| **POST** | `/api/posts` | Crée un nouvel article |
| **PUT** | `/api/posts/{id}` | Met à jour un article existant |
| **DELETE** | `/api/posts/{id}` | Supprime un article |
| **GET** | `/health` | Health check de l'API |

### Diagrammes de cas d'utilisation

#### Cas d'utilisation principaux

\image html use-case-diagram.png "Diagramme de cas d'utilisation"
\image latex use-case-diagram.eps "Diagramme de cas d'utilisation" width=\textwidth

Ce diagramme montre les interactions entre les utilisateurs et le système.

### Flux de création d'un article

#### Diagramme de séquence - POST /api/posts

\image html sequence-diagram.png "Flux de création d'un article"
\image latex sequence-diagram.eps "Flux de création d'un article" width=\textwidth

Ce diagramme détaille le flux complet de création d'un article, de la requête HTTP jusqu'à la persistance en base.

### Flux de mise à jour d'un article

#### Diagramme de communication - PUT /api/posts/{id}

\image html communication-diagram.png "Interactions lors d'une mise à jour"
\image latex communication-diagram.eps "Interactions lors d'une mise à jour" width=\textwidth

Ce diagramme montre les interactions entre objets lors de la mise à jour d'un article.

### Validation automatique

Toutes les données entrantes sont validées automatiquement avec **Data Annotations** :

- **Titre** : 3-150 caractères, requis
- **Contenu** : minimum 3 caractères, requis
- **CreatedAt** : généré automatiquement (UTC)

### Support CORS

L'API est configurée pour accepter les requêtes du frontend Blazor (`http://localhost:5170`).

### Migrations automatiques

Les migrations Entity Framework Core sont appliquées automatiquement au démarrage de l'application.

---

## Modèle de données

### Entité Post

L'entité principale **Post** représente un article de blog :

| Propriété | Type | Description |
|-----------|------|-------------|
| `Id` | `int` | Clé primaire (auto-incrémentée) |
| `Title` | `string` | Titre de l'article |
| `Content` | `string` | Contenu complet de l'article |
| `CreatedAt` | `DateTime` | Date et heure de création (UTC) |

**Contraintes de la base de données :**
- `Id` : PRIMARY KEY, AUTO_INCREMENT
- `Title` : NOT NULL, LONGTEXT
- `Content` : NOT NULL, LONGTEXT
- `CreatedAt` : NOT NULL, DATETIME(6)

---

## Technologies utilisées

### Backend
- **ASP.NET Core 8.0** : Framework web moderne et performant
- **Entity Framework Core 9.0** : ORM pour .NET
- **Pomelo.EntityFrameworkCore.MySql 9.0** : Provider MySQL/MariaDB

### Base de données
- **MariaDB 10.11** : Système de gestion de base de données relationnel
- **phpMyAdmin** : Interface web pour la gestion de MariaDB

### Documentation
- **Swashbuckle (Swagger)** : Génération de documentation OpenAPI
- **Doxygen** : Génération de documentation technique avec code source

### Outils de développement
- **Docker Compose** : Orchestration des conteneurs
- **Visual Studio 2022 / VS Code** : Environnements de développement

---

## Configuration

### Chaîne de connexion

La connexion à la base de données est configurée dans `appsettings.json` :

```json
{
  "ConnectionStrings": {
    "Default": "Server=localhost;Port=3306;Database=blogdb;User=root;Password=example"
  }
}
```

### CORS

Le CORS est configuré pour accepter les requêtes depuis :
- `http://localhost:5170` (Frontend Blazor)

### Swagger

L'interface Swagger est accessible en mode développement à :
- `https://localhost:5001/swagger` (ou le port configuré)

---

## Structure du code source

Ce document contient le code source complet de tous les fichiers du projet, organisé par namespace :

1. **BlogApi.Controllers** : Contrôleurs API
2. **BlogApi.Models** : Entités du domaine
3. **BlogApi.DTOs** : Objets de transfert de données
4. **BlogApi.Repositories** : Interfaces et implémentations des repositories
5. **BlogApi.Persistence** : Configuration Entity Framework Core

Chaque classe inclut :
- Le code source complet avec coloration syntaxique
- Les commentaires XML pour la documentation
- Les dépendances et références

---

## Contexte pédagogique

Ce projet a été réalisé dans le cadre des ateliers progressifs du cours APROG :

- **Ateliers 1-4** : Développement initial avec SQLite
- **Atelier 5** : Migration vers MariaDB avec Docker
- **Ateliers suivants** : Fonctionnalités avancées

---

## Contact et support

Pour toute question concernant ce projet :
- **Enseignant** : [Nom de l'enseignant]
- **Email** : [Email]
- **Année** : 2025-2026

---

\note Cette documentation a été générée automatiquement avec **Doxygen** pour le TPI (Travail Pratique Individuel).

\author [Votre nom - À personnaliser]
\date [Date - À personnaliser]
\version 1.0.0
