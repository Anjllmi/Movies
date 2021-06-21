using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Movies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        //Base de datos de movies
        public DbSet<Movie> Movies { get; set; }

        //Se añade el nuevo modelo de tabla
        //Que se añade en terminal con:
        // dotnet ef migrations add "ne table category"
        //luego se actualiza la base de datos:
        //dotnet ef database update
        public DbSet<Category> Categories { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        //Constructor
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            /*
             * Se cambió de Warning a Information en appsettings lo siguiente:
             * "Microsoft": "Information",
             */
        }

        /*
         * 
         * dotnet ef migrations add "Initial Migration" para crear la base de datos
         * 
         * 
         * Es necesario instalar a través de la Consola del Administrador de paquetes:
         * Install-Package Microsoft.EntityFrameworkCore.Design -Version 5.0.7
         * 
         * luego para crear la base de datos con los datos en Migrations/Initial Migratio.cs:
         * dotnet ef database update
         */

        //Conecta con la base de datos
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //    => optionsBuilder.UseNpgsql("Host=localhost;Database=movies;Username=postgres;Password=ajllmi");

    }
}
