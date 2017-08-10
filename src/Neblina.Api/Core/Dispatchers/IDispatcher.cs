using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neblina.Api.Core.Dispatchers
{
    public interface IDispatcher
    {
        void Enqueue(int id);
    }
}
