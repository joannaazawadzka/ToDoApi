namespace ToDo.Core.Api.Models.ToDoTasksManagement.UpdateCompletionPercentage
{
    /// <summary>
    /// Represents the request model for updating the completion percentage of a ToDo task.
    /// </summary>
    public class UpdateCompletionPercentageRequest
    {
        /// <summary>
        /// Gets or sets the completion percentage of the task (between 0 and 100).
        /// </summary>
        public int CompletionPercentage { get; set; }
    }
}
