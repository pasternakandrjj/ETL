using ETL.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection.Emit;

namespace ETL
{
    public class ETLDbContext : DbContext
    {
        public ETLDbContext(DbContextOptions<ETLDbContext> options) : base(options)
        {
        }

        public DbSet<ETLData> ETLDatas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<ETLData>()
            //    .HasIndex(r => r.Width);

            //modelBuilder.Entity<ETLData>()
            //    .HasIndex(r => r.Height);
        }
    }
}