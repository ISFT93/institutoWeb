using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Caching.Distributed;

namespace instituto93.Web.Auth;

// Guarda valores cifrados con Data Protection en IDistributedCache, para que los tokens
// no queden en claro si el caché pasa a ser Redis o SQL Server.
public abstract class ProtectedCacheStore<T> where T : class
{
    private readonly IDistributedCache _cache;
    private readonly IDataProtector _protector;
    private readonly string _keyPrefix;

    protected ProtectedCacheStore(IDistributedCache cache, IDataProtectionProvider dataProtection, string purpose)
    {
        _cache = cache;
        _protector = dataProtection.CreateProtector(purpose);
        _keyPrefix = $"{purpose}:";
    }

    public static string NewKey() => WebEncoders.Base64UrlEncode(RandomNumberGenerator.GetBytes(32));

    protected async Task<T?> ReadAsync(string key, CancellationToken cancellationToken)
    {
        var protectedBytes = await _cache.GetAsync(_keyPrefix + key, cancellationToken);
        if (protectedBytes is null)
            return null;

        try
        {
            return JsonSerializer.Deserialize<T>(_protector.Unprotect(protectedBytes));
        }
        catch (CryptographicException)
        {
            return null;
        }
    }

    protected Task WriteAsync(string key, T value, DateTimeOffset expiresAt, CancellationToken cancellationToken)
    {
        var bytes = _protector.Protect(JsonSerializer.SerializeToUtf8Bytes(value));
        return _cache.SetAsync(
            _keyPrefix + key,
            bytes,
            new DistributedCacheEntryOptions { AbsoluteExpiration = expiresAt },
            cancellationToken);
    }

    protected Task DeleteAsync(string key, CancellationToken cancellationToken) =>
        _cache.RemoveAsync(_keyPrefix + key, cancellationToken);
}

public sealed class TokenSessionStore(IDistributedCache cache, IDataProtectionProvider dataProtection)
    : ProtectedCacheStore<TokenSet>(cache, dataProtection, "instituto93.Auth.Session")
{
    public Task<TokenSet?> GetAsync(string sessionId, CancellationToken cancellationToken = default) =>
        ReadAsync(sessionId, cancellationToken);

    public Task SetAsync(string sessionId, TokenSet tokens, CancellationToken cancellationToken = default) =>
        WriteAsync(sessionId, tokens, tokens.RefreshTokenExpiresAt, cancellationToken);

    public Task RemoveAsync(string sessionId, CancellationToken cancellationToken = default) =>
        DeleteAsync(sessionId, cancellationToken);
}

public sealed record LoginTicket(TokenSet Tokens, CurrentUser User);

// Traslada el resultado del login desde el circuito interactivo (sin HttpContext)
// a una request HTTP que pueda emitir la cookie. Es de un solo uso y dura un minuto.
public sealed class LoginTicketStore(IDistributedCache cache, IDataProtectionProvider dataProtection)
    : ProtectedCacheStore<LoginTicket>(cache, dataProtection, "instituto93.Auth.LoginTicket")
{
    private static readonly TimeSpan Lifetime = TimeSpan.FromMinutes(1);

    public async Task<string> CreateAsync(LoginTicket ticket, CancellationToken cancellationToken = default)
    {
        var id = NewKey();
        await WriteAsync(id, ticket, DateTimeOffset.UtcNow.Add(Lifetime), cancellationToken);
        return id;
    }

    public async Task<LoginTicket?> RedeemAsync(string id, CancellationToken cancellationToken = default)
    {
        var ticket = await ReadAsync(id, cancellationToken);
        if (ticket is not null)
            await DeleteAsync(id, cancellationToken);
        return ticket;
    }
}
