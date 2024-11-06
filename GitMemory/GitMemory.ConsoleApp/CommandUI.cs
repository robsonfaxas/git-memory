using GitMemory.Application.Commands;
using GitMemory.Application.Interfaces;
using GitMemory.Domain.Entities;
using GitMemory.Domain.Entities.Enums;
using GitMemory.Domain.UI;
using MediatR;

namespace GitMemory.ConsoleApp
{
    public class CommandUI : ICommandUI
    {
        private readonly IMediator _mediator;        
        private readonly IInteractionWindow _interactionWindow;
        private readonly IGitCommandFactory _commandFactory;
        private CommandContextConfiguration _contextConfiguration;

        /// <summary>
        /// 1º item is the command
        /// All the other items are arguments/parameters to the method
        /// </summary>
        public List<string> Args { get; set; } = new List<string>();

        public CommandUI(IMediator mediator, IInteractionWindow interactionWindow, IGitCommandFactory commandFactory,
                        CommandContextConfiguration contextConfiguration)
        {
            _mediator = mediator;            
            _interactionWindow = interactionWindow;
            _commandFactory = commandFactory;
            _contextConfiguration = contextConfiguration;            
        }

        public async Task Run()
        {
            try
            {
                BuildContext();
                var commandResponse = await _mediator.Send(ParseRequest());
                _interactionWindow.Write(commandResponse);
            }
            catch (ArgumentException ex)
            {
                _interactionWindow.Write(new CommandResponse(ex.Message, ResponseTypeEnum.Error));
            }
        }

        private IGitCommandRequest ParseRequest()
        {
            if (Args.Count >= 1)
            {
                // Using the factory to get the appropriate command
                return _commandFactory.GetCommand(Args.First(), Args.Skip(1).ToList());
            }
            else
            {
                throw new ArgumentException("Command not provided.");
            }
        }

        private CommandContextConfiguration BuildContext()
        {            
            _contextConfiguration.GlobalSettingsDirectory = GetGlobalSettingsDirectory();
            _contextConfiguration.CurrentDirectory = GetCurrentDirectory();
            CommandContextAccessor.Current = _contextConfiguration;
            return _contextConfiguration;
        }

        /// <summary>
        /// Check if UI has sent a directive requesting to override GlobalSettings folder
        /// otherwise gets UserProfile directory
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private string GetGlobalSettingsDirectory()
        {
            bool foundGlobalSettingsOverride = false;
            foreach (var arg in Args.ToList())
            {
                if (arg.ToLower().Equals("--globalsettingsfolder"))
                {
                    foundGlobalSettingsOverride = true;
                    Args.Remove(arg);
                    continue;
                }
                if (foundGlobalSettingsOverride)
                {
                    Args.Remove(arg);
                    if (Directory.Exists(arg))
                        return arg;
                    else
                        throw new ArgumentException("GlobalSettings folder does not exist");
                }
            }
            return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) ?? "";
        }

        /// <summary>
        /// Check if UI has sent a directive requesting to override CurrentDirectory folder
        /// otherwise gets the CurrentDirectory from the context
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private string GetCurrentDirectory()
        {
            bool foundCurrentDirectoryOverride = false;
            foreach (var arg in Args.ToList())
            {
                if (arg.ToLower().Equals("--currentdirectory"))
                {
                    foundCurrentDirectoryOverride = true;
                    Args.Remove(arg);
                    continue;
                }
                if (foundCurrentDirectoryOverride)
                {
                    Args.Remove(arg);
                    if (Directory.Exists(arg))
                        return arg;
                    else
                        throw new ArgumentException("CurrentDirectory folder does not exist");
                }
            }
            return Directory.GetCurrentDirectory() ?? "";
        }

        public CommandContextConfiguration SetContext(string globalSettingsDirectory, string currentDirectory)
        {
            _contextConfiguration.GlobalSettingsDirectory = globalSettingsDirectory;
            _contextConfiguration.CurrentDirectory = currentDirectory;
            CommandContextAccessor.Current = _contextConfiguration;
            return _contextConfiguration;
        }
    }
}
