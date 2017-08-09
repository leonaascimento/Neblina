using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neblina.Api.Core
{
    public interface ITransactionDispatcher
    {
        void Enqueue(int id);
        void Execute(int id);
    }
}
