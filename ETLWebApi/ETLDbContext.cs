﻿using ETLWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ETLWebApi
{
    public class ETLDbContext : DbContext
    {
        public ETLDbContext(DbContextOptions<ETLDbContext> options) : base(options) { }

        public DbSet<ETLData> ETLDatas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ETLData>()
                .HasIndex(r => r.PULocationID);

            modelBuilder.Entity<ETLData>()
                .HasIndex(r => r.PickupDatetime);

            modelBuilder.Entity<ETLData>()
                .HasIndex(r => r.DropoffDatetime);
        }
    }
}