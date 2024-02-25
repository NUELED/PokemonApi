using AutoMapper;
using Pokemon.Application.Common.DTO;
using Pokemon.Application.Helper;
using Pokemon.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.Infrastructure.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<MyPokemon, PokemonDTO>().ReverseMap();
        }
    }
}
