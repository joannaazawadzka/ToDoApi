using ToDo.Core.Domain.Entities;

namespace ToDo.Core.Application.ToDoTasksManagement.Queries.GetMany
{
    /// <summary>
    /// Enum representing various filter criteria for retrieving ToDo tasks.
    /// </summary>
    public enum GetToDoTasksFilterEnum
    {
        /// <summary>
        /// Retrieves all ToDo tasks.
        /// </summary>
        All = 0,

        /// <summary>
        /// Retrieves only ToDo tasks for today.
        /// </summary>
        OnlyToday = 10,

        /// <summary>
        /// Retrieves only ToDo tasks for the next day.
        /// </summary>
        OnlyNextDay = 20,

        /// <summary>
        /// Retrieves only ToDo tasks for the current week.
        /// </summary>
        OnlyCurrentWeek = 30,

        /// <summary>
        /// Retrieves only ToDo tasks for the next week.
        /// </summary>
        OnlyNextWeek = 40
    }
}
