﻿using GitMemory.Domain.Entities;

namespace GitMemory.Domain.Service
{
    public interface ISetRepoCommandService
    {
        Task<CommandResponse> ExecuteCommand(List<string> commands);
    }
}
