namespace AccesoDatos;

using AccesoDatos.Conexiones;
using AccesoDatos.DBConfig;
using Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AgregarAccesoDatos(this IServiceCollection services)
    {
        services.AddTransient<BD>();
        services.AddScoped<BDConfig>();
        services.AddTransient<IFabricaConexion, FabricaConexionDesdeConfig>();

        return services;
    }
}
