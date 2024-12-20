﻿using GitMemory.ConsoleApp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using GitMemory.Application.Configuration;
using GitMemory.Domain.UI;
using GitMemory.Application.Commands;
using GitMemory.Application.Interfaces;
using GitMemory.Domain.Entities;

public class Program
{
    public static async Task Main(string[] args)
    {
        var host = new Program().CreateHostBuilder().Build();
        var serviceProvider = host.Services;
        var app = serviceProvider.GetRequiredService<ICommandUI>();
        app.Args = args.ToList();
        await app.Run();
    }

    public virtual IHostBuilder CreateHostBuilder() =>
        Host.CreateDefaultBuilder()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
                services.AddScoped<ICommandUI, CommandUI>();
                services.AddSingleton<IInteractionWindow, UserInteraction>();
                services.AddTransient<SetBrainCommand>();
                services.AddTransient<PickCommand>();
                services.AddTransient<UnpickCommand>();
                services.AddTransient<ErrorLogCommand>();
                services.AddTransient<StatusCommand>();
                services.AddScoped<CommandContextConfiguration>();
                services.AddTransient<IGitCommandFactory, GitCommandFactory>();
                services.AddApplicationServices();
            });
}
