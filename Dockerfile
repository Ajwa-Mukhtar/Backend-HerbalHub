# Use official .NET SDK image for build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# copy csproj and restore
COPY *.csproj ./
RUN dotnet restore "./HerbalHub.csproj"

# copy everything else and build
COPY . ./
RUN dotnet publish "./HerbalHub.csproj" -c Release -o /out

# final image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /out .
ENTRYPOINT ["dotnet", "HerbalHub.dll"]
