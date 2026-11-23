# Build Stage

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY src/ ./

RUN dotnet restore "ApiGateway.Api/ApiGateway.Api.csproj"

RUN dotnet publish "ApiGateway.Api/ApiGateway.Api.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore

# Runtime Stage

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

WORKDIR /app

ENV ASPNETCORE_HTTP_PORTS=8080

EXPOSE 8080

COPY --from=build /app/publish ./

ENTRYPOINT ["dotnet", "ApiGateway.Api.dll"]