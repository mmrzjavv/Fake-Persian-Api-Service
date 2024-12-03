namespace FakeApiFarsi.Domain;

public interface IFakeDataRepository<T>
{
    Task<List<T>> GenerateFakeDataAsync(int skip, int take);
}