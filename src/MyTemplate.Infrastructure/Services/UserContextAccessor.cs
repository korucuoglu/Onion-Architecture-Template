﻿using System.Security.Authentication;
using Microsoft.AspNetCore.Http;
using MyTemplate.Application.ApplicationManagement.Services;
using ClaimTypes = MyTemplate.Application.ApplicationManagement.Common.Constants.ClaimTypes;

namespace MyTemplate.Infrastructure.Services;

public class UserContextAccessor : IUserContextAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IHashService _hashService;

    public UserContextAccessor(IHttpContextAccessor httpContextAccessor, IHashService hashService)
    {
        _httpContextAccessor = httpContextAccessor;
        _hashService = hashService;
    }

    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
    public int UserId => _hashService.Decode(EncodedUserId);

    public string EncodedUserId
    {
        get
        {
            if (!IsAuthenticated)
            {
                throw new AuthenticationException();
            }

            return _httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.Id)!.Value;
        }
    }
}