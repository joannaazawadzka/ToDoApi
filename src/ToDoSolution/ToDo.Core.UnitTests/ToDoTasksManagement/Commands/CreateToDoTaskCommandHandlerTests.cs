using FluentAssertions;
using Moq;
using ToDo.Core.Application.RepositoryInterfaces;
using ToDo.Core.Application.ToDoTasksManagement.Commands.CreateOne;
using ToDo.Core.Domain.Entities;
using Xunit;

namespace ToDo.Core.UnitTests.ToDoTasksManagement.Commands
{
    /// <summary>
    /// Unit tests for the CreateToDoTaskCommand.Handler class.
    /// Verifies the behavior of the command handler responsible for creating a ToDo task.
    /// </summary>
    public class CreateToDoTaskCommandHandlerTests
    {
        /// <summary>
        /// GIVEN valid details for a new ToDo task,
        /// WHEN the CreateToDoTaskCommand method is handled,
        /// THEN the task should be created, and a valid response with task details should be returned.
        /// </summary>
        [Fact]
        public async Task GivenValidTaskDetails_WhenCreateToDoTaskCommandIsHandled_ThenTaskIsCreatedAndResponseIsReturned()
        {
            // Arrange
            var expiryAt = DateTime.UtcNow.AddDays(1);
            var title = "Test Task";
            var description = "This is a test description";

            var toDoTaskRepositoryMock = new Mock<IToDoTaskRepository>();
            toDoTaskRepositoryMock
                .Setup(repo => repo.Create(It.IsAny<ToDoTaskEntity>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var handler = new CreateToDoTaskCommand.Handler(toDoTaskRepositoryMock.Object);

            var command = new CreateToDoTaskCommand(expiryAt, title, description);

            // Act
            var response = await handler.Handle(command, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.ExpiryAt.Should().Be(expiryAt);
            response.Title.Should().Be(title);
            response.Description.Should().Be(description);

            toDoTaskRepositoryMock.Verify(repo => repo.Create(It.IsAny<ToDoTaskEntity>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
