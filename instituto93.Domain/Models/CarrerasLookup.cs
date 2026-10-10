namespace instituto93.Domain.Models
{
    // Par mínimo que necesita el formulario de preinscripción para armar el selector de carreras.
    public sealed record CarrerasLookup(int CarreraId, string Nombre);
}
