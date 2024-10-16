﻿using GitMemory.Application.Commands;
using GitMemory.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace GitMemory.ConsoleApp
{
    public class GitCommandFactory : IGitCommandFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public GitCommandFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IGitCommandRequest GetCommand(string commandName, List<string> parameters)
        {
            switch (commandName.ToLower())
            {
                case "set-repo":
                    return _serviceProvider.GetRequiredService<SetRepoCommand>()
                        .Initialize(parameters);
                case "pick":
                    return _serviceProvider.GetRequiredService<PickCommand>()
                        .Initialize(parameters);
                case "unpick":
                    return _serviceProvider.GetRequiredService<UnpickCommand>()
                        .Initialize(parameters);
                case "errorlog":
                    return _serviceProvider.GetRequiredService<ErrorLogCommand>()
                        .Initialize(parameters);
                default:
                    throw new ArgumentException("Invalid command.");
            }
        }
    }
}
