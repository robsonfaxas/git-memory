using GitMemory.Application.Commands;
using GitMemory.Application.Interfaces;
using GitMemory.Domain.Entities;
using GitMemory.Domain.UI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using GitMemory.Application.Configuration;

namespace GitMemory.ConsoleApp.IntegrationTests.Configuration
{
    internal class ProgramTest
    {
        public static async Task MainTestAsync(string[] args)
        {
            var host = new ProgramTest().CreateHostBuilder().Build();
            var serviceProvider = host.Services;
            var app = serviceProvider.GetRequiredService<ICommandUI>();
            app.Args = args.ToList();
            await app.Run();
        }

        public IHostBuilder CreateHostBuilder() =>
        Host.CreateDefaultBuilder()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
                services.AddScoped<ICommandUI, CommandUI>();
                services.AddSingleton<IInteractionWindow, InteractionWindowTest>();
                services.AddTransient<SetRepoCommand>();
                services.AddTransient<PickCommand>();
                services.AddTransient<UnpickCommand>();
                services.AddTransient<ErrorLogCommand>();
                services.AddScoped<CommandContextConfiguration>();
                services.AddTransient<IGitCommandFactory, GitCommandFactory>();

                services.AddApplicationServices();
            });
    }
}
