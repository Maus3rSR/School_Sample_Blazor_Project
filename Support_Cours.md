# Cheatsheet / Support de cours

## Ressources documentaire

[Blazor pour .NET 9](https://learn.microsoft.com/fr-fr/aspnet/core/blazor/?view=aspnetcore-9.0)

[DbContext: utilisation](https://learn.microsoft.com/fr-fr/ef/ef6/fundamentals/working-with-dbcontext)\
[DbContext c'est quoi ?](https://dotnettutorials.net/lesson/dbcontext-entity-framework-core/)

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

## Génération CRUD

### Installation des packages pour du Scaffolding

Vous pouvez mettre ceci dans un script bash et le lancer.

```bash
#!/bin/bash

# INSTALLATION DE PACKAGES DANS LE PROJET

# Package pour génération de code pour le web avec aspnet-codegenerator
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design

# Packages NuGet des fournisseurs de base de données
dotnet add package Microsoft.EntityFrameworkCore.InMemory
dotnet add package Microsoft.EntityFrameworkCore.SQLite
dotnet add package Microsoft.EntityFrameworkCore.SqlServer

# pour utiliser la méthode d'extension AddDatabaseDeveloperPageExceptionFilter dans le fichier Programme, qui capture les exceptions liées à la base de données.
dotnet add package Microsoft.EntityFrameworkCore.Tools

# Packages pour componsant Blazor de table de données "clé en main" + Adapteur à EntityFramework
dotnet add package Microsoft.AspNetCore.Components.QuickGrid
dotnet add package Microsoft.AspNetCore.Components.QuickGrid.EntityFrameworkAdapter
dotnet add package Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore
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

### Migration base de données

Pour créer une migration avec EntityFramework

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

## Identity

### Génération des fichiers nécéssaires en CLI

```bash
# Lancer la commande et se laisser guider par les étapes (choisir Identity)
dotnet scaffold
```

ou avec dotnet aspnet-code-generator

```bash
# Ajout du package NuGet
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
aspnet-code-generator blazor-identity
```

### Override de l'UI

https://learn.microsoft.com/en-us/aspnet/core/security/authentication/scaffold-identity?view=aspnetcore-9.0&tabs=visual-studio#full

```bash
# Commande pour lister les fichiers que l'on peut générer
# lf = list files
dotnet aspnet-codegenerator identity -lf
```

Générer certains fichiers d'UI Identity

```bash
# -dc la classe DbContext à générer, le DbContext sert de pont entre l'entité (classe qui représente les données de la base) et la base de données
# --files listes des fichiers à générer
dotnet aspnet-codegenerator identity -dc BlazorWebApp.Data.DbContext --files "Account.Register"
```
