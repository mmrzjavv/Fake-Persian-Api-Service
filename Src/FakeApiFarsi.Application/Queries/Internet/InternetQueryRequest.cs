using FakeApiFarsi.Application.Queries.DTOs;
using FakeApiFarsi.Domain;
using FakeApiFarsi.infrastructure.OperationRseult;
using MediatR;

namespace FakeApiFarsi.Application.Queries.Internet;

public class InternetQueryRequest
{
    public class InternetQuery :BaseGetDto, IRequest<OperationResult<Domain.Internet.Internet>>
    {
     
    }

    public class InternetQueryHandler(IFakeDataRepository<Domain.Internet.Internet> internetRepository)
        : IRequestHandler<InternetQuery, OperationResult<Domain.Internet.Internet>>
    {
        public async Task<OperationResult<Domain.Internet.Internet>> Handle(InternetQuery request,
            CancellationToken cancellationToken)
        {
            OperationResult<Domain.Internet.Internet> op = new("GetInternets");
            try
            {
                return op.Succeed("اطلاعات با موفقیت دریافت شد", null,
                    await internetRepository.GenerateFakeDataAsync(request.Skip, request.Take));
            }
            catch (Exception ex)
            {
                return op.Fail("خطا در طی عملیات", ex.Message);
            }
        }
    }
}