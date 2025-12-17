# Use .NET SDK for building
from mcr.microsoft.com/dotnet/sdk:8.0 AS build
workdir /app

# Copy everything from solution root
copy . .

# Restore dependencies
run dotnet restore

# Publish the API project
run dotnet publish -c Release -o /app/publish ./Social.Api/Social.Api.csproj

# Use smaller runtime image
from mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
workdir /app

# Copy published files from build stage
copy --from=build /app/publish .

# Run the API
entrypoint ["dotnet", "Social.Api.dll"]
