#!/usr/bin/env bash
set -euo pipefail

root_dir="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

ASPNETCORE_ENVIRONMENT=Development dotnet watch run --non-interactive \
  --project "$root_dir/instituto93.Controller/instituto93.Controller.csproj" \
  --no-launch-profile \
  --urls "http://0.0.0.0:8001" &
api_pid=$!

cleanup() {
  kill "$api_pid" 2>/dev/null || true
  wait "$api_pid" 2>/dev/null || true
}
trap cleanup EXIT INT TERM

ASPNETCORE_ENVIRONMENT=Development dotnet watch run --non-interactive \
  --project "$root_dir/instituto93.Web/instituto93.Web.csproj" \
  --no-launch-profile \
  --urls "http://localhost:8080"
