using Bogus;
using FakeApiFarsi.Domain;
using FakeApiFarsi.Domain.Internet;

namespace FakeApiFarsi.Infrastructure.Internet
{
    public class InternetFakeDataRepository : IFakeDataRepository<Domain.Internet.Internet>
    {
        private readonly Faker<Domain.Internet.Internet> _internetFaker;

        public InternetFakeDataRepository()
        {
            _internetFaker = new Faker<Domain.Internet.Internet>("fa")
                .RuleFor(internet => internet.Email, f => f.Internet.Email())  
                .RuleFor(internet => internet.UserName, f => f.Internet.UserName())  
                .RuleFor(internet => internet.Url, f => f.Internet.Url())  
                .RuleFor(internet => internet.IpAddress, f => f.Internet.Ip())  
                .RuleFor(internet => internet.UserAgent, f => f.Internet.UserAgent())  
                .RuleFor(internet => internet.DomainName, f => f.Internet.DomainName());  
        }

        public async Task<List<Domain.Internet.Internet>> GenerateFakeDataAsync(int skip, int take)
        {
            await Task.Yield();
            return _internetFaker.Generate(skip + take)
                .Skip(skip)
                .Take(take)
                .ToList();
        }
    }
}