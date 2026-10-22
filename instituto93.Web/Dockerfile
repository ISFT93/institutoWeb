FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY instituto93.Web.csproj ./
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

RUN mkdir -p /app/keys && chown -R $APP_UID /app/keys
COPY --from=build /app/publish .

ENV ASPNETCORE_HTTP_PORTS=8080 \
    DataProtection__KeysPath=/app/keys \
    Api__BaseUrl=http://localhost:8000

VOLUME /app/keys
EXPOSE 8080
USER $APP_UID

ENTRYPOINT ["dotnet", "instituto93.Web.dll"]
