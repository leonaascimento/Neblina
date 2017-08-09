using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Neblina.Binder.Api.Persistence;

namespace Neblina.Binder.Api.Migrations
{
    [DbContext(typeof(BinderContext))]
    partial class BinderContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2");

            modelBuilder.Entity("Neblina.Binder.Api.Core.Models.Bank", b =>
                {
                    b.Property<int>("BankId");

                    b.Property<string>("Name")
                        .HasMaxLength(100);

                    b.Property<string>("ReceiveUrl");

                    b.Property<string>("StatusUrl");

                    b.HasKey("BankId");

                    b.ToTable("Banks");
                });
        }
    }
}
