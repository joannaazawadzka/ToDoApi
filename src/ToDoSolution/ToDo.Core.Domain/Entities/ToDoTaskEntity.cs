using ToDo.Common.Domain.Entities;

namespace ToDo.Core.Domain.Entities
{
    public class ToDoTaskEntity : BaseEntity
    {
        public DateTime ExpiryAt { get; private set; }
        public string Title { get; private set; } = string.Empty;
        public string? Description { get; private set; }
        public int CompletionPercentage { get; private set; }

        // Emptry private constructor used only for EF Core purpose
        private ToDoTaskEntity() { }

        public ToDoTaskEntity(DateTime expiryAt, string title, string? description)
        {
            Id = Guid.NewGuid();
            ExpiryAt = expiryAt;
            Title = title;
            Description = description;
            CompletionPercentage = 0;
        }

        public void MarkAsDone()
        {
            CompletionPercentage = 100;
        }

        public bool IsDone() => CompletionPercentage == 100;

        public void UpdateTitle(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ApplicationException($"For the ToDo task with id: {Id} the title is null or empty");
            }

            Title = title;
        }

        public void UpdateDescription(string? description)
        {
            Description = description;
        }

        public void UpdateCompletionPercentage(int completionPercentage)
        {
            if (completionPercentage > 100 || completionPercentage < 0)
            {
                throw new ApplicationException($"For the ToDo task with id: {Id} the completionPercentage must be between 0 - 100");
            }

            CompletionPercentage = completionPercentage;
        }

        public void UpdateExpiryAt(DateTime expiryAt)
        {
            ExpiryAt = expiryAt;
        }
    }
}
