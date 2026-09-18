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
    if (-not $apiProcess.HasExited) {
        Stop-Process -Id $apiProcess.Id
        $apiProcess.WaitForExit()
    }
}
