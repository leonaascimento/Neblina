using Neblina.Binder.Api.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neblina.Binder.Api.Core
{
    public interface IUnitOfWork
    {
        IBankRepository Banks { get; }
        void SaveAndApply();
    }
}
