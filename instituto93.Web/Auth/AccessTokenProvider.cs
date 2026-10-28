using Microsoft.AspNetCore.Authentication;

namespace instituto93.Web.Auth;

public sealed class AccessTokenProvider(IHttpContextAccessor httpContextAccessor)
{
    public async Task<string> GetAsync()
    {
        var httpContext = httpContextAccessor.HttpContext
            ?? throw new InvalidOperationException("No hay un contexto HTTP disponible para obtener la sesión.");

        return await httpContext.GetTokenAsync(AuthCookie.AccessToken)
            ?? throw new InvalidOperationException("La sesión no contiene un token de acceso.");
    }
}
