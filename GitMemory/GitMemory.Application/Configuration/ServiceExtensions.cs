﻿using GitMemory.Application.Commands;
using GitMemory.Application.Handlers;
using GitMemory.Domain.Entities;
using GitMemory.Domain.Repositories;
using GitMemory.Domain.Service;
using GitMemory.Domain.Service.Pick;
using GitMemory.Domain.Service.SetRepo;
using GitMemory.Domain.Service.Unpick;
using GitMemory.Infrastructure.CommandsServices;
using GitMemory.Infrastructure.CommandsServices.Pick;
using GitMemory.Infrastructure.CommandsServices.SetRepo;
using GitMemory.Infrastructure.CommandsServices.Unpick;
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
            services.AddScoped<IRequestHandler<SetRepoCommand, CommandResponse>, SetRepoCommandHandler>();
            services.AddScoped<IRequestHandler<PickCommand, CommandResponse>, PickCommandHandler>();
            services.AddScoped<IRequestHandler<UnpickCommand, CommandResponse>, UnpickCommandHandler>();
            services.AddScoped<IRequestHandler<ErrorLogCommand, CommandResponse>, ErrorLogCommandHandler>();
            services.AddScoped<IPickCommandService, PickCommandService>();
            services.AddScoped<IUnpickCommandService, UnpickCommandService>();
            services.AddScoped<ISetRepoCommandService, SetRepoCommandService>();
            services.AddScoped<IGitMemoryGlobalSettings, GitMemoryGlobalSettings>();
            services.AddScoped<IUserSettings, UserSettings>();
            services.AddScoped<ISettingsService, SettingsService>();
            services.AddScoped<IMemoryPoolService, MemoryPoolService>();
            services.AddScoped<IMemoryPoolRepository, MemoryPoolRepository>();
            services.AddScoped<IErrorLogRepository, ErrorLogRepository>();
            services.AddScoped<IErrorLogService, ErrorLogService>();
            return services;
        }
    }
}
