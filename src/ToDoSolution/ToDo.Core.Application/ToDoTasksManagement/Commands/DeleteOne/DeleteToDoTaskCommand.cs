using MediatR;
using ToDo.Core.Application.RepositoryInterfaces;

namespace ToDo.Core.Application.ToDoTasksManagement.Commands.DeleteOne
{
    public class DeleteToDoTaskCommand : IRequest<Unit>
    {
        public Guid Id { get; private set; }

        public DeleteToDoTaskCommand(Guid id)
        {
            Id = id;
        }

        public sealed class Handler : IRequestHandler<DeleteToDoTaskCommand, Unit>
        {
            private readonly IToDoTaskRepository _toDoTaskRepository;

            public Handler(IToDoTaskRepository toDoTaskRepository)
            {
                _toDoTaskRepository = toDoTaskRepository;
            }

            public async Task<Unit> Handle(DeleteToDoTaskCommand request, CancellationToken cancellationToken)
            {
                var toDoTaskToRemove = await _toDoTaskRepository.GetOne(request.Id, cancellationToken);
                if (toDoTaskToRemove == null)
                {
                    throw new ApplicationException($"ToDo task with id: {request.Id} does not exist");
                }

                await _toDoTaskRepository.Delete(toDoTaskToRemove, cancellationToken);

                return Unit.Value;
            }
        }
    }
}
