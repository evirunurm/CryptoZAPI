using Castle.Core.Logging;
using CryptoZAPI.Controllers;
using CryptoZAPITests.MockData;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NomixServices;
using Repo;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CryptoZAPITests.System.Controllers;

public class TestCurrencyController
{
    [Fact]
    public async void GetAll_ShouldReturn200Status()
    {
        /// Arrange
        var repositoryService = new Mock<IRepository>();
        var loggerService = new Mock<ILogger>();
        var nomicsService = new Mock<INomics>();


        repositoryService.Setup(_ => _.GetAllCurrencies()).Returns(CurrencyMockData.GetAll());
        var sut = new CurrenciesController(null, nomicsService.Object, repositoryService.Object);

        /// Act
        var result = await sut.GetAll();
        var okObjectResult = result as OkObjectResult;

        // /// Assert
        okObjectResult.StatusCode.Should().Be(200);
    }

    [Fact]
    public async void GetEmpty_ShouldReturn204Status()
    {
        /// Arrange
        var repositoryService = new Mock<IRepository>();
        var loggerService = new Mock<ILogger>();
        var nomicsService = new Mock<INomics>();

        repositoryService.Setup(service => service.GetAllCurrencies()).Returns(CurrencyMockData.GetEmpty());
        var sut = new CurrenciesController(null, nomicsService.Object, repositoryService.Object);

        /// Act
        var result = await sut.GetAll();
        var noContenetResult = result as NoContentResult;

        /// Assert
        noContenetResult.StatusCode.Should().Be(204);

        // Makes sure the repository is called only once 
        // repositoryService.Verify(service => service.GetAllCurrencies(), Times.Exactly(1));
    }


}

