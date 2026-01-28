# Use the official .NET runtime as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use the SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["src/Company.Template.Api/Company.Template.Api.csproj", "src/Company.Template.Api/"]
COPY ["src/Company.Template.Application/Company.Template.Application.csproj", "src/Company.Template.Application/"]
COPY ["src/Company.Template.Domain/Company.Template.Domain.csproj", "src/Company.Template.Domain/"]
COPY ["src/Company.Template.Infrastructure/Company.Template.Infrastructure.csproj", "src/Company.Template.Infrastructure/"]
COPY ["src/Company.Template.Grpc/Company.Template.Grpc.csproj", "src/Company.Template.Grpc/"]
COPY ["src/Company.Template.Shared/Company.Template.Shared.csproj", "src/Company.Template.Shared/"]

RUN dotnet restore "src/Company.Template.Api/Company.Template.Api.csproj"

# Copy the rest of the application code
COPY . .

# Build the application
WORKDIR "/src/src/Company.Template.Api"
RUN dotnet build "Company.Template.Api.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "Company.Template.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage/image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Create a non-root user
RUN adduser --disabled-password --gecos '' appuser && chown -R appuser /app
USER appuser

ENTRYPOINT ["dotnet", "Company.Template.Api.dll"]