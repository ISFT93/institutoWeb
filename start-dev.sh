#!/usr/bin/env bash
set -uo pipefail

root_dir="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

# Job control: cada proceso en segundo plano corre en su propio grupo de procesos.
# Asi Ctrl+C solo llega a este script y el trap se lo reenvia a cada grupo.
# Sin esto, los procesos en segundo plano de un script ignoran SIGINT.
# Los </dev/null son necesarios: en su propio grupo, leer del terminal detiene el
# proceso (SIGTTIN) y `wait -n` lo confunde con una salida.
set -m

export ASPNETCORE_ENVIRONMENT=Development

dotnet watch run --non-interactive \
  --project "$root_dir/instituto93.Controller/instituto93.Controller.csproj" \
  --no-launch-profile \
  --urls "http://0.0.0.0:8000" </dev/null &
api_pid=$!

dotnet watch run --non-interactive \
  --project "$root_dir/instituto93.Web/instituto93.Web.csproj" \
  --no-launch-profile \
  --urls "http://localhost:8080" </dev/null &
web_pid=$!

pids=("$api_pid" "$web_pid")
cleaning=0

cleanup() {
  ((cleaning)) && return
  cleaning=1
  trap '' INT TERM   # ignora Ctrl+C adicionales mientras se apaga

  echo
  echo "Deteniendo servicios..."

  # Cada servicio es lider de su propio grupo (set -m), asi que se senaliza el grupo
  # completo (dotnet watch + app hija), igual que haria un Ctrl+C real.
  # Escala: SIGINT (5s) -> SIGTERM (3s) -> SIGKILL.
  for sig in INT TERM KILL; do
    for pid in "${pids[@]}"; do
      kill "-$sig" -- "-$pid" 2>/dev/null || true
    done

    case $sig in INT) tries=50 ;; TERM) tries=30 ;; *) tries=0 ;; esac
    for ((i = 0; i < tries; i++)); do
      alive=0
      for pid in "${pids[@]}"; do
        kill -0 -- "-$pid" 2>/dev/null && alive=1
      done
      ((alive)) || break 2
      sleep 0.1
    done
  done

  wait 2>/dev/null || true
}

trap 'cleanup; exit 130' INT TERM
trap cleanup EXIT

# Si uno de los dos termina por su cuenta, se detiene el otro.
wait -n 2>/dev/null
