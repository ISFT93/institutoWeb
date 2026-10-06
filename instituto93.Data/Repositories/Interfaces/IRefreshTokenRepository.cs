using instituto93.Domain.Models;

namespace instituto93.Data.Repositories.Interfaces;

public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken token, CancellationToken cancellationToken = default);
    Task<RefreshToken?> GetByHashAsync(byte[] tokenHash, CancellationToken cancellationToken = default);

    // Marca el token actual como usado e inserta su reemplazo en una transacción.
    // Devuelve false si el token ya había sido usado o revocado (otro request lo rotó antes).
    Task<bool> RotateAsync(int currentId, RefreshToken replacement, CancellationToken cancellationToken = default);

    Task RevokeFamilyAsync(Guid familyId, CancellationToken cancellationToken = default);

    // Borra las familias del usuario que ya no tienen ningún token utilizable.
    Task DeleteInactiveFamiliesAsync(int usuarioId, CancellationToken cancellationToken = default);
}
