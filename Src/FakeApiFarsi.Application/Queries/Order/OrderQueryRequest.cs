using FakeApiFarsi.Application.Queries.DTOs;
using FakeApiFarsi.Domain;
using FakeApiFarsi.infrastructure.OperationRseult;
using MediatR;

namespace FakeApiFarsi.Application.Queries.Order;

public class OrderQueryRequest
{
    public class OrderQuery :BaseGetDto, IRequest<OperationResult<Domain.Order.Order>>
    {
      
    }
    public class OrderQueryHandler(IFakeDataRepository<Domain.Order.Order> orderRepository) : IRequestHandler<OrderQuery , OperationResult<Domain.Order.Order>>
    {
        public async Task<OperationResult<Domain.Order.Order>> Handle(OrderQuery request, CancellationToken cancellationToken)
        {
            OperationResult<Domain.Order.Order> op = new ("Order");
            try
            {
                return op.Succeed("اطلاعات با موفقیت دریافت شد", null,
                    await orderRepository.GenerateFakeDataAsync(request.Skip, request.Take));
            }
            catch (Exception ex)
            {
                return op.Fail("خطا در طی عملیات", ex.Message);
            }
        }
    }
}