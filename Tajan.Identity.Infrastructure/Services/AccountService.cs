using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Tajan.Identity.Infrastructure.Models;
using Tajan.Identity.Application.Contracts;
using Tajan.Standard.Application.Abstractions;
using Tajan.Standard.Domain.Wrappers;
using Tajan.Identity.Infrastructure.Settings;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Tajan.Standard.Application.ServiceIngtegrations.NotificationService;
using Tajan.Identity.Infrastructure.Helpers;
using Tajan.Identity.Application.Dtos;
using Tajan.Identity.Application.AuthenticationUsecases;
using Azure;

namespace Tajan.Identity.Infrastructure.Services;


public class AccountService : IAccountService
{

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    //private readonly IPublishEndpoint _publishEndpoint;
    private readonly JwtSetting _jwtSettings;
    private readonly IApplicationDbContext _appDbContext;

    public AccountService(UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IOptionsMonitor<JwtSetting> jwtSettings,
        SignInManager<ApplicationUser> signInManager,
        //IPublishEndpoint publishEndpoint,
        IApplicationDbContext appDbContext)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtSettings = jwtSettings.CurrentValue;
        _signInManager = signInManager;
        //_publishEndpoint = publishEndpoint;
        _appDbContext = appDbContext;
    }

    public async Task<Result<object>> AuthenticateAsync(LoginCommand request, string ipAddress, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.Username);
        if (user is null)
            return Result.Failure<object>(error: Error.InvalidUser);

        var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);
        if (!result.Succeeded)
            return Result.Failure<object>(error: Error.InvalidUser);

        var userRoles = await _userManager.GetRolesAsync(user);
        if (userRoles is null)
            throw new Exception();

        JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user);
        AuthenticationResponse response = new AuthenticationResponse();
        response.Id = user.Id;
        response.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        response.Email = user.Email;
        response.UserName = user.UserName;
        var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
        response.Roles = rolesList.ToList();
        response.IsVerified = user.EmailConfirmed;
        var refreshToken = GenerateRefreshToken(ipAddress);
        response.RefreshToken = refreshToken.Token;
        return Result.Success<object>(response);
    }

    public async Task<Result<object>> RegisterAsync(RegisterCommand request, string origin, CancellationToken cancellationToken)
    {
        var userWithSameUserName = await _userManager.FindByNameAsync(request.Username);
        if (userWithSameUserName is not null)
            return Result.Failure<object>(error: Error.InvalidUser);

        var user = new ApplicationUser
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserName = request.Username,
            PhoneNumber = request.Username,
            PhoneNumberConfirmed = false
        };
        var userFromDb = await _userManager.Users.Where(x => x.UserName == request.Username).FirstOrDefaultAsync();
        if (userFromDb is null)
        {
            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "USER");

                user.CreateOtp();

                await _userManager.UpdateAsync(user);

                //var smsRequest = new SendSmsRequest(
                //    Attributes: null,
                //    Message: $"کد تایید: {user.VerifyCode} \n آزمون",
                //    Receiver: user.PhoneNumber,
                //    Subject: "کد تایید ثبت نام");

                //await _publishEndpoint.Publish<SendSingleSms>(new
                //{

                //});

                return Result.Success<object>(true,System.Net.HttpStatusCode.OK);
            }
            else
            {
                return Result.Failure<object>(Error.InvalidUser);
            }
        }
        else
        {
            return Result.Failure<object>(Error.InvalidUser);
        }
    }

    private async Task<JwtSecurityToken> GenerateJWToken(ApplicationUser user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);

        var roleClaims = new List<Claim>();

        for (int i = 0; i < roles.Count; i++)
        {
            roleClaims.Add(new Claim("roles", roles[i]));
        }

        string ipAddress = IpHelper.GetIpAddress();

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("uid", user.Id),
            new Claim("ip", ipAddress)
        }
        .Union(userClaims)
        .Union(roleClaims);

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwtSettings.ValidIssuer,
            audience: _jwtSettings.ValidAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddMonths(12),
            signingCredentials: signingCredentials);
        return jwtSecurityToken;
    }

    private string RandomTokenString()
    {
        return new Guid().ToString();
    }

    private async Task<string> SendVerificationEmail(ApplicationUser user, string origin, CancellationToken cancellationToken)
    {
        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        var route = "api/account/confirm-email/";
        var _enpointUri = new Uri(string.Concat($"{origin}/", route));
        var verificationUri = QueryHelpers.AddQueryString(_enpointUri.ToString(), "userId", user.Id);
        verificationUri = QueryHelpers.AddQueryString(verificationUri, "code", code);
        //Email Service Call Here
        return verificationUri;
    }

    private RefreshToken GenerateRefreshToken(string ipAddress)
    {
        return new RefreshToken
        {
            Token = RandomTokenString(),
            Expires = DateTime.UtcNow.AddDays(7),
            Created = DateTime.UtcNow,
            CreatedByIp = ipAddress
        };
    }

    public async Task<Result<object>> GetUsersByIds(List<string> ids, CancellationToken cancellationToken)
    {
        var users = await _userManager.Users
            .Where(x => ids.Contains(x.Id))
            .ToListAsync(cancellationToken);
        return Result.Success<object>(data: users, status: System.Net.HttpStatusCode.OK);
    }

    public async Task<Result<object>> ConfirmMobileNumberAsync(string phoneNumber, int code, CancellationToken cancellationToken)
    {
        ApplicationUser user = await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber);
        if (user is null)
            return Result.Failure<object>(error: Error.InvalidUser);
        if (user.VerifyCode != code)
            return Result.Failure<object>(error: Error.InvalidUser);

        if (user.VerifyCodeExpirationDate <= DateTime.Now)
            return Result.Failure<object>(error: Error.InvalidUser);

        user.PhoneNumberConfirmed = true;
        await _userManager.UpdateAsync(user);

        return Result.Success<object>(data: true, status: System.Net.HttpStatusCode.OK);
    }

    public async Task<Result<object>> SendOtp(string mobileNumber, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == mobileNumber);

            if (user is null)
                return Result.Failure<object>(error: Error.InvalidUser);

            user.VerifyCode = 1234;
            user.VerifyCodeExpirationDate = DateTime.Now.AddMinutes(3);

            await _userManager.UpdateAsync(user);

            //var smsRequest = new SendSmsByTemplateRequest(
            //    Data: user.VerifyCode.ToString(),
            //    Template: $"Otp",
            //    Receiver: user.PhoneNumber,
            //    Subject: "کد تایید");
            //await _publishEndpoint.Publish<SendSingleSms>(new
            //{

            //});

            return Result.Success<object>(data: true, status: System.Net.HttpStatusCode.OK);
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    public async Task<Result<object>> VerifyOtp(string mobileNumber, int code, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == mobileNumber);
        if (user is null)
            return Result.Failure<object>(error: Error.InvalidUser);

        if (user.VerifyCode != code)
            return Result.Failure<object>(error: Error.InvalidUser);

        if (user.VerifyCodeExpirationDate <= DateTime.Now)
            return Result.Failure<object>(error: Error.InvalidUser);

        var userRoles = await _userManager.GetRolesAsync(user);
        if (userRoles is null)
            throw new Exception();

        if (!user.PhoneNumberConfirmed)
        {
            user.PhoneNumberConfirmed = true;
            _appDbContext.Set<ApplicationUser>().Update(user);
            await _appDbContext.SaveChangesAsync();
        }

        var roles = _roleManager.Roles.Where(r => userRoles.Any(x => x == r.Name)).Select(x => x.Id).ToList();

        JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user);
        AuthenticationResponse response = new AuthenticationResponse();
        response.Id = user.Id;
        response.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        response.Email = user.Email;
        response.UserName = user.UserName;
        var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
        response.Roles = rolesList.ToList();
        response.IsVerified = user.EmailConfirmed;
        var refreshToken = GenerateRefreshToken("::1");
        response.RefreshToken = refreshToken.Token;
        return Result.Success<object>(data: true, status: System.Net.HttpStatusCode.OK);
    }

}
