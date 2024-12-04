namespace ToDo.Common.Domain.Entities
{
    public interface IEntity
    {
        Guid Id { get; }
        DateTime CreatedAt { get; }
        DateTime? UpdatedAt { get; }

        void SetCreatedAt(DateTime createdAt);
        void SetUpdatedAt(DateTime createdAt);
    }
}
