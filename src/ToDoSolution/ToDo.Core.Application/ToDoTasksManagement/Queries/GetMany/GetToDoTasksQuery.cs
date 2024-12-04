using MediatR;
using ToDo.Core.Application.RepositoryInterfaces;
using ToDo.Core.Application.ToDoTasksManagement.Queries.GetMany;

namespace ToDo.Core.Application.ToDoTasksManagement.Queries.GetOne
{
    public class GetToDoTasksQuery : IRequest<ICollection<GetToDoTaskQueryResponse>>
    {
        public GetToDoTasksFilterEnum FilterBy { get; private set; }

        public GetToDoTasksQuery(GetToDoTasksFilterEnum filterBy)
        {
            FilterBy = filterBy;
        }

        public sealed class Handler : IRequestHandler<GetToDoTasksQuery, ICollection<GetToDoTaskQueryResponse>>
        {
            private readonly IToDoTaskRepository _toDoTaskRepository;

            public Handler(IToDoTaskRepository toDoTaskRepository)
            {
                _toDoTaskRepository = toDoTaskRepository;
            }

            public async Task<ICollection<GetToDoTaskQueryResponse>> Handle(GetToDoTasksQuery request, CancellationToken cancellationToken)
            {
                var toDoTasks = await _toDoTaskRepository.GetManyReadonly(request.FilterBy, cancellationToken);

                var response = toDoTasks
                    .Select(x => new GetToDoTaskQueryResponse(x))
                    .ToList();

                return response;
            }
        }
    }
}
