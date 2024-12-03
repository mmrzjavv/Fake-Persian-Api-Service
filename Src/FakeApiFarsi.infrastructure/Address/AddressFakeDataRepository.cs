using Bogus;
using FakeApiFarsi.Domain;
using FakeApiFarsi.Domain.Address;

namespace FakeApiFarsi.Infrastructure.Address
{
    public class AddressFakeDataRepository : IFakeDataRepository<Domain.Address.Address>
    {
        private readonly Faker<Domain.Address.Address> _addressFaker;

        public AddressFakeDataRepository()
        {
            _addressFaker = new Faker<Domain.Address.Address>("fa")
                .RuleFor(address => address.StreetAddress, f => f.Address.StreetAddress())  // خیابان
                .RuleFor(address => address.City, f => f.Address.City())  // شهر
                .RuleFor(address => address.State, f => f.Address.State())  // استان
                .RuleFor(address => address.Country, f => f.Address.Country())  // کشور
                .RuleFor(address => address.ZipCode, f => f.Address.ZipCode());  // کد پستی
        }

        public async Task<List<Domain.Address.Address>> GenerateFakeDataAsync(int skip, int take)
        {
            await Task.Yield();
            return _addressFaker.Generate(skip + take)
                .Skip(skip)
                .Take(take)
                .ToList();
        }
    }
}