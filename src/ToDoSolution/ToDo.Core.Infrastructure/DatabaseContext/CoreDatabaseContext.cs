using Microsoft.EntityFrameworkCore;
using ToDo.Common.Infrastructure.DatabaseContext;
using ToDo.Core.Application.DatabaseContext;
using ToDo.Core.Domain.Entities;

namespace ToDo.Core.Infrastructure.DatabaseContext
{
    public class CoreDatabaseContext(DbContextOptions<CoreDatabaseContext> options) : BaseDatabaseContext(options), ICoreDatabaseContext
    {
        public DbSet<ToDoTaskEntity> ToDoTasks => Set<ToDoTaskEntity>();
    }
}
