using MediatR;
using ToDo.Core.Application.RepositoryInterfaces;

namespace ToDo.Core.Application.ToDoTasksManagement.Commands.UpdateCompletionPercentage
{
    public class UpdateToDoTaskCommand : IRequest<UpdateToDoTaskCommandResponse>
    {
        public Guid Id { get; private set; }
        public DateTime ExpiryAt { get; private set; }
        public string Title { get; private set; }
        public string? Description { get; private set; }
        public int CompletionPercentage { get; private set; }

        public UpdateToDoTaskCommand(Guid id, DateTime expiryAt, string title, string? description, int completionPercentage)
        {
            ExpiryAt = expiryAt;
            Title = title;
            Description = description;
            CompletionPercentage = completionPercentage;
            Id = id;
        }

        public sealed class Handler : IRequestHandler<UpdateToDoTaskCommand, UpdateToDoTaskCommandResponse>
        {
            private readonly IToDoTaskRepository _toDoTaskRepository;

            public Handler(IToDoTaskRepository toDoTaskRepository)
            {
                _toDoTaskRepository = toDoTaskRepository;
            }

            public async Task<UpdateToDoTaskCommandResponse> Handle(UpdateToDoTaskCommand request, CancellationToken cancellationToken)
            {
                var toDoTaskToUpdate = await _toDoTaskRepository.GetOne(request.Id, cancellationToken);
                if (toDoTaskToUpdate == null)
                {
                    throw new ApplicationException($"ToDo task with id: {request.Id} does not exist");
                }

                var hasAnythingChanged = false;

                if (request.Title != toDoTaskToUpdate.Title)
                {
                    toDoTaskToUpdate.UpdateTitle(request.Title);
                    hasAnythingChanged = true;
                }

                if (request.Description != toDoTaskToUpdate.Description)
                {
                    toDoTaskToUpdate.UpdateDescription(request.Description);
                    hasAnythingChanged = true;
                }

                if (request.CompletionPercentage != toDoTaskToUpdate.CompletionPercentage)
                {
                    toDoTaskToUpdate.UpdateCompletionPercentage(request.CompletionPercentage);
                    hasAnythingChanged = true;
                }

                if (request.ExpiryAt != toDoTaskToUpdate.ExpiryAt)
                {
                    toDoTaskToUpdate.UpdateExpiryAt(request.ExpiryAt);
                    hasAnythingChanged = true;
                }

                if (hasAnythingChanged)
                {
                    await _toDoTaskRepository.Update(toDoTaskToUpdate, cancellationToken);
                }

                var response = new UpdateToDoTaskCommandResponse(toDoTaskToUpdate);
                return response;
            }
        }
    }
}
