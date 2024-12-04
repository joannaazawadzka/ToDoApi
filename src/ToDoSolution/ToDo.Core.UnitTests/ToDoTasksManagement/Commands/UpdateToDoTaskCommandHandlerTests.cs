using FluentAssertions;
using Moq;
using ToDo.Core.Application.RepositoryInterfaces;
using ToDo.Core.Application.ToDoTasksManagement.Commands.UpdateCompletionPercentage;
using ToDo.Core.Domain.Entities;
using Xunit;

namespace ToDo.Core.UnitTests.ToDoTasksManagement.Commands
{
    /// <summary>
    /// Unit tests for the UpdateToDoTaskCommand.Handler class.
    /// Verifies the behavior of the command handler responsible for updating a ToDo task.
    /// </summary>
    public class UpdateToDoTaskCommandHandlerTests
    {
        /// <summary>
        /// GIVEN a valid task ID for an existing ToDo task with new properties,
        /// WHEN the UpdateToDoTaskCommand is handled,
        /// THEN the task should be updated, and the handler should return a response with the updated task.
        /// </summary>
        [Fact]
        public async Task GivenExistingTaskId_WhenUpdateToDoTaskCommandIsHandled_ThenTaskIsUpdatedAndResponseIsReturned()
        {
            // Arrange
            var existingToDoTask = new ToDoTaskEntity(DateTime.UtcNow.AddDays(1), "Old Task", "Old Description");

            var updatedTitle = "Updated Task";
            var updatedDescription = "Updated Description";
            var updatedCompletionPercentage = 80;
            var updatedExpiryAt = DateTime.UtcNow.AddDays(2);

            var toDoTaskRepositoryMock = new Mock<IToDoTaskRepository>();
            toDoTaskRepositoryMock
                .Setup(repo => repo.GetOne(existingToDoTask.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingToDoTask);

            toDoTaskRepositoryMock
                .Setup(repo => repo.Update(It.IsAny<ToDoTaskEntity>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var handler = new UpdateToDoTaskCommand.Handler(toDoTaskRepositoryMock.Object);

            var command = new UpdateToDoTaskCommand(existingToDoTask.Id, updatedExpiryAt, updatedTitle, updatedDescription, updatedCompletionPercentage);

            // Act
            var response = await handler.Handle(command, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Id.Should().Be(existingToDoTask.Id);
            response.Title.Should().Be(updatedTitle);
            response.Description.Should().Be(updatedDescription);
            response.CompletionPercentage.Should().Be(updatedCompletionPercentage);
            response.ExpiryAt.Should().Be(updatedExpiryAt);

            toDoTaskRepositoryMock.Verify(repo => repo.GetOne(existingToDoTask.Id, It.IsAny<CancellationToken>()), Times.Once);
            toDoTaskRepositoryMock.Verify(repo => repo.Update(It.IsAny<ToDoTaskEntity>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// GIVEN a task ID that does not correspond to any existing ToDo task,
        /// WHEN the UpdateToDoTaskCommand is handled,
        /// THEN the handler should throw an ApplicationException and no update operation should occur.
        /// </summary>
        [Fact]
        public async Task GivenNonExistingTaskId_WhenUpdateToDoTaskCommandIsHandled_ThenApplicationExceptionIsThrownAndTaskIsNotUpdated()
        {
            // Arrange
            var taskId = Guid.NewGuid();

            var toDoTaskRepositoryMock = new Mock<IToDoTaskRepository>();
            toDoTaskRepositoryMock
                .Setup(repo => repo.GetOne(taskId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((ToDoTaskEntity)null);

            var handler = new UpdateToDoTaskCommand.Handler(toDoTaskRepositoryMock.Object);

            var command = new UpdateToDoTaskCommand(taskId, DateTime.UtcNow, "New Task", "New Description", 10);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ApplicationException>();

            toDoTaskRepositoryMock.Verify(repo => repo.GetOne(taskId, It.IsAny<CancellationToken>()), Times.Once);
            toDoTaskRepositoryMock.Verify(repo => repo.Update(It.IsAny<ToDoTaskEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        /// <summary>
        /// GIVEN a task ID with no changes to any properties,
        /// WHEN the UpdateToDoTaskCommand is handled,
        /// THEN the task should not be updated, and no update operation should occur.
        /// </summary>
        [Fact]
        public async Task GivenExistingTaskIdWithNoChanges_WhenUpdateToDoTaskCommandIsHandled_ThenNoUpdateIsPerformed()
        {
            // Arrange
            var existingToDoTask = new ToDoTaskEntity(DateTime.UtcNow.AddDays(1), "Task", "Description");

            var toDoTaskRepositoryMock = new Mock<IToDoTaskRepository>();
            toDoTaskRepositoryMock
                .Setup(repo => repo.GetOne(existingToDoTask.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingToDoTask);

            var handler = new UpdateToDoTaskCommand.Handler(toDoTaskRepositoryMock.Object);

            var command = new UpdateToDoTaskCommand(existingToDoTask.Id, existingToDoTask.ExpiryAt, existingToDoTask.Title, existingToDoTask.Description, existingToDoTask.CompletionPercentage);

            // Act
            var response = await handler.Handle(command, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Id.Should().Be(existingToDoTask.Id);
            response.Title.Should().Be(existingToDoTask.Title);
            response.Description.Should().Be(existingToDoTask.Description);
            response.CompletionPercentage.Should().Be(existingToDoTask.CompletionPercentage);
            response.ExpiryAt.Should().Be(existingToDoTask.ExpiryAt);
            response.UpdatedAt.Should().BeNull();

            toDoTaskRepositoryMock.Verify(repo => repo.GetOne(existingToDoTask.Id, It.IsAny<CancellationToken>()), Times.Once);
            toDoTaskRepositoryMock.Verify(repo => repo.Update(It.IsAny<ToDoTaskEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
