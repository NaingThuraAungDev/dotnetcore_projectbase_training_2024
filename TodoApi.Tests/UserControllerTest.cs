using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using TodoApi.Controllers;
using TodoApi.DTO;
using TodoApi.Models;

namespace TodoApi.Tests
{
    public class UserControllerTest
    {
        [Fact]
        public async Task GetUser_ReturnsExpectedUsers()
        {
            // Arrange
            var mockSet = new Mock<DbSet<User>>();
            var mockContext = new Mock<AppDB>();
            mockContext.Setup(m => m.User).Returns(mockSet.Object);

            var users = new List<User>
            {
                new User { user_id = 1, user_name = "TestUser1", is_lock = false, login_fail_count = 0 },
            }.AsQueryable();

            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

            var controller = new UserController(mockContext.Object);

            // Act
            var result = await controller.GetUser();

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedUsers = Assert.IsType<List<GetUserResponseDTO>>(actionResult.Value);
            Assert.Equal(users.Count(), returnedUsers.Count);
            // Further assertions to verify the content of returnedUsers can be added here
        }

    }
}