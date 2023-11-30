using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Kernel; // Add this namespace for EnumRelay
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Backend.Tests.Controllers.Statistic
{
    public class LikesReceivedTests
    {
        private readonly IStatisticRepository _statisticRepository;

        public LikesReceivedTests()
        {
            _statisticRepository = A.Fake<IStatisticRepository>();
        }

        [Fact]
        public async Task LikesReceivedTests_ValidResponse_ReturnsOkResult()
        {
            // Arrange
            var fixture = new Fixture();
            int userId = fixture.Create<int>();
            int likes = fixture.Create<int>();
            A.CallTo(() => _statisticRepository.NumberOfLikesReceivedPerUser(userId))
                .Returns(likes);
            var controller = new StatisticController(_statisticRepository);

            // Act
            var actionResult = await controller.LikesReceivedPerUser(userId);
            var result = actionResult as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeEquivalentTo(new { status = "success", message = likes });
        }

        [Fact]
        public async Task LikesReceivedTests_Exception_ReturnsBadRequestResult()
        {
            // Arrange
            var fixture = new Fixture();
            int userId = fixture.Create<int>();
            A.CallTo(() => _statisticRepository.NumberOfLikesReceivedPerUser(userId))
                .Throws(new Exception("Something went wrong"));
            var controller = new StatisticController(_statisticRepository);

            // Act
            var actionResult = await controller.LikesReceivedPerUser(userId);
            var result = actionResult as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(500);
            result.Value.Should().BeEquivalentTo("An error occurred while processing the request: Something went wrong");
        }

    }
}