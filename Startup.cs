using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;
using Microsoft.Extensions.Hosting;

namespace TryDockerTest;

public class Startup
{
    public void ConfigureServices(IServiceCollection services, HostBuilderContext context)
    {
        services.AddScoped<MySqlConnection>(sp => new MySqlConnection("Server=127.0.0.1; Port=3306; User ID=root; Password=root;"));
    }
}
