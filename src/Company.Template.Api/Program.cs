using Company.Template.Api.Extensions;
using Company.Template.Application;
using Company.Template.Grpc.Services;
using Company.Template.Infrastructure;
using Company.Template.Shared.Validators;
using FluentValidation;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Company.Template.Api", Version = "v1" });
    
    // Include XML comments
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

// Add gRPC
builder.Services.AddGrpc();

// Add application layers
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Add validators
builder.Services.AddValidatorsFromAssemblyContaining<CreateProductDtoValidator>();

// Add health checks (simplified for demo)
builder.Services.AddHealthChecks();

// Add observability
builder.Services.AddObservability(builder.Configuration);

// Add resilience
builder.Services.AddResilience();

// Add authentication (placeholder)
builder.Services.AddAuthentication()
    .AddJwtBearer(options =>
    {
        // Configure JWT authentication here
        // This is a placeholder for production implementation
    });

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Map gRPC services
app.MapGrpcService<ProductGrpcService>();

// Map health checks
app.MapHealthChecks("/health");

// Add gRPC reflection in development (commented out for now)
// if (app.Environment.IsDevelopment())
// {
//     app.MapGrpcReflectionService();
// }

try
{
    Log.Information("Starting Company.Template.Api");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

// Make Program class accessible for integration tests
public partial class Program { }