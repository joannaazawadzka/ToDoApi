namespace ToDo.Core.Api.Models.ToDoTasksManagement.CreateToDoTask
{
    /// <summary>
    /// Represents the request model for creating a ToDo task.
    /// </summary>
    public class CreateToDoTaskRequest
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
    }
}
