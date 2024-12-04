using ToDo.Core.Domain.Entities;

namespace ToDo.Core.Application.ToDoTasksManagement.Commands.UpdateCompletionPercentage
{
    public class UpdateToDoTaskCommandResponse
    {
        public Guid Id { get; private set; }
        public DateTime ExpiryAt { get; private set; }
        public string Title { get; private set; }
        public string? Description { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public int CompletionPercentage { get; private set; }

        public UpdateToDoTaskCommandResponse(ToDoTaskEntity toDoTaskEntity)
        {
            Id = toDoTaskEntity.Id;
            ExpiryAt = toDoTaskEntity.ExpiryAt;
            Title = toDoTaskEntity.Title;
            Description = toDoTaskEntity.Description;
            CreatedAt = toDoTaskEntity.CreatedAt;
            UpdatedAt = toDoTaskEntity.UpdatedAt;
            CompletionPercentage = toDoTaskEntity.CompletionPercentage;
        }
    }
}
