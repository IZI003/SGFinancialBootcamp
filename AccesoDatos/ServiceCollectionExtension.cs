namespace AccesoDatos;

using Microsoft.Extensions.DependencyInjection;

using AccesoDatos.Conexiones;
using AccesoDatos.DBConfig;

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
