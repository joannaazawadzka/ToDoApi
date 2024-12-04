namespace ToDo.Core.Api.Models.ToDoTasksManagement.UpdateToDoTask
{
    /// <summary>
    /// Represents the request model for updating an existing ToDo task.
    /// </summary>
    public class UpdateToDoTaskRequest
    {
        /// <summary>
        /// Gets or sets the expiry date of the task.
        /// </summary>
        public DateTime ExpiryAt { get; set; }

        /// <summary>
        /// Gets or sets the title of the task (max 100 characters).
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets an optional description of the task (max 5000 characters).
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the completion percentage of the task (between 0 and 100).
        /// </summary>
        public int CompletionPercentage { get; set; }
    }
}
