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
        //app.Args = args.ToList();
        app.Args = new List<string>() { "pick", "AKSJDLKADSKLAMDKLAMD" };
        await app.Run();
    }
}
