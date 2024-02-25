using CsvHelper;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pokemon.Application.Common.DTO;
using Pokemon.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.Infrastructure.Data
{
    public  static class PokemonInitializer
    {
        public  static  WebApplication Seed(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                try
                {
                    context.Database.EnsureCreated();

                    string fileName = "pokemon.csv";
                    var exactpath = Path.Combine(Directory.GetCurrentDirectory(), "../Pokemon.Application\\Helper", fileName);
                    
                    if (File.Exists(exactpath))
                    {
                       
                        using (var reader = new StreamReader(exactpath))
                        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                        {
                            csv.Context.RegisterClassMap<PokemonMap>();
                            var records = csv.GetRecords<MyPokemon>();

                            // Add or update entities from CSV file
                            var trackedIds = new HashSet<int>();
                            foreach (var newPokemon in records)
                            {
                                var existingPokemon = context.myPokemons.FirstOrDefault(p => p.Id == newPokemon.Id);
                               
                                if (existingPokemon == null)
                                {
                                    context.myPokemons.Add(newPokemon);
                                }
                                else
                                {
                                }
                                
                            }

                           context.SaveChanges();             
                        }
                    }
                }
                catch (Exception)
                {

                    throw;
                }

                return app;
            }


        }
    }
}
