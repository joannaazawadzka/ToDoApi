using MediatR;
using ToDo.Core.Application.RepositoryInterfaces;
using ToDo.Core.Application.ToDoTasksManagement.Commands.UpdateCompletionPercentage;

namespace ToDo.Core.Application.ToDoTasksManagement.Commands.UpdateOne
{
    public class UpdateToDoTaskCompletionPercentageCommand : IRequest<UpdateToDoTaskCommandResponse>
    {
        public Guid Id { get; private set; }
        public int CompletionPercentage { get; private set; }

        public UpdateToDoTaskCompletionPercentageCommand(Guid id, int completionPercentage)
        {
            Id = id;
            CompletionPercentage = completionPercentage;
        }

        public sealed class Handler : IRequestHandler<UpdateToDoTaskCompletionPercentageCommand, UpdateToDoTaskCommandResponse>
        {
            private readonly IToDoTaskRepository _toDoTaskRepository;

            public Handler(IToDoTaskRepository toDoTaskRepository)
            {
                _toDoTaskRepository = toDoTaskRepository;
            }

            public async Task<UpdateToDoTaskCommandResponse> Handle(UpdateToDoTaskCompletionPercentageCommand request, CancellationToken cancellationToken)
            {
                var toDoTaskToUpdate = await _toDoTaskRepository.GetOne(request.Id, cancellationToken);
                if (toDoTaskToUpdate == null)
                {
                    throw new ApplicationException($"ToDo task with id: {request.Id} does not exist");
                }

                if (request.CompletionPercentage != toDoTaskToUpdate.CompletionPercentage)
                {
                    toDoTaskToUpdate.UpdateCompletionPercentage(request.CompletionPercentage);
                    await _toDoTaskRepository.Update(toDoTaskToUpdate, cancellationToken);
                }

                var response = new UpdateToDoTaskCommandResponse(toDoTaskToUpdate);
                return response;
            }
        }
    }
}
