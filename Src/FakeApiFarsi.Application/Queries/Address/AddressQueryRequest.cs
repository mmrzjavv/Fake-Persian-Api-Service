using FakeApiFarsi.Domain;
using FakeApiFarsi.infrastructure.OperationRseult;
using MediatR;

namespace FakeApiFarsi.Application.Queries.Address;

public class AddressQueryRequest
{
    public class AddressQuery : IRequest<OperationResult<Domain.Address.Address>>
    {
        public int Skip { get; set; }
        public int Take { get; set; }
    }

    public class AddressQueryHandler(IFakeDataRepository<Domain.Address.Address> addressRepository)
        : IRequestHandler<AddressQuery, OperationResult<Domain.Address.Address>>
    {
        public async Task<OperationResult<Domain.Address.Address>> Handle(AddressQuery request,
            CancellationToken cancellationToken)
        {
            OperationResult<Domain.Address.Address> op = new("Address");
            try
            {
                return op.Succeed("اطلاعات با موفقیت دریافت شد", null,
                    await addressRepository.GenerateFakeDataAsync(request.Skip, request.Take));
            }
            catch (Exception ex)
            {
                return op.Fail("خطا در طی عملیات", ex.Message);
            }
        }
    }
}