# Template Usage Guide

This guide explains how to use the Company.Template as a foundation for creating new microservices with different entities.

## 🎯 Creating a New Entity

Follow these steps to add a new entity (e.g., `Customer`) to your microservice:

### 1. Domain Layer

**Create the Entity** (`src/Company.Template.Domain/Entities/Customer.cs`):
```csharp
using Company.Template.Domain.Common;

namespace Company.Template.Domain.Entities;

public class Customer : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }

    public static Customer Create(string firstName, string lastName, string email, DateTime dateOfBirth)
    {
        return new Customer
        {
            Id = Guid.NewGuid(),
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            DateOfBirth = dateOfBirth,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Update(string firstName, string lastName, string email, DateTime dateOfBirth)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        DateOfBirth = dateOfBirth;
        UpdatedAt = DateTime.UtcNow;
    }
}
```

**Create Domain Events**:
- `CustomerCreatedEvent.cs`
- `CustomerUpdatedEvent.cs`
- `CustomerDeletedEvent.cs`

**Create Repository Interface** (`src/Company.Template.Domain/Repositories/ICustomerRepository.cs`):
```csharp
using Company.Template.Domain.Entities;

namespace Company.Template.Domain.Repositories;

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Customer>> GetAllAsync(int skip = 0, int take = 100, CancellationToken cancellationToken = default);
    Task<int> GetCountAsync(CancellationToken cancellationToken = default);
    Task<Customer> AddAsync(Customer customer, CancellationToken cancellationToken = default);
    Task<Customer> UpdateAsync(Customer customer, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
```

### 2. Shared Layer

**Create DTOs** (`src/Company.Template.Shared/DTOs/CustomerDto.cs`):
```csharp
namespace Company.Template.Shared.DTOs;

public record CustomerDto(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    DateTime DateOfBirth,
    DateTime CreatedAt,
    DateTime? UpdatedAt);

public record CreateCustomerDto(
    string FirstName,
    string LastName,
    string Email,
    DateTime DateOfBirth);

public record UpdateCustomerDto(
    string FirstName,
    string LastName,
    string Email,
    DateTime DateOfBirth);

public record CustomersPagedResponse(
    IEnumerable<CustomerDto> Customers,
    int TotalCount,
    int PageNumber,
    int PageSize,
    bool HasNextPage,
    bool HasPreviousPage);
```

**Create Validators**:
- `CreateCustomerDtoValidator.cs`
- `UpdateCustomerDtoValidator.cs`

### 3. Application Layer

**Create Commands**:
- `CreateCustomerCommand.cs`
- `UpdateCustomerCommand.cs`
- `DeleteCustomerCommand.cs`

**Create Queries**:
- `GetCustomerByIdQuery.cs`
- `ListCustomersQuery.cs`

**Create Handlers**:
- `CreateCustomerCommandHandler.cs`
- `UpdateCustomerCommandHandler.cs`
- `DeleteCustomerCommandHandler.cs`
- `GetCustomerByIdQueryHandler.cs`
- `ListCustomersQueryHandler.cs`

**Update AutoMapper Profile** (`src/Company.Template.Application/Mappings/CustomerMappingProfile.cs`):
```csharp
using AutoMapper;
using Company.Template.Domain.Entities;
using Company.Template.Shared.DTOs;

namespace Company.Template.Application.Mappings;

public class CustomerMappingProfile : Profile
{
    public CustomerMappingProfile()
    {
        CreateMap<Customer, CustomerDto>();
        CreateMap<CreateCustomerDto, Customer>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
        CreateMap<UpdateCustomerDto, Customer>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
    }
}
```

### 4. Infrastructure Layer

**Create Repository Implementation** (`src/Company.Template.Infrastructure/Repositories/CustomerRepository.cs`):
```csharp
using Company.Template.Domain.Entities;
using Company.Template.Domain.Repositories;
using Company.Template.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Company.Template.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CustomerRepository> _logger;

    public CustomerRepository(ApplicationDbContext context, ILogger<CustomerRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    // Implement all interface methods similar to ProductRepository
}
```

**Update DbContext** (`src/Company.Template.Infrastructure/Data/ApplicationDbContext.cs`):
```csharp
public DbSet<Customer> Customers { get; set; } = null!;

// In OnModelCreating method:
modelBuilder.Entity<Customer>(entity =>
{
    entity.HasKey(e => e.Id);
    entity.Property(e => e.Id).ValueGeneratedNever();
    entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
    entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
    entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
    entity.Property(e => e.DateOfBirth).IsRequired();
    entity.Property(e => e.CreatedAt).IsRequired();
    entity.Property(e => e.UpdatedAt).IsRequired(false);
    
    entity.HasIndex(e => e.Email).IsUnique();
    entity.HasIndex(e => e.CreatedAt);
});
```

**Create Event Handlers**:
- `CustomerCreatedEventHandler.cs`
- `CustomerUpdatedEventHandler.cs`
- `CustomerDeletedEventHandler.cs`

**Update DependencyInjection** (`src/Company.Template.Infrastructure/DependencyInjection.cs`):
```csharp
services.AddScoped<ICustomerRepository, CustomerRepository>();
```

### 5. gRPC Layer

