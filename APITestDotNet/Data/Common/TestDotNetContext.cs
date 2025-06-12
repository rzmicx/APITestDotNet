using System;
using System.Collections.Generic;
using APITestDotNet.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace APITestDotNet.Data.Common;

public partial class TestDotNetContext : DbContext
{
    public TestDotNetContext()
    {
    }

    public TestDotNetContext(DbContextOptions<TestDotNetContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Msuser> Msusers { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = config.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Msuser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__msuser__3213E83F9AA99208");

            entity.ToTable("msuser");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Active).HasColumnName("active");
            entity.Property(e => e.Passcode).HasColumnName("passcode");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("username");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Product__3214EC07F905F91D");

            entity.ToTable("Product");

            entity.Property(e => e.CreateBy)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Price).HasColumnType("decimal(17, 2)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
