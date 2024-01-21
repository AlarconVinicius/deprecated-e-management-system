﻿using EMS.Authentication.API.Models;
using System.Security.Claims;

namespace EMS.Authentication.API.Business;

public interface IAuthService
{
    Task<UserResponse> RegisterUserAsync(RegisterUser registerUser);
    Task<UserResponse> LoginUserAsync(LoginUser loginUser);
    Task<UserResponse> AddClaimAsync(User user, Claim claim);
    Task<UserResponse> RemoveClaimAsync(User user, Claim claim);
    Task<UserResponse> AddRoleAsync(User user, string role);
    Task<UserResponse> RemoveRoleAsync(User user, string role);
}