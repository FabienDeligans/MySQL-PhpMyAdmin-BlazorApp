# Application Blazor et utilisation de Docker pour une base de données MySQL

[TOC]

## Création des containers Docker



### Création du docker-compose

```yaml
version: '3'

services:
  # Database
  db:
    image: mysql
    volumes:
      - db_data:/var/lib/mysqlTest
    restart: always
    environment:
      MYSQL_ROOT_PASSWORD: root
      MYSQL_USER: user
      MYSQL_PASSWORD: user
    networks:
      - mysql-test-network
    container_name: MySql-root-root
    ports:
      - '3306:3306'
  # phpmyadmin
  phpmyadmin:
    depends_on:
      - db
    image: mysql-phpadmin
    restart: always
    ports:
      - '80:80'
    environment:
      PMA_HOST: db
      MYSQL_ROOT_PASSWORD: root
    networks:
      - mysql-test-network
    container_name: phpMyAdmin-user-root
  
networks:
  mysql-test-network:
volumes:
  db_data:
```

### Explication

En premier lieu, on build l'image de MySQL en spécifiant un volume. Ceci permet de persister les données lorsque le container est éteint. 

Les variables d'environnements permettent de préciser le password du compte root : ici : 

- login : root

- MDP : root

  ainsi que les différents utilisateurs, ici : 

- login : user
- MDP : user

Pour chaque utilisateur, il faudra paramétrer les droits dans PhpMyAdmin

Il faut ensuite définir un réseau sur lequel communiquera MySQL et PhpMyAdmin.

Enfin on ouvre les ports nécessaires. MySQL sera accessible à localhost:3306 et exposera le port 3306.



En second lieu, on va builder l'image de PhpMyAdmin. 

```depends_on: db``` permet de lier les deux containers. On ouvre les ports. Ici l'image sera accessible à : localhost:80 ou bien à l'adresse IP sur lequel est exécuté le container sur le port 80. Les variables d'environnements ici permettent de préciser à quel base de données se lier ainsi que le password root pour les droits. Il faut préciser le même network pour que les deux containers puissent communiquer. 

Enfin on précise les volumes et network. 

Pour exécuter MySQL et PhpMyAdmin, il vous faudra Docker ainsi qu'une fenêtre PowerShell lancée à partir du dossier contenant le fichier 

``docker-compose.yml``. Lancer la commande suivante :  ``docker-compose up -d``



## Création de l'application Blazor

Ouvrir VisualStudio et Créer un nouveau projet, choisir "Application Blazor".

![image-20210206180939702](C:\Users\delig\AppData\Roaming\Typora\typora-user-images\image-20210206180939702.png)

On va choisir la version Server qui est la plus sûre dans un environnement de production à l'heure actuelle. On va se baser sur la version **3.1 de .NetCore**  pour notre application. 

**On ne va pas utiliser Docker pour le déploiement de l'application.** 

Il nous faudra des NuGets : 

![image-20210206182212765](C:\Users\delig\AppData\Roaming\Typora\typora-user-images\image-20210206182212765.png)

On sera vigilant au fait d'utiliser la version 3.1.11 de Microsoft.EntityFrameworkCore.Tools car les versions suivantes sont basées sur .Net Core 5.

Pour l'exemple, l'application est construite ainsi : 

![image-20210206181633295](C:\Users\delig\AppData\Roaming\Typora\typora-user-images\image-20210206181633295.png)

Une famille peut avoir 0 ou plusieurs enfants et 0 ou plusieurs parents

Chaque enfant ou parent appartient obligatoirement à une famille. 

On crée les modèles ainsi.

```c#
namespace MySqlBlazorApp.Models
{
    public interface IEntity
    {
        public int Id { get; set; }
    }
}
```

```c#
using System.Collections.Generic;

namespace MySqlBlazorApp.Models
{
    public class Famille : IEntity
    {
        public int Id { get; set; }
        public string NomDeFamille { get; set; }
        public IEnumerable<Parent> Parents { get; set; }
        public IEnumerable<Enfant> Enfants { get; set; }
    }
}
```

