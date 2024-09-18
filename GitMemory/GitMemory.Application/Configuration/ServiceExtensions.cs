using GitMemory.Application.Commands;
using GitMemory.Application.Handlers;
using GitMemory.Domain.Interfaces;
using GitMemory.Infrastructure.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace GitMemory.Application.Configuration
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IRequestHandler<ProcessCommand>, ProcessCommandHandler>();
            services.AddScoped<ICommandService, CommandService>();
            return services;
        }
    }
}
