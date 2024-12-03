using Bogus;
using FakeApiFarsi.Domain;

namespace FakeApiFarsi.infrastructure.Todo;

public class TodoFakeDataRepository : IFakeDataRepository<Domain.Todo.Todo>
{
    private readonly Faker<Domain.Todo.Todo> _faker;

    public TodoFakeDataRepository()
    {
        _faker = new Faker<Domain.Todo.Todo>("fa")
            .RuleFor(t => t.Id, f => f.IndexFaker + 1)
            .RuleFor(t => t.Title, f => f.Lorem.Sentence(3))
            .RuleFor(t => t.Description, f => f.Lorem.Paragraph())
            .RuleFor(t => t.IsCompleted, f => f.Random.Bool())
            .RuleFor(t => t.DueDate, f => f.Date.Future());
    }

    public async Task<List<Domain.Todo.Todo>> GenerateFakeDataAsync(int skip, int take)
    {
        await Task.Yield();
        return _faker.Generate(skip + take)
            .Skip(skip).Take(take).ToList();
    }
}