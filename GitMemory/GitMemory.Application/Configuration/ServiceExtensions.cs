using GitMemory.Application.Commands;
using GitMemory.Application.Handlers;
using GitMemory.Domain.Service;
using GitMemory.Infrastructure.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace GitMemory.Application.Configuration
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IRequestHandler<SetRepoCommand>, SetRepoCommandHandler>();
            services.AddScoped<ISetRepoCommandService, SetRepoCommandService>();
            return services;
        }
    }
}
