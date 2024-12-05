using Bogus;
using FakeApiFarsi.Domain;

namespace FakeApiFarsi.infrastructure.Product;

public class ProductFakeDataRepository : IFakeDataRepository<Domain.Product.Product>
{
    private readonly Faker<Domain.Product.Product> _productFaker;

    public ProductFakeDataRepository()
    {
        _productFaker = new Faker<Domain.Product.Product>("fa")
            .RuleFor(product => product.Id, f => f.IndexFaker + 1)
            .RuleFor(product => product.Name, f => f.Commerce.ProductName())
            .RuleFor(product => product.Category, f => f.Commerce.Categories(1).First())
            .RuleFor(product => product.Price, f => f.Finance.Amount(10000, 500000))
            .RuleFor(product => product.Description, f => f.Lorem.Sentence());
    }

    public async Task<List<Domain.Product.Product>> GenerateFakeDataAsync(int skip, int take)
    {
        await Task.Yield();
        return _productFaker.Generate(skip + take)
            .Skip(skip)
            .Take(take)
            .ToList();
    }
}