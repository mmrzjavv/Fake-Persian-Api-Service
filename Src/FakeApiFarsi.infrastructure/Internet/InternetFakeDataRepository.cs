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
            _internetFaker = new Faker<Domain.Internet.Internet>("en")
                .RuleFor(internet => internet.Id, f => f.IndexFaker + 1)
                .RuleFor(internet => internet.Email, f => f.Internet.Email())
                .RuleFor(internet => internet.UserName, f => f.Internet.UserName())
                .RuleFor(internet => internet.Url, f => $"http://{f.Internet.DomainWord()}.{f.Internet.DomainSuffix()}")
                .RuleFor(internet => internet.IpAddress, f => f.Internet.Ip())
                .RuleFor(internet => internet.UserAgent, f => f.Internet.UserAgent())
                .RuleFor(internet => internet.DomainName, f => $"{f.Internet.DomainWord()}.{f.Internet.DomainSuffix()}");
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