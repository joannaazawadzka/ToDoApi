using FluentAssertions;
using Moq;
using ToDo.Core.Application.RepositoryInterfaces;
using ToDo.Core.Application.ToDoTasksManagement.Queries.GetOne;
using ToDo.Core.Domain.Entities;
using Xunit;

namespace ToDo.Core.UnitTests.ToDoTasksManagement.Queries
{
    /// <summary>
    /// Unit tests for the GetToDoTaskQuery.Handler class.
    /// Verifies the behavior of the query handler responsible for retrieving a ToDo task.
    /// </summary>
    public class GetToDoTaskQueryHandlerTests
    {
        /// <summary>
        /// GIVEN a valid task ID for an existing ToDo task,
        /// WHEN the GetToDoTaskQuery is handled,
        /// THEN the handler should return a response with the task details.
        /// </summary>
        [Fact]
        public async Task GivenExistingTaskId_WhenGetToDoTaskQueryIsHandled_ThenTaskDetailsAreReturned()
        {
            // Arrange
            var existingToDoTask = new ToDoTaskEntity(DateTime.UtcNow.AddDays(1), "Task Title", "Task Description");

            var toDoTaskRepositoryMock = new Mock<IToDoTaskRepository>();
            toDoTaskRepositoryMock
                .Setup(repo => repo.GetOneReadonly(existingToDoTask.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingToDoTask);

            var handler = new GetToDoTaskQuery.Handler(toDoTaskRepositoryMock.Object);
            var query = new GetToDoTaskQuery(existingToDoTask.Id);

            // Act
            var response = await handler.Handle(query, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Id.Should().Be(existingToDoTask.Id);
            response.Title.Should().Be(existingToDoTask.Title);
            response.Description.Should().Be(existingToDoTask.Description);
            response.CompletionPercentage.Should().Be(existingToDoTask.CompletionPercentage);
            response.ExpiryAt.Should().Be(existingToDoTask.ExpiryAt);
            response.IsDone.Should().Be(existingToDoTask.IsDone());

            toDoTaskRepositoryMock.Verify(repo => repo.GetOneReadonly(existingToDoTask.Id, It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// GIVEN a task ID that does not correspond to any existing ToDo task,
        /// WHEN the GetToDoTaskQuery is handled,
        /// THEN the handler should throw an ApplicationException.
        /// </summary>
        [Fact]
        public async Task GivenNonExistingTaskId_WhenGetToDoTaskQueryIsHandled_ThenApplicationExceptionIsThrown()
        {
            // Arrange
            var taskId = Guid.NewGuid();  // A non-existing task ID

            var toDoTaskRepositoryMock = new Mock<IToDoTaskRepository>();
            toDoTaskRepositoryMock
                .Setup(repo => repo.GetOneReadonly(taskId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((ToDoTaskEntity)null);

            var handler = new GetToDoTaskQuery.Handler(toDoTaskRepositoryMock.Object);
            var query = new GetToDoTaskQuery(taskId);

            // Act
            Func<Task> act = async () => await handler.Handle(query, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ApplicationException>().WithMessage($"ToDo task with id: {taskId} does not exist");

            toDoTaskRepositoryMock.Verify(repo => repo.GetOneReadonly(taskId, It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// GIVEN a task ID with invalid data or a null response,
        /// WHEN the GetToDoTaskQuery is handled,
        /// THEN the handler should return an appropriate error response or throw.
        /// </summary>
        [Fact]
        public async Task GivenInvalidTaskId_WhenGetToDoTaskQueryIsHandled_ThenApplicationExceptionIsThrown()
        {
            // Arrange
            var invalidTaskId = Guid.Empty;

            var toDoTaskRepositoryMock = new Mock<IToDoTaskRepository>();

            var handler = new GetToDoTaskQuery.Handler(toDoTaskRepositoryMock.Object);
            var query = new GetToDoTaskQuery(invalidTaskId);

            // Act
            Func<Task> act = async () => await handler.Handle(query, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ApplicationException>().WithMessage($"ToDo task with id: {invalidTaskId} does not exist");

            toDoTaskRepositoryMock.Verify(repo => repo.GetOneReadonly(invalidTaskId, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
