using MediatR;
using Microsoft.AspNetCore.Mvc;
using ToDo.Core.Api.Models.ToDoTasksManagement.CreateToDoTask;
using ToDo.Core.Api.Models.ToDoTasksManagement.UpdateCompletionPercentage;
using ToDo.Core.Api.Models.ToDoTasksManagement.UpdateToDoTask;
using ToDo.Core.Application.ToDoTasksManagement.Commands.CreateOne;
using ToDo.Core.Application.ToDoTasksManagement.Commands.DeleteOne;
using ToDo.Core.Application.ToDoTasksManagement.Commands.MarkAsDone;
using ToDo.Core.Application.ToDoTasksManagement.Commands.UpdateCompletionPercentage;
using ToDo.Core.Application.ToDoTasksManagement.Commands.UpdateOne;
using ToDo.Core.Application.ToDoTasksManagement.Queries.GetMany;
using ToDo.Core.Application.ToDoTasksManagement.Queries.GetOne;

namespace ToDo.Core.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ToDoTasksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ToDoTasksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates a new ToDo task.
        /// </summary>
        /// <param name="request">The request model containing the details of the task to be created.</param>
        /// <returns>Returns the created ToDo task.</returns>
        /// <remarks>
        /// This method allows the creation of a new ToDo task, requiring a title (up to 100 characters), an optional description (up to 5000 characters),
        /// and an expiry date. The response contains the details of the created task.
        /// </remarks>
        [HttpPost]
        [Route("")]
        [ProducesResponseType(typeof(CreateToDoTaskCommandResponse), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateOne([FromBody] CreateToDoTaskRequest request)
        {
            var result = await _mediator.Send(new CreateToDoTaskCommand(request.ExpiryAt, request.Title, request.Description));
            return Ok(result);
        }

        /// <summary>
        /// Updates an existing ToDo task.
        /// </summary>
        /// <param name="toDoTaskId">The unique ID of the task to update.</param>
        /// <param name="request">The request model containing the updated task details.</param>
        /// <returns>Returns the updated ToDo task.</returns>
        /// <remarks>
        /// This endpoint allows updating a ToDo task's title (up to 100 characters), description (up to 5000 characters), expiry date, 
        /// and completion percentage (between 0 and 100).
        /// </remarks>
        [HttpPut]
        [Route("{toDoTaskId}")]
        [ProducesResponseType(typeof(UpdateToDoTaskCommandResponse), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateOne([FromRoute] Guid toDoTaskId, [FromBody] UpdateToDoTaskRequest request)
        {
            var result = await _mediator.Send(new UpdateToDoTaskCommand(toDoTaskId, request.ExpiryAt, request.Title, request.Description, request.CompletionPercentage));
            return Ok(result);
        }

        /// <summary>
        /// Marks a ToDo task as done by updating its completion percentage to 100%.
        /// </summary>
        /// <param name="toDoTaskId">The unique ID of the task to mark as done.</param>
        /// <returns>Returns the updated ToDo task with completion set to 100%.</returns>
        [HttpPatch]
        [Route("{toDoTaskId}/done")]
        [ProducesResponseType(typeof(UpdateToDoTaskCommandResponse), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> MarkAsDone([FromRoute] Guid toDoTaskId)
        {
            var result = await _mediator.Send(new MarkToDoTaskAsDoneCommand(toDoTaskId));
            return Ok(result);
        }

        /// <summary>
        /// Updates the completion percentage of an existing ToDo task.
        /// </summary>
        /// <param name="toDoTaskId">The unique ID of the task to update.</param>
        /// <param name="request">The request model containing the new completion percentage (0-100).</param>
        /// <returns>Returns the updated ToDo task with the new completion percentage.</returns>
        /// <remarks>
        /// The completion percentage must be a number between 0 and 100.
        /// </remarks>
        [HttpPatch]
        [Route("{toDoTaskId}/completionPercentage")]
        [ProducesResponseType(typeof(UpdateToDoTaskCommandResponse), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateCompletionPercentage([FromRoute] Guid toDoTaskId, [FromBody] UpdateCompletionPercentageRequest request)
        {
            var result = await _mediator.Send(new UpdateToDoTaskCompletionPercentageCommand(toDoTaskId, request.CompletionPercentage));
            return Ok(result);
        }

        /// <summary>
        /// Deletes a ToDo task by its unique ID.
        /// </summary>
        /// <param name="toDoTaskId">The unique ID of the task to delete.</param>
        /// <returns>Returns a status code indicating whether the deletion was successful.</returns>
        [HttpDelete]
        [Route("{toDoTaskId}")]
        [ProducesResponseType(202)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteOne([FromRoute] Guid toDoTaskId)
        {
            await _mediator.Send(new DeleteToDoTaskCommand(toDoTaskId));
            return Accepted();
        }

        /// <summary>
        /// Retrieves a ToDo task by its unique ID.
        /// </summary>
        /// <param name="toDoTaskId">The unique ID of the task to retrieve.</param>
        /// <returns>Returns the requested ToDo task details.</returns>
        [HttpGet]
        [Route("{toDoTaskId}")]
        [ProducesResponseType(typeof(GetToDoTaskQueryResponse), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetOne([FromRoute] Guid toDoTaskId)
        {
            var result = await _mediator.Send(new GetToDoTaskQuery(toDoTaskId));
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a list of ToDo tasks based on the specified filter criteria.
        /// </summary>
        /// <param name="filterBy">The filter criteria to apply when retrieving tasks. 
        /// Possible values for this parameter are defined in the <c>GetToDoTasksFilterEnum</c>:
        /// 
        /// - <c>0</c>: Retrieves all ToDo tasks (default).
        /// - <c>10</c>: Retrieves only ToDo tasks for today.
        /// - <c>20</c>: Retrieves only ToDo tasks for the next day.
        /// - <c>30</c>: Retrieves only ToDo tasks for the current week.
        /// - <c>40</c>: Retrieves only ToDo tasks for the next week.
        /// 
        /// The default value is <c>0</c> (All tasks).</param>
        /// <returns>Returns a collection of ToDo tasks that match the specified filter criteria.</returns>
        /// <remarks>
        /// Use this endpoint to filter tasks based on the specified criteria. If no filter is provided, 
        /// the default filter <c>0</c> will return all tasks.
        /// </remarks>
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(ICollection<GetToDoTaskQueryResponse>), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetMany([FromQuery] GetToDoTasksFilterEnum filterBy = GetToDoTasksFilterEnum.All)
        {
            var result = await _mediator.Send(new GetToDoTasksQuery(filterBy));
            return Ok(result);
        }
    }
}
