using ToDo.Core.Domain.Entities;

namespace ToDo.Core.Application.ToDoTasksManagement.Commands.CreateOne
{
    public class CreateToDoTaskCommandResponse
    {
        public Guid Id { get; private set; }
        public DateTime ExpiryAt { get; private set; }
        public string Title { get; private set; }
        public string? Description { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public int CompletionPercentage { get; private set; }

        public CreateToDoTaskCommandResponse(ToDoTaskEntity newToDoTask)
        {
            Id = newToDoTask.Id;
            ExpiryAt = newToDoTask.ExpiryAt;
            Title = newToDoTask.Title;
            Description = newToDoTask.Description;
            CreatedAt = newToDoTask.CreatedAt;
            CompletionPercentage = newToDoTask.CompletionPercentage;
        }
    }
}
