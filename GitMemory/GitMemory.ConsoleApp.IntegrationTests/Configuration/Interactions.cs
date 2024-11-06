using GitMemory.Domain.Entities;
using GitMemory.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitMemory.ConsoleApp.IntegrationTests.Configuration
{
    /// <summary>
    /// All the interaction between a user and GitMemory will be simulated using these 3 queues.
    /// So for every test where it's expected to happen an interaction, all the interactions should be included in one 
    /// or more out of these queues.
    /// 
    /// - StringRequest: when it's requested a text answer from the User (repo location, argument value, etc)
    /// - DialogResultRequest: when it's requested a default answer to the user  (yes/no/cancel/ok)
    /// - Output: the message returned to the user (e.g: "Repository saved.")
    /// </summary>
    public class Interactions
    {
        public static Queue<string> StringRequest { get; set; } = new Queue<string>();
        public static Queue<DialogResultEnum> DialogResultRequest { get; set; } = new Queue<DialogResultEnum>();
        public static Queue<CommandResponse> Output { get; set; } = new Queue<CommandResponse>();
    }
}
