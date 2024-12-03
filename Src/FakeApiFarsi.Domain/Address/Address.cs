namespace FakeApiFarsi.Domain.Address
{
    public record Address(
        string StreetAddress,
        string City,
        string State,
        string Country,
        string ZipCode
    );
}