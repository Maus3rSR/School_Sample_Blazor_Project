# Cheatsheet / Support de cours

## Ressources documentaire

[Blazor pour .NET 9](https://learn.microsoft.com/fr-fr/aspnet/core/blazor/?view=aspnetcore-9.0)

## Création d'un nouveau projet Blazor (CLI)

```bash
dotnet new blazor -o BlazorWebApp
```

## Lancement projet avec hotreload lors de changement de code

```bash
dotnet watch
```

## Outils globaux utiles

```bash
# CLI pour EntityFramework
dotnet tool install --global dotnet-ef

# dotnet-aspnet-codegenerator => Generateur de code
dotnet tool install --global dotnet-aspnet-codegenerator --allow-roll-forward # --allow-roll-forward pour la compatibilité avec .NET 9

# Alternatif à dotnet-aspnet-codegenerator pour la génération de code avec une meilleure expérience de génération (preview)
# C'est une CLI qui vous guide, et génère certainement la commande dotnet-code-generator nécéssaire
# Voir la documentation https://devblogs.microsoft.com/dotnet/introducing-dotnet-scaffold/#using-dotnet-scaffold
dotnet tool install --global Microsoft.dotnet-scaffold
```

## Génération d'interfaces CRUD (Create Read Update Delete)

**Note :**
Vous pouvez utiliser l'interface contextuelle de l'explorateur de solution à partir d'un clic droit sur le projet : "Ajouter > Nouvel élément généré automatiquement" ou via CLI.

### Installation des packages nécéssaires pour générer du CRUD

Vous pouvez mettre ceci dans un script bash et le lancer.

```bash
#!/bin/bash

# INSTALLATION DE PACKAGES DANS LE PROJET

# Package pour génération de code pour le web avec aspnet-codegenerator
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design

# Packages NuGet des fournisseurs de base de données (vous pouvez un installer ou plusieurs au choix)
dotnet add package Microsoft.EntityFrameworkCore.InMemory
dotnet add package Microsoft.EntityFrameworkCore.SQLite
dotnet add package Microsoft.EntityFrameworkCore.SqlServer

# pour utiliser la méthode d'extension AddDatabaseDeveloperPageExceptionFilter dans le fichier Programme, qui capture les exceptions liées à la base de données.
dotnet add package Microsoft.EntityFrameworkCore.Tools

# Détection et diagnostic des erreurs de migrations
dotnet add package Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore

# Packages pour componsants Blazor de table de données "clé en main" + Adapteur à EntityFramework du QuickGrid
dotnet add package Microsoft.AspNetCore.Components.QuickGrid
dotnet add package Microsoft.AspNetCore.Components.QuickGrid.EntityFrameworkAdapter
```

### Générer des interfaces CRUD

Commande pour générer un CRUD (Create Read Update Delete) classique

```bash
# -dbProvider quel type de base de données on utilise (sqlserver, sqlite, cosmos, postgres)
# -dc la classe DbContext à générer, le DbContext sert de pont entre l'entité (classe qui représente les données de la base) et la base de données
# -m le nom du modèle
# -outDir le chemin où seront placés les composants Blazor de CRUD
dotnet aspnet-codegenerator blazor CRUD -dbProvider sqlite -dc BlazorWebApp.Data.DbContext -m Movie -outDir Components/Pages
```

## EntityFramework Core

**Trigger Warning** Documentation sur EntityFramework Core et non Entity Framework 6 (obsolète) !

[EF CLI documentation](https://learn.microsoft.com/en-us/ef/core/cli/dotnet)

[DbContext c'est quoi ?](https://dotnettutorials.net/lesson/dbcontext-entity-framework-core/)
[Récupérer des données](https://learn.microsoft.com/en-us/ef/core/querying/)
[Sauvegarde des données](https://learn.microsoft.com/en-us/ef/core/saving/)]
[Relation dans les base de données](https://learn.microsoft.com/en-us/ef/core/modeling/relationships)
[Charger des données liées](https://learn.microsoft.com/en-us/ef/core/querying/related-data/)

## Migration base de données

### Principe du système de migration de EF Core

Approche "code-first" : on écrit le code de la base de données dans le code C# et on génère la base de données à partir de ce code.
Objectif: Gain de temps sur la gestion base de données en laissant à EF Core le soin de gérer les migrations.

[Comprendre Migrations, Snapshots et Synchronisation](https://ardalis.com/entity-framework-core-understanding-migrations-snapshots-synchronization/)
[Schéma de séquence lors d'une migration](https://ardalis.com/img/ef-core-migrations-sequence.png)

### Utilisation

Ajouter une migration va générer un fichier de migration dans le dossier Migrations.
Ce fichier contient les instructions pour créer la base de données et les tables à partir de la définition des classes qui étendent de `DbContext`.
Pour créer une migration avec EntityFramework :

```bash
# InitialCreate est le nom de la migration
dotnet ef migrations add InitialCreate
```

Si l'on souhaite supprimer la migration créée

```bash
dotnet ef migrations remove
```

Puis pour appliquer les migrations sur la base de données

```bash
dotnet ef database update
```

### Seeding (amorçage de données)

[Initialiser avec des données](https://learn.microsoft.com/en-us/ef/core/modeling/data-seeding)

## Sécurité

**Trigger Warning:** Ne jamais mettre de secrets dans le `appsettings.json` ou dans le code en dur ! Vous aller versionner des informations qui seront de-facto compromises.
Exemple de secrets : connectionString, API Key, Secret Key, Password, Tokens, etc.
[Gérer ses secrets de manière plus sécurisée](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-9.0&tabs=windows#access-a-secret)

## Identity

[Comprendre le modèle de données Identité](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/customize-identity-model?view=aspnetcore-9.0#the-identity-model)

### Génération des fichiers Identity nécéssaires en CLI

**Note :**
- Vous pouvez utiliser l'interface contextuelle de l'explorateur de solution à partir d'un clic droit sur le projet : "Ajouter > Nouvel élément généré automatiquement" ou via CLI.
- Si vous n'avez pas générer les fichiers Identity à la création du projet et que vous souhaitez garder un seul fichier de `DbContext`, il faudra le modifier pour l'étendre avec `IdentityDbContext`.

```bash
# Lancer la commande et se laisser guider par les étapes (choisir Identity)
dotnet scaffold
```

ou avec dotnet aspnet-code-generator

```bash
# Ajout du package NuGet
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
# ou identity dans le cas de MVC
aspnet-code-generator blazor-identity
```

### Override de l'UI d'Identity

https://learn.microsoft.com/en-us/aspnet/core/security/authentication/scaffold-identity?view=aspnetcore-9.0&tabs=visual-studio#full

```bash
# Commande pour lister les fichiers que l'on peut générer
# lf = list files
dotnet aspnet-codegenerator identity -lf
```

Générer certains fichiers d'UI Identity

```bash
# -dc la classe DbContext à générer, le DbContext sert de pont entre l'entité (classe qui représente les données de la base) et la base de données
# --files listes des fichiers à générer séparé par des espaces
dotnet aspnet-codegenerator identity -dc BlazorWebApp.Data.DbContext --files "Account.Register"
```

### Override du modèle de données IdentityUser

[Ajouter des données supplémentaires au modèle de donnée Identity](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/customize-identity-model?view=aspnetcore-9.0#customize-the-model)

Attributs supplémentaires possibles à ajouter au modèle :
- `[PersonalData]`: Indique que la propriété est une donnée personnelle et doit être supprimable/exportable (RGPD)
- `[ProtectedPersonalData]`: Indique que la propriété est une donnée personnelle et est cryptée (à vérifier dans le framework, aucune documentation précise trouvée à ce sujet)

### Authentification

[Authentification requise pour tous les écrans](https://learn.microsoft.com/en-us/aspnet/core/security/authorization/secure-data?view=aspnetcore-9.0#require-authenticated-users)

### Autorisation

[Autorisation basée sur les Rôles](https://learn.microsoft.com/en-us/aspnet/core/security/authorization/roles?view=aspnetcore-9.0)
[Autorisation basée sur les Revendications](https://learn.microsoft.com/en-us/aspnet/core/security/authorization/claims?view=aspnetcore-9.0)
[Autorisation basée sur les Politiques](https://learn.microsoft.com/en-us/aspnet/core/security/authorization/policies?view=aspnetcore-9.0)
[Autorisation basée sur les Ressources (propriété de données)](https://learn.microsoft.com/en-us/aspnet/core/security/authorization/resourcebased?view=aspnetcore-9.0)

[Différence Rôle/Revendication/Politique](https://abdelmajid-baco.medium.com/exploring-roles-claims-and-policies-in-application-security-519f4e8eb5e2)

[Utilisation des autorisations pour les pages Razor](https://learn.microsoft.com/en-us/aspnet/core/security/authorization/razor-pages-authorization?view=aspnetcore-9.0)
[Utilisation des autorisations pour les composants Blazor](https://learn.microsoft.com/en-us/aspnet/core/blazor/security/?view=aspnetcore-9.0&tabs=visual-studio#authorization)

[Politique d'autorisation](https://learn.microsoft.com/en-us/aspnet/core/security/authorization/policies?view=aspnetcore-9.0)

### Emails (confirmation de compte & récupération de mot de passe)

[Configuration des emails](https://learn.microsoft.com/fr-fr/aspnet/core/security/authentication/accconfirm?view=aspnetcore-9.0&tabs=visual-studio)
[Debug email](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/accconfirm?view=aspnetcore-9.0&tabs=visual-studio#debug-email)

## Divers

[Configuration](https://learn.microsoft.com/fr-fr/aspnet/core/fundamentals/configuration/?view=aspnetcore-9.0)
[Injection de dépendances](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection)