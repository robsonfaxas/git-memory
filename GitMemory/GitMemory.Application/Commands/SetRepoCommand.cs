﻿using GitMemory.Application.Interfaces;
using GitMemory.Domain.UI;

namespace GitMemory.Application.Commands
{
    public class SetRepoCommand : IGitCommandRequest
    {
        public List<string> Parameters { get; set; }
        public IInteractionWindow InteractionWindow { get; set; }

        public SetRepoCommand(List<string> commands, IInteractionWindow logger)
        {
            Parameters = commands;
            InteractionWindow = logger;
        }
    }
}
