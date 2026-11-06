# Contexto de build: raíz del repo (necesita Application, Data y Domain).
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY instituto93.Domain/instituto93.Domain.csproj instituto93.Domain/
COPY instituto93.Data/instituto93.Data.csproj instituto93.Data/
COPY instituto93.Application/instituto93.Application.csproj instituto93.Application/
COPY instituto93.Controller/instituto93.Controller.csproj instituto93.Controller/
RUN dotnet restore instituto93.Controller/instituto93.Controller.csproj

COPY instituto93.Domain/ instituto93.Domain/
COPY instituto93.Data/ instituto93.Data/
COPY instituto93.Application/ instituto93.Application/
COPY instituto93.Controller/ instituto93.Controller/
RUN dotnet publish instituto93.Controller/instituto93.Controller.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_HTTP_PORTS=8000
EXPOSE 8000
USER $APP_UID

ENTRYPOINT ["dotnet", "instituto93.Controller.dll"]
