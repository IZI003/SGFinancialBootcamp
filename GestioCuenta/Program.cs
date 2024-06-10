using Cuenta;
using Microsoft.AspNetCore;

public class Program
{
    public static void Main(string[] args)
    {
        CreateWebHostBuilder(args).Build().Run();
    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                config.SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", true)
                    .AddEnvironmentVariables();

                if (context.HostingEnvironment.IsDevelopment())
                {
                    Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;
                    config.AddUserSecrets<Program>();
                }
            })
            .UseStartup<Startup>();
}