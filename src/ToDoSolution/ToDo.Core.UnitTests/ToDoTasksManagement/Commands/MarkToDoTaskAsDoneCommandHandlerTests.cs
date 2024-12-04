﻿using FluentAssertions;
using Moq;
using ToDo.Core.Application.RepositoryInterfaces;
using ToDo.Core.Application.ToDoTasksManagement.Commands.MarkAsDone;
using ToDo.Core.Domain.Entities;
using Xunit;

namespace ToDo.Core.UnitTests.ToDoTasksManagement.Commands
{
    /// <summary>
    /// Unit tests for the MarkToDoTaskAsDoneCommand.Handler class.
    /// Verifies the behavior of the command handler responsible for marking a ToDo task as done.
    /// </summary>
    public class MarkToDoTaskAsDoneCommandHandlerTests
    {
        /// <summary>
        /// GIVEN a valid task ID for an existing ToDo task,
        /// WHEN the MarkToDoTaskAsDoneCommand is handled,
        /// THEN the task should be marked as done, and the handler should return a response with the updated task, especially CompletionPercentage should equal 100.
        /// </summary>
        [Fact]
        public async Task GivenExistingTaskId_WhenMarkToDoTaskAsDoneCommandIsHandled_ThenTaskIsMarkedAsDoneAndResponseIsReturned()
        {
            // Arrange
            var toDoTask = new ToDoTaskEntity(DateTime.UtcNow.AddDays(1), "Test Task", "Description");

            var toDoTaskRepositoryMock = new Mock<IToDoTaskRepository>();
            toDoTaskRepositoryMock
                .Setup(repo => repo.GetOne(toDoTask.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(toDoTask);

            toDoTaskRepositoryMock
                .Setup(repo => repo.Update(It.IsAny<ToDoTaskEntity>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var handler = new MarkToDoTaskAsDoneCommand.Handler(toDoTaskRepositoryMock.Object);

            var command = new MarkToDoTaskAsDoneCommand(toDoTask.Id);

            // Act
            var response = await handler.Handle(command, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Id.Should().Be(toDoTask.Id);
            response.Title.Should().Be(toDoTask.Title);
            response.Description.Should().Be(toDoTask.Description);
            response.CompletionPercentage.Should().Be(100);  // Marked as done
            response.ExpiryAt.Should().Be(toDoTask.ExpiryAt);

            toDoTaskRepositoryMock.Verify(repo => repo.GetOne(toDoTask.Id, It.IsAny<CancellationToken>()), Times.Once);
            toDoTaskRepositoryMock.Verify(repo => repo.Update(toDoTask, It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// GIVEN a task ID that does not correspond to any existing ToDo task,
        /// WHEN the MarkToDoTaskAsDoneCommand is handled,
        /// THEN the handler should throw an ApplicationException and no update operation should occur.
        /// </summary>
        [Fact]
        public async Task GivenNonExistingTaskId_WhenMarkToDoTaskAsDoneCommandIsHandled_ThenApplicationExceptionIsThrownAndTaskIsNotUpdated()
        {
            // Arrange
            var taskId = Guid.NewGuid();

            var toDoTaskRepositoryMock = new Mock<IToDoTaskRepository>();
            toDoTaskRepositoryMock
                .Setup(repo => repo.GetOne(taskId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((ToDoTaskEntity)null);

            var handler = new MarkToDoTaskAsDoneCommand.Handler(toDoTaskRepositoryMock.Object);

            var command = new MarkToDoTaskAsDoneCommand(taskId);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ApplicationException>();

            toDoTaskRepositoryMock.Verify(repo => repo.GetOne(taskId, It.IsAny<CancellationToken>()), Times.Once);
            toDoTaskRepositoryMock.Verify(repo => repo.Update(It.IsAny<ToDoTaskEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
