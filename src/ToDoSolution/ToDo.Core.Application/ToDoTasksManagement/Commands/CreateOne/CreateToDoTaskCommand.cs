using MediatR;
using ToDo.Core.Application.RepositoryInterfaces;
using ToDo.Core.Domain.Entities;

namespace ToDo.Core.Application.ToDoTasksManagement.Commands.CreateOne
{
    public class CreateToDoTaskCommand : IRequest<CreateToDoTaskCommandResponse>
    {
        public DateTime ExpiryAt { get; private set; }
        public string Title { get; private set; }
        public string? Description { get; private set; }

        public CreateToDoTaskCommand(DateTime expiryAt, string title, string? description)
        {
            ExpiryAt = expiryAt;
            Title = title;
            Description = description;
        }

        public sealed class Handler : IRequestHandler<CreateToDoTaskCommand, CreateToDoTaskCommandResponse>
        {
            private readonly IToDoTaskRepository _toDoTaskRepository;

            public Handler(IToDoTaskRepository toDoTaskRepository)
            {
                this._toDoTaskRepository = toDoTaskRepository;
            }

            public async Task<CreateToDoTaskCommandResponse> Handle(CreateToDoTaskCommand request, CancellationToken cancellationToken)
            {
                var newToDoTask = new ToDoTaskEntity(request.ExpiryAt, request.Title, request.Description);
                await _toDoTaskRepository.Create(newToDoTask, cancellationToken);

                var response = new CreateToDoTaskCommandResponse(newToDoTask);
                return response;
            }
        }
    }
}
