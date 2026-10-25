namespace instituto93.Domain.Rules;

public static class NotaFinalRules
{
    public const decimal Minima = 1m;
    public const decimal Maxima = 10m;
    public const decimal Aprobacion = 4m;

    public const string EstadoPendiente = "Pendiente";
    public const string EstadoAprobado = "Aprobado";
    public const string EstadoDesaprobado = "Desaprobado";

    public static bool EsValida(decimal? nota) => nota is null or >= Minima and <= Maxima;
}
