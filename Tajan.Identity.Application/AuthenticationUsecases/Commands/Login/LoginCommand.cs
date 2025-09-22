using MediatR;
using Tajan.Standard.Domain.Wrappers;

namespace Tajan.Identity.Application.AuthenticationUsecases;

public record LoginCommand(string Username,string Password) 
    : IRequest<Result<object>>;
