using Pokemon.Application.Common.DTO;
using Pokemon.Application.Helper;
using Pokemon.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.Application.Common.Interfaces
{
    public interface IPokemonRepository
    {
        public Task<PokemonDTO> Create(PokemonDTO objDTO);
        public Task<bool> UpdatePokemonIfExists(PokemonDTO objDTO);
        public Task<bool> DeletePokemonIfExists(int id);
        public Task<PokemonDTO> Get(int id);
        public Task<IEnumerable<PokemonDTO>> GetAll(int? id = null);
        public Task<IEnumerable<PokemonDTO>> GetAll_PaginationOne(PokemonParameters parameters);
        public Task<PagedList<MyPokemon>> GetAll_PaginationTwo(PokemonParameters parameters);
    }
}
