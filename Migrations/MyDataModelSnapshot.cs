﻿// <auto-generated />
using System;
using MQTT.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MQTT.Migrations
{
    [DbContext(typeof(MyData))]
    partial class MyDataModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MQTT.Models.Data", b =>
                {
                    b.Property<string>("name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("timestamp")
                        .HasColumnType("datetime2");

                    b.Property<string>("value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("name", "timestamp");

                    b.ToTable("Datas");
                });
#pragma warning restore 612, 618
        }
    }
}
