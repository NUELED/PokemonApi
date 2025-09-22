using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pokemon.Application.Common.DTO;
using Pokemon.Application.Common.Interfaces;
using Pokemon.Domain.Entities;
using Pokemon.Infrastructure.Cache;
using System.Dynamic;
using System.Security.Principal;
using System.Text.Json.Serialization;

namespace Pokemon.V1.Controllers
{
    
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    //[Route("api/[controller]")]
    [ApiVersion("1.0", Deprecated = false)]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonRepository _service;
        private readonly IMessagePublisher _messagePublisher;
        private readonly RedisCache _redisCache;   
        private bool _isFromCache = false;   
        public PokemonController(IPokemonRepository service, RedisCache redisCache, IMessagePublisher messagePublisher)
        {
            _service = service;
            _redisCache = redisCache;   
            _messagePublisher = messagePublisher;   
        }



        [HttpGet]
        [Route("getAllPokemons")]    
        public async Task<IActionResult> GetAllPokemons([FromQuery] PokemonParameters parameter)
        {
           // var instanceId = GetInstanceId();
           // var cacheKey = $"pokemons_Cache_{instanceId}";
            var cacheKey = "pokemons_Cache";
            try
            {
              var pokemonsFromCache = _redisCache.GetCachedData<IEnumerable<PokemonDTO>>(cacheKey);
                if (pokemonsFromCache == null) 
                {
                    var pokemons = await _service.GetAll_PaginationOne(parameter);
                    await _redisCache.SetCachedDataAsync(cacheKey, pokemons, TimeSpan.FromMinutes(5));
                    pokemonsFromCache = pokemons;
                    _isFromCache = false;
                }
                else
                    _isFromCache = true;    
               
                return Ok(new {Pokemons = pokemonsFromCache, IsFromCache = _isFromCache});
            }
            catch (Exception)
            {

                return BadRequest(new ErrorResponse
                {
                    ErrorMessage = "Please there was an issue processing your at the moment.",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
            
        }


        //[HttpGet]
        //[Route("getAllPokemons2")]
        //public async Task<IActionResult> GetAllPokemons2([FromQuery] PokemonParameters parameter)
        //{
        //    try
        //    {
        //        var pokey = await _service.GetAll_PaginationTwo(parameter);
        //        var metadata = new
        //        {
        //            pokey.TotalCount,
        //            pokey.PageSize,
        //            pokey.CurrentPage,
        //            pokey.HasNext,
        //            pokey.HasPrevious,
        //        };

        //        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
        //        var response = new SuccessResponse { Title = "Success", Data = pokey, StatusCode = StatusCodes.Status200OK, SuccessMessage = "Pokemons retrieved." };
        //        return Ok(response);
        //    }
        //    catch (Exception)
        //    {

        //        return BadRequest(new ErrorResponse
        //        {
        //            ErrorMessage = "Please there was an issue processing your at the moment.",
        //            StatusCode = StatusCodes.Status400BadRequest
        //        });
        //    }

        //}



        [HttpGet]   
        [Route("getPokemonById")]
        public async Task<IActionResult> GetPokemonById(int id)
        {
            try
            {
                if (id == null || id == 0)
                {
                    return BadRequest(new ErrorResponse { Title="Unsuccessfull", ErrorMessage = "Invalid Id", StatusCode = StatusCodes.Status400BadRequest });
                }
             
                var action = await _service.Get(id);
                var response = new SuccessResponse { Title = "Success", Data = action , StatusCode= StatusCodes.Status200OK, SuccessMessage="Pokemon retrieved."};
                return Ok(response);
            
            }
            catch (Exception)
            {

                return BadRequest(new ErrorResponse
                {
                    Title = "Unsuccessfull",
                    ErrorMessage = "The Pokemon does not exist.",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
     
        }


        [HttpPost]
        [Route("createPokemon")]
        public async Task<IActionResult> CreatePokemon([FromBody] PokemonDTO pokemonDTO)
        {
            if (!ModelState.IsValid) return BadRequest(pokemonDTO);
            try
            {
                var action = await _service.Create(pokemonDTO);
                var response = new SuccessResponse { Title = "Success", Data = action, StatusCode = StatusCodes.Status200OK, SuccessMessage = "Pokemon created." };

                 _messagePublisher.PublishMessageAsync("PokemonsQueue", action);
                return Ok(response);
            }
            catch (Exception)
            {


                return BadRequest(new ErrorResponse
                {
                    ErrorMessage = "Please there was an issue processing your at the moment.",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
          
        }



        [HttpPut]
        [Route("updatePokemon")]
        public async Task<IActionResult> UpdatePokemon(PokemonDTO pokemonDTO)
        {
            try
            {
                var isUpdated = await _service.UpdatePokemonIfExists(pokemonDTO);
                if (isUpdated)
                {
                    var response = new SuccessResponse { Title = "Success", StatusCode = StatusCodes.Status200OK, SuccessMessage = "Pokemon updated." };
                    return Ok(response);
                }
                else
                {
                    return NotFound(new ErrorResponse
                    {
                        ErrorMessage = "Pokemon not found",
                        StatusCode = StatusCodes.Status404NotFound
                    });
                }
            }
            catch (Exception)
            {
                return BadRequest(new ErrorResponse
                {
                    ErrorMessage = "Could not update Pokemon",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
        }



        [HttpDelete]
        [Route("deletePokemon")]
        public async Task<IActionResult> DeletePokemon(int id)
        {
            
            try
            {
                var isDeleted = await _service.DeletePokemonIfExists(id);
                if (isDeleted)
                {
                    var response = new SuccessResponse { Title = "Success", StatusCode = StatusCodes.Status200OK, SuccessMessage = "Pokemon deleted." };
                    return Ok(response);
                }
                else
                {
                    return NotFound(new ErrorResponse
                    {
                        ErrorMessage = "Pokemon not found",
                        StatusCode = StatusCodes.Status404NotFound
                    });
                }
            }
            catch (Exception)
            {
                return BadRequest(new ErrorResponse
                {
                    ErrorMessage = "Could not delete Pokemon",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }

        }

        private string GetInstanceId()
        {
            var instanceId = HttpContext.Session.GetString("InstanceId");
            if (string.IsNullOrEmpty(instanceId))
            {
                instanceId = Guid.NewGuid().ToString();
                HttpContext.Session.SetString("InstanceId", instanceId);
            }
            return instanceId;
        }

    }
}
