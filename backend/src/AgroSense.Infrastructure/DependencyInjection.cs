using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AgroSense.Application.Common.Interfaces;
using AgroSense.Infrastructure.Persistence;
using AgroSense.Infrastructure.Services;

namespace AgroSense.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<AppDbContext>(options =>
        {
            if (!string.IsNullOrEmpty(connectionString) && !connectionString.Contains("YOUR_NEONDB_HOST"))
            {
                options.UseNpgsql(connectionString);
            }
            else
            {
                // Local development fallback: SQLite (zero-config local DB)
                options.UseSqlite("Data Source=agrosense_local.db");
            }
        });

        services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());

        services.AddHttpClient<IMLService, MLService>();

        return services;
    }
}

