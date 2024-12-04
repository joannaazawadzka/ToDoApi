﻿using FluentAssertions;
using ToDo.Core.Domain.Entities;
using Xunit;

namespace ToDo.Core.UnitTests.Domain.Entities
{
    public class ToDoTaskEntityTests
    {
        /// <summary>
        /// GIVEN valid parameters for expiry date, title, and description,
        /// WHEN a new ToDoTaskEntity is created,
        /// THEN the properties should be correctly initialized, including the inherited base properties.
        /// </summary>
        [Fact]
        public void GivenValidParameters_WhenToDoTaskEntityIsCreated_ThenPropertiesAreInitialized()
        {
            // Arrange
            var expiryAt = DateTime.UtcNow.AddDays(1);
            var title = "Test Task";
            var description = "Test Description";

            // Act
            var toDoTask = new ToDoTaskEntity(expiryAt, title, description);

            // Assert
            toDoTask.ExpiryAt.Should().Be(expiryAt);
            toDoTask.Title.Should().Be(title);
            toDoTask.Description.Should().Be(description);
            toDoTask.CompletionPercentage.Should().Be(0); // Default value
            toDoTask.Id.Should().NotBeEmpty(); // Id should be generated by the base class
        }

        /// <summary>
        /// GIVEN a ToDoTaskEntity,
        /// WHEN MarkAsDone is called,
        /// THEN the completion percentage should be set to 100.
        /// </summary>
        [Fact]
        public void GivenToDoTaskEntity_WhenMarkAsDoneIsCalled_ThenCompletionPercentageIs100()
        {
            // Arrange
            var toDoTask = new ToDoTaskEntity(DateTime.UtcNow.AddDays(1), "Test Task", "Test Description");

            // Act
            toDoTask.MarkAsDone();

            // Assert
            toDoTask.CompletionPercentage.Should().Be(100);
        }

        /// <summary>
        /// GIVEN a ToDoTaskEntity with a completion percentage,
        /// WHEN IsDone is called,
        /// THEN it should return true if the completion percentage is 100, false otherwise.
        /// </summary>
        [Fact]
        public void GivenToDoTaskEntity_WhenIsDoneIsCalled_ThenItReturnsCorrectStatus()
        {
            // Arrange
            var toDoTaskNotDone = new ToDoTaskEntity(DateTime.UtcNow.AddDays(1), "Test Task", "Test Description");
            var toDoTaskDone = new ToDoTaskEntity(DateTime.UtcNow.AddDays(1), "Test Task", "Test Description");
            toDoTaskDone.MarkAsDone();

            // Act & Assert
            toDoTaskNotDone.IsDone().Should().BeFalse();
            toDoTaskDone.IsDone().Should().BeTrue();
        }

        /// <summary>
        /// GIVEN a ToDoTaskEntity with a new title,
        /// WHEN UpdateTitle is called with a valid title,
        /// THEN the title should be updated.
        /// </summary>
        [Fact]
        public void GivenToDoTaskEntity_WhenUpdateTitleIsCalled_ThenTitleIsUpdated()
        {
            // Arrange
            var toDoTask = new ToDoTaskEntity(DateTime.UtcNow.AddDays(1), "Old Title", "Test Description");

            // Act
            toDoTask.UpdateTitle("New Title");

            // Assert
            toDoTask.Title.Should().Be("New Title");
        }

        /// <summary>
        /// GIVEN a ToDoTaskEntity,
        /// WHEN UpdateTitle is called with a null or empty title,
        /// THEN an ApplicationException should be thrown.
        /// </summary>
        [Fact]
        public void GivenToDoTaskEntity_WhenUpdateTitleIsCalledWithInvalidTitle_ThenApplicationExceptionIsThrown()
        {
            // Arrange
            var toDoTask = new ToDoTaskEntity(DateTime.UtcNow.AddDays(1), "Old Title", "Test Description");

            // Act
            Action actNullTitle = () => toDoTask.UpdateTitle(null);
            Action actEmptyTitle = () => toDoTask.UpdateTitle("");

            // Assert
            actNullTitle.Should().Throw<ApplicationException>().WithMessage($"For the ToDo task with id: {toDoTask.Id} the title is null or empty");
            actEmptyTitle.Should().Throw<ApplicationException>().WithMessage($"For the ToDo task with id: {toDoTask.Id} the title is null or empty");
        }

