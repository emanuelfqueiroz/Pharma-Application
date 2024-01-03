using Microsoft.AspNetCore.Http;
using PharmaRep.Application.Common;
using System.Security.Claims;

namespace PharmaRep.Infra.Security;

internal class HttpIdentifier(IHttpContextAccessor httpContextAccessor) : IIdentifierService
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public int GetUserId() => int.Parse(
            _httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

    public bool HasUserId() => _httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier) != null;
}