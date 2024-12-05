namespace FakeApiFarsi.infrastructure.Helpers.TokenHandler.Model;

public class UserDataClaim
{
 
    public class UserTokenData
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public string Name { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string MobileNo { get; init; } = string.Empty;
    }
    public class UserData
    {
        public string Name { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string MobileNo { get; init; } = string.Empty;
    }
}