        /// <summary>
        /// GIVEN a ToDoTaskEntity,
        /// WHEN UpdateDescription is called,
        /// THEN the description should be updated.
        /// </summary>
        [Fact]
        public void GivenToDoTaskEntity_WhenUpdateDescriptionIsCalled_ThenDescriptionIsUpdated()
        {
            // Arrange
            var toDoTask = new ToDoTaskEntity(DateTime.UtcNow.AddDays(1), "Test Task", "Old Description");

            // Act
            toDoTask.UpdateDescription("New Description");

            // Assert
            toDoTask.Description.Should().Be("New Description");
        }

        /// <summary>
        /// GIVEN a ToDoTaskEntity,
        /// WHEN UpdateCompletionPercentage is called with a valid value,
        /// THEN the completion percentage should be updated.
        /// </summary>
        [Fact]
        public void GivenToDoTaskEntity_WhenUpdateCompletionPercentageIsCalledWithValidValue_ThenCompletionPercentageIsUpdated()
        {
            // Arrange
            var toDoTask = new ToDoTaskEntity(DateTime.UtcNow.AddDays(1), "Test Task", "Test Description");

            // Act
            toDoTask.UpdateCompletionPercentage(75);

            // Assert
            toDoTask.CompletionPercentage.Should().Be(75);
        }

        /// <summary>
        /// GIVEN a ToDoTaskEntity,
        /// WHEN UpdateCompletionPercentage is called with an invalid value outside the 0-100 range,
        /// THEN an ApplicationException should be thrown.
        /// </summary>
        [Fact]
        public void GivenToDoTaskEntity_WhenUpdateCompletionPercentageIsCalledWithInvalidValue_ThenApplicationExceptionIsThrown()
        {
            // Arrange
            var toDoTask = new ToDoTaskEntity(DateTime.UtcNow.AddDays(1), "Test Task", "Test Description");

            // Act
            Action actInvalidValueNegative = () => toDoTask.UpdateCompletionPercentage(-1);
            Action actInvalidValueTooHigh = () => toDoTask.UpdateCompletionPercentage(101);

            // Assert
            actInvalidValueNegative.Should().Throw<ApplicationException>().WithMessage($"For the ToDo task with id: {toDoTask.Id} the completionPercentage must be between 0 - 100");
            actInvalidValueTooHigh.Should().Throw<ApplicationException>().WithMessage($"For the ToDo task with id: {toDoTask.Id} the completionPercentage must be between 0 - 100");
        }

        /// <summary>
        /// GIVEN a ToDoTaskEntity,
        /// WHEN UpdateExpiryAt is called with a new expiry date,
        /// THEN the expiry date should be updated.
        /// </summary>
        [Fact]
        public void GivenToDoTaskEntity_WhenUpdateExpiryAtIsCalled_ThenExpiryAtIsUpdated()
        {
            // Arrange
            var toDoTask = new ToDoTaskEntity(DateTime.UtcNow.AddDays(1), "Test Task", "Test Description");
            var newExpiryAt = DateTime.UtcNow.AddDays(5);

            // Act
            toDoTask.UpdateExpiryAt(newExpiryAt);

            // Assert
            toDoTask.ExpiryAt.Should().Be(newExpiryAt);
        }

        /// <summary>
        /// GIVEN a ToDoTaskEntity,
        /// WHEN SetCreatedAt is called,
        /// THEN CreatedAt should be set to the correct date.
        /// </summary>
        [Fact]
        public void GivenToDoTaskEntity_WhenSetCreatedAtIsCalled_ThenCreatedAtIsSetCorrectly()
        {
            // Arrange
            var toDoTask = new ToDoTaskEntity(DateTime.UtcNow.AddDays(1), "Test Task", "Test Description");
            var createdAt = DateTime.UtcNow;

            // Act
            toDoTask.SetCreatedAt(createdAt);

            // Assert
            toDoTask.CreatedAt.Should().Be(createdAt);
        }

        /// <summary>
        /// GIVEN a ToDoTaskEntity,
        /// WHEN SetUpdatedAt is called,
        /// THEN UpdatedAt should be set correctly.
        /// </summary>
        [Fact]
        public void GivenToDoTaskEntity_WhenSetUpdatedAtIsCalled_ThenUpdatedAtIsSetCorrectly()
        {
            // Arrange
            var toDoTask = new ToDoTaskEntity(DateTime.UtcNow.AddDays(1), "Test Task", "Test Description");
            var updatedAt = DateTime.UtcNow.AddHours(1);

            // Act
            toDoTask.SetUpdatedAt(updatedAt);

            // Assert
            toDoTask.UpdatedAt.Should().Be(updatedAt);
        }
    }
}