using MediatR;
using ToDo.Core.Application.RepositoryInterfaces;
using ToDo.Core.Application.ToDoTasksManagement.Commands.UpdateOne;

namespace ToDo.Core.Application.ToDoTasksManagement.Commands.MarkAsDone
{
    public class MarkToDoTaskAsDoneCommand : IRequest<UpdateToDoTaskCommandResponse>
    {
        public Guid Id { get; private set; }

        public MarkToDoTaskAsDoneCommand(Guid id)
        {
            Id = id;
        }

        public sealed class Handler : IRequestHandler<MarkToDoTaskAsDoneCommand, UpdateToDoTaskCommandResponse>
        {
            private readonly IToDoTaskRepository _toDoTaskRepository;

            public Handler(IToDoTaskRepository toDoTaskRepository)
            {
                _toDoTaskRepository = toDoTaskRepository;
            }

            public async Task<UpdateToDoTaskCommandResponse> Handle(MarkToDoTaskAsDoneCommand request, CancellationToken cancellationToken)
            {
                var toDoTaskToUpdate = await _toDoTaskRepository.GetOne(request.Id, cancellationToken);
                if (toDoTaskToUpdate == null)
                {
                    throw new ApplicationException($"ToDo task with id: {request.Id} does not exist");
                }

                toDoTaskToUpdate.MarkAsDone();
                await _toDoTaskRepository.Update(toDoTaskToUpdate, cancellationToken);

                var response = new UpdateToDoTaskCommandResponse(toDoTaskToUpdate);
                return response;
            }
        }
    }
}
