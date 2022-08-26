namespace CryptoZAPITests.System.Controllers;

public class TestHistoryController
{
    [Fact]
    public async void GetAll_ShouldReturn200Status()
    {
        /// Arrange
        var repositoryService = new Mock<IRepository>();
        var loggerService = new Mock<ILogger>();


        //repositoryService.Setup(_ => _.GetAllHistoriesForUser(guid,1)).Returns(HistoryMockData.GetAll());
        var sut = new HistoryController(null, repositoryService.Object, null);

        /// Act
        var result = await sut.GetAll(1, 1);
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

        //repositoryService.Setup(service => service.GetAllHistoriesForUser(guid, 1)).Returns(HistoryMockData.GetEmpty());
        var sut = new HistoryController(null, repositoryService.Object, null);

        /// Act
        var result = await sut.GetAll(1,1);
        var noContenetResult = result as NoContentResult;

        /// Assert
        noContenetResult.StatusCode.Should().Be(204);

        // Makes sure the repository is called only once 
        // repositoryService.Verify(service => service.GetAllCurrencies(), Times.Exactly(1));
    }


}

