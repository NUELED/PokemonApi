using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pokemon.Application.Common.DTO;
using Pokemon.Application.Common.Interfaces;
using Pokemon.Application.Helper;
using Pokemon.Domain.Entities;
using Pokemon.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.Infrastructure.Repository
{
    public class PokemonRepository : IPokemonRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly ILogger<PokemonRepository> _logger;    

        public PokemonRepository(ApplicationDbContext db, IMapper mapper, ILogger<PokemonRepository> logger)
        {
             _db = db;
             _mapper = mapper; 
            _logger = logger;   
        }


        public async Task<PokemonDTO> Create(PokemonDTO objDTO)
        {
            
                var obj = _mapper.Map<MyPokemon>(objDTO);
                obj.Id = 0;
                _logger.LogInformation("Adding a new Pokemon to the database.");
             
               var addedObj = await _db.myPokemons.AddAsync(obj);
                await _db.SaveChangesAsync();
                var objReturned = _mapper.Map<PokemonDTO>(addedObj.Entity);

                return objReturned;
        }

        public async Task<bool> DeletePokemonIfExists(int id)
        {
            var obj = await _db.myPokemons.FirstOrDefaultAsync(c => c.Id == id);
            if (obj != null)
            {
                _logger.LogInformation("Deleting Pokemon with ID: " + id);
                _db.myPokemons.Remove(obj);
                await _db.SaveChangesAsync();
                return true; 
            }
            return false; 
        }

        public async Task<PokemonDTO> Get(int id)
        {
               _logger.LogInformation("Fetching Pokemon with ID: " + id);
               var obj = await _db.myPokemons.FirstOrDefaultAsync(c => c.Id == id);
                if (obj != null)
                {
                    return _mapper.Map<PokemonDTO>(obj);
                }
              if (_db.myPokemons.Any(x => x.Id != id)) throw new ApplicationException("This pokemon" + id + "does not exists");
              return new PokemonDTO();
        }

        public async Task<IEnumerable<PokemonDTO>> GetAll(int? id = null)
        {
            _logger.LogInformation("Fetching allPokemons from the database.");
            return _mapper.Map<IEnumerable<PokemonDTO>>(_db.myPokemons); 
        }


        public async Task<bool> UpdatePokemonIfExists(PokemonDTO objDTO)
        {
            var objFromDb = await _db.myPokemons.FirstOrDefaultAsync(c => c.Id == objDTO.Id);
            if (objFromDb != null)
            {
   
                objFromDb.Name = objDTO.Name;
                objFromDb.Type1 = objDTO.Type1;
                objFromDb.Type2 = objDTO.Type2;
                objFromDb.Total = objDTO.Total;
                objFromDb.HP = objDTO.HP;
                objFromDb.Attack = objDTO.Attack;
                objFromDb.Defense = objDTO.Defense;
                objFromDb.SpAtk = objDTO.SpAtk;
                objFromDb.SpDef = objDTO.SpDef;
                objFromDb.Speed = objDTO.Speed;
                objFromDb.Generation = objDTO.Generation;
                objFromDb.Legendary = objDTO.Legendary;

                _logger.LogInformation("Updating a Pokemon.");
                await _db.SaveChangesAsync();
                return true; 
            }

            return false; 
        }

        public async Task<IEnumerable<PokemonDTO>> GetAll_PaginationOne(PokemonParameters parameters)
        {
            
            var fromDb = await _db.myPokemons.OrderBy(x => x.Id)
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();
            var response = _mapper.Map<IList<PokemonDTO>>(fromDb);
            return response;

        }


        public async Task<PagedList<MyPokemon>> GetAll_PaginationTwo(PokemonParameters parameters)
        {
            var pageList =  PagedList<MyPokemon>.ToPagedList2(_db.myPokemons.OrderBy(x => x.Id), parameters.PageNumber, parameters.PageSize);
            return  pageList;
        }






    }
}
