FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

# Create directory for DataProtection keys
RUN mkdir -p /root/.aspnet/DataProtection-Keys
VOLUME /root/.aspnet/DataProtection-Keys

ENTRYPOINT ["dotnet", "MyWebApp.dll"]