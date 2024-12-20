﻿using GitMemory.Domain.Entities;

namespace GitMemory.Domain.Service.Unpick
{
    public interface IUnpickCommandService
    {
        Task<Command> ExecuteCommand(List<string> commands);
    }
}
