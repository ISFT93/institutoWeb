namespace instituto93.Domain.Rules;

public static class NotaEvaluacionRules
{
    public const decimal Minima = 0m;
    public const decimal Maxima = 10m;
    public const decimal Aprobacion = 4m;

    public static bool EsValida(decimal? nota) => nota is null or >= Minima and <= Maxima;
    public static string Estado(decimal? nota, bool ausente) => ausente
        ? "Ausente"
        : nota is null ? "Sin cargar" : nota >= Aprobacion ? "Aprobado/a" : "Desaprobado/a";
}
