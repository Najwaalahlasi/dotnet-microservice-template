# Company.Template - .NET Microservice Template

A production-ready .NET 7.0 microservice template implementing CQRS, Domain-Driven Design, and modern architectural patterns.

## 🏗️ Architecture Overview

This template demonstrates a clean architecture approach with:

- **Domain Layer**: Core business logic and entities
- **Application Layer**: CQRS commands/queries with MediatR
- **Infrastructure Layer**: Data access, messaging, and external services
- **API Layer**: REST and gRPC endpoints
- **Shared Layer**: Common DTOs and validation

## 🚀 Features

### Core Features
- ✅ **CQRS Pattern** with MediatR
- ✅ **Domain-Driven Design** with rich domain models
- ✅ **Entity Framework Core** with PostgreSQL
- ✅ **gRPC Services** for high-performance communication
- ✅ **REST API** with OpenAPI/Swagger documentation
- ✅ **RabbitMQ Integration** for messaging
- ✅ **AutoMapper** for object mapping
- ✅ **FluentValidation** for input validation

### Observability & Monitoring
- ✅ **Structured Logging** with Serilog
- ✅ **Health Checks** for dependencies
- ✅ **Metrics & Tracing** (OpenTelemetry ready)

### Quality & Testing
- ✅ **Unit Tests** with xUnit and FluentAssertions
- ✅ **Integration Tests** with TestContainers
- ✅ **Code Coverage** reporting
- ✅ **Static Code Analysis**

### DevOps & Deployment
- ✅ **Docker Support** with multi-stage builds
- ✅ **Docker Compose** for local development
- ✅ **GitHub Actions** CI/CD pipeline
- ✅ **Security Scanning** with Trivy

## 🛠️ Technology Stack

| Category | Technology |
|----------|------------|
| **Framework** | .NET 7.0 |
| **Database** | PostgreSQL with EF Core |
| **Messaging** | RabbitMQ |
| **API** | ASP.NET Core Web API + gRPC |
| **Documentation** | OpenAPI/Swagger |
| **Logging** | Serilog |
| **Testing** | xUnit, FluentAssertions, Moq |
| **Containerization** | Docker & Docker Compose |
| **CI/CD** | GitHub Actions |

## 🏃‍♂️ Quick Start

### Prerequisites

- [.NET 7.0 SDK](https://dotnet.microsoft.com/download/dotnet/7.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [PostgreSQL](https://www.postgresql.org/) (or use Docker)

### Local Development Setup

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd Company.Template
   ```

2. **Start dependencies with Docker Compose**
   ```bash
   docker-compose up -d postgres rabbitmq
   ```

3. **Restore NuGet packages**
   ```bash
   dotnet restore
   ```

4. **Update database schema**
   ```bash
   dotnet ef database update --project src/Company.Template.Infrastructure --startup-project src/Company.Template.Api
   ```

5. **Run the application**
   ```bash
   dotnet run --project src/Company.Template.Api
   ```

6. **Access the application**
   - REST API: https://localhost:5001/swagger
   - Health Checks: https://localhost:5001/health
   - gRPC: localhost:5001 (use gRPC client tools)

### Using Docker

1. **Build and run everything**
   ```bash
   docker-compose up --build
   ```

2. **Access the application**
   - REST API: http://localhost:5000/swagger
   - Health Checks: http://localhost:5000/health
   - RabbitMQ Management: http://localhost:15672 (guest/guest)

## 📋 API Examples

### REST API Examples

**Create a Product**
```bash
curl -X POST "https://localhost:5001/api/products" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Sample Product",
    "description": "A sample product for testing",
    "price": 29.99
  }'
```

**Get a Product**
```bash
curl -X GET "https://localhost:5001/api/products/{id}"
```

**List Products**
```bash
curl -X GET "https://localhost:5001/api/products?pageNumber=1&pageSize=10"
```

### gRPC Examples

Use tools like [grpcurl](https://github.com/fullstorydev/grpcurl) or [BloomRPC](https://github.com/bloomrpc/bloomrpc):

```bash
# List available services
grpcurl -plaintext localhost:5001 list

# Create a product
grpcurl -plaintext -d '{
  "name": "gRPC Product",
  "description": "Created via gRPC",
  "price": 19.99
}' localhost:5001 product.ProductService/CreateProduct
```

## 🧪 Testing

### Run Unit Tests
```bash
dotnet test tests/Company.Template.UnitTests
```

### Run Integration Tests
```bash
dotnet test tests/Company.Template.IntegrationTests
```

### Run All Tests with Coverage
```bash
dotnet test --collect:"XPlat Code Coverage"
```

## 📦 Project Structure

```
Company.Template/
├── src/
│   ├── Company.Template.Api/          # REST API & gRPC host
│   ├── Company.Template.Application/  # CQRS handlers & business logic
│   ├── Company.Template.Domain/       # Domain entities & interfaces
│   ├── Company.Template.Grpc/         # gRPC service implementations
│   ├── Company.Template.Infrastructure/ # Data access & external services
│   └── Company.Template.Shared/       # Shared DTOs & validation
├── tests/
│   ├── Company.Template.UnitTests/    # Unit tests
│   └── Company.Template.IntegrationTests/ # Integration tests
├── docs/                              # Documentation
├── build/                             # Build scripts
├── docker-compose.yml                 # Local development environment
└── .github/workflows/                 # CI/CD pipelines
```

## 🔧 Configuration

### Database Connection
Update `appsettings.json` or use environment variables:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=CompanyTemplateDb;Username=postgres;Password=postgres"
  }
}
```

### Message Broker
Configure RabbitMQ connection:

```json
{
  "ConnectionStrings": {
    "RabbitMQ": "amqp://guest:guest@localhost:5672/"
  }
}
```

## 🚀 Deployment

### Docker Deployment
```bash
# Build the image
docker build -f src/Company.Template.Api/Dockerfile -t company-template-api .

# Run with dependencies
docker-compose -f docker-compose.yml -f docker-compose.prod.yml up
```

### Kubernetes Deployment
See `k8s/` directory for Kubernetes manifests (coming soon).

## 📈 Monitoring & Observability

### Health Checks
- **Endpoint**: `/health`
- **Checks**: Database connectivity, RabbitMQ connectivity

### Logging
- **Structured logging** with Serilog
- **Log levels**: Information, Warning, Error
- **Outputs**: Console, File, (Elasticsearch ready)

### Metrics (Future)
- Application metrics with OpenTelemetry
- Custom business metrics
- Performance counters

## 🔒 Security

### Authentication & Authorization
- JWT Bearer token authentication (placeholder)
- Role-based authorization
- API key authentication for service-to-service

### Security Scanning
- Dependency vulnerability scanning
- Container image scanning with Trivy
- Static code analysis

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

### Development Guidelines
- Follow SOLID principles
- Write unit tests for new features
- Update documentation
- Follow conventional commit messages

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🆘 Support

- **Documentation**: Check the `docs/` directory
- **Issues**: Create an issue on GitHub
- **Discussions**: Use GitHub Discussions for questions

## 🗺️ Roadmap

- [ ] Add OpenTelemetry tracing
- [ ] Implement Event Sourcing example
- [ ] Add Kubernetes manifests
- [ ] Add API versioning
- [ ] Add rate limiting
- [ ] Add caching with Redis
- [ ] Add background job processing
- [ ] Add multi-tenancy support

---

**Happy coding! 🎉**