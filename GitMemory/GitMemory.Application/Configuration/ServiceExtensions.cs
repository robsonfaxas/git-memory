using GitMemory.Application.Commands;
using GitMemory.Application.Handlers;
using GitMemory.Domain.Entities;
using GitMemory.Domain.Repositories;
using GitMemory.Domain.Service;
using GitMemory.Infrastructure.CommandsServices;
using GitMemory.Infrastructure.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace GitMemory.Application.Configuration
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IRequestHandler<SetRepoCommand, CommandResponse>, SetRepoCommandHandler>();
            services.AddScoped<IRequestHandler<PickCommand, CommandResponse>, PickCommandHandler>();
            services.AddScoped<IPickCommandService, PickCommandService>();
            services.AddScoped<ISetRepoCommandService, SetRepoCommandService>();
            services.AddScoped<IGitMemoryGlobalSettings, GitMemoryGlobalSettings>();
            services.AddScoped<IUserSettings, UserSettings>();
            services.AddScoped<ISettingsService, SettingsService>();
            return services;
        }
    }
}
