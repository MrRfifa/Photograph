using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Kernel; // Add this namespace for EnumRelay
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Backend.Tests.Controllers.Statistic
{
    public class CommentsDoneTests
    {
        private readonly IStatisticRepository _statisticRepository;

        public CommentsDoneTests()
        {
            _statisticRepository = A.Fake<IStatisticRepository>();
        }
        [Fact]
        public async Task CommentsDoneTests_Exception_ReturnsBadRequestResult()
        {
            // Arrange
            var fixture = new Fixture();
            int userId = fixture.Create<int>();
            A.CallTo(() => _statisticRepository.NumberOfCommentsDonePerUser(userId))
                .Throws(new Exception("Something went wrong"));
            var controller = new StatisticController(_statisticRepository);

            // Act
            var actionResult = await controller.CommentsDonePerUser(userId);
            var result = actionResult as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(500);
            result.Value.Should().BeEquivalentTo("An error occurred while processing the request: Something went wrong");
        }

        [Fact]
        public async Task CommentsDoneTests_ValidResponse_ReturnsOkResult()
        {
            // Arrange
            var fixture = new Fixture();
            int userId = fixture.Create<int>();
            int comments = fixture.Create<int>();
            A.CallTo(() => _statisticRepository.NumberOfCommentsDonePerUser(userId))
                .Returns(comments);
            var controller = new StatisticController(_statisticRepository);

            // Act
            var actionResult = await controller.CommentsDonePerUser(userId);
            var result = actionResult as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeEquivalentTo(new { status = "success", message = comments });
        }


    }
}