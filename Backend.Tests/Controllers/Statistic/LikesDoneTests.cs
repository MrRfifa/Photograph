
namespace Backend.Tests.Controllers.Statistic
{
    public class LikesDoneTests
    {
        private readonly IStatisticRepository _statisticRepository;

        public LikesDoneTests()
        {
            _statisticRepository = A.Fake<IStatisticRepository>();
        }
        [Fact]
        public async Task LikesDoneTests_Exception_ReturnsBadRequestResult()
        {
            // Arrange
            var fixture = new Fixture();
            int userId = fixture.Create<int>();
            A.CallTo(() => _statisticRepository.NumberOfLikesDonePerUser(userId))
                .Throws(new Exception("Something went wrong"));
            var controller = new StatisticController(_statisticRepository);

            // Act
            var actionResult = await controller.LikesDonePerUser(userId);
            var result = actionResult as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(500);
            result.Value.Should().BeEquivalentTo("An error occurred while processing the request: Something went wrong");
        }

        [Fact]
        public async Task LikesDoneTests_ValidResponse_ReturnsOkResult()
        {
            // Arrange
            var fixture = new Fixture();
            int userId = fixture.Create<int>();
            int likes = fixture.Create<int>();
            A.CallTo(() => _statisticRepository.NumberOfLikesDonePerUser(userId))
                .Returns(likes);
            var controller = new StatisticController(_statisticRepository);

            // Act
            var actionResult = await controller.LikesDonePerUser(userId);
            var result = actionResult as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeEquivalentTo(new { status = "success", message = likes });
        }
    }
}