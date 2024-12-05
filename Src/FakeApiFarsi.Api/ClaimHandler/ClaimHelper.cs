using System.Security.Claims;
using Newtonsoft.Json;
using static FakeApiFarsi.infrastructure.Helpers.TokenHandler.Model.UserDataClaim;

namespace FakeApiFarsi.Api.ClaimHandler
{
    public static class ClaimHelper
    {
        public static UserTokenData GetClaimData(this IEnumerable<Claim>? claims)
        {
            if (claims == null || !claims.Any())
            {
                throw new ArgumentException("Claims collection is null or empty.");
            }

            var userDataClaim = claims.FirstOrDefault(x => x.Type == ClaimTypes.UserData)?.Value;
            if (userDataClaim == null)
            {
                throw new InvalidOperationException("User data claim not found.");
            }

            var userData = JsonConvert.DeserializeObject<UserData>(userDataClaim);

            return new UserTokenData()
            {
                Username = claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value ?? string.Empty,
                Id = int.TryParse(claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value, out var userId) ? userId : 1,
                RoleId = int.TryParse(claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value, out var roleId) ? roleId : 1, 
                Name = userData?.Name ?? string.Empty,
                LastName = userData?.LastName ?? string.Empty,
                MobileNo = userData?.MobileNo ?? string.Empty
            };
        }


    }
}
