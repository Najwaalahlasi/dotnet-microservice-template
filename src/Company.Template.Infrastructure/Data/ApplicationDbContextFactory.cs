using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Company.Template.Infrastructure.Data;

/// <summary>
/// Design-time factory for ApplicationDbContext
/// This is used by EF Core tools for migrations
/// </summary>
public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        
        // Use SQLite for design-time to avoid PostgreSQL version conflicts
        optionsBuilder.UseSqlite("Data Source=temp.db");

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}