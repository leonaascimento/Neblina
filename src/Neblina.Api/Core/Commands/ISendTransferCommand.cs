using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neblina.Api.Core.Commands
{
    public interface ISendTransferCommand
    {
        void Enqueue(int id);
    }
}
