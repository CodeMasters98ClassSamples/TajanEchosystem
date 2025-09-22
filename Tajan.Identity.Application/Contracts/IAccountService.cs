using System.Threading;
using Tajan.Identity.Application.AuthenticationUsecases;
using Tajan.Standard.Domain.Wrappers;

namespace Tajan.Identity.Application.Contracts;

public interface IAccountService
{
    Task<Result<object>> AuthenticateAsync(LoginCommand request, string ipAddress, CancellationToken cancellationToken);
    Task<Result<object>> RegisterAsync(RegisterCommand request, string origin, CancellationToken cancellationToken);
    Task<Result<object>> ConfirmMobileNumberAsync(string mobileNumber, int code, CancellationToken cancellationToken);
    Task<Result<object>> GetUsersByIds(List<string> ids, CancellationToken cancellationToken);
    Task<Result<object>> SendOtp(string mobileNumber, CancellationToken cancellationToken);
    Task<Result<object>> VerifyOtp(string mobileNumber, int code, CancellationToken cancellationToken);
   
}