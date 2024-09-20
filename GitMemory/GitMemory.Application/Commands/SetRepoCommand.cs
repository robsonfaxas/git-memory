using GitMemory.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitMemory.Application.Commands
{
    public class SetRepoCommand : IGitCommandRequest
    {
        public List<string> Commands { get; set; }
        
        public SetRepoCommand(List<string> commands)
        {
            Commands = commands;
        }
    }
}
