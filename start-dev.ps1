$ErrorActionPreference = "Stop"
$env:ASPNETCORE_ENVIRONMENT = "Development"

$apiProject = Join-Path $PSScriptRoot "instituto93.Controller/instituto93.Controller.csproj"
$webProject = Join-Path $PSScriptRoot "instituto93.Web/instituto93.Web.csproj"

$apiProcess = Start-Process -FilePath "dotnet" -NoNewWindow -PassThru -ArgumentList @(
    "watch", "run", "--non-interactive",
    "--project", $apiProject,
    "--no-launch-profile",
    "--urls", "http://0.0.0.0:8000"
)

try {
    dotnet watch run --non-interactive --project $webProject --no-launch-profile --urls "http://localhost:8080"
}
finally {
    # Ctrl+C ya llega al API por la consola compartida: se le da tiempo a cerrar de forma ordenada.
    if (-not $apiProcess.HasExited) {
        $null = $apiProcess.WaitForExit(10000)
    }
    # Si sigue vivo, se mata el arbol completo (dotnet watch + app hija) para no dejar puertos ocupados.
    if (-not $apiProcess.HasExited) {
        & taskkill /PID $apiProcess.Id /T /F | Out-Null
    }
}
