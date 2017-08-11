using Microsoft.EntityFrameworkCore;
using Neblina.Binder.Api.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neblina.Binder.Api.Persistence
{
    public class BinderContext : DbContext
    {
        public BinderContext(DbContextOptions<BinderContext> options)
            : base(options)
        {
        }

        public DbSet<Bank> Banks { get; set; }
    }
}
