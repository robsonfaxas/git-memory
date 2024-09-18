using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitMemory.Application.Commands
{
    public class ProcessCommand : IRequest
    {
        public string Command { get; }

        public ProcessCommand(string command)
        {
            Command = command;
        }
    }
}
