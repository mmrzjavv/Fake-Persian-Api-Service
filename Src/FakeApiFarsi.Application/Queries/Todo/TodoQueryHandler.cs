using FakeApiFarsi.Domain;
using FakeApiFarsi.infrastructure.OperationRseult;
using MediatR;

namespace FakeApiFarsi.Application.Queries.Todo;

public abstract class TodoQueryHandler
{
    public class TodoCommandRequest : IRequest<OperationResult<Domain.Todo.Todo>>
    {
        public int Skip { get; set; }
        public int Take { get; set; }
    }

    public class TodoCommandHandler(IFakeDataRepository<Domain.Todo.Todo> todoRepository)
        : IRequestHandler<TodoCommandRequest, OperationResult<Domain.Todo.Todo>>
    {
        public async Task<OperationResult<Domain.Todo.Todo>> Handle(TodoCommandRequest request,
            CancellationToken cancellationToken)
        {
            OperationResult<Domain.Todo.Todo> op = new("Todo");
            try
            {
                return op.Succeed("اطلاعات با موفقیت دریافت شد", null,
                    await todoRepository.GenerateFakeDataAsync(request.Skip, request.Take));
            }
            catch (Exception ex)
            {
                return op.Fail("خطا در طی عملیات", ex.Message);
            }
        }
    }
}