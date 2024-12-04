using MediatR;
using ToDo.Core.Application.RepositoryInterfaces;

namespace ToDo.Core.Application.ToDoTasksManagement.Queries.GetOne
{
    public class GetToDoTaskQuery : IRequest<GetToDoTaskQueryResponse>
    {
        public Guid Id { get; private set; }

        public GetToDoTaskQuery(Guid id)
        {
            Id = id;
        }

        public sealed class Handler : IRequestHandler<GetToDoTaskQuery, GetToDoTaskQueryResponse>
        {
            private readonly IToDoTaskRepository _toDoTaskRepository;

            public Handler(IToDoTaskRepository toDoTaskRepository)
            {
                _toDoTaskRepository = toDoTaskRepository;
            }

            public async Task<GetToDoTaskQueryResponse> Handle(GetToDoTaskQuery request, CancellationToken cancellationToken)
            {
                var toDoTask = await _toDoTaskRepository.GetOneReadonly(request.Id, cancellationToken);
                if (toDoTask == null)
                {
                    throw new ApplicationException($"ToDo task with id: {request.Id} does not exist");
                }

                var response = new GetToDoTaskQueryResponse(toDoTask);
                return response;
            }
        }
    }
}
