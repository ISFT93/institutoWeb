#!/bin/bash
# Crea la base y aplica, en orden, los scripts de /scripts/base y luego los de /scripts/web.
# Cada script aplicado se registra en dbo.__ScriptsAplicados para no repetirlo en el próximo arranque.
set -euo pipefail

SQLCMD=/opt/mssql-tools18/bin/sqlcmd
DB="${MSSQL_DATABASE:-instituto_db}"
run() { "$SQLCMD" -C -b -S "${MSSQL_HOST:-sqlserver}" -U sa -P "$MSSQL_SA_PASSWORD" "$@"; }

run -Q "IF DB_ID(N'$DB') IS NULL CREATE DATABASE [$DB];"
run -d "$DB" -Q "IF OBJECT_ID(N'dbo.__ScriptsAplicados') IS NULL CREATE TABLE dbo.__ScriptsAplicados (Nombre NVARCHAR(260) NOT NULL PRIMARY KEY, AplicadoEn DATETIME2 NOT NULL DEFAULT SYSDATETIME());"

for dir in base web; do
  for file in $(find "/scripts/$dir" -maxdepth 1 -name '*.sql' | sort); do
    name="$dir/$(basename "$file")"
    applied=$(run -d "$DB" -h -1 -W -Q "SET NOCOUNT ON; SELECT COUNT(*) FROM dbo.__ScriptsAplicados WHERE Nombre = N'$name';" | tr -d '[:space:]')
    if [ "$applied" != "0" ]; then
      echo "= $name (ya aplicado)"
      continue
    fi

    # Los scripts vienen con codificaciones mixtas (UTF-16, UTF-8, Windows-1252): se pasan a UTF-8.
    if [ "$(head -c2 "$file" | od -An -tx1 | tr -d ' ')" = "fffe" ]; then
      from=UTF-16
    elif iconv -f UTF-8 -t UTF-8 "$file" >/dev/null 2>&1; then
      from=UTF-8
    else
      from=CP1252
    fi

    iconv -f "$from" -t UTF-8 "$file" | sed -e '1s/^\xef\xbb\xbf//' > /tmp/script.sql

    echo "> $name"
    # -I: QUOTED_IDENTIFIER ON como en SSMS (sqlcmd lo deja OFF y falla con índices filtrados).
    run -d "$DB" -I -f 65001 -i /tmp/script.sql
    run -d "$DB" -Q "INSERT INTO dbo.__ScriptsAplicados (Nombre) VALUES (N'$name');"
  done
done

echo "Base '$DB' lista."
