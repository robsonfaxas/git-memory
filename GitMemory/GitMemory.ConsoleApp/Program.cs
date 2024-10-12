using GitMemory.ConsoleApp;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using GitMemory.Application.Configuration;
using GitMemory.Domain.UI;
using GitMemory.Application.Commands;
using GitMemory.Application.Interfaces;
using GitMemory.Domain.Entities;

class Program
{

    static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
                services.AddScoped<ICommandUI, CommandUI>();                
                services.AddSingleton<IInteractionWindow, UserInteraction>();
                services.AddTransient<SetRepoCommand>();
                services.AddTransient<PickCommand>();
                services.AddTransient<UnpickCommand>();
                services.AddTransient<ErrorLogCommand>();
                services.AddScoped<CommandContextConfiguration>();
                services.AddTransient<IGitCommandFactory, GitCommandFactory>();
                
                services.AddApplicationServices();                
            })
            .Build();

        var serviceProvider = host.Services;

        var app = serviceProvider.GetRequiredService<ICommandUI>();
        app.Args = args.ToList();
        app.Args = new List<string>() { "pick", "2181e36dda5a84947fb98656cd2a78810bfbf85a", "0693eb213621d7014858f6537376efb1e62e7c29" };
        await app.Run();
        app.Args = new List<string>() { "unpick", "2181e36dda5a84947fb98656cd2a78810bfbf85a", "0693eb213621d7014858f6537376efb1e62e7c29" };
        await app.Run();
        app.Args = new List<string>() { "pick", "12" };
        await app.Run();
        app.Args = new List<string>() { "unpick", "--all" };
        await app.Run();
    }
}
