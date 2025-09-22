using MediatR;
using Tajan.Identity.Application.Contracts;
using Tajan.Standard.Domain.Wrappers;

namespace Tajan.Identity.Application.AuthenticationUsecases;

public class LoginCommandHandler(IAccountService accountService) : IRequestHandler<LoginCommand, Result<object>>
{
    public async Task<Result<object>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var result = await accountService.AuthenticateAsync(request: request, "", cancellationToken);
        return result;
    }
}
