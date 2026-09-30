namespace instituto93.Domain.Models
{
    // Refresh token opaco. Solo se persiste el hash; el valor en claro lo recibe el cliente una única vez.
    public class RefreshToken
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public Guid FamilyId { get; set; }
        public byte[] TokenHash { get; set; } = [];
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime FamilyExpiresAt { get; set; }
        public DateTime? UsedAt { get; set; }
        public DateTime? RevokedAt { get; set; }
        public int? ReplacedById { get; set; }
    }
}