```c#
using System;

namespace MySqlBlazorApp.Models
{
    public class Enfant : IEntity
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public DateTime BirthDate { get; set; }
        public Famille Famille { get; set; }
        public int FamilleId { get; set; }

        public int Age()
        {
            var now = DateTime.Today;
            var age = now.Year - BirthDate.Year;
            if (BirthDate > now.AddYears(-age))
            {
                age--;
            }
            return age;
        }
    }
}

```

```c#
using System.ComponentModel.DataAnnotations;

namespace MySqlBlazorApp.Models
{
    public class Parent : IEntity
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Adresse { get; set; }
        public string Cp { get; set; }
        public string Ville { get; set; }
        public string Mail { get; set; }
        public string Tel { get; set; }
        public Famille Famille { get; set; }
        public int FamilleId { get; set; }
    }
}
```

On va créer ensuite le Context de la base de données. Ceci permettra d'y accéder et de manipuler les données. 

```c#
using Microsoft.EntityFrameworkCore;
using MySqlBlazorApp.Models;

namespace MySqlBlazorApp.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        public DbSet<Famille> Famille { get; set; }
        public DbSet<Parent> Parent { get; set; }
        public DbSet<Enfant> Enfant { get; set; }
    }
}
```

Dans le fichier ``appsettings.json`` on précisera la ConnectionString : 

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "Context": "Server=localhost;Port=3306;Database=DemoBlazorMysql;Uid=root;Pwd=root;"
  }
}

```

Dans la méthode ``ConfigureServices`` du fichier ``Startup.cs`` on crée le service qui permettra d'utiliser la connexion en utilisant l'injection de dépendance. 

```c#
...
public void ConfigureServices(IServiceCollection services)
{
    services.AddRazorPages();
    services.AddServerSideBlazor();
    services.AddSingleton<WeatherForecastService>();

    //TODO
    // Create connection
    services.AddDbContext<Context>(options => options.UseMySQL(Configuration.GetConnectionString("Context")));
}
...
```

Ici, le paramètre `` Configuration.GetConnectionString("Context")`` fait référence à la ConnectionString inscrite dans le fichier ``appsettings.json``.



## Création de la base de données

A partir de VisualStudio, on va créer la base de données. 

lancer la Console du Gestionnaire de package

 ![image-20210206183609842](C:\Users\delig\AppData\Roaming\Typora\typora-user-images\image-20210206183609842.png)

Pour créer la structure du script SQL qui sera injecté dans MySQL, on va créer une migration par la commande suivante : 

``add-migration <NomDeVotreMigration>``

Ensuite on va créer la base de données par la commande : 

``update-database``

**Et voilà, votre application est prête à utiliser MySQL.**



## Exemple

Dans le dossier **Pages** ouvrir le fichier ``Index.razor``

Remplacer le code par : 

```c#
@page "/"
@using MySqlBlazorApp.Data
@using MySqlBlazorApp.Models
// TODO Inject context
@inject Context Context

<button @onclick="Create">Create family</button>
Nb Family : <input value="@Count"/>


@code
{
    private int Count { get; set; } 
    protected override void OnInitialized()
    {
        // TODO => use Context to access database or create controller
        Count = Context.Famille.Count(); 
    }

    private void Create()
    {
        var fam = new Famille
        {
            NomDeFamille = "deligans"
        };
        Context.Famille.Add(fam);
        Context.SaveChanges(); 
        
        Count = Context.Famille.Count(); 

        InvokeAsync(StateHasChanged); 
    }
}
```

![image-20210206184358933](C:\Users\delig\AppData\Roaming\Typora\typora-user-images\image-20210206184358933.png)

Lorsque l'on clique sur le bouton "Create family", on crée et on enregistre une famille dans la base de données. La balise input affiche le compte de famille dans la base de données. 

![image-20210206184615442](C:\Users\delig\AppData\Roaming\Typora\typora-user-images\image-20210206184615442.png)