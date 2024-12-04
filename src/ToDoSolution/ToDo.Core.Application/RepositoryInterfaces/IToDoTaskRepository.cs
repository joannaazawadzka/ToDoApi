using ToDo.Core.Application.ToDoTasksManagement.Queries.GetMany;
using ToDo.Core.Domain.Entities;

namespace ToDo.Core.Application.RepositoryInterfaces
{
    public interface IToDoTaskRepository
    {
        Task Create(ToDoTaskEntity entity, CancellationToken cancellationToken = default);
        Task Delete(ToDoTaskEntity entity, CancellationToken cancellationToken = default);
        Task Update(ToDoTaskEntity entity, CancellationToken cancellationToken = default);
        Task<ToDoTaskEntity?> GetOne(Guid id, CancellationToken cancellationToken = default);
        Task<ToDoTaskEntity?> GetOneReadonly(Guid id, CancellationToken cancellationToken = default);
        Task<ICollection<ToDoTaskEntity>> GetManyReadonly(GetToDoTasksFilterEnum filterBy = GetToDoTasksFilterEnum.All, CancellationToken cancellationToken = default);
    }
}
