using Microsoft.EntityFrameworkCore;
using ToDo.Core.Domain.Entities;

namespace ToDo.Core.Application.DatabaseContext
{
    public interface ICoreDatabaseContext
    {
        DbSet<ToDoTaskEntity> ToDoTasks { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
