using FluentAssertions;
using MediatR;
using Moq;
using ToDo.Core.Application.RepositoryInterfaces;
using ToDo.Core.Application.ToDoTasksManagement.Commands.DeleteOne;
using ToDo.Core.Domain.Entities;
using Xunit;

namespace ToDo.Core.UnitTests.ToDoTasksManagement.Commands
{
    /// <summary>
    /// Unit tests for the DeleteToDoTaskCommand.Handler class.
    /// Verifies the behavior of the command handler responsible for deleting a ToDo task.
    /// </summary>
    public class DeleteToDoTaskCommandHandlerTests
    {
        /// <summary>
        /// GIVEN a valid task ID for an existing ToDo task,
        /// WHEN the DeleteToDoTaskCommand is handled,
        /// THEN the task should be deleted, and the handler should return Unit.Value.
        /// </summary>
        [Fact]
        public async Task GivenExistingTaskId_WhenDeleteToDoTaskCommandIsHandled_ThenTaskIsDeletedAndUnitValueReturned()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var toDoTask = new ToDoTaskEntity(DateTime.UtcNow.AddDays(1), "Test Task", "Description");

            var toDoTaskRepositoryMock = new Mock<IToDoTaskRepository>();
            toDoTaskRepositoryMock
                .Setup(repo => repo.GetOne(taskId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(toDoTask);

            toDoTaskRepositoryMock
                .Setup(repo => repo.Delete(It.IsAny<ToDoTaskEntity>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var handler = new DeleteToDoTaskCommand.Handler(toDoTaskRepositoryMock.Object);

            var command = new DeleteToDoTaskCommand(taskId);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);

            toDoTaskRepositoryMock.Verify(repo => repo.GetOne(taskId, It.IsAny<CancellationToken>()), Times.Once);
            toDoTaskRepositoryMock.Verify(repo => repo.Delete(toDoTask, It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// GIVEN a task ID that does not correspond to any existing ToDo task,
        /// WHEN the DeleteToDoTaskCommand is handled,
        /// THEN the handler should throw an ApplicationException and no delete operation should occur.
        /// </summary>
        [Fact]
        public async Task GivenNonExistingTaskId_WhenDeleteToDoTaskCommandIsHandled_ThenApplicationExceptionIsThrownAndTaskIsNotDeleted()
        {
            // Arrange
            var taskId = Guid.NewGuid();

            var toDoTaskRepositoryMock = new Mock<IToDoTaskRepository>();
            toDoTaskRepositoryMock
                .Setup(repo => repo.GetOne(taskId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((ToDoTaskEntity)null);

            var handler = new DeleteToDoTaskCommand.Handler(toDoTaskRepositoryMock.Object);

            var command = new DeleteToDoTaskCommand(taskId);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ApplicationException>();

            toDoTaskRepositoryMock.Verify(repo => repo.GetOne(taskId, It.IsAny<CancellationToken>()), Times.Once);
            toDoTaskRepositoryMock.Verify(repo => repo.Delete(It.IsAny<ToDoTaskEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
