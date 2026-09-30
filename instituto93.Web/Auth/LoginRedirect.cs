using Microsoft.AspNetCore.Components;

namespace instituto93.Web.Auth;

public static class LoginRedirect
{
    public const string LoginPath = "/login";

    // forceLoad: la navegación tiene que ser una request HTTP para que la cookie se revalide.
    public static void Navigate(NavigationManager navigation)
    {
        var returnUrl = "/" + navigation.ToBaseRelativePath(navigation.Uri);
        navigation.NavigateTo($"{LoginPath.TrimStart('/')}?returnUrl={Uri.EscapeDataString(returnUrl)}", forceLoad: true);
    }

    // Solo rutas locales, para evitar open redirects (//evil.com, /\evil.com).
    public static string SanitizeReturnUrl(string? returnUrl)
    {
        if (string.IsNullOrEmpty(returnUrl) || returnUrl[0] != '/')
            return "/";

        if (returnUrl.Length > 1 && returnUrl[1] is '/' or '\\')
            return "/";

        if (returnUrl.StartsWith(LoginPath, StringComparison.OrdinalIgnoreCase) ||
            returnUrl.StartsWith("/account/", StringComparison.OrdinalIgnoreCase))
            return "/";

        return returnUrl;
    }
}
