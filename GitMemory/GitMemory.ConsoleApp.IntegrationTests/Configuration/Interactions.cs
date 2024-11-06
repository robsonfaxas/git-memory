using GitMemory.Domain.Entities;
using GitMemory.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitMemory.ConsoleApp.IntegrationTests.Configuration
{
    public class Interactions
    {
        public static Queue<string> StringRequest { get; set; } = new Queue<string>();
        public static Queue<DialogResultEnum> DialogResultRequest { get; set; } = new Queue<DialogResultEnum>();
        public static Queue<CommandResponse> Output { get; set; } = new Queue<CommandResponse>();
    }
}
