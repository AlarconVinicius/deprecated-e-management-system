using EMS.Authentication.API.Models;
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
    public AuthService(SignInManager<IdentityUser> signInManager,
                       UserManager<IdentityUser> userManager,
                       RoleManager<IdentityRole> roleManager,
                       IOptions<AppSettings> appSettings,
                       INotifier notifier) : base(notifier)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _roleManager = roleManager;
        _appSettings = appSettings.Value;
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
        //await _userManager.AddClaimAsync(userIdentity!, new Claim("Permission", PermissionEnum.Reader.ToString()));
        //await _signInManager.SignInAsync(user, false);

        //var clienteResult = await RegisterClient(registerUser);

        //if (!clienteResult.ValidationResult.IsValid)
        //{
        //    await _userManager.DeleteAsync(user);
        //    return CustomResponse(clienteResult.ValidationResult);
        //}

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

    //private async Task<ResponseMessage> RegistrarCliente(UsuarioRegistro usuarioRegistro)
    //{
    //    var usuario = await _userManager.FindByEmailAsync(usuarioRegistro.Email);

    //    var usuarioRegistrado = new UsuarioRegistradoIntegrationEvent(
    //        Guid.Parse(usuario!.Id), usuarioRegistro.Nome, usuarioRegistro.Email, usuarioRegistro.Cpf);

    //    try
    //    {
    //        return await _bus.RequestAsync<UsuarioRegistradoIntegrationEvent, ResponseMessage>(usuarioRegistrado);
    //    }
    //    catch
    //    {
    //        await _userManager.DeleteAsync(usuario);
    //        throw;
    //    }
    //}

    private async Task<bool> AddRoleAsync(RegisterUser registerUser)
    {
        var roleExists = await _roleManager.RoleExistsAsync(registerUser.Role.ToString());
        var userIdentity = await _userManager.FindByEmailAsync(registerUser.Email);
        if (!roleExists)
        {
            // Se a role não existir, crie-a
            var createRoleResult = await _roleManager.CreateAsync(new IdentityRole(registerUser.Role.ToString()));

            if (!createRoleResult.Succeeded)
            {
                Notify($"Erro ao criar a role: {registerUser.Role}");
                return false;
            }
        }

        // Adicionar o usuário à role
        await _userManager.AddToRoleAsync(userIdentity!, registerUser.Role.ToString());
        return true;
    }
}
