namespace instituto93.Domain.Rules;

public static class NotaFinalRules
{
    public const decimal Minima = 1m;
    public const decimal Maxima = 10m;
    public const decimal Aprobacion = 4m;

    public const string EstadoPendiente = "Pendiente";
    public const string EstadoAprobado = "Aprobado";
    public const string EstadoDesaprobado = "Desaprobado";
    public const string EstadoAusente = "Ausente";

    public static string? EnLetras(decimal? nota) => nota switch
    {
        null => null,
        1 => "Uno", 2 => "Dos", 3 => "Tres", 4 => "Cuatro", 5 => "Cinco",
        6 => "Seis", 7 => "Siete", 8 => "Ocho", 9 => "Nueve", 10 => "Diez",
        _ => nota.Value.ToString("0.##")
    };

    public static bool EsValida(decimal? nota) => nota is null or >= Minima and <= Maxima;
}
