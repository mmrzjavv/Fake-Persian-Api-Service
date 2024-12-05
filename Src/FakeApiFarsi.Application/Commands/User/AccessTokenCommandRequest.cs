using Bogus.Extensions.Poland;
using FakeApiFarsi.infrastructure.Helpers.TokenHandler;
using FakeApiFarsi.Infrastructure.Helpers.TokenHandler;
using FakeApiFarsi.infrastructure.Helpers.TokenHandler.Model;
using FakeApiFarsi.infrastructure.OperationRseult;
using MediatR;

namespace FakeApiFarsi.Application.Commands.User;

public class AccessTokenCommandRequest
{
    public class AccessTokenCommand :UserDataClaim.UserTokenData ,IRequest<OperationResult<TokenModel>>
    {
    }

    public class AccessTokenCommandHandler(TokenHelper tokenHelper) : IRequestHandler<AccessTokenCommand, OperationResult<TokenModel>>
    {
        public async Task<OperationResult<TokenModel>> Handle(AccessTokenCommand request, CancellationToken cancellationToken)
        {
            OperationResult<TokenModel> op = new("CreateAccessToken");
            try
            {
                var accessToken = tokenHelper.GenerateToken(request);
                if (accessToken is null)
                {
                    return op.Fail("خطا در ساخت اکسس توکن");
                }

                if (accessToken.Token is null)
                {
                    return op.Fail("خطا در ساخت اکسس توکن");
                }
                return op.Succeed("عملیات با موفقیت انجام شد" , accessToken);
            }
            catch (Exception ex)
            {
                return op.Fail("خطا در طی عملیات", ex.Message);
            }
        }
    }
   
}