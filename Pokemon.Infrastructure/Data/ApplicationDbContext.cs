using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Pokemon.Application.Common.DTO;
using Pokemon.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Pokemon.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

            //try
            //{
            //    //The code below is to create a database if the database does not exist.

            //    var databaseCreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
            //    if (databaseCreator != null)
            //    {
            //        if (!databaseCreator.CanConnect()) databaseCreator.Create();
            //        if (!databaseCreator.HasTables()) databaseCreator.CreateTables();
            //    }


            //}
            //catch (Exception ex)
            //{

            //    Console.WriteLine(ex.Message);
            //}

        }


        public DbSet<MyPokemon> myPokemons { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }




    }
}
