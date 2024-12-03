namespace FakeApiFarsi.Domain.Internet
{
    public class Internet
    {
        public required string Email { get; set; }
        public required string UserName { get; set; }
        public required string Url { get; set; }
        public required string IpAddress { get; set; }
        public required string UserAgent { get; set; }
        public required string DomainName { get; set; }
    }
}