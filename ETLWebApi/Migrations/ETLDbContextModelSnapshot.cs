﻿// <auto-generated />
using System;
using ETLWebApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ETLWebApi.Migrations
{
    [DbContext(typeof(ETLDbContext))]
    partial class ETLDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.30")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ETLWebApi.Models.ETLData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("DOLocationID")
                        .HasColumnType("int");

                    b.Property<int>("PULocationID")
                        .HasColumnType("int");

                    b.Property<double>("fare_amount")
                        .HasColumnType("float");

                    b.Property<int>("passenger_count")
                        .HasColumnType("int");

                    b.Property<string>("store_and_fwd_flag")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("tip_amount")
                        .HasColumnType("float");

                    b.Property<DateTime>("tpep_dropoff_datetime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("tpep_pickup_datetime")
                        .HasColumnType("datetime2");

                    b.Property<double>("trip_distance")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("ETLDatas");
                });
#pragma warning restore 612, 618
        }
    }
}
