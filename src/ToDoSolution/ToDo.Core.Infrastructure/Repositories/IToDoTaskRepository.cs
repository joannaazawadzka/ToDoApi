using Microsoft.EntityFrameworkCore;
using ToDo.Core.Application.DatabaseContext;
using ToDo.Core.Application.RepositoryInterfaces;
using ToDo.Core.Application.ToDoTasksManagement.Queries.GetMany;
using ToDo.Core.Domain.Entities;

namespace ToDo.Core.Infrastructure.Repositories
{
    public class ToDoTaskRepository : IToDoTaskRepository
    {
        private readonly ICoreDatabaseContext _databaseContext;

        public ToDoTaskRepository(ICoreDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task Create(ToDoTaskEntity entity, CancellationToken cancellationToken = default)
        {
            await _databaseContext.ToDoTasks.AddAsync(entity, cancellationToken);
            await _databaseContext.SaveChangesAsync(cancellationToken);
        }

        public async Task Delete(ToDoTaskEntity entity, CancellationToken cancellationToken = default)
        {
            _databaseContext.ToDoTasks.Remove(entity);
            await _databaseContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<ToDoTaskEntity?> GetOne(Guid id, CancellationToken cancellationToken = default)
        {
            return await _databaseContext.ToDoTasks.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<ToDoTaskEntity?> GetOneReadonly(Guid id, CancellationToken cancellationToken = default)
        {
            return await _databaseContext.ToDoTasks.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task Update(ToDoTaskEntity entity, CancellationToken cancellationToken = default)
        {
            _databaseContext.ToDoTasks.Update(entity);
            await _databaseContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<ICollection<ToDoTaskEntity>> GetManyReadonly(GetToDoTasksFilterEnum filterBy = GetToDoTasksFilterEnum.All, CancellationToken cancellationToken = default)
        {
            var query = _databaseContext.ToDoTasks.AsNoTracking();

            var currentDate = DateTime.Now;
            switch (filterBy)
            {
                case GetToDoTasksFilterEnum.OnlyToday:
                    var startOfToday = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day);
                    var endOfToday = startOfToday.AddDays(1).AddTicks(-1);
                    query = query.Where(x => x.ExpiryAt > startOfToday && x.ExpiryAt < endOfToday);
                    break;
                case GetToDoTasksFilterEnum.OnlyNextDay:
                    var startOfNextDay = new DateTime(currentDate.AddDays(1).Year, currentDate.AddDays(1).Month, currentDate.AddDays(1).Day);
                    var endOfNextDay = startOfNextDay.AddDays(1).AddTicks(-1);
                    query = query.Where(x => x.ExpiryAt > startOfNextDay && x.ExpiryAt < endOfNextDay);
                    break;
                case GetToDoTasksFilterEnum.OnlyCurrentWeek:
                    var startOfWeek = currentDate.AddDays(-(int)currentDate.DayOfWeek);
                    var endOfWeek = startOfWeek.AddDays(7).AddTicks(-1);
                    query = query.Where(x => x.ExpiryAt >= startOfWeek && x.ExpiryAt <= endOfWeek);
                    break;
                case GetToDoTasksFilterEnum.OnlyNextWeek:
                    var nextWeekStart = currentDate.AddDays(7 - (int)currentDate.DayOfWeek);
                    var nextWeekEnd = nextWeekStart.AddDays(7).AddTicks(-1);
                    query = query.Where(x => x.ExpiryAt >= nextWeekStart && x.ExpiryAt <= nextWeekEnd);
                    break;
            }

            query = query.OrderByDescending(x => x.CreatedAt);

            return await query.ToListAsync();
        }
    }
}
