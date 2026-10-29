using Microsoft.Extensions.DependencyInjection;
using AgroSense.Application.Interfaces;
using AgroSense.Application.Services;

namespace AgroSense.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ISensorService, SensorService>();
        services.AddScoped<IPredictionService, PredictionService>();
        services.AddScoped<IDeviceService, DeviceService>();
        return services;
    }
}

