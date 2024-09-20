using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitMemory.Application.Interfaces
{
    public interface IGitCommandRequest : IRequest
    {
        List<string> Commands { get; set; }
    }
}