**Update Protobuf** (`src/Company.Template.Grpc/Protos/customer.proto`):
```protobuf
syntax = "proto3";

option csharp_namespace = "Company.Template.Grpc";

package customer;

service CustomerService {
  rpc CreateCustomer (CreateCustomerRequest) returns (CustomerResponse);
  rpc GetCustomer (GetCustomerRequest) returns (CustomerResponse);
  rpc UpdateCustomer (UpdateCustomerRequest) returns (CustomerResponse);
  rpc DeleteCustomer (DeleteCustomerRequest) returns (DeleteCustomerResponse);
  rpc ListCustomers (ListCustomersRequest) returns (ListCustomersResponse);
}

message CreateCustomerRequest {
  string first_name = 1;
  string last_name = 2;
  string email = 3;
  string date_of_birth = 4;
}

// Add other message definitions...
```

**Create gRPC Service** (`src/Company.Template.Grpc/Services/CustomerGrpcService.cs`):
```csharp
using Company.Template.Application.Commands;
using Company.Template.Application.Queries;
using Grpc.Core;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Company.Template.Grpc.Services;

public class CustomerGrpcService : CustomerService.CustomerServiceBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<CustomerGrpcService> _logger;

    public CustomerGrpcService(IMediator mediator, ILogger<CustomerGrpcService> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    // Implement all gRPC methods
}
```

### 6. API Layer

**Create Controller** (`src/Company.Template.Api/Controllers/CustomersController.cs`):
```csharp
using Company.Template.Application.Commands;
using Company.Template.Application.Queries;
using Company.Template.Shared.DTOs;
using Company.Template.Shared.Validators;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Company.Template.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CustomersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<CustomersController> _logger;
    private readonly IValidator<CreateCustomerDto> _createValidator;
    private readonly IValidator<UpdateCustomerDto> _updateValidator;

    public CustomersController(
        IMediator mediator,
        ILogger<CustomersController> logger,
        IValidator<CreateCustomerDto> createValidator,
        IValidator<UpdateCustomerDto> updateValidator)
    {
        _mediator = mediator;
        _logger = logger;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    // Implement all CRUD endpoints similar to ProductsController
}
```

**Register gRPC Service** (`src/Company.Template.Api/Program.cs`):
```csharp
app.MapGrpcService<CustomerGrpcService>();
```

### 7. Database Migration

**Create Migration**:
```bash
dotnet ef migrations add AddCustomerEntity --project src/Company.Template.Infrastructure --startup-project src/Company.Template.Api
```

**Update Database**:
```bash
dotnet ef database update --project src/Company.Template.Infrastructure --startup-project src/Company.Template.Api
```

### 8. Tests

**Create Unit Tests**:
- `CustomerTests.cs` (Domain)
- `CreateCustomerCommandHandlerTests.cs` (Application)
- `GetCustomerByIdQueryHandlerTests.cs` (Application)

**Create Integration Tests**:
- `CustomersControllerTests.cs`

## 🔄 Renaming the Template

To create a completely new microservice:

### 1. Global Find & Replace

Replace all occurrences of:
- `Company.Template` → `YourCompany.YourService`
- `Product` → `YourEntity`
- `CompanyTemplateDb` → `YourServiceDb`

### 2. Update Project Names

Rename all project folders and `.csproj` files:
- `Company.Template.Api` → `YourCompany.YourService.Api`
- `Company.Template.Domain` → `YourCompany.YourService.Domain`
- etc.

### 3. Update Solution File

Update `Company.Template.sln` to reflect new project names and paths.

### 4. Update Docker & CI/CD

Update:
- `Dockerfile` paths
- `docker-compose.yml` service names
- GitHub Actions workflow paths

## 📋 Checklist for New Entity

- [ ] Domain entity with business logic
- [ ] Domain events (Created, Updated, Deleted)
- [ ] Repository interface and implementation
- [ ] DTOs and validators
- [ ] CQRS commands and queries
- [ ] Command and query handlers
- [ ] AutoMapper profiles
- [ ] gRPC service and proto definitions
- [ ] REST API controller
- [ ] Database configuration and migration
- [ ] Event handlers for integration events
- [ ] Unit tests
- [ ] Integration tests
- [ ] Update documentation

## 🎨 Customization Tips

### Adding Business Logic
- Keep business rules in domain entities
- Use domain events for side effects
- Implement specification pattern for complex queries

### Adding Validation
- Use FluentValidation for input validation
- Add domain validation in entity methods
- Consider cross-field validation rules

### Adding Caching
- Implement caching in query handlers
- Use distributed cache for multi-instance scenarios
- Consider cache invalidation strategies

### Adding Security
- Implement authentication/authorization
- Add input sanitization
- Consider rate limiting

## 🚀 Advanced Patterns

### Event Sourcing
- Store events instead of current state
- Rebuild state from events
- Implement event store

### CQRS with Separate Read Models
- Use different models for reads and writes
- Implement eventual consistency
- Consider materialized views

### Saga Pattern
- Coordinate distributed transactions
- Handle compensation logic
- Implement saga orchestration

---

This template provides a solid foundation for building modern, scalable microservices. Customize it according to your specific business requirements and architectural preferences.