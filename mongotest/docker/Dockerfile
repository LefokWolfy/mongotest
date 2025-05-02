# filepath: Dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
# Since the build context is the parent folder, copy the csproj from one level up.
COPY ["../mongotest.csproj", "./"]
RUN dotnet restore "mongotest.csproj"
# Copy all files from the parent directory (the project root).
COPY ../. .
WORKDIR "/src/."
RUN dotnet build "mongotest.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "mongotest.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "mongotest.dll"]