using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Pokemon.Application.Common.DTO;
using Pokemon.Application.Common.Interfaces;
using Pokemon.Domain.Entities;
using Pokemon.V1.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.UintTest
{
    public class PokemonControllerTest
    {
       private readonly IPokemonRepository _service;
        public PokemonControllerTest()
        {
            _service = A.Fake<IPokemonRepository>();
        }


        [Fact]
        public async void PokemonController_GetAllPokemons_ReturnOK()
        {
            //Arrange
            var Pokemons = A.Fake<List<PokemonDTO>>();
            var successResponse = A.Fake<SuccessResponse>();
            var paginationaparameters = A.Fake<PokemonParameters>();
            A.CallTo(() => _service.GetAll_PaginationOne(paginationaparameters))
                           .Returns(Pokemons);
            var controller = new PokemonController(_service);


            //Act
            var result = await  controller.GetAllPokemons(paginationaparameters);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));

            var okResult = Assert.IsType<OkObjectResult>(result);
            var successResp = Assert.IsType<SuccessResponse>(okResult.Value);        
        }



        [Fact]
        public async void PokemonController_GetPokemonById_ReturnOK()
        {
            //Arrange
            var id = 1;
            var successResponse = A.Fake<SuccessResponse>();
            var Pokemon = A.Fake<PokemonDTO>();
            A.CallTo(() => _service.Get(id))
                           .Returns(Pokemon);
            var controller = new PokemonController(_service);


            //Act
            var result = await controller.GetPokemonById(id);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
        }




        [Fact]
        public async void PokemonController_CreatePokemon_ReturnOK()
        {
            //Arrange
            var Pokemon = A.Fake<PokemonDTO>();
            var successResponse = A.Fake<SuccessResponse>();
            A.CallTo(() => _service.Create(Pokemon))
                           .Returns(Pokemon);
            var controller = new PokemonController(_service);


            //Act
            var result = await controller.CreatePokemon(Pokemon);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
        }


        [Fact]
        public async void PokemonController_UpdatePokemon_ReturnOK()
        {
            //Arrange
            var Pokemon = A.Fake<PokemonDTO>();
            var successResponse = A.Fake<SuccessResponse>();
            A.CallTo(() => _service.UpdatePokemonIfExists(Pokemon))
                           .Returns(true);
            var controller = new PokemonController(_service);


            //Act
            var result = await  controller.UpdatePokemon(Pokemon);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
        }


        [Fact]
        public async void PokemonController_DeletePokemon_ReturnOK()
        {
            //Arrange
            var id = 2;
            var Pokemon = A.Fake<PokemonDTO>();
            var successResponse = A.Fake<SuccessResponse>();
            A.CallTo(() => _service.DeletePokemonIfExists(id))
                           .Returns(true);
            var controller = new PokemonController(_service);


            //Act
            var result = await controller.DeletePokemon(id);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
        }




    }
}
