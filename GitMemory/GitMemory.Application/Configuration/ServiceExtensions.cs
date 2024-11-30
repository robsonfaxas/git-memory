using GitMemory.Application.Commands;
using GitMemory.Application.Handlers;
using GitMemory.Domain.Entities;
using GitMemory.Domain.Repositories;
using GitMemory.Domain.Service;
using GitMemory.Domain.Service.Pick;
using GitMemory.Domain.Service.SetRepo;
using GitMemory.Domain.Service.Stage;
using GitMemory.Domain.Service.Status;
using GitMemory.Domain.Service.Unpick;
using GitMemory.Domain.Service.Unstage;
using GitMemory.Infrastructure.CommandsServices;
using GitMemory.Infrastructure.CommandsServices.Pick;
using GitMemory.Infrastructure.CommandsServices.SetRepo;
using GitMemory.Infrastructure.CommandsServices.Stage;
using GitMemory.Infrastructure.CommandsServices.Status;
using GitMemory.Infrastructure.CommandsServices.Unpick;
using GitMemory.Infrastructure.CommandsServices.Unstage;
using GitMemory.Infrastructure.Repositories;
using GitMemory.Infrastructure.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace GitMemory.Application.Configuration
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IRequestHandler<SetRepoCommand, Command>, SetRepoCommandHandler>();
            services.AddScoped<IRequestHandler<PickCommand, Command>, PickCommandHandler>();
            services.AddScoped<IRequestHandler<UnpickCommand, Command>, UnpickCommandHandler>();
            services.AddScoped<IRequestHandler<ErrorLogCommand, Command>, ErrorLogCommandHandler>();
            services.AddScoped<IRequestHandler<StageCommand, Command>, StageCommandHandler>();
            services.AddScoped<IRequestHandler<UnstageCommand, Command>, UnstageCommandHandler>();
            services.AddScoped<IRequestHandler<StageCommand, Command>, StageCommandHandler>();
            services.AddScoped<IPickCommandService, PickCommandService>();
            services.AddScoped<IUnpickCommandService, UnpickCommandService>();
            services.AddScoped<ISetRepoCommandService, SetRepoCommandService>();
            services.AddScoped<IGitMemoryGlobalSettings, GitMemoryGlobalSettings>();
            services.AddScoped<IStageCommandService, StageCommandService>();
            services.AddScoped<IUnstageCommandService, UnstageCommandService>();
            services.AddScoped<IStatusCommandService, StatusCommandService>();
            services.AddScoped<IUserSettings, UserSettingsRepository>();
            services.AddScoped<ISettingsService, SettingsService>();
            services.AddScoped<IMemoryPoolService, MemoryPoolService>();
            services.AddScoped<IMemoryPoolRepository, MemoryPoolRepository>();
            services.AddScoped<IErrorLogRepository, ErrorLogRepository>();
            services.AddScoped<IErrorLogService, ErrorLogService>();
            return services;
        }
    }
}
