﻿using GitMemory.Domain.Entities;
using GitMemory.Domain.Entities.Enums;
using GitMemory.Domain.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitMemory.ConsoleApp.IntegrationTests.Configuration
{
    public class InteractionWindowTest : IInteractionWindow
    {
        public string Title { get; set; } = string.Empty;

        public string Read()
        {
            if (Interactions.StringRequest.Any())
                return Interactions.StringRequest.Dequeue();
            else
                throw new Exception("No inputs were configured for this test. Please, in \"Arrange\" section of this xunit test, add the expected user inputs for this test.");
        }

        public DialogResultEnum Read(DialogButtonsEnum buttons, CommandResponse command)
        {
            if (Interactions.DialogResultRequest.Any())
                return Interactions.DialogResultRequest.Dequeue();
            else
                throw new Exception("No inputs were configured for this test. Please, in \"Arrange\" section of this xunit test, add the expected dialog user inputs for this test.");            
        }

        public void Write(CommandResponse command)
        {
            Interactions.Output.Enqueue(command);
        }
    }
}
