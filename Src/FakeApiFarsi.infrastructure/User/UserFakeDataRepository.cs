using Bogus;
using FakeApiFarsi.Domain;

namespace FakeApiFarsi.infrastructure.User;

public class UserFakeDataRepository : IFakeDataRepository<Domain.User.User>
{
    private readonly Faker<Domain.User.User> _userFaker;

    public UserFakeDataRepository()
    {
        _userFaker = new Faker<Domain.User.User>("fa")
            .RuleFor(user => user.Id, f => f.IndexFaker + 1)
            .RuleFor(user => user.FullName, f => f.Name.FullName())
            .RuleFor(user => user.Email, f => f.Internet.Email())
            .RuleFor(user => user.PhoneNumber, f => f.Phone.PhoneNumber())
            .RuleFor(user => user.Address, f => f.Address.FullAddress());
    }

    public async Task<List<Domain.User.User>> GenerateFakeDataAsync(int skip, int take)
    {
        await Task.Yield();
        return _userFaker.Generate(skip + take)
            .Skip(skip)
            .Take(take)
            .ToList();
    }
}