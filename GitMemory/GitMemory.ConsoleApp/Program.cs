using GitMemory.ConsoleApp;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using GitMemory.Application.Configuration;
using GitMemory.Domain.UI;

class Program
{

    static async Task Main(string[] args)
    {
        var services = new ServiceCollection();
        

        var host = Host.CreateDefaultBuilder()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
                services.AddScoped<ICommandUI, CommandUI>();                
                services.AddApplicationServices();                
            })
            .Build();

        var serviceProvider = host.Services;

        var app = new CommandUI(serviceProvider.GetRequiredService<IMediator>());
        app.Args = args.ToList();        
        await app.Run();
    }
}
