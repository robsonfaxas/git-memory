﻿using GitMemory.Application.Interfaces;

namespace GitMemory.Application.Commands
{
    public class SetBrainCommand : IGitCommandRequest
    {
        public List<string> Parameters { get; set; } = new List<string>();

        public SetBrainCommand Initialize(List<string> commands)
        {
            Parameters = commands;            
            return this;
        }
    }
}
