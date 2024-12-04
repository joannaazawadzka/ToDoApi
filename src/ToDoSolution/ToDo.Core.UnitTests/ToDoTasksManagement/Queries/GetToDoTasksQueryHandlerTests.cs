using FluentAssertions;
using Moq;
using ToDo.Core.Application.RepositoryInterfaces;
using ToDo.Core.Application.ToDoTasksManagement.Queries.GetMany;
using ToDo.Core.Domain.Entities;
using Xunit;

namespace ToDo.Core.UnitTests.ToDoTasksManagement.Queries
{
    /// <summary>
    /// Unit tests for the GetToDoTasksQuery.Handler class.
    /// Verifies the behavior of the query handler responsible for retrieving multiple ToDo tasks.
    /// </summary>
    public class GetToDoTasksQueryHandlerTests
    {
        /// <summary>
        /// GIVEN a filter for retrieving all tasks,
        /// WHEN the GetToDoTasksQuery is handled,
        /// THEN all tasks should be returned.
        /// </summary>
        [Fact]
        public async Task GivenAllTasksFilter_WhenGetToDoTasksQueryIsHandled_ThenAllTasksAreReturned()
        {
            // Arrange
            var tasks = new List<ToDoTaskEntity>
            {
                new ToDoTaskEntity(DateTime.UtcNow.AddDays(1), "Task 1", "Description 1"),
                new ToDoTaskEntity(DateTime.UtcNow.AddDays(2), "Task 2", "Description 2")
            };

            var toDoTaskRepositoryMock = new Mock<IToDoTaskRepository>();
            toDoTaskRepositoryMock
                .Setup(repo => repo.GetManyReadonly(GetToDoTasksFilterEnum.All, It.IsAny<CancellationToken>()))
                .ReturnsAsync(tasks);

            var handler = new GetToDoTasksQuery.Handler(toDoTaskRepositoryMock.Object);
            var query = new GetToDoTasksQuery(GetToDoTasksFilterEnum.All);

            // Act
            var response = await handler.Handle(query, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Should().HaveCount(2);
            response.First().Title.Should().Be("Task 1");
            response.Last().Title.Should().Be("Task 2");

            toDoTaskRepositoryMock.Verify(repo => repo.GetManyReadonly(GetToDoTasksFilterEnum.All, It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// GIVEN a filter for tasks only for today,
        /// WHEN the GetToDoTasksQuery is handled,
        /// THEN only today's tasks should be returned.
        /// </summary>
        [Fact]
        public async Task GivenOnlyTodayFilter_WhenGetToDoTasksQueryIsHandled_ThenOnlyTodayTasksAreReturned()
        {
            // Arrange
            var tasks = new List<ToDoTaskEntity>
            {
                new ToDoTaskEntity(DateTime.UtcNow, "Today Task", "Description")
            };

            var toDoTaskRepositoryMock = new Mock<IToDoTaskRepository>();
            toDoTaskRepositoryMock
                .Setup(repo => repo.GetManyReadonly(GetToDoTasksFilterEnum.OnlyToday, It.IsAny<CancellationToken>()))
                .ReturnsAsync(tasks);

            var handler = new GetToDoTasksQuery.Handler(toDoTaskRepositoryMock.Object);
            var query = new GetToDoTasksQuery(GetToDoTasksFilterEnum.OnlyToday);

            // Act
            var response = await handler.Handle(query, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Should().HaveCount(1);
            response.First().Title.Should().Be("Today Task");

            toDoTaskRepositoryMock.Verify(repo => repo.GetManyReadonly(GetToDoTasksFilterEnum.OnlyToday, It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// GIVEN a filter that does not match any tasks,
        /// WHEN the GetToDoTasksQuery is handled,
        /// THEN an empty list should be returned.
        /// </summary>
        [Fact]
        public async Task GivenNoMatchingTasks_WhenGetToDoTasksQueryIsHandled_ThenEmptyListIsReturned()
        {
            // Arrange
            var tasks = new List<ToDoTaskEntity>();

            var toDoTaskRepositoryMock = new Mock<IToDoTaskRepository>();
            toDoTaskRepositoryMock
                .Setup(repo => repo.GetManyReadonly(GetToDoTasksFilterEnum.OnlyNextWeek, It.IsAny<CancellationToken>()))
                .ReturnsAsync(tasks);

            var handler = new GetToDoTasksQuery.Handler(toDoTaskRepositoryMock.Object);
            var query = new GetToDoTasksQuery(GetToDoTasksFilterEnum.OnlyNextWeek);

            // Act
            var response = await handler.Handle(query, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Should().BeEmpty();

            toDoTaskRepositoryMock.Verify(repo => repo.GetManyReadonly(GetToDoTasksFilterEnum.OnlyNextWeek, It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// GIVEN a filter and tasks available,
        /// WHEN the GetToDoTasksQuery is handled,
        /// THEN the handler should return a correctly filtered list.
        /// </summary>
        [Fact]
        public async Task GivenFilterWithTasks_WhenGetToDoTasksQueryIsHandled_ThenFilteredTasksAreReturned()
        {
            // Arrange
            var tasks = new List<ToDoTaskEntity>
            {
                new ToDoTaskEntity(DateTime.UtcNow.AddDays(1), "Next Day Task", "Description 1"),
                new ToDoTaskEntity(DateTime.UtcNow.AddDays(7), "Next Week Task", "Description 2")
            };

            var toDoTaskRepositoryMock = new Mock<IToDoTaskRepository>();
            toDoTaskRepositoryMock
                .Setup(repo => repo.GetManyReadonly(GetToDoTasksFilterEnum.OnlyNextDay, It.IsAny<CancellationToken>()))
                .ReturnsAsync(tasks.Where(t => t.ExpiryAt.Date == DateTime.UtcNow.AddDays(1).Date).ToList());

            var handler = new GetToDoTasksQuery.Handler(toDoTaskRepositoryMock.Object);
            var query = new GetToDoTasksQuery(GetToDoTasksFilterEnum.OnlyNextDay);

            // Act
            var response = await handler.Handle(query, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Should().HaveCount(1);
            response.First().Title.Should().Be("Next Day Task");

            toDoTaskRepositoryMock.Verify(repo => repo.GetManyReadonly(GetToDoTasksFilterEnum.OnlyNextDay, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
