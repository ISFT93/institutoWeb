using System.Collections.Concurrent;
using instituto93.Web.Services;

namespace instituto93.Web.Auth;

// Un lock por sesión: con rotación de refresh tokens, dos renovaciones en paralelo harían
// que la API detecte reutilización y revoque la sesión.
public sealed class SessionRefreshLocks
{
    private readonly ConcurrentDictionary<string, SemaphoreSlim> _locks = new();

    public SemaphoreSlim Get(string sessionId) => _locks.GetOrAdd(sessionId, _ => new SemaphoreSlim(1, 1));

    public void Remove(string sessionId) => _locks.TryRemove(sessionId, out _);
}

public sealed class TokenSessionManager(
    TokenSessionStore store,
    SessionRefreshLocks locks,
    AuthApiClient authApi,
    ILogger<TokenSessionManager> logger)
{
    // Se renueva un poco antes del vencimiento para que el token no expire en tránsito.
    private static readonly TimeSpan RefreshMargin = TimeSpan.FromSeconds(60);

    public async Task<string> StartAsync(TokenSet tokens, CancellationToken cancellationToken = default)
    {
        var sessionId = TokenSessionStore.NewKey();
        await store.SetAsync(sessionId, tokens, cancellationToken);
        return sessionId;
    }

    /// <summary>
    /// Devuelve tokens vigentes para la sesión, renovándolos si el access token venció o está por vencer.
    /// Devuelve null si la sesión no existe o la API rechazó el refresh token (hay que volver a iniciar sesión).
    /// Lanza <see cref="HttpRequestException"/> si la API no está disponible; en ese caso la sesión se conserva.
    /// </summary>
    public async Task<TokenSet?> GetValidTokensAsync(
        string sessionId,
        bool forceRefresh = false,
        CancellationToken cancellationToken = default)
    {
        var tokens = await store.GetAsync(sessionId, cancellationToken);
        if (tokens is null)
            return null;

        if (!forceRefresh && !NeedsRefresh(tokens))
            return tokens;

        var gate = locks.Get(sessionId);
        await gate.WaitAsync(cancellationToken);
        try
        {
            var current = await store.GetAsync(sessionId, cancellationToken);
            if (current is null)
                return null;

            var refreshedByOther = current.RefreshToken != tokens.RefreshToken;
            if (!NeedsRefresh(current) && (!forceRefresh || refreshedByOther))
                return current;

            if (current.RefreshTokenExpiresAt <= DateTimeOffset.UtcNow)
            {
                await RemoveAsync(sessionId);
                return null;
            }

            // Sin cancelación: si la API ya rotó el token y se corta la request antes de guardarlo,
            // el refresh token viejo quedaría inservible y la sesión se perdería.
            var refreshed = await authApi.RefreshAsync(current.RefreshToken, CancellationToken.None);
            if (refreshed is null)
            {
                logger.LogInformation("La API rechazó el refresh token; se cierra la sesión.");
                await RemoveAsync(sessionId);
                return null;
            }

            await store.SetAsync(sessionId, refreshed, CancellationToken.None);
            return refreshed;
        }
        finally
        {
            gate.Release();
        }
    }

    public async Task EndAsync(string sessionId, CancellationToken cancellationToken = default)
    {
        var tokens = await store.GetAsync(sessionId, cancellationToken);
        await RemoveAsync(sessionId);

        if (tokens is null)
            return;

        try
        {
            await authApi.LogoutAsync(tokens.RefreshToken, cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            // La sesión local ya se eliminó; el refresh token vencerá solo.
            logger.LogWarning(ex, "No se pudo revocar el refresh token en la API.");
        }
    }

    private async Task RemoveAsync(string sessionId)
    {
        await store.RemoveAsync(sessionId, CancellationToken.None);
        locks.Remove(sessionId);
    }

    private static bool NeedsRefresh(TokenSet tokens) =>
        tokens.AccessTokenExpiresAt - RefreshMargin <= DateTimeOffset.UtcNow;
}
