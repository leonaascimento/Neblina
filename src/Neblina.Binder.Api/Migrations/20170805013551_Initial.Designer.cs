﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Neblina.Binder.Api.Persistence;

namespace Neblina.Binder.Api.Migrations
{
    [DbContext(typeof(BinderContext))]
    [Migration("20170805013551_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2");

            modelBuilder.Entity("Neblina.Binder.Api.Models.Bank", b =>
                {
                    b.Property<int>("BankId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BaseUrl");

                    b.Property<string>("Name");

                    b.HasKey("BankId");

                    b.ToTable("Banks");
                });
        }
    }
}
