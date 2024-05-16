# ExpressVoiture - Instructions d'installation

## Prérequis

Avant de commencer, assurez-vous d'avoir installé les éléments suivants :

- [Visual Studio 2019 ou plus récent](https://visualstudio.microsoft.com/) avec le workload **ASP.NET et développement web**
- [SQL Server Management Studio (SSMS)](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms)

## Étapes d'installation

### 1. Cloner le projet

Clonez le projet depuis GitHub en utilisant la commande suivante :

```sh
git clone https://github.com/EveCrystali/P5.git
```

### 2. Ouvrir le projet

Ouvrez le projet **ExpressVoiture** dans Visual Studio.

### 3. Configurer la chaîne de connexion

Dans le projet **ExpressVoiture**, ouvrez le fichier `appsettings.json` à la racine. Vous y trouverez la section `ConnectionStrings` qui définit les chaînes de connexion pour la base de données utilisée dans cette application.

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=aspnet-ExpressVoitures-4c232545-f432-4e84-8e78-db4d8bfeaa8b;Trusted_Connection=True;MultipleActiveResultSets=true"

  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*"
}
```

### 4. Restaurer la base de données

Un backup de la base de données est fourni à la racine du projet. Pour restaurer cette base de données :

1. Ouvrez SQL Server Management Studio (SSMS).
2. Connectez-vous à `(localdb)\mssqllocaldb` avec **l'authentification Windows**.
3. Cliquez droit sur **Databases** et sélectionnez **Restore Database...**.
4. Dans la fenêtre de restauration, sélectionnez **Device** puis **Add**.
5. Parcourez jusqu'à l'emplacement du fichier de backup (`.bak`) fourni dans le projet et sélectionnez-le.
6. Complétez le processus de restauration en suivant les instructions à l'écran.

### 5. Mettre à jour la chaîne de connexion

Si vous avez utilisé un nom de base de données différent lors de la restauration, mettez à jour la chaîne de connexion dans `appsettings.json` en conséquence. Par exemple :

```json
"DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=NomDeVotreBaseDeDonnees;Trusted_Connection=True;MultipleActiveResultSets=true"
```

### 6. Mettre à jour les migrations et la base de données

Dans Visual Studio, ouvrez la **Console du Gestionnaire de Package** (Tools > NuGet Package Manager > Package Manager Console) et exécutez les commandes suivantes :

```sh
Update-Database
```

### 7. Exécuter l'application

Appuyez sur **F5** ou cliquez sur le bouton **IIS Express** pour démarrer l'application.

### 8. Connexion en tant qu'administrateur

Pour vous connecter en tant qu'administrateur, utilisez les identifiants suivants :

- **Email** : admin@email.com
- **Mot de passe** : 9vBZBB.QH83GeE

### 9. Résolution des problèmes de connexion

Si vous avez des difficultés à vous connecter, essayez d’abord de vous connecter à l’aide de Microsoft SQL Server Management Studio (assurez-vous que le type d’authentification est « Authentification Windows »), ou consultez le site [What's my SQL Server Name?](https://sqlserver-help.com/2011/06/19/help-whats-my-sql-server-name/).
