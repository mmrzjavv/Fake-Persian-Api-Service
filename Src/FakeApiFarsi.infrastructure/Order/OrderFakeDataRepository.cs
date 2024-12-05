using Bogus;
using FakeApiFarsi.Domain;

namespace FakeApiFarsi.infrastructure.Order;


    public class OrderFakeDataRepository : IFakeDataRepository<Domain.Order.Order>
    {
        private readonly Faker<Domain.Order.Order> _orderFaker;

        public OrderFakeDataRepository()
        {
            _orderFaker = new Faker<Domain.Order.Order>("fa")
                .RuleFor(order => order.Id, f => f.IndexFaker + 1)
                .RuleFor(order => order.OrderDate, f => f.Date.Past(1))
                .RuleFor(order => order.CustomerName, f => f.Name.FullName())
                .RuleFor(order => order.TotalAmount, f => f.Finance.Amount(100000, 1000000))
                .RuleFor(order => order.Status, f => f.PickRandom(new[] { "تکمیل شده", "در حال پردازش", "لغو شده" }));
        }

        public async Task<List<Domain.Order.Order>> GenerateFakeDataAsync(int skip, int take)
        {
            await Task.Yield();
            return _orderFaker.Generate(skip + take)
                .Skip(skip)
                .Take(take)
                .ToList();
        }
    }
