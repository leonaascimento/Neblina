﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neblina.Api.Core.Commands
{
    public interface ICommand
    {
        bool Execute(int id, int tries = 3, int waitInterval = 100);
    }
}
