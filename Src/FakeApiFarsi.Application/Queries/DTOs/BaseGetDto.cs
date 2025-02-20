namespace FakeApiFarsi.Application.Queries.DTOs;

public class BaseGetDto
{
    public int Skip { get; set; } = 0;
    public int Take { get; set; } = 10;
}