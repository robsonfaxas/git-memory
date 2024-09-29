﻿using GitMemory.Domain.Entities.Memories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitMemory.Domain.Service
{
    public interface IErrorLogService
    {
        void Log(string message);
        void Log(Exception ex);
    }
}