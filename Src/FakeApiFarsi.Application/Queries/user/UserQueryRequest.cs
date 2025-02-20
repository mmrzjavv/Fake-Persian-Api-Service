using FakeApiFarsi.Application.Queries.DTOs;
using FakeApiFarsi.Domain;
using FakeApiFarsi.Domain.User;
using FakeApiFarsi.infrastructure.OperationRseult;
using MediatR;

namespace FakeApiFarsi.Application.Queries.user;

public class UserQueryRequest
{
    public class UserQuery :BaseGetDto, IRequest<OperationResult<User>>
    {
    
    }
    public class UserQueryHandler(IFakeDataRepository<User> userRepository) : IRequestHandler<UserQuery, OperationResult<User>>
    {
        public async Task<OperationResult<User>> Handle(UserQuery request, CancellationToken cancellationToken)
        {
            OperationResult<User> op = new("User");
            try
            {
                return op.Succeed("اطلاعات با موفقیت دریافت شد", null,
                    await userRepository.GenerateFakeDataAsync(request.Skip, request.Take));
            }
            catch (Exception ex)
            {
                return op.Fail("خطا در طی عملیات", ex.Message);
            }
        }
    }
}