using EMS.Authentication.API.Models;
using EMS.Core.Messages.Integration;
using EMS.MessageBus;
using EMS.WebAPI.Core.Authentication;
using EMS.WebAPI.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EMS.Authentication.API.Business;

public class AuthService : MainService, IAuthService
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly AppSettings _appSettings;
    private readonly IMessageBus _bus;
    public AuthService(SignInManager<IdentityUser> signInManager,
                       UserManager<IdentityUser> userManager,
                       RoleManager<IdentityRole> roleManager,
                       IOptions<AppSettings> appSettings,
                       IMessageBus bus,
                       INotifier notifier) : base(notifier)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _roleManager = roleManager;
        _appSettings = appSettings.Value;
        _bus = bus;
    }

    public async Task<UserResponse> RegisterUserAsync(RegisterUser registerUser)
    {
        if (registerUser.Role == ERole.SuperAdmin)
        {
            Notify("Role não permitida para registro.");
            return null!;
        }
        var user = new IdentityUser
        {
            UserName = registerUser.Email,
            Email = registerUser.Email,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, registerUser.Password!);

        if (!result.Succeeded)
        {
            foreach (var errors in result.Errors)
            {
                Notify(errors.Description);
            }
            return null!;
        }
        if (!await AddRoleAsync(registerUser)) return null!;
        if(registerUser.Role == ERole.Admin)
        {
            var addClaim = new AddUserClaim { Email = registerUser.Email, Type = "ClaimControl", Value = "Add,Updade,Delete" };
            await AddClaimAsync(addClaim);
        }
        var clientResult = await RegisterClient(registerUser);
        var clientPlanResult = await RegisterClientPlan(registerUser);

        if (!clientResult.ValidationResult.IsValid || !clientPlanResult.ValidationResult.IsValid)
        {
            await DeleteClient(registerUser);
            await _userManager.DeleteAsync(user);
            Notify(clientResult.ValidationResult);
            Notify(clientPlanResult.ValidationResult);
            return null!;
        }

        return await GenerateJwt(registerUser.Email);
    }

    public async Task<UserResponse> LoginUserAsync(LoginUser loginUser)
    {
        var result = await _signInManager.PasswordSignInAsync(loginUser.Email!, loginUser.Password!, false, true);

        if (result.Succeeded)
            return await GenerateJwt(loginUser.Email);

        if (result.IsLockedOut)
        {
            Notify("Usuário temporariamente bloqueado por tentativas inválidas");
            return null!;
        }

        Notify("Usuário ou senha inválidos.");
        return null!;
    }
    public async Task<UserResponse> AddClaimAsync(AddUserClaim userClaim)
    {
        IdentityResult result;
        var userIdentity = await _userManager.FindByEmailAsync(userClaim.Email);
        var existingClaims = await _userManager.GetClaimsAsync(userIdentity!);
        var existingClaim = existingClaims.FirstOrDefault(c => c.Type == userClaim.Type);

        if (existingClaim != null)
        {
            var updatedClaim = new Claim(userClaim.Type, $"{existingClaim.Value},{userClaim.Value}");
            result = await _userManager.ReplaceClaimAsync(userIdentity!, existingClaim, updatedClaim);
        }
        else
        {
            result = await _userManager.AddClaimAsync(userIdentity!, new Claim(userClaim.Type, userClaim.Value));
        }

        if (result.Succeeded)
        {
            return await GenerateJwt(userClaim.Email);
        }

        foreach (var error in result.Errors)
        {
            Notify(error.Description);
        }

        return null!;
    }

    public async Task<UserResponse> RemoveClaimAsync(AddUserClaim userClaim)
    {
        IdentityResult result;
        var userIdentity = await _userManager.FindByEmailAsync(userClaim.Email);
        var existingClaims = await _userManager.GetClaimsAsync(userIdentity!);
        var existingClaim = existingClaims.FirstOrDefault(c => c.Type == userClaim.Type);

        if (existingClaim != null)
        {
            var values = existingClaim.Value.Split(',');

            values = values.Where(v => v != userClaim.Value).ToArray();

            var updatedClaim = new Claim(userClaim.Type, string.Join(",", values));

            result = await _userManager.ReplaceClaimAsync(userIdentity!, existingClaim, updatedClaim);
        }
        else
        {
            Notify($"Claim {userClaim.Type} não encontrada.");
            return null!;
        }

        if (result.Succeeded)
        {
            return await GenerateJwt(userClaim.Email);
        }

        foreach (var error in result.Errors)
        {
            Notify(error.Description);
        }

        return null!;
    }
    public Task<UserResponse> AddRoleAsync(User user, string role)
    {
        throw new NotImplementedException();
    }

    public Task<UserResponse> RemoveRoleAsync(User user, string role)
    {
        throw new NotImplementedException();
    }

    #region Identity
    private async Task<bool> AddRoleAsync(RegisterUser registerUser)
    {
        var roleExists = await _roleManager.RoleExistsAsync(registerUser.Role.ToString());
        var userIdentity = await _userManager.FindByEmailAsync(registerUser.Email);
        if (!roleExists)
        {
            var createRoleResult = await _roleManager.CreateAsync(new IdentityRole(registerUser.Role.ToString()));

            if (!createRoleResult.Succeeded)
            {
                Notify($"Erro ao criar a role: {registerUser.Role}");
                return false;
            }
        }

        await _userManager.AddToRoleAsync(userIdentity!, registerUser.Role.ToString());
        return true;
    }
    private async Task<UserResponse> GenerateJwt(string email)
    {
        var userDb = await _userManager.FindByEmailAsync(email);
        if (userDb == null)
        {
            return null!;
        }
        var claims = (await _userManager.GetClaimsAsync(userDb)).ToList();
        var userRoles = await _userManager.GetRolesAsync(userDb);
        AddStandardClaims(claims, userDb);
        AddUserRolesClaims(claims, userRoles);

        var token = GenerateToken(claims);

        return CreateResponse(token, userDb, claims);
    }

    private void AddStandardClaims(List<Claim> claims, IdentityUser user)
    {
        claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
        claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email!));
        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));
    }
    private void AddUserRolesClaims(List<Claim> claims, IList<string> userRoles)
    {
        foreach (var userRole in userRoles)
        {
            claims.Add(new Claim("role", userRole));
        }
    }

    private SecurityToken GenerateToken(List<Claim> claims)
    {
        var identityClaims = new ClaimsIdentity();
        identityClaims.AddClaims(claims);

        var key = Encoding.ASCII.GetBytes(_appSettings.Secret!);

        var tokenHandler = new JwtSecurityTokenHandler();

        return tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Subject = identityClaims,
            Issuer = _appSettings.Issuer,
            Audience = _appSettings.Audience,
            Expires = DateTime.UtcNow.AddHours(_appSettings.ExpirationHours),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        });

    }

    private UserResponse CreateResponse(SecurityToken token, IdentityUser user, List<Claim> claims)
    {
        var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);

        return new UserResponse
        {
            AccessToken = encodedToken,
            ExpiresIn = TimeSpan.FromHours(_appSettings.ExpirationHours).TotalSeconds,
            UserToken = new UserToken
            {
                Id = user.Id,
                Email = user.Email!,
                Claims = claims.Select(c => new UserClaim { Type = c.Type, Value = c.Value })
            }
        };
    }

    private static long ToUnixEpochDate(DateTime date)
        => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    #endregion
    private async Task<ResponseMessage> RegisterClient(RegisterUser registerUser)
    {
        var userDb = await _userManager.FindByEmailAsync(registerUser.Email);

        var registerUserEvent = new RegisteredIdentityIntegrationEvent(Guid.Parse(userDb!.Id), registerUser.Name, registerUser.Email, registerUser.Cpf);

        try
        {
            return await _bus.RequestAsync<RegisteredIdentityIntegrationEvent, ResponseMessage>(registerUserEvent);
        }
        catch
        {
            await _userManager.DeleteAsync(userDb);
            throw;
        }
    }

    private async Task<ResponseMessage> RegisterClientPlan(RegisterUser registerUser)
    {
        var userDb = await _userManager.FindByEmailAsync(registerUser.Email);

        var registerClientPlanEvent = new RegisteredUserIntegrationEvent(Guid.Parse(userDb!.Id), registerUser.PlanId, registerUser.Name, registerUser.Email, registerUser.Cpf, true);

        try
        {
            return await _bus.RequestAsync<RegisteredUserIntegrationEvent, ResponseMessage>(registerClientPlanEvent);
        }
        catch
        {
            await DeleteClient(registerUser);
            await _userManager.DeleteAsync(userDb);
            throw;
        }
    }
    private async Task<ResponseMessage> DeleteClient(RegisterUser registerUser)
    {
        var userDb = await _userManager.FindByEmailAsync(registerUser.Email);

        var deleteClientPlanEvent = new DeletedUserIntegrationEvent(Guid.Parse(userDb.Id));

        try
        {
            return await _bus.RequestAsync<DeletedUserIntegrationEvent, ResponseMessage>(deleteClientPlanEvent);
        }
        catch
        {
            throw;
        }
    }

}
