using CsvHelper.Configuration;
using Pokemon.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.Application.Common.DTO
{
    public class PokemonMap : ClassMap<MyPokemon>
    {
        public PokemonMap()
        {
            Map(m => m.Name).Index(1);
            Map(m => m.Type1).Index(2);
            Map(m => m.Type2).Index(3);
            Map(m => m.Total).Index(4);
            Map(m => m.HP).Index(5);
            Map(m => m.Attack).Index(6);
            Map(m => m.Defense).Index(7);
            Map(m => m.SpAtk).Index(8);
            Map(m => m.SpDef).Index(9);
            Map(m => m.Speed).Index(10);
            Map(m => m.Generation).Index(11);
            Map(m => m.Legendary).Index(12);
        }
    }
}
