using FakeApiFarsi.Domain;
using FakeApiFarsi.infrastructure.OperationRseult;
using MediatR;

namespace FakeApiFarsi.Application.Queries.Product;

public class ProductQueryRequest
{
    public class ProductQuery : IRequest<OperationResult<Domain.Product.Product>>
    {
        public int Skip { get; set; }
        public int Take { get; set; }
    }
    
    public class ProductQueryHandler(IFakeDataRepository<Domain.Product.Product> productRepository) : IRequestHandler<ProductQuery , OperationResult<Domain.Product.Product>>
    {
        public async Task<OperationResult<Domain.Product.Product>> Handle(ProductQuery request, CancellationToken cancellationToken)
        {
            OperationResult<Domain.Product.Product> op = new("Product");
            try
            {
                return op.Succeed("اطلاعات با موفقیت دریافت شد", null,
                    await productRepository.GenerateFakeDataAsync(request.Skip, request.Take));
            }
            catch (Exception ex)
            {
                return op.Fail("خطا در طی عملیات", ex.Message);
            }
        }
    }
}